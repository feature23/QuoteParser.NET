using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using QuoteParser.Features;
using static QuoteParser.Parse;
using static QuoteParser.Features.QuoteMarkFeature;
using KeyPhrasesClass = QuoteParser.Features.KeyPhrases;

namespace QuoteParser
{
    public class QuoteParser
    {
        private static Relation GetRelation(int startHeaderLinesIndex, int endHeaderLinesIndex, int quoteMarkIndex)
        {
            if (quoteMarkIndex < startHeaderLinesIndex)
                return Relation.QuoteMarkFirst;
            if (quoteMarkIndex > endHeaderLinesIndex)
                return Relation.HeaderLinesFirst;
            return Relation.QuoteMarkInHeaderLines;
        }

        private static bool IsTextBetween(int startIndex, int endIndex, IList<QuoteMarkMatchingResult> matchingLines)
        {
            return RangeHelper.Range(startIndex + 1, endIndex - 1).Any(i => matchingLines[i] == QuoteMarkMatchingResult.NotEmpty);
        }

        private static bool IsQuotedTextBetween(int startIndex, int endIndex, IList<QuoteMarkMatchingResult> matchingLines)
        {
            return RangeHelper.Range(startIndex + 1, endIndex - 1).Any(i => matchingLines[i] == QuoteMarkMatchingResult.VNotEmpty);
        }

        private static bool IsQuoteMarksAroundHeaderLines(int startHeaderLinesIndex,
            int endHeaderLinesIndex,
            int quoteMarkIndex,
            IList<QuoteMarkMatchingResult> matchingLines)
        {
            bool headerLinesContainsQuoteMarks = RangeHelper.Range(startHeaderLinesIndex, endHeaderLinesIndex).All(i => matchingLines[i].HasQuoteMark);

            if (headerLinesContainsQuoteMarks)
                return true;

            foreach (int i in RangeHelper.Range(endHeaderLinesIndex + 1, quoteMarkIndex))
            {
                if (matchingLines[i].HasQuoteMark)
                    return true;
                if (matchingLines[i] == QuoteMarkMatchingResult.NotEmpty)
                    return false;
            }

            return true;
        }

        private readonly Builder _builder;
        
        private readonly bool _deleteQuoteMarks;
        private readonly bool _recursive;

        private readonly QuoteMarkFeature _quoteMarkFeature;
        private readonly QuoteHeaderLinesParser _quoteHeaderLinesParser;
        private readonly QuoteMarkParser _quoteMarkParser;

        private IList<string> _lines = new List<string>();

        public class Builder
        {
            internal int _headerLinesCount = 3;
            internal int _multiLineHeaderLinesCount = 6;
            internal int _maxQuoteBlocksCount = 3;
            internal int _minimumQuoteBlockSize = 7;
            internal bool _deleteQuoteMarks = true;
            internal bool _recursive;
            internal IList<string> _keyPhrases = KeyPhrasesClass.Default;

            public Builder HeaderLinesCount(int value)
            {
                _headerLinesCount = value;
                return this;
            }

            public Builder MultiLineHeaderLinesCount(int value)
            {
                _multiLineHeaderLinesCount = value;
                return this;
            }

            public Builder MinimumQuoteBlockSize(int value)
            {
                _minimumQuoteBlockSize = value;
                return this;
            }

            public Builder DeleteQuoteMarks(bool value)
            {
                _deleteQuoteMarks = value;
                return this;
            }

            public Builder Recursive(bool value)
            {
                _recursive = value;
                return this;
            }

            public Builder KeyPhrases(IList<string> value)
            {
                _keyPhrases = value;
                return this;
            }

            public QuoteParser Build()
            {
                return new QuoteParser(this);
            }
        }

        private QuoteParser(Builder builder)
        {
            _builder = builder;

            _deleteQuoteMarks = builder._deleteQuoteMarks;
            _recursive = builder._recursive;

            if (!_deleteQuoteMarks && _recursive)
                throw new InvalidOperationException("Can't perform recursive parsing without deleting '>'");

            _quoteMarkFeature = new QuoteMarkFeature();
            _quoteHeaderLinesParser = new QuoteHeaderLinesParser(
                builder._headerLinesCount,
                builder._multiLineHeaderLinesCount,
                builder._keyPhrases
            );

            _quoteMarkParser = new QuoteMarkParser(
                builder._maxQuoteBlocksCount,
                builder._minimumQuoteBlockSize
            );
        }

        public Content Parse(Stream emlFile)
        {
            var msg = GetMimeMessage(emlFile);
            string emailText = GetEmailText(msg);
            return Parse(emailText.Lines(), ContainInReplyToHeader(msg));
        }

