using System;
using System.Collections.Generic;
using System.Linq;
using QuoteParser.Features;
using static QuoteParser.Features.QuoteMarkFeature;
using static QuoteParser.Features.MiddleColonFeature;

namespace QuoteParser
{
    internal class QuoteHeaderLinesParser
    {
        private int _headerLinesCount;
        private int _multiLineHeaderLinesCount;
        private IList<string> _keyPhrases;

        // For single line headers
        private readonly AbstractQuoteFeature[] _featureSet;
        private readonly int _sufficientFeatureCount;
        private readonly int _maxFeatureCount;
        private IDictionary<string, int> _foundFeatureMap = new Dictionary<string, int>();

        // For multi line and FWD headers
        private int _middleColonCount;
        private readonly MiddleColonFeature _middleColonFeature = new MiddleColonFeature();
        private bool _lineMatchesMiddleColon;
        private int _firstMiddleColonLineIndex = -1;

        // For phraseFeature
        private bool _foundPhraseFeature;
        private int _phraseFeatureLineIndex = -1;
        private readonly PhraseFeature _phraseFeature;

        private IList<string> _lines = new List<string>();

        public QuoteHeaderLinesParser()
            : this(3, 6, KeyPhrases.Default)
        {
        }

        public QuoteHeaderLinesParser(int headerLinesCount, int multiLineHeaderLinesCount, IList<string> keyPhrases)
        {
            _headerLinesCount = headerLinesCount;
            _multiLineHeaderLinesCount = multiLineHeaderLinesCount;
            _keyPhrases = keyPhrases;

            _phraseFeature = new PhraseFeature(keyPhrases);
            _featureSet = new AbstractQuoteFeature[]
            {
                new DateFeature(), 
                new TimeFeature(), 
                new EmailFeature(), 
                new LastColonFeature(),
            };
            _maxFeatureCount = _featureSet.Length;

            _sufficientFeatureCount = 2;
        }

        private void Prepare()
        {
            _foundFeatureMap.Clear();

            _middleColonCount = 0;
            _lineMatchesMiddleColon = false;
            _firstMiddleColonLineIndex = -1;

            _foundPhraseFeature = false;
            _phraseFeatureLineIndex = -1;
        }

        internal ValueTuple<int, int>? Parse(IList<string> lines)
            => Parse(lines, new QuoteMarkFeature().MatchLines(lines));

        internal ValueTuple<int, int>? Parse(IList<string> lines, 
            IList<QuoteMarkMatchingResult> matchedLinesQuoteMark)
        {
            Prepare();
            _lines = lines;

            for (int lineIndex = 0; lineIndex < lines.Count; lineIndex++)
            {
                string line = lines[lineIndex];

                ResetSingleLineFeatures(oldLineIndex: lineIndex - _headerLinesCount, all: false);

                bool anyFeatureMatches = false;

                _featureSet.ForEach(feature =>
                {
                    if (feature.Matches(line))
                    {
                        UpdateSingleLineFeature(lineIndex, feature);
                        anyFeatureMatches = true;
                    }
                });

                if (_middleColonFeature.Matches(line))
                {
                    UpdateMultiLineFeature(lineIndex);
                    anyFeatureMatches = true;
                }

                if (_phraseFeature.Matches(line))
                {
                    UpdatePhraseFeature(lineIndex);
                }

                if (anyFeatureMatches)
                {
                    ResetFeatures(oldLineIndex: lineIndex - _headerLinesCount);
                }
                else
                {
                    ResetFeatures(all: true);
                }

                if (HeaderFound(matchedLinesQuoteMark))
                {
                    return IdentifyHeader();
                }
            }

            return null;
        }

        private void UpdatePhraseFeature(int lineIndex)
        {
            _foundPhraseFeature = true;
            _phraseFeatureLineIndex = lineIndex;
        }

        private bool HeaderFound(IList<QuoteMarkMatchingResult> matchedLinesQuoteMark)
        {
            if (_foundPhraseFeature) return true;
            if (_foundFeatureMap.Count > _sufficientFeatureCount) return true;
            if (_foundFeatureMap.Count == _sufficientFeatureCount
                && CheckForSecondaryFeatures(matchedLinesQuoteMark)) return true;
            return false;
        }

        private bool CheckForSecondaryFeatures(IList<QuoteMarkMatchingResult> matchedLinesQuoteMark)
        {
            var sortedIndexes = _foundFeatureMap.Values.Sorted();
            int startIdx = sortedIndexes.First();
            int endIdx = sortedIndexes.Last();

            if (CheckMiddleColonSuggestion(startIdx, endIdx, _lines, _middleColonFeature)) return true;
            if (CheckQuoteMarkSuggestion(endIdx, _lines, matchedLinesQuoteMark)) return true;
            return false;
        }

