using System.Text.RegularExpressions;

namespace QuoteParser.Features
{
    public abstract class AbstractQuoteFeature
    {
        protected const string _whitespace = "[\\p{C}\\p{Z}\\s]";
        protected const string _startWhitespaceOptional = "(.*[\\s\\p{C}\\p{Z}>])?";
        protected const string _endWhitespaceOptional = "(\\p{C}\\p{Z}\\s].*)?";
        protected const string _startBracketsOptional = "\\p{C}*[\\.,\\{\\[<\\*\\(:\"'`\\|\\\\/~]?\\p{C}*";
        protected const string _endBracketsOptional = "\\p{C}*[\\.,}\\]>\\*\\):\"'`\\|\\\\/~;]?\\p{C}*";

        public abstract string Name { get; }

        protected abstract Regex GetRegex();

        public virtual bool Matches(string line) => GetRegex().IsMatch(line);
    }
}
