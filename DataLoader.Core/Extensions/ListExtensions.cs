using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader.Core.Extensions
{
  public static  class ListExtensions
    {
        public static string ToLines<T>(this List<T> list, string prefix, Func<T, string> body, string delimiter,
            string suffix)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(prefix);


            for (int i = 0; i < list.Count - 1; i++)
                sb.AppendLine($"{body(list[i])}{delimiter}");

            sb.AppendLine($"{body(list.Last())}{suffix}");
            return sb.ToString().Trim();
        }
    }
}