        public Content Parse(IEnumerable<string> lines, bool hasInReplyToEmlHeader = true)
        {
            _lines = lines.ToList();

            var matchingLines = _quoteMarkFeature.MatchLines(_lines);
            var headerLinesIndexes = _quoteHeaderLinesParser.Parse(_lines, matchingLines);

            // This condition means: for EMLs without In-Reply-To header or References header
            // search for quotation marks(>) only if quoteHeaderLines is null. It works well for
            // the test data, but it is weird.. because it skips quotation marks most of the time.
            //
            // As alternative this condition may be deleted, but it works
            // worse with some cases...
            var quoteMarkIndex = (!hasInReplyToEmlHeader && headerLinesIndexes != null)
                ? null
                : _quoteMarkParser.Parse(_lines, matchingLines);

            if (headerLinesIndexes == null && quoteMarkIndex == null)
                return new Content(_lines, null, null);
            if (headerLinesIndexes != null && quoteMarkIndex == null)
                return CreateContent(headerLinesIndexes.Value.Item1, headerLinesIndexes.Value.Item2 + 1, matchingLines);
            if (headerLinesIndexes == null)
                return CreateContent(quoteMarkIndex.Value, quoteMarkIndex.Value, matchingLines);

            int startHeaderLinesIndex = headerLinesIndexes.Value.Item1;
            int endHeaderLineIndex = headerLinesIndexes.Value.Item2;

            var relation = GetRelation(
                startHeaderLinesIndex, 
                endHeaderLineIndex, 
                quoteMarkIndex.Value
            );

            switch (relation)
            {
                case Relation.QuoteMarkInHeaderLines:
                    return CreateContent(
                        startHeaderLinesIndex,
                        endHeaderLineIndex + 1,
                        matchingLines
                    );
                case Relation.QuoteMarkFirst:
                    return GetContentQuoteMarkFirstCase(
                        startHeaderLinesIndex,
                        endHeaderLineIndex,
                        quoteMarkIndex.Value,
                        matchingLines
                    );
                case Relation.HeaderLinesFirst:
                    return GetContentHeaderLinesFirstCase(
                        startHeaderLinesIndex,
                        endHeaderLineIndex,
                        quoteMarkIndex.Value,
                        matchingLines
                    );
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Content GetContentHeaderLinesFirstCase(int startHeaderLinesIndex, int endHeaderLineIndex, int quoteMarkIndex,
            IList<QuoteMarkMatchingResult> matchingLines)
        {
            bool isTextBetween = IsTextBetween(endHeaderLineIndex, quoteMarkIndex, matchingLines);
            bool isQuoteMarksAroundHeaderLines = IsQuoteMarksAroundHeaderLines(
                startHeaderLinesIndex,
                endHeaderLineIndex,
                quoteMarkIndex,
                matchingLines
            );

            if (!isTextBetween && !isQuoteMarksAroundHeaderLines)
            {
                throw new InvalidOperationException($"Relation = {Relation.HeaderLinesFirst}. Both isTextBetween and isHeaderLinesHasQuoteMark are false, but it can't be!");
            }

            if (isTextBetween && isQuoteMarksAroundHeaderLines)
            {
                return CreateContent(
                    quoteMarkIndex,
                    quoteMarkIndex,
                    matchingLines
                );
            }
            else
            {
                return CreateContent(startHeaderLinesIndex, endHeaderLineIndex + 1, matchingLines);
            }
        }

        private Content GetContentQuoteMarkFirstCase(int startHeaderLinesIndex, int endHeaderLineIndex, int quoteMarkIndex,
            IList<QuoteMarkMatchingResult> matchingLines)
        {
            bool isTextBetween = IsTextBetween(quoteMarkIndex, startHeaderLinesIndex, matchingLines);
            bool isQuotedTextBetween = IsQuotedTextBetween(quoteMarkIndex, startHeaderLinesIndex, matchingLines);

            if (isTextBetween)
            {
                int i = startHeaderLinesIndex;
                while (i > 0 && matchingLines[i - 1] != QuoteMarkMatchingResult.NotEmpty)
                    --i;
                if (i == 0 || matchingLines.SubList(i, _lines.Count).Any(it => it.HasQuoteMark))
                {
                    throw new InvalidOperationException($"Relation = {Relation.QuoteMarkFirst}. Found quoteMark(>), but in the lines after {i - 1} must not be any quote mark!");
                }

                return CreateContent(startHeaderLinesIndex, endHeaderLineIndex + 1, matchingLines);
            }
            else if (isQuotedTextBetween)
            {
                return CreateContent(
                    quoteMarkIndex,
                    quoteMarkIndex,
                    matchingLines
                );
            }

            int startIndex = startHeaderLinesIndex;
            if (matchingLines.SubList(startHeaderLinesIndex, endHeaderLineIndex + 1).All(it => it.HasQuoteMark))
            {
                while (startIndex > 0 && matchingLines[startIndex - 1].HasQuoteMark)
                    --startIndex;
            }

            return CreateContent(startIndex, endHeaderLineIndex + 1, matchingLines);
        }

        private Content CreateContent(int fromIndex, 
            int toIndex, 
            IList<QuoteMarkMatchingResult> matchingLines)
        {
            DeleteQuoteMarks(fromIndex, toIndex, matchingLines);

            if (!_recursive)
            {
                return new Content(
                    _lines.SubList(0, fromIndex),
                    new QuoteHeader(fromIndex, toIndex, _lines.SubList(fromIndex, toIndex)),
                    new Content(
                        _lines.SubList(toIndex, _lines.LastIndex() - 1),
                        null,
                        null
                    )
                );
            }
            else
            {
                return new Content(
                    _lines.SubList(0, fromIndex),
                    new QuoteHeader(fromIndex, toIndex, _lines.SubList(fromIndex, toIndex)),
                    Parse(_lines.SubList(toIndex, _lines.LastIndex() + 1))
                );
            }
        }
        
        private void DeleteQuoteMarks(int fromIndex,
            int toIndex,
            IList<QuoteMarkMatchingResult> matchingLines)
        {
            if (_deleteQuoteMarks &&
                (fromIndex == toIndex ||
                 CheckQuoteMarkSuggestion(toIndex, _lines, matchingLines)))
            {
                _lines = _lines.Select((s, i) =>
                {
                    string line = s.TrimStart();
                    if (i >= fromIndex)
                    {
                        if (line.StartsWith("> ")) return line.Substring(2);
                        if (line.StartsWith(">")) return line.Substring(1);
                        return s;
                    }
                    else
                    {
                        return s;
                    }
                }).ToList();
            }
        }
    }
}
