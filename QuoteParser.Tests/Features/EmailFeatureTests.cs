using QuoteParser.Features;
using Xunit;

namespace QuoteParser.Tests.Features
{
    public class EmailFeatureTests
    {
        private readonly EmailFeature _emailFeature = new EmailFeature();

        [Theory]
        [InlineData("13. 2015, Lorem  ipsum dolor sit amet, consec@tetuer adipiscing elit. Aenean 12:34")]
        [InlineData("On 8 December 2014 at 15:16, superman <xxx@xxx.com> wrote:")]
        [InlineData("On Monday, September 15, 2014 6:50 AM, \"yyyy@yyy.com\" <yyyy@yyy.com> wrote:")]
        [InlineData("12:34, lorem ipsum dolor sit amet - 3100 consectetuer adipiscing eli@t.cc")]
        [InlineData("12:34, lorem ipsum dolor sit amet - 3100 consectetuer adipiscing \"eli@t.cc\"")]
        [InlineData("12:34, lorem ipsum dolor sit amet - 3100 consectetuer adipiscing /eli@t.cc/  ")]
        [InlineData("12:34,\t\t\t\t lorem ipsum dolor sit amet - 3100 consectetuer adipiscing \t\t\t\tp@gg.ru\t\t\t\t")]
        [InlineData("From:<eeee@eee.com> ")]
        [InlineData("From : \"papa pa\" < eeee@eee.com > ")]
        [InlineData("From \"papa pa\" <eeee@eee.com>: ")]
        [InlineData("Reply: xxx   17  Jul 2015  13:25:18  eee-ddd@fff.com: ")]
        [InlineData("Reply: xxx  \u200e 17  Jul 2015  13:25:18  eee‎-ddd@fff‎.com: ")]
        [InlineData("Reply: xxx  \u200e 17  Jul 2015  13:25:18  eee‎-ddd\u200e‎@\u200e‎fff‎.com: ")]
        public void ShouldMatchEmailRegex(string line)
        {
            Assert.True(_emailFeature.Matches(line));
        }

        [Theory]
        [InlineData("@lorem  ipsum dolor sit amet, 04/04/05, consectetuer 01:01:33 adipiscing elit. Aenean : ")]
        [InlineData("lorem@  ipsum dolor sit amet, 04/04/05, consectetuer 01:01:33 adipiscing elit. Aenean : ")]
        [InlineData("Lorem:  ipsum dolor sit amet, 1.2.2000 consectetuer @ adipiscing elit. Aenean ")]
        public void ShouldNotMatchEmailRegex(string line)
        {
            Assert.False(_emailFeature.Matches(line));
        }
    }
}
