using System.Text.RegularExpressions;

namespace QuoteParser.Features
{
    internal class EmailFeature : AbstractQuoteFeature
    {
        public override string Name => "EMAIL";

        protected override Regex GetRegex()
        {
            // Full regex for testing needs
            //var regex = "(.*[\\s\\p{C}\\p{Z}>])?\\S+@\\S+([\\p{C}\\p{Z}\\s].*)?";

            // @ symbol surrounded with at least one non-whitespace symbol.
            return new Regex($"{_startWhitespaceOptional}\\S+@\\S+{_endWhitespaceOptional}");
        }
    }
}
