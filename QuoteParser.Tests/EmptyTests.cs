using Xunit;

namespace QuoteParser.Tests
{
    public class EmptyTests : EmailTestBase
    {
        public EmptyTests()
            : base("empty")
        {
        }

        [Theory]
        [InlineData(93)]
        [InlineData(102)]
        [InlineData(146)]
        [InlineData(205)]
        [InlineData(1165)]
        [InlineData(3505)]
        [InlineData(20454)]
        public void TestEmail(int emailNum)
        {
            Check(emailNum, expectedQuoteHeader: null);
        }
    }
}
