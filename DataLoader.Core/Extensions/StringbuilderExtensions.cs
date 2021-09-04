using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader.Core.Extensions
{

    public static class StringbuilderExtensions
    {
        public static string AppendLine(this StringBuilder sb, int indent, string text)
        {
            var lines = text.Lines();
            var space = " ";
            foreach (var line in lines)
                sb.AppendLine($"{space.Repeat(indent)}{line}");

            return sb.ToString();
        }
    }
}

