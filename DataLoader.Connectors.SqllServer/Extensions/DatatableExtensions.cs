using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLoader.Core.Extensions;

namespace DataLoader.Connectors.SqllServer.Extensions
{
   public static  class DatatableExtensions
    {
        public static string GenerateSqlServerTable(this DataTable dt)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE {dt.TableName}");

            var columns = dt.Columns.Cast<DataColumn>().ToList();

            sb.AppendLine(2, columns.ToLines("(", c => $"{c.ColumnName} {dt.SqlServerColumnDataType(c.Ordinal)}", ",", ")"));
               
            return sb.ToString();
        }

        private static string SqlServerColumnDataType(this DataTable dt, int pos)
        {
            var column = dt.Columns[pos];
            if (column.DataType == typeof(int))
                return "int";

            if (column.DataType == typeof(string))
            {
                var max = (from DataRow d in dt.Rows
                    where d[pos].GetType() != typeof(DBNull)
                    select ((string)d[pos]).Length).Max();

                return $"varchar({max})";
            }

            return "";
        }



        public static string GenerateSqlServerInsertProcedure(this DataTable dt)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"CREATE Procedure Insert{dt.TableName} ");
            var columns = dt.Columns.Cast<DataColumn>().ToList();

            sb.AppendLine(2, columns.ToLines("(", c => $"@{c.ColumnName} {dt.SqlServerColumnDataType(c.Ordinal)}", ",", ")"));

            sb.AppendLine($"As");
            sb.AppendLine(2, $"insert into {dt.TableName}");
            sb.AppendLine(4, columns.ToLines("(", c => c.ColumnName, ",", ")"));
            sb.AppendLine(2, "Values");
            sb.AppendLine(4, columns.ToLines("(", c => $"@{c.ColumnName}", ",", ")"));

            return sb.ToString();
        }
    }
}
