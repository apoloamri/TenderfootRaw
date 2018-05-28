using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Tenderfoot.Database.System
{
    public class Query : Connect
    {
        public Query(string connectionString, string sql, Dictionary<string, object> parameters) : base(sql, parameters, connectionString) { }

        public Query(string sql, Dictionary<string, object> parameters) : base(sql, parameters) { }

        public List<DataRow> GetListDataRow()
        {
            this.SqlConnection.Open();
            this.SqlDataReader = this.SqlCommand.ExecuteReader();

            DataTable dataTable = new DataTable();

            dataTable.Load(this.SqlDataReader);

            this.SqlConnection.Close();

            return dataTable?.Select()?.ToList();
        }

        public List<Dictionary<string, object>> GetListDictionary()
        {
            var data = this.GetListDataRow();

            return data.Select(item => {
                return item?
                    .Table?
                    .Columns?
                    .Cast<DataColumn>()?
                    .ToDictionary(x => x.ColumnName, x => item[x]);
            })?.ToList();
        }
        
        public long GetScalar()
        {
            this.SqlConnection.Open();

            var scalar = Convert.ToInt64(this.SqlCommand.ExecuteScalar());
            
            this.SqlConnection.Close();

            return scalar;
        }
    }
}
