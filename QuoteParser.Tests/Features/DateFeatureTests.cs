using QuoteParser.Features;
using Xunit;

namespace QuoteParser.Tests.Features
{
    public class DateFeatureTests
    {
        private readonly DateFeature _dateFeature = new DateFeature();

        [Theory]
        [InlineData("@lorem  ipsum dolor sit amet, 04/04/05, consectetuer 01:01:33 adipiscing elit. Aenean : ")]
        [InlineData("Lorem:  ipsum dolor sit amet, 1.2.2000. consectetuer @ adipiscing elit. Aenean ")]
        [InlineData("13. 2015, Lorem  ipsum dolor sit amet, consec@tetuer adipiscing elit. Aenean 12:34")]
        [InlineData("On 8 December 2014 at 15:16, superman <xxx@xxx.com> wrote:")]
        [InlineData("On Monday, September 15, 2014. 6:50 AM, \"yyyy@yyy.com\" <yyyy@yyy.com> wrote:")]
        [InlineData("> On 05 Nov smth 2014, at 00:30:33, yyyy@yyy.com wrote:")]
        [InlineData("On Monday, September 2014, 15,  6:50 AM, \"yyyy@yyy.com\" <yyyy@yyy.com> wrote:")]
        [InlineData("> On 2014 Nov smth 05, at 00:30:33, yyyy@yyy.com wrote:")]
        [InlineData("> Date:17 Jul 2015 13:25:18 GMT+3")]
        [InlineData("> Date:2000-01-15. 13:25:18 GMT+3")]
        [InlineData("12:34,\t\t\t\t lorem ipsum dolor sit consectetuer adipiscing \t\t\t\tpa7@yx.ru 01 may 2016:")]
        [InlineData("12:34,\t\t\t\t lorem ipsum dolor sit 01 de may de 2016. consectetuer adipiscing \t\t\t\tp@c.tu\t\t\t\t")]
        [InlineData("12:34,\t\t\t\t lorem ipsum dolor sit 2016, de may de 04 consectetuer adipiscing \t\t\t\tp@c.tu\t\t\t\t")]
        [InlineData("Reply: xxx   17 Jul 2015  13:25:18  eee-ddd@fff.com: ")]
        [InlineData("Reply: xxx   17-01-2015  13:25:18  eee-ddd@fff.com: ")]
        [InlineData("Gesendet: Donnerstag‎, ‎30‎. ‎Oktober‎ ‎2014 ‎18‎:‎31\u200e")]
        [InlineData("Gesendet: Donnerstag‎, ‎30‎. ‎Oktober‎ ‎2014 ‎18‎:‎31‎ ‎:‎\u200e")]
        [InlineData("Gesendet: Donnerstag‎, ‎30‎-‎08‎-‎2014 ‎18‎:‎31‎ ‎:‎\u200e")]
        public void ShouldMatchDateRegex(string line)
        {
            Assert.True(_dateFeature.Matches(line));
        }

        [Theory]
        [InlineData("12:34, lorem ipsum dolor sit 12 amet - 2100 consectetuer adipiscing eli@t.cc Aenean ")]
        [InlineData("12:34, lorem ipsum dolor sit 01 May 1999, consectetuer adipiscing eli@t.cc Aenean ")]
        [InlineData("12:34,\t\t\t\t lorem ipsum dolor sit (01 may 2016). consectetuer adipiscing \t\t\t\tpa7@yx.ru\t\t\t\t")]
        [InlineData("12:34,\t\t\t\t lorem ipsum dolor Feb 2016. consectetuer adipiscing \t\t\t\tp@x.ru\t\t\t\t")]
        [InlineData("text text: +7(962)12-03-2000")]
        [InlineData("Reply: xxx   17-Jul-2015  13:25:18  eee-ddd@fff.com: ")]
        public void ShouldNotMatchDateRegex(string line)
        {
            Assert.False(_dateFeature.Matches(line));
        }
    }
}
