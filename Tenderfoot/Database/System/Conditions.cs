using Tenderfoot.TfSystem;
using Tenderfoot.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tenderfoot.Database.System
{
    public class Conditions<TableEntity> where TableEntity : Entity, new()
    {
        public string[] Columns { get; private set; }

        public void AddColumns(params TableColumn[] columns)
        {
            if (columns == null)
            {
                return;
            }

            this.Columns = columns.Select(x => x.Get)?.ToArray();
        }

        public int? Limit { get; private set; }

        public void LimitBy(int limit)
        {
            this.Limit = limit;
        }
        
        /// <summary>
        /// Adds an order to be followed when the result is read.
        /// </summary>
        /// <param name="column">The column where the order will depend.</param>
        /// <param name="order">The given order. May it be ascending or descending.</param>
        public void OrderBy(TableColumn column, Order order)
        {
            if (column == null)
            {
                return;
            }

            if (this.Order.IsEmpty())
            {
                this.Order = $"{column.Get} {order.GetString()}";
            }
            else
            {
                this.Order += $", {column.Get} {order.GetString()}";
            }
        }
        
        private int ColumnCount = 0;
        private string OptionalName = "q_";
        private string Param = ConnectProvider.Param();
        public string Order { get; private set; }
        public string WhereBase { get; private set; } = "";
        public List<string> MultiWhere { get; private set; } = new List<string>();
        public Dictionary<string, object> Parameters { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Sets the criteria for the result when selecting.
        /// </summary>
        /// <param name="column">The column where the criteria will depend.</param>
        /// <param name="condition">The condition of the criteria.</param>
        /// <param name="value">The value to be compared with.</param>
        public void Where(
            TableColumn column,
            Is condition,
            object value)
        {
            this.Where(Operator.AND, column, condition, value);
        }

        public void Where(
            Operator? oper,
            TableColumn column, 
            Is condition, 
            object value)
        {
            if (column == null)
            {
                return;
            }

            if (value == null)
            {
                value = DBNull.Value;
            }

            string columnParameter = this.OptionalName + column.Get + this.ColumnCount;
            string statement = string.Empty;
            
            if (!column.Property.PropertyType.IsArray)
            {
                statement = $"{column.Get} {GetCondition(condition)} {Param}{columnParameter} ";
            }
            else
            {
                statement = $"{Param}{columnParameter} {GetCondition(condition)} ANY({column.Get}) ";
            }

            this.AddWhere(oper, statement);

            var property = typeof(TableEntity).GetProperty(column.ColumnName);
            
            if (column.Property.GetCustomAttribute<EncryptAttribute>() != null &&
                column.Property.PropertyType == typeof(string))
            {
                value = Encryption.Encrypt(Convert.ToString(value), true);
            }

            this.Parameters.Add(columnParameter, value);
            this.ColumnCount++;
        }

        public void Where(string where, object value)
        {
            this.Where(Operator.AND, where, value);
        }

        public void Where(Operator? oper, string where, object value)
        {
            if (value == null)
            {
                value = DBNull.Value;
            }

            var columnParameter = $"{this.OptionalName}custom{this.ColumnCount}";
            var statement = string.Format(where, this.Param + columnParameter) + " ";

            this.AddWhere(oper, statement);
            this.Parameters.Add(columnParameter, value);
            this.ColumnCount++;
        }

        public void Exists<T>(Operator? oper, Schema<T> schema, params Relation[] columnOn) where T : Entity, new()
        {
            this.ExistsBase(oper, schema, false, columnOn);
        }

        public void NotExists<T>(Operator? oper, Schema<T> schema, params Relation[] columnOn) where T : Entity, new()
        {
            this.ExistsBase(oper, schema, true, columnOn);
        }

        private void ExistsBase<T>(Operator? oper, Schema<T> schema, bool notExists, params Relation[] columnOn) where T : Entity, new()
        {
            string existence = notExists ?
                "NOT" :
                string.Empty;

            string customName = "sub";

            var onString = new List<string>();

            foreach (var column in columnOn)
            {
                onString.Add($"{column.Column1.GetCustomName(customName)} {GetCondition(column.Condition ?? Is.EqualTo)} {column.Column2.Get}");
            }
            
            string statement = $"{existence} EXISTS ({Operations.SELECT} 1 FROM {schema.TableName} AS {customName} WHERE {string.Join(", ", onString)})";

            this.AddWhere(oper, statement);
        }

        private void AddWhere(Operator? oper, string statement)
        {
            if (this.WhereBase.IsEmpty())
            {
                this.WhereBase += $"{statement} ";
            }
            else
            {
                this.WhereBase += $"{oper ?? Operator.AND} {statement} ";
            }
        }

        /// <summary>
        /// Ends the above criterias which will be set into a group, and sets a group of criterias.
        /// </summary>
        /// <param name="oper">The operator that'll separate the group above and below.</param>
        public void End(Operator? oper = null)
        {
            this.MultiWhere.Add($"({this.WhereBase}) {oper} ");
            this.WhereBase = string.Empty;
        }
        
        public static string GetCondition(Is condition)
        {
            switch (condition)
            {
                case Is.EqualTo:
                    return "=";
                case Is.NotEqualTo:
                    return "!=";
                case Is.GreaterThan:
                    return ">";
                case Is.LessThan:
                    return "<";
                case Is.GreaterThanEqualTo:
                    return ">=";
                case Is.LessThanEqualTo:
                    return "<=";
                case Is.Like:
                    return "LIKE";
                case Is.NotLike:
                    return "NOT LIKE";
                default:
                    return "=";
            }
        }
    }
}
