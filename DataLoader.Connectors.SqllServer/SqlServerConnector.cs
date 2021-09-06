using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DataLoader.Connectors.SqllServer.Extensions;

namespace DataLoader.Connectors.SqllServer
{
    public class SqlServerConnector
    {
        public SqlServerConnector(SqlServerConfig config)
        {
            connectionString = config.ConnectionString;
        }

        public string GenerateSql(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            var sql = dt.GenerateSqlServerTable();
            sb.AppendLine(sql);
            sb.AppendLine("");
            sb.AppendLine("Go");
            sb.AppendLine("");

            var insertProcSql = dt.GenerateSqlServerInsertProcedure();
            sb.AppendLine(insertProcSql);
            return sb.ToString();
        }

        private string connectionString;

        public void TransferData(DataTable dt)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = $"Insert{dt.TableName}";
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.DataType == typeof(int))
                            cmd.Parameters.Add(new SqlParameter($"@{col.ColumnName}", SqlDbType.Int));

                        if (col.DataType == typeof(decimal))
                            cmd.Parameters.Add($"@{col.ColumnName}", SqlDbType.Decimal, 5);

                        if (col.DataType == typeof(string))
                        {
                            var max = (from DataRow d in dt.Rows
                                where d[col.Ordinal].GetType() != typeof(DBNull)
                                select ((string)d[col.Ordinal]).Length).Max();
                            cmd.Parameters.Add(new SqlParameter($"@{col.ColumnName}", SqlDbType.VarChar));
                        }
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (DataColumn col in dt.Columns)
                            cmd.Parameters[$"@{col.ColumnName}"].Value = row[col.ColumnName];

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
