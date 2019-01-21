using System.Collections.Generic;
using QuoteParser.Features;
using Xunit;

namespace QuoteParser.Tests.Features
{
    public class QuoteMarkFeatureTests
    {
        private readonly QuoteMarkFeature _quoteMarkFeature = new QuoteMarkFeature();

        [Theory]
        [InlineData(">")]
        [InlineData(">>>")]
        [InlineData("\t\t >  > >")]
        [InlineData(">text")]
        [InlineData(">>>text")]
        [InlineData("\t >\t >  >\t\ttext")]
        [InlineData(" >\u00a0>\u00a0> text")]
        [InlineData(" ‎>‎\u00a0>\u200e‎‎>‎ text")]
        public void ShouldMatchQuoteMarkRegex(string line)
        {
            Assert.True(_quoteMarkFeature.Matches(line));
        }

        [Theory]
        [InlineData("text > text")]
        public void ShouldNotMatchQuoteMarkRegex(string line)
        {
            Assert.False(_quoteMarkFeature.Matches(line));
        }

        [Fact]
        public void MatchLinesMustGetAppropriateValue()
        {
            var lines = new List<string>()
            {
                "text > text",
                ">   ",
                ">text",
                "\t> TEXT"
            };

            var matching = _quoteMarkFeature.MatchLines(lines);

            Assert.Equal(QuoteMarkMatchingResult.NotEmpty, matching[0]);
            Assert.Equal(QuoteMarkMatchingResult.VEmpty, matching[1]);
            Assert.Equal(QuoteMarkMatchingResult.VNotEmpty, matching[2]);
            Assert.Equal(QuoteMarkMatchingResult.VNotEmpty, matching[3]);
        }

        [Fact]
        public void MatchLinesMustGetAppropriateValue_2()
        {
            var lines = new List<string>()
            {
                ">text > text",
                " \t  ",
                "\t>  > TEXT",
                "\t>  ",
                "\t TEXT"
            };

            var matching = _quoteMarkFeature.MatchLines(lines);

            Assert.Equal(QuoteMarkMatchingResult.VNotEmpty, matching[0]);
            Assert.Equal(QuoteMarkMatchingResult.Empty, matching[1]);
            Assert.Equal(QuoteMarkMatchingResult.VNotEmpty, matching[2]);
            Assert.Equal(QuoteMarkMatchingResult.VEmpty, matching[3]);
            Assert.Equal(QuoteMarkMatchingResult.NotEmpty, matching[4]);
        }
    }
}
