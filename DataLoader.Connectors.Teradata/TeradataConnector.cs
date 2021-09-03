using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLoader.Connectors.Teradata.Extensions;
using Teradata.Client.Provider;


namespace DataLoader.Connectors.Teradata
{
    public class TeradataConnector
    {
        public TeradataConnector(TeradataConfig config)
        {
            TdConnectionStringBuilder connectionStringBuilder = new TdConnectionStringBuilder();
            connectionStringBuilder.DataSource = config.DataSource;
            connectionStringBuilder.Database = config.Database;
            connectionStringBuilder.UserId = config.UserId;
            connectionStringBuilder.Password = config.Password;
            if (!String.IsNullOrWhiteSpace(config.AuthenticationMechanism))
                connectionStringBuilder.AuthenticationMechanism = config.AuthenticationMechanism;

            ConnectionString = connectionStringBuilder.ConnectionString;

        }

        public TeradataConnector(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string  ConnectionString { get; set; }


        public DataTable SelectData(string tableName, string query)
        {
            using (TdConnection cn = new TdConnection())
            {
                cn.ConnectionString = ConnectionString ;
                cn.Open();

                TdCommand cmd = cn.CreateCommand();
                cmd.CommandText = query;

                using (TdDataReader reader = cmd.ExecuteReader())
                {
                   DataTable dt = reader.ToDataTable();
                   dt.TableName = tableName;
                   return dt;
                }
            }
        }
    }


  
}
