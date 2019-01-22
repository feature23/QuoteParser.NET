using System.Linq;
using Xunit;

namespace QuoteParser.Tests
{
    public class ErrorTests : EmailTestBase
    {
        public ErrorTests()
            : base("error")
        {
        }

        [Theory]
        [InlineData(136, 0, 1, "12.05.2014 12:51, asdasd asdasd пишет:")]
        [InlineData(166, 2, 3, "Time: 2014-06-05 12:20 GMT+4 (MSK)")]
        [InlineData(1729, 10, 11, "In reply to:")]
        [InlineData(3953, 0, 1, "slon slon slon slon at slon.slon.slon/slon is slon slon slon slon of 11/25/2014 07:31 EST:")]
        [InlineData(8863, 33, 33)]
        [InlineData(9757, 2, 4, "On Fri, May 15, 2015 at 5:01 PM, text text <", "text.text@text.com> wrote:")]
        [InlineData(17382, 26, 27, "##- Please type your reply above this line -##")]
        [InlineData(22912, 4, 5, "INFO\u00a0\u00a0 | jvm 1\u00a0\u00a0\u00a0 | 2016/07/05 16:27:17 | text:\u00a0\u00a0 text: ")]
        public void TestEmail(int emailNum, int startIndex, int endIndex, params string[] lines)
        {
            var text = lines.ToList();

            var expected = new QuoteHeader(startIndex, endIndex, text);

            Check(emailNum, expected);
        }

        [Theory]
        [InlineData(3549)]
        public void TestNull(int emailNum)
        {
            Check(emailNum, expectedQuoteHeader: null);
        }
    }
}
