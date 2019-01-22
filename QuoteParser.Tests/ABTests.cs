using System.Collections.Generic;
using Xunit;

namespace QuoteParser.Tests
{
    public class ABTests : EmailTestBase
    {
        public ABTests()
            : base("AB")
        {
        }

        // NOTE: line numbers are off from upstream library in some cases because of handling multiple lines.
        // See note in SLTests.cs for more information.
        [Theory]
        [InlineData(149, 5, 5)]
        [InlineData(4237, 15, 15)] // NOTE: was 16, 16
        [InlineData(5370, 10, 10)]
        public void TestEmail(int emailNum, int startIndex, int endIndex)
        {
            var expected = new QuoteHeader(startIndex, endIndex, new List<string>());

            Check(emailNum, expected);
        }
    }
}
