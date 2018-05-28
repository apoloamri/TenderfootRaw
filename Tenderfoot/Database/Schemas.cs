using Tenderfoot.Database.Tables;
using Tenderfoot.TfSystem;
using Tenderfoot.Tools;
using Tenderfoot.Tools.Extensions;
using System.Linq;
using System.Threading;
using Tenderfoot.Database.System;
using System.Collections.Generic;

namespace Tenderfoot.Database
{
    public class Schemas
    {
        /// <summary>
        /// Creates the schema table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">Table name for the schema to be created.</param>
        /// <returns></returns>
        public static Schema<T> CreateTable<T>(string tableName) where T : Entity, new()
        {
            if (TfSettings.Database.Migrate == false)
            {
                return new Schema<T>(tableName);
            }

            CreateDB();

            var table = InformationSchemaTables;
            table.Case.Where(table._(x => x.table_name), Is.EqualTo, tableName);
            table.Case.AddColumns(table._(x => x.table_name));

            var newSchema = new Schema<T>(tableName);

            if (table.Count == 0)
            {
                newSchema.CreateTable();
            }
            else
            {
                var columns = InformationSchemaColumns;

                columns.Case.Where(columns._(x => x.table_name), Is.EqualTo, tableName);
                columns.Case.AddColumns(columns._(x => x.column_name));

                var currentColumns = columns.Select.Entities?.Select(x => x.column_name)?.ToList();
                var entityColumns = newSchema.Entity.GetColumns();

                if (CompareLists.UnorderedEqual(entityColumns, currentColumns))
                {
                    return newSchema;
                }

                newSchema.AddTableColumns(entityColumns.MissingItems(currentColumns));

                Thread.Sleep(1000);

                currentColumns = columns.Select.Entities?.Select(x => x.column_name)?.ToList();

                newSchema.DropTableColumns(currentColumns.MissingItems(entityColumns));
            }

            return newSchema;
        }

        private static void CreateDB()
        {
            var connectionString = TfSettings.Database.DefaultConnectionString;
            var dbName = TfSettings.Database.DatabaseName;
            var param = ConnectProvider.Param();
            var query = new Query(
                connectionString,
                $"{Operations.SELECT} COUNT('datname') FROM pg_catalog.pg_database where datname = {param}dbName;",
                new Dictionary<string, object>() { { "dbName", dbName } });

            if (query.GetScalar() > 0)
            {
                return;
            }

            var userId = TfSettings.Database.UserId;
            var encoding = TfSettings.Database.Encoding;
            var nonQuery = new NonQuery(
                connectionString,
                $"{Operations.CREATE_DATABASE.GetString()} {dbName} WITH OWNER = {userId} ENCODING = '{encoding}';",
                null);
            nonQuery.ExecuteNonQuery(Operations.CREATE_DATABASE);
        }
        
        private static Schema<InformationSchema> InformationSchemaTables => new Schema<InformationSchema>("information_schema.tables");
        private static Schema<InformationSchema> InformationSchemaColumns => new Schema<InformationSchema>("information_schema.columns");
        public static Schema<Accesses> Accesses => CreateTable<Accesses>("accesses");
        public static Schema<SystemLogs> SystemLogs => CreateTable<SystemLogs>("system_logs");
        public static Schema<Emails> Emails => CreateTable<Emails>("emails");
        public static Schema<Sessions> Sessions => CreateTable<Sessions>("sessions");
        public static Schema<SetupChanges> SetupChanges => CreateTable<SetupChanges>("setup_changes");
    }
}
