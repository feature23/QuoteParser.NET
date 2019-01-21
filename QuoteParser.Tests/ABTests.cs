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

        [Theory]
        [InlineData(149, 5, 5)]
        [InlineData(4237, 16, 16)] // NOTE.PI: fails, but passes with 15, 15. Need to determine why off by 1 from Java
        [InlineData(5370, 10, 10)]
        public void TestEmail(int emailNum, int startIndex, int endIndex)
        {
            var expected = new QuoteHeader(startIndex, endIndex, new List<string>());

            Check(emailNum, expected);
        }
    }
}
