using System.Collections.Generic;
using System.IO;

namespace QuoteParser
{
    internal static class StringExtensions
    {
        public static IEnumerable<string> Lines(this string input)
        {
            using (var sr = new StringReader(input))
            {
                string line;
                while (null != (line = sr.ReadLine()))
                    yield return line;
            }
        }

        public static bool IsEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }
    }
}