        private void UpdateSingleLineFeature(int lineIndex, AbstractQuoteFeature feature)
        {
            if (feature is LastColonFeature)
            {
                // LastColonFeature cannot be the first feature of the quote.
                if (_foundFeatureMap.Count == 0)
                    return;

                var sortedIndexes = _foundFeatureMap.Values.Sorted();
                int startIdx = sortedIndexes.First();
                int endIdx = sortedIndexes.Last();

                // LastColonFeature cannot be in multi line header
                if (CheckMiddleColonSuggestion(startIdx, endIdx, _lines, _middleColonFeature))
                {
                    int headerLinesCount = endIdx - startIdx + 1;

                    // It seems like MiddleColonFeature instead LastColonFeature.
                    if (_middleColonCount <= headerLinesCount)
                    {
                        UpdateMultiLineFeature(lineIndex);

                        // If line contains the real MiddleColonFeature
                        // then we should decrease its count.
                        if (_middleColonFeature.Matches(_lines[lineIndex]))
                        {
                            _middleColonCount--;
                        }
                    }

                    return;
                }
            }

            _foundFeatureMap[feature.Name] = lineIndex;
        }

        private void UpdateMultiLineFeature(int lineIndex)
        {
            _lineMatchesMiddleColon = true;

            if (_middleColonCount == _multiLineHeaderLinesCount)
            {
                _firstMiddleColonLineIndex++;
            }
            else
            {
                if (_middleColonCount == 0)
                {
                    _firstMiddleColonLineIndex = lineIndex;
                }

                _middleColonCount++;
            }
        }

        private void ResetFeatures(int oldLineIndex = -1, bool all = false)
        {
            ResetSingleLineFeatures(oldLineIndex, all);
            ResetMultiLineFeatures();
        }

        private void ResetMultiLineFeatures(bool shouldReset = true)
        {
            if (!_lineMatchesMiddleColon && shouldReset)
            {
                _middleColonCount = 0;
            }

            _lineMatchesMiddleColon = false;
        }

        private void ResetSingleLineFeatures(int oldLineIndex, bool all)
        {
            if (all)
            {
                _foundFeatureMap.Clear();
            }
            else
            {
                _foundFeatureMap = _foundFeatureMap
                    .Where(i => i.Value > oldLineIndex)
                    .ToDictionary(i => i.Key, i => i.Value);
            }
        }

        private ValueTuple<int, int>? IdentifyHeader()
        {
            int fromIndex = int.MaxValue;
            int toIndex = int.MinValue;

            if (_foundPhraseFeature)
            {
                fromIndex = _phraseFeatureLineIndex;
                toIndex = _phraseFeatureLineIndex;
            }
            else
            {
                _foundFeatureMap.ForEach(it =>
                {
                    fromIndex = Math.Min(fromIndex, it.Value);
                    toIndex = Math.Max(toIndex, it.Value);
                });

                while (CheckForAllRemainingFeatures(fromIndex, toIndex))
                {
                    toIndex++;
                }

                if (CheckForMultiLineHeader(fromIndex, toIndex))
                {
                    fromIndex = _firstMiddleColonLineIndex;
                    toIndex = _firstMiddleColonLineIndex + _middleColonCount - 1;
                }
            }

            return (fromIndex, toIndex);
        }

        private bool CheckForAllRemainingFeatures(int fromIndex, int toIndex)
        {
            var remainingFeatures = _featureSet.Where(it => !_foundFeatureMap.ContainsKey(it.Name)).ToList();

            if (remainingFeatures.Any() && // One suggestion is not found.
                toIndex < _lines.Count - 1 && // There is the following line to check.
                toIndex - fromIndex + 1 < _headerLinesCount) // Found suggestions are placed in less than HEADER_LINES_COUNT lines
            {
                int lineIndex = toIndex + 1;

                bool anyFeatureMatches = false;
                remainingFeatures.ForEach(feature =>
                {
                    if (feature.Matches(_lines[lineIndex]))
                    {
                        UpdateSingleLineFeature(lineIndex, feature);
                        anyFeatureMatches = true;
                    }
                });

                if (anyFeatureMatches)
                {
                    if (_middleColonFeature.Matches(_lines[lineIndex]))
                    {
                        UpdateMultiLineFeature(lineIndex);
                    }
                    ResetMultiLineFeatures();

                    return true;
                }
            }

            return false;
        }

        private bool CheckForMultiLineHeader(int fromIndex, int toIndex)
        {
            if (_middleColonCount >= toIndex - fromIndex + 1)
            {
                int cnt = _multiLineHeaderLinesCount - _middleColonCount;
                int lineIndex = toIndex + 1;

                while (cnt > 0 && lineIndex < _lines.Count)
                {
                    if (_middleColonFeature.Matches(_lines[lineIndex]))
                    {
                        UpdateMultiLineFeature(lineIndex);
                    }
                    else
                    {
                        break;
                    }

                    lineIndex++;
                    cnt--;
                }

                return _firstMiddleColonLineIndex <= fromIndex &&
                       _firstMiddleColonLineIndex + _middleColonCount - 1 >= toIndex;
            }

            return false;
        }
    }
}