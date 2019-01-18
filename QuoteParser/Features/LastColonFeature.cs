using System.Text.RegularExpressions;

namespace QuoteParser.Features
{
    internal class LastColonFeature : AbstractQuoteFeature
    {
        public override string Name => "LAST_COLON";

        protected override Regex GetRegex()
        {
            // Full regex for testing needs
            //var regex = "(.*[\\s\\p{C}\\p{Z}>])?.*:[\\p{C}\\p{Z}\\s]*";

            // Regex for matching colon(:) in the end of the line.
            return new Regex($"{_startWhitespaceOptional}.*:{_whitespace}*$");
        }
    }
}
