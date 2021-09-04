using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader.Core.Extensions
{
  public static  class StringExtensions
    {
        public static List<string> Lines(this string text)
        {
            var lines = new List<string>();

            using (var sr = new StringReader(text))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    lines.Add(line);
            }
            return lines;
        }

        public static string Repeat(this string text, int repetitions)
        {
            if (repetitions > 1)
            {
                var sb = new StringBuilder(text);
                for (int i = 0; i < repetitions; i++)
                {
                    sb.Append(text);
                }
                return sb.ToString();
            }
            return text;

        }

    }
}
