using System;
using System.Collections.Generic;
using System.Data;
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
    }
}
