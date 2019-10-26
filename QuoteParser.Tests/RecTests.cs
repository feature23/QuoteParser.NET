using System.Collections.Generic;
using System.IO;
using Xunit;

namespace QuoteParser.Tests
{
    public class RecTests : EmailTestBase
    {
        public RecTests()
            : base("recursive")
        {
        }

        protected override QuoteParser CreateQuoteParser()
        {
            return new QuoteParser.Builder()
                .DeleteQuoteMarks(true)
                .Recursive(true)
                .Build();
        }

        [Fact]
        public void TestEmail270()
        {
            const int emailNum = 270;

            var expectedQuoteHeader = new QuoteHeader(
                startIndex: 4,
                endIndex: 6,
                text: new List<string>
                {
                    "On Tue, Jun 3, 2014 at 11:17 AM, asd asd <",
                    "asd-asd@asd.com> wrote:"
                }
            );

            var expectedInnerQuoteHeader = new QuoteHeader(
                startIndex: 4,
                endIndex: 5,
                text: new List<string>
                {
                    "In reply to:"
                }
            );
            
            var content = Parser.Parse(GetResourceText(emailNum));
            Assert.Equal(expectedQuoteHeader, content.Header);
            Assert.Equal(expectedInnerQuoteHeader, content.Quote?.Header);
        }

        [Fact]
        public void TestEmail6510()
        {
            const int emailNum = 6510;

            var expectedQuoteHeader1 = new QuoteHeader(
                startIndex: 2,
                endIndex: 3,
                text: new List<string>
                {
                    "-----Original Message-----"
                }
            );

            var expectedQuoteHeader2 = new QuoteHeader(
                startIndex: 0,
                endIndex: 4,
                text: new List<string>
                {
                    "From: \"text text (text)\" <text.text@text.com>",
                    "Sent: Friday, February 13, 2015, 6:44:58 PM",
                    "To: ",
                    "Subject: [text] Update: [text-text text] text-123456"
                }
            );

            var expectedQuoteHeader3 = new QuoteHeader(
                startIndex: 4,
                endIndex: 5,
                text: new List<string>
                {
                    "##- Please type your reply above this line -##  "
                }
            );
            
            var content = Parser.Parse(GetResourceText(emailNum));
            Assert.Equal(expectedQuoteHeader1, content.Header);
            Assert.Equal(expectedQuoteHeader2, content.Quote?.Header);
            Assert.Equal(expectedQuoteHeader3, content.Quote?.Quote?.Header);
        }
    }
}
