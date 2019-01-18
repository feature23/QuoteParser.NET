using System.Text.RegularExpressions;

namespace QuoteParser.Features
{
    internal class DateFeature : AbstractQuoteFeature
    {
        public override string Name => "DATE";

        protected override Regex GetRegex()
        {
            // Short date format starting with the day number (e.g. 15-02-2016)
            var shortDateForward = "[0-3]?[0-9]\\p{C}*[/.-]\\p{C}*[0-3]?[0-9]\\p{C}*[/.-]\\p{C}*(20)?[0-9]{2}";
            
            // Short date format starting with the year (2016-02-15)
            var shortDateReversed = "(20)?[0-9]{2}\\p{C}*[/.-]\\p{C}*[0-3]?[0-9]\\p{C}*[/.-]\\p{C}*[0-3]?[0-9]";
            var shortDate = $"(({shortDateForward})|({shortDateReversed}))\\p{{C}}*[,\\.]{{0,2}}";

            // Full date format with up to three possible arbitrary words 
            // between number of the day and year. Number of the day goes first
            // (e.g. 15 Feb 2016; 15 Thu, Feb 2016, 15 x y z 2016).
            var fullDateForward = $"([0-3]?[0-9]\\p{{C}}*[\\.,]{{0,2}}{_whitespace}+)" +
                              $"(\\S+{_whitespace}+){{0,3}}(20\\d\\d\\p{{C}}*[\\.,]{{0,2}})";

            // The same as previous but year and number of the day are swapped.
            var fullDateReversed = $"(20\\d\\d\\p{{C}}*[\\.,]{{0,2}}{_whitespace}+)" +
                               $"(\\S+{_whitespace}+){{0,3}}([0-3]?[0-9]\\p{{C}}*[\\.,]{{0,2}})";
            var fullDate = $"({fullDateForward})|({fullDateReversed})";

            // Final date regex. Colon(:) is possible before the date, 
            // arbitrary bracket is possible after the date.
            var date = $"(.*[\\p{{C}}\\p{{Z}}\\s:>])?(({shortDate})|({fullDate})){_endBracketsOptional}" +
                   _endWhitespaceOptional;

            // Full date regex for testing needs.
            //var regex = "(.*[\\p{C}\\p{Z}\\s:>])?(((([0-3]?[0-9]\\p{C}*[/.-]\\p{C}*[0-3]?[0-9]\\p{C}*[/.-]\\p{C}*(20)?[0-9]{2})|((20)?[0-9]{2}\\p{C}*[/.-]\\p{C}*[0-3]?[0-9]\\p{C}*[/.-]\\p{C}*[0-3]?[0-9]))\\p{C}*[,\\.]{0,2})|((([0-3]?[0-9]\\p{C}*[\\.,]{0,2}[\\p{C}\\p{Z}\\s]+)(\\S+[\\p{C}\\p{Z}\\s]+){0,3}(20\\d\\d\\p{C}*[\\.,]{0,2}))|((20\\d\\d\\p{C}*[\\.,]{0,2}[\\p{C}\\p{Z}\\s]+)(\\S+[\\p{C}\\p{Z}\\s]+){0,3}([0-3]?[0-9]\\p{C}*[\\.,]{0,2}))))\\p{C}*[\\.,}\\]>\\*\\):\"'`\\|\\\\/~;]?\\p{C}*([\\p{C}\\p{Z}\\s].*)?";

            return new Regex(date);
        }
    }
}
