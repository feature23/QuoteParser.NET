using System.Collections.Generic;
using QuoteParser.Features;

namespace QuoteParser
{
    internal class QuoteMarkParser
    {
        private int _maxQuoteBlocksCount;
        private int _minimumQuoteBlockSize;

        public QuoteMarkParser()
            : this(3, 7)
        {
        }

        public QuoteMarkParser(int maxQuoteBlocksCount, int minimumQuoteBlockSize)
        {
            _maxQuoteBlocksCount = maxQuoteBlocksCount;
            _minimumQuoteBlockSize = minimumQuoteBlockSize;
        }

        public int? Parse(IList<string> lines)
            => Parse(lines, new QuoteMarkFeature().MatchLines(lines));

        public int? Parse(IList<string> lines, 
            IList<QuoteMarkMatchingResult> matchingLines)
        {
            int quoteBlocksCount = GetQuoteBlocksCount(matchingLines);
            if (quoteBlocksCount > _maxQuoteBlocksCount)
                return null;

            int startQuotedBlockIndex;
            int endQuotedBlockIndex = matchingLines.Count - 1;

            while (endQuotedBlockIndex > -1 &&
                   !matchingLines[endQuotedBlockIndex].HasQuoteMark)
            {
                endQuotedBlockIndex--;
            }

            int quoteMarksCount = 0;
            if (endQuotedBlockIndex == -1)
            {
                return null;
            }
            else
            {
                int matchingQuoteMarkIndex = endQuotedBlockIndex;
                int lineIndex = endQuotedBlockIndex;

                while (lineIndex > -1)
                {
                    if (matchingLines[lineIndex].HasQuoteMark)
                    {
                        quoteMarksCount++;
                        matchingQuoteMarkIndex = lineIndex;
                    }

                    if (matchingLines[lineIndex] == QuoteMarkMatchingResult.NotEmpty)
                    {
                        break;
                    }

                    lineIndex--;
                }

                startQuotedBlockIndex = matchingQuoteMarkIndex;
            }

            return (quoteMarksCount < _minimumQuoteBlockSize)
                ? null
                : (int?)startQuotedBlockIndex;
        }

        private int GetQuoteBlocksCount(IList<QuoteMarkMatchingResult> matchingLines)
        {
            int from = matchingLines.IndexOfFirst(it => it.HasQuoteMark);
            int to = matchingLines.IndexOfLast(it => it.HasQuoteMark);

            if (from == -1 || to == -1)
            {
                return 0;
            }

            int quoteBlocksCount = 0;
            foreach (int i in RangeHelper.Range(from + 1, to))
            {
                if (matchingLines[i - 1].HasQuoteMark &&
                    matchingLines[i].IsTextWithoutQuoteMark)
                {
                    quoteBlocksCount++;
                }
            }

            return quoteBlocksCount + 1;
        }
    }
}