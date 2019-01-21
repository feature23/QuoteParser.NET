using QuoteParser.Features;
using Xunit;

namespace QuoteParser.Tests.Features
{
    public class MiddleColonFeatureTests
    {
        private readonly MiddleColonFeature _middleColonFeature = new MiddleColonFeature();

        [Theory]
        [InlineData("> Date: 17 Jul 2015 13:25:18 GMT+3")]
        [InlineData("     From: \"papa pa\" <eeee@eee.com> ")]
        [InlineData("From : \"papa pa\" <eeee@eee.com> ")]
        [InlineData("Onderwerp: [XXX YYY] Re: xxx ccc vvv bbb")]
        [InlineData("Reply: xxx   17  Jul 2015  13:25:18  eee-ddd@fff.com: ")]
        [InlineData("From‎ ‎:‎ \"papa pa\"\u200e<eeee@eee.com> ")]
        public void ShouldMatchMiddleColonRegex(string line)
        {
            Assert.True(_middleColonFeature.Matches(line));
        }

        [Theory]
        [InlineData("12:34, lorem ipsum dolor sit amet - 3100 consectetuer adipiscing eli@t.cc Aenean ")]
        [InlineData("lorem ipsum dolor sit amet - 3100 consectetuer adipiscing eli@t.cc Aenean: ")]
        [InlineData("lorem ipsum dolor sit amet - 3100 12:34:\t\t\t\t  consectetuer adipiscing \t\t\t\tc@f.rt\t\t\t\t")]
        [InlineData(":lorem ipsum dolor sit amet")]
        [InlineData("\t \t :lorem ipsum dolor sit amet")]
        [InlineData("From \"papa pa\" <eeee@eee.com>: ")]
        [InlineData(":From \"papa pa\" <eeee@eee.com>")]
        public void ShouldNotMatchMiddleColonRegex(string line)
        {
            Assert.False(_middleColonFeature.Matches(line));
        }
    }
}
