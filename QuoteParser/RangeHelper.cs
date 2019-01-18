using System.Collections.Generic;

namespace QuoteParser
{
    internal static class RangeHelper
    {
        public static IEnumerable<int> Range(int startInclusive, int endInclusive)
        {
            for (int i = startInclusive; i <= endInclusive; i++)
                yield return i;
        }
    }
}
