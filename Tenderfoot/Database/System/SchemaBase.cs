using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Tenderfoot.TfSystem;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.Database.System
{
    public class SchemaBase<T> where T : Entity, new()
    {
        public string TableName { get; set; }
        
        protected SchemaBase(string tableName) { this.TableName = tableName; }

        protected NonQuery NonQuery { get; set; } = null;
        
        protected List<JoinItem> Join { get; set; } = new List<JoinItem>();
        
        protected NonConditions NonConditions { get; set; } = new NonConditions();

        protected void ClearNonConditions() { this.NonConditions = new NonConditions(); }

        protected string GetWhere(List<JoinItem> join = null, bool skipEntity = false)
        {
            string where = string.Empty;
            string joinedTables = string.Empty;

            if (join != null && join.Count() > 0)
            {
                if (this.Join.Count() > 0)
                {
                    joinedTables = $"FROM {string.Join(", ", this.Join.Select(x => x.TableName))}";
                    where += $"{string.Join(", ", this.Join.Select(x => x.OnString))} AND ";
                }
            }

            if (this.Case.MultiWhere?.Count() > 0)
            {
                where += string.Join(" ", this.Case.MultiWhere);
            }

            if (this.Case.WhereBase.IsEmpty() && !skipEntity)
            {
                this.GetWhereFromEntity();
            }

            where +=
                !this.Case.WhereBase.IsEmpty() ?
                this.Case.WhereBase :
                string.Empty;

            return 
                !where.IsEmpty() ?
                $"{joinedTables} WHERE {where}" :
                string.Empty;
        }
        
        protected string CreateColumns()
        {
            var columnList = new List<string>();

            var type = this.Entity.GetType();

            foreach (var property in type.GetProperties())
            {
                string column = this.CreateColumn(property);

                if (column.IsEmpty())
                {
                    continue;
                }
                 
                columnList.Add(column);
            }

            return string.Join(",", columnList);
        }
        
        protected string AddColumns(params string[] columns)
        {
            var columnList = new List<string>();

            foreach (var column in columns)
            {
                var property = this.Entity.GetType().GetProperty(column);
                var newColumn = this.CreateColumn(property);

                if (newColumn.IsEmpty())
                {
                    continue;
                }

                columnList.Add($"{Operations.ADD} {newColumn}");
            }

            return string.Join(",", columnList); ;
        }

        protected string DropColumns(params string[] columns)
        {
            var columnList = new List<string>();

            foreach (var column in columns)
            {
                string alteredColumn = $"{Operations.DROP_COLUMN.GetString()} {column}";
                columnList.Add(alteredColumn);
            }

            return string.Join(",", columnList); ;
        }

        private void GetWhereFromEntity()
        {
            foreach (var property in this.Entity.GetType().GetProperties())
            {
                var value = property.GetValue(this.Entity);
                if (value?.ToString().IsEmpty() ?? true)
                {
                    continue;
                }
                if (property.GetCustomAttribute<EncryptAttribute>() != null &&
                    property.PropertyType == typeof(string))
                {
                    value = Encryption.Encrypt(Convert.ToString(value));
                }
                if (property.PropertyType.IsArray)
                {
                    this.Case.Where($"{{0}} = ANY({this.TableName}.{property.Name})", value);
                }
                else
                {
                    this.Case.Where($"{this.TableName}.{property.Name} = {{0}}", value);
                }
            }
        }
        
        private string CreateColumn(PropertyInfo property)
        {
            var customAttributes = property.GetCustomAttributes(false);

            string column = property.Name;
            string dataType = this.GetColumnDataType(property.PropertyType, customAttributes, out bool unArray);
            string attributes = string.Empty;
            
            if (dataType.IsEmpty())
            {
                return string.Empty;
            }

            if (customAttributes != null)
            {
                attributes += $" {this.GetColumnAttributes(customAttributes)}";
            }

            if (attributes.Contains(DataType.SERIAL.GetString()))
            {
                dataType = string.Empty;
            }

            string array = 
                property.PropertyType.IsArray && !unArray ? 
                "[]" : 
                string.Empty;

            return $"{column} {dataType}{array} {attributes}";
        }

        private string GetColumnDataType(Type type, object[] attributes, out bool unArray)
        {
            bool hasSerial = false;
            bool hasText = false;
            unArray = false;

            foreach (var attribute in attributes)
            {
                if (attribute is SerialAttribute)
                {
                    hasSerial = true;
                }

                if (attribute is TextAttribute)
                {
                    hasText = true;
                }

                if (attribute is NonTableColumnAttribute)
                {
                    return string.Empty;
                }
            }
            
            if (type == typeof(string) ||
                type == typeof(string[]))
            {
                return
                    !hasText ?
                    DataType.CHARACTER_VARYING.GetString() :
                    DataType.TEXT.GetString();
            }
            else if (
                type == typeof(int) ||
                type == typeof(int?) ||
                type == typeof(int[]) ||
                type == typeof(int?[]))
            {
                return
                    !hasSerial ?
                    DataType.INTEGER.GetString() :
                    DataType.SERIAL.GetString();
            }
            else if (
                type == typeof(long) ||
                type == typeof(long?) ||
                type == typeof(long[]) ||
                type == typeof(long?[]))
            {
                return
                    !hasSerial ?
                    DataType.BIGINT.GetString() :
                    DataType.BIGSERIAL.GetString();
            }
            else if (
                type == typeof(double) ||
                type == typeof(double?) ||
                type == typeof(double[]) ||
                type == typeof(double?[]))
            {
                return
                    !hasSerial ?
                    DataType.DOUBLE_PRECISION.GetString() :
                    DataType.DOUBLE_PRECISION.GetString();
            }
            else if (
                type == typeof(short) ||
                type == typeof(short?) ||
                type == typeof(bool) ||
                type == typeof(bool?) ||
                type == typeof(short[]) ||
                type == typeof(short?[]) ||
                type == typeof(bool[]) ||
                type == typeof(bool?[]))
            {
                return
                    !hasSerial ?
                    DataType.SMALLINT.GetString() :
                    DataType.SMALLSERIAL.GetString();
            }
            else if (
                type == typeof(DateTime) ||
                type == typeof(DateTime?) ||
                type == typeof(DateTime[]) ||
                type == typeof(DateTime?[]))
            {
                return DataType.TIMESTAMP_WITHOUT_TIME_ZONE.GetString();
            }
            else if (
                type == typeof(byte[]) ||
                type == typeof(byte?[]) ||
                type == typeof(Byte[]) ||
                type == typeof(Byte?[]))
            {
                unArray = true;
                return DataType.BYTEA.GetString();
            }

            return DataType.CHARACTER_VARYING.GetString();
        }

        private string GetColumnAttributes(object[] attributes)
        {
            var attributeString = string.Empty; 

            foreach (var attribute in attributes)
            {
                if (attribute is LengthAttribute)
                {
                    var attr = attribute as LengthAttribute;
                    attributeString = $"({attr.LengthCount}) ";
                }

                if (attribute is NotNullAttribute)
                {
                    var attr = attribute as NotNullAttribute;
                    attributeString += $"{ColumnAttributes.NOT_NULL.GetString()} ";
                }

                if (attribute is DefaultAttribute)
                {
                    var attr = attribute as DefaultAttribute;
                    
                    if (!attr.DefaultObject.IsEmpty())
                    {
                        attributeString += $"DEFAULT ";

                        if (attr.DefaultObject.Contains("(") && attr.DefaultObject.Contains(")"))
                        {
                            attributeString += $"{attr.DefaultObject}";
                        }
                        else
                        {
                            attributeString += $"'{attr.DefaultObject}'";
                        }
                    }
                }

                if (attribute is PrimaryKeyAttribute)
                {
                    attributeString += $"{ColumnAttributes.PRIMARY_KEY.GetString()} ";
                }

                if (attribute is UniqueAttribute)
                {
                    attributeString += $"{ColumnAttributes.UNIQUE} ";
                }
            }

            return attributeString;
        }

        /// <summary>
        /// Clears the current relationships.
        /// </summary>
        public void ClearRelation() { this.Join = new List<JoinItem>(); }

        /// <summary>
        /// Creates the criterias for selecting entities.
        /// </summary>
        public Conditions<T> Case { get; set; } = new Conditions<T>();

        /// <summary>
        /// Clears the current criterias.
        /// </summary>
        public void ClearCase() { this.Case = new Conditions<T>(); }

        /// <summary>
        /// The entity of the schema.
        /// </summary>
        public T Entity { get; set; } = new T();

        /// <summary>
        /// Clears the values setted on the entity.
        /// </summary>
        public void ClearEntity() { this.Entity = new T(); }

        public void Clear()
        {
            this.Case = new Conditions<T>();
            this.Entity = new T();
            this.Join = new List<JoinItem>();
        }

        /// <summary>
        /// Returns the name of the particular selected column.
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="expression"></param>
        /// <returns>The column name.</returns>
        public TableColumn _<TProp>(Expression<Func<T, TProp>> expression)
        {
            var body = expression.Body as MemberExpression;
            
            if (body == null)
            {
                return null;
            }

            var column = body.Member.Name;
            var property = this.Entity.GetType().GetProperty(column);

            if (property == null)
            {
                return null;
            }

            return new TableColumn()
            {
                ColumnName = column,
                Property = property,
                TableName = this.TableName
            };
        }

        public Relation Relation(TableColumn column1, TableColumn column2, Is? condition = null)
        {
            if (column1 == null || column2 == null)
            {
                return null;
            }

            return new Relation()
            {
                Column1 = column1,
                Column2 = column2,
                Condition = condition
            };
        }
    }

    public enum DataType { BIGINT, BYTEA, BIGSERIAL, CHARACTER_VARYING, DOUBLE_PRECISION, INTEGER, SERIAL, SMALLINT, SMALLSERIAL, TIMESTAMP_WITHOUT_TIME_ZONE, TEXT }
    public enum Operations { SELECT, INSERT, UPDATE, DELETE, CREATE_TABLE, ALTER_TABLE, ADD, DROP_COLUMN, CREATE_DATABASE }
    public enum ColumnAttributes { NOT_NULL, PRIMARY_KEY, UNIQUE }

    public class TableColumn
    {
        public string ColumnName { get; set; }
        public string TableName { get; set; }
        public string Get => $"{this.TableName}.{this.ColumnName}";
        public string GetCustomName(string customName)
        {
            return $"{customName}.{this.ColumnName}";
        }
        public PropertyInfo Property { get; set; }
    }

    public class Relation
    {
        public TableColumn Column1 { get; set; }
        public TableColumn Column2 { get; set; }
        public Is? Condition { get; set; }
    }

    public class JoinItem
    {
        public Join? Join { get; set; }
        public string TableName { get; set; }
        public string OnString { get; set; }
        public Type EntityType { get; set; }
    }
}
