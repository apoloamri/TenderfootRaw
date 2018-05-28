using Npgsql;
using System;
using System.Collections.Generic;
using Tenderfoot.TfSystem;
using Tenderfoot.TfSystem.Diagnostics;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.Database.System
{
    public class Connect
    {
        public dynamic SqlConnection { get; set; } = null;
        public dynamic SqlCommand { get; set; } = null;
        public dynamic SqlDataReader { get; set; } = null;
        public dynamic SqlTransaction { get; set; } = null;

        public Connect(string sql, Dictionary<string, object> parameters, string connectionString)
        {
            this.SqlConnection = ConnectProvider.SqlConnection(connectionString);
            this.WriteSql(sql, parameters);
        }

        public Connect(string sql, Dictionary<string, object> parameters)
        {
            this.SqlConnection = ConnectProvider.SqlConnection();
            this.WriteSql(sql, parameters);
        }

        public Connect()
        {
            this.SqlConnection = ConnectProvider.SqlConnection();
        }

        public void WriteSql(string sql, Dictionary<string, object> parameters)
        {
            if (sql.IsEmpty())
            {
                throw new TfException("SQL not provided.");
            }

            if (this.SqlCommand == null)
            {
                this.SqlCommand = this.SqlConnection.CreateCommand();
            }

            TfDebug.WriteLine("Executing query", sql);
            
            this.SqlCommand.CommandText = sql;

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    this.SqlCommand.Parameters.Add(ConnectProvider.SqlParameter(parameter.Key, parameter.Value));
                }

                TfDebug.WriteLine("SQL Parameters", string.Join(Environment.NewLine, parameters));
            }
        }
    }

    public static class ConnectProvider
    {
        public static string Param()
        {
            return ":";
        }

        public static NpgsqlConnection SqlConnection(string connection = null)
        {
            if (connection.IsEmpty())
            {
                connection = TfSettings.Database.ConnectionString;
            }
            
            TfDebug.WriteLine("Connecting database", connection);

            return new NpgsqlConnection(connection);
        }

        public static NpgsqlParameter SqlParameter(string key, object value)
        {
            return new NpgsqlParameter(key, value);
        }
    }
}
