using QuoteParser.Features;
using Xunit;

namespace QuoteParser.Tests.Features
{
    public class ColonFeatureTests
    {
        private readonly LastColonFeature _colonFeature = new LastColonFeature();

        [Theory]
        [InlineData("@lorem  ipsum dolor sit amet, 04/04/05, consectetuer 01:01:33 adipiscing elit. Aenean : ")]
        [InlineData("On 8 December 2014 at 15:16, superman <xxx@xxx.com> wrote:")]
        [InlineData("From \"papa pa\" <eeee@eee.com>: ")]
        [InlineData("Reply: xxx   17  Jul 2015  13:25:18  eee-ddd@fff.com: ")]
        [InlineData("   In reply to:   ")]
        [InlineData("Gesendet: Donnerstag‎, ‎30‎. ‎Oktober‎ ‎2014 ‎18‎:‎31‎:")]
        [InlineData("Gesendet: Donnerstag‎, ‎30‎. ‎Oktober‎ ‎2014 ‎18‎:‎31‎:\u200e")]
        [InlineData("Gesendet: Donnerstag‎, ‎30‎. ‎Oktober‎ ‎2014 ‎18‎:‎31‎ ‎:‎\u200e")]
        public void ShouldMatchColonRegex(string line)
        {
            Assert.True(_colonFeature.Matches(line));
        }

        [Theory]
        [InlineData("Lorem:  ipsum dolor sit amet, 1.2.2000 consectetuer @ adipiscing elit. Aenean ")]
        [InlineData("13. 2015, Lorem  ipsum dolor sit amet, consec@tetuer adipiscing elit. Aenean 12:34")]
        [InlineData("12:34, lorem ipsum dolor sit amet - 3100 consectetuer adipiscing eli@t.cc Aenean ")]
        [InlineData("> Date: 17 Jul 2015 13:25:18 GMT+3")]
        [InlineData(":From \"papa pa\" <eeee@eee.com>")]
        [InlineData("Onderwerp: [XXX YYY] Re: xxx ccc vvv bbb")]
        [InlineData("Gesendet: Donnerstag‎, ‎30‎. ‎Oktober‎ ‎2014 ‎18‎:‎31\u200e")]
        public void ShouldNotMatchColonRegex(string line)
        {
            Assert.False(_colonFeature.Matches(line));
        }
    }
}
