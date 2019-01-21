using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace QuoteParser.Features
{
    internal class MiddleColonFeature : AbstractQuoteFeature
    {
        public override string Name => "MIDDLE_COLON";

        protected override Regex GetRegex()
        {
            // Full date regex for testing needs.
            //var regex = "[\\s\\p{C}\\p{Z}>]*\\S+[\\p{C}\\p{Z}\\s]*:[\\p{C}\\p{Z}\\s]+\\S+.*";

            // This regex matches the colon after any word except the last one with optional
            // whitespace before the colon and obligatory whitespace after the colon.
            return new Regex($"^[\\s\\p{{C}}\\p{{Z}}>]*\\S+{_whitespace}*:{_whitespace}+\\S+.*");
        }

        public static bool CheckMiddleColonSuggestion(int startIdx, int endIdx, IList<string> lines,
            MiddleColonFeature middleColonFeature)
        {
            return lines.SubList(startIdx, endIdx + 1).Any(it => middleColonFeature.Matches(it));
        }
    }
}
