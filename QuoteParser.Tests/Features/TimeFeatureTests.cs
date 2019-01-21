using QuoteParser.Features;
using Xunit;

namespace QuoteParser.Tests.Features
{
    public class TimeFeatureTests
    {
        private readonly TimeFeature _timeFeature = new TimeFeature();

        [Theory]
        [InlineData("On 8 December 2014 at 15:16 superman <xxx@xxx.com> wrote:")]
        [InlineData("On 8 December 2014 at 15:16, superman <xxx@xxx.com> wrote:")]
        [InlineData("Lorem:  ipsum dolor sit amet, 1.2.2000 12:34: consectetuer @ adipiscing elit. Aenean ")]
        [InlineData("Lorem:  ipsum dolor sit amet, 1.2.2000 12:34. consectetuer @ adipiscing elit. Aenean ")]
        [InlineData("Lorem:  ipsum dolor sit amet, 1.2.2000 <12:34> consectetuer @ adipiscing elit. Aenean ")]
        [InlineData("13. 2015, Lorem  ipsum dolor sit amet, consec@tetuer adipiscing elit. Aenean 12:34")]
        [InlineData("13. 2015, Lorem  ipsum dolor sit amet, consec@tetuer adipiscing elit. Aenean 12:34:")]
        [InlineData("@lorem  ipsum dolor sit amet, 04/04/05, consectetuer 01:01:33 adipiscing elit. Aenean : ")]
        [InlineData("> On 05 Nov smth 2014, at 00:30:33, yyyy@yyy.com wrote:")]
        [InlineData("@lorem  ipsum dolor sit amet, 04/04/05, consectetuer 01:01:33: adipiscing elit. Aenean : ")]
        [InlineData("@lorem  ipsum dolor sit amet, 04/04/05, consectetuer  adipiscing elit. Aenean 01:01:33:")]
        [InlineData("@lorem  ipsum dolor sit amet, 04/04/05, consectetuer  adipiscing elit. Aenean \t01:01:33: \t")]
        [InlineData("On Monday, September 15, 2014 6:50AM \"yyyy@yyy.com\" <yyyy@yyy.com> wrote:")]
        [InlineData("On Monday, September 2014, 15,  6:50AM: \"yyyy@yyy.com\" <yyyy@yyy.com> wrote:")]
        [InlineData("On Monday, September 2014, 15,  6:50A.M.: \"yyyy@yyy.com\" <yyyy@yyy.com> wrote:")]
        [InlineData("12:34, lorem ipsum dolor sit amet - 3100 consectetuer adipiscing eli@t.cc Aenean ")]
        [InlineData("Reply: xxx   17  Jul 2015  13:25:18  eee-ddd@fff.com: ")]
        [InlineData("Gesendet: Donnerstag‎, ‎30‎. ‎Oktober‎ ‎2014 ‎18‎:‎31 ")]
        [InlineData("Gesendet: Donnerstag‎, ‎30‎. ‎Oktober‎ ‎2014 ‎18‎:‎31‎\u00a0:‎ ")]
        [InlineData("Gesendet: Donnerstag‎, ‎30‎.\u200e‎Oktober‎ ‎2014 ‎18‎:‎31‎:‎31‎  ")]
        [InlineData("@lorem  ipsum dolor sit amet, 04/04/05, consectetuer 1:01:33: adipiscing elit. Aenean : ")]
        public void ShouldMatchTimeRegex(string line)
        {
            Assert.True(_timeFeature.Matches(line));
        }

        [Theory]
        [InlineData("@lorem  ipsum dolor sit amet, 04/04/05, consectetuer 15.45 adipiscing elit. Aenean : ")]
        [InlineData("@lorem  ipsum dolor sit amet, 04/04/05, consectetuer 25:26 adipiscing elit. Aenean : ")]
        public void ShouldNotMatchTimeRegex(string line)
        {
            Assert.False(_timeFeature.Matches(line));
        }
    }
}
