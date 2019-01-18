using System.Text.RegularExpressions;

namespace QuoteParser.Features
{
    internal class TimeFeature : AbstractQuoteFeature
    {
        public override string Name => "TIME";

        protected override Regex GetRegex()
        {
            var hhmm = "([01]?[0-9]|2[0-3])\\p{C}*:\\p{C}*([0-5][0-9])";

            var sec = "\\p{C}*:\\p{C}*[0-5][0-9]";

            var ampm = "\\p{C}*[aApP]\\p{C}*[,\\.]{0,2}\\p{C}*[mM]\\p{C}*[,\\.]{0,2}";

            // Time regex with optional seconds and meridian. Could be surrounded with brackets. 
            var time = $"{_startWhitespaceOptional}({_startBracketsOptional}" +
                       $"({hhmm})({sec})?({ampm})?" +
                       $"{_endBracketsOptional}){_endWhitespaceOptional}";

            // Full time regex for testing needs.
            //var regex = "(.*[\\s\\p{C}\\p{Z}>])?(\\p{C}*[\\.,\\{\\[<\\*\\(:\"'`\\|\\\\/~]?\\p{C}*(([01]?[0-9]|2[0-3])\\p{C}*:\\p{C}*([0-5][0-9]))(\\p{C}*:\\p{C}*[0-5][0-9])?(\\p{C}*[aApP]\\p{C}*[,\\.]{0,2}\\p{C}*[mM]\\p{C}*[,\\.]{0,2})?\\p{C}*[\\.,}\\]>\\*\\):\"'`\\|\\\\/~;]?\\p{C}*)([\\s\\p{C}\\p{Z}].*)?";

            return new Regex(time);
        }
    }
}
