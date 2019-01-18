using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace QuoteParser.Features
{
    public class QuoteMarkMatchingResult
    {
        public static readonly QuoteMarkMatchingResult VEmpty = new QuoteMarkMatchingResult();
        public static readonly QuoteMarkMatchingResult VNotEmpty = new QuoteMarkMatchingResult();
        public static readonly QuoteMarkMatchingResult Empty = new QuoteMarkMatchingResult();
        public static readonly QuoteMarkMatchingResult NotEmpty = new QuoteMarkMatchingResult();

        public bool HasQuoteMark => this == VEmpty || this == VNotEmpty;
        public bool IsTextWithoutQuoteMark => this == NotEmpty;
    }

    public class QuoteMarkFeature : AbstractQuoteFeature
    {
        public override string Name => "QUOTE_MARK";

        protected override Regex GetRegex()
        {
            // Regex for matching greater-than(>) symbol in the beginning of the line.
            return new Regex($"{_whitespace}*>.*");
        }

        public IList<QuoteMarkMatchingResult> MatchLines(IList<string> lines)
        {
            return lines.Select(it =>
                {
                    bool matchesQuoteMark = Matches(it);
                    bool containText;

                    if (matchesQuoteMark)
                    {
                        int quoteMarkIndex = it.IndexOf('>');
                        if (quoteMarkIndex == -1)
                        {
                            throw new InvalidOperationException($"Line \"{it}\" must contain '>' symbol, but it is not.");
                        }

                        containText = !it.Substring(quoteMarkIndex + 1).Trim().IsEmpty();
                    }
                    else
                    {
                        containText = !it.Trim().IsEmpty();
                    }

                    if (matchesQuoteMark && containText) return QuoteMarkMatchingResult.VNotEmpty;
                    if (matchesQuoteMark) return QuoteMarkMatchingResult.VEmpty;
                    if (containText) return QuoteMarkMatchingResult.NotEmpty;
                    return QuoteMarkMatchingResult.Empty;
                })
                .ToList();
        }

        internal static bool CheckQuoteMarkSuggestion(int endIdx, IList<string> lines,
            IList<QuoteMarkMatchingResult> matchedLinesQuoteMark)
        {
            int idx = endIdx + 1;

            while (idx < lines.Count)
            {
                if (matchedLinesQuoteMark[idx].IsTextWithoutQuoteMark)
                    return false;
                if (matchedLinesQuoteMark[idx].HasQuoteMark)
                    return true;
                idx++;
            }

            return false;
        }
    }
}
