using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader.Core.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToFormatedString(this TimeSpan span)
        {
            var parts = new List<string>();

            if (span.Hours > 0)
                parts.Add($"{span.Hours} Hours");

            if (span.Minutes > 0)
                parts.Add($"{span.Minutes} Minutes");

            if (span.Seconds > 0)
                parts.Add($"{span.Seconds} Seconds");

            if (parts.Count > 0)
                return string.Join(" ", parts);

            if (span.Milliseconds > 0)
                return $"{span.Milliseconds} Milliseconds";

            return $"{span.TotalMilliseconds} Milliseconds";
        }
    }
}
