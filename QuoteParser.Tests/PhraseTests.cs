using System.Linq;
using Xunit;

namespace QuoteParser.Tests
{
    public class PhraseTests : EmailTestBase
    {
        public PhraseTests()
            : base("phrases")
        {
        }

        [Theory]
        [InlineData(92, 9, 10, "In reply to:")]
        [InlineData(2555, 2, 3, "-------- Original message --------")]
        [InlineData(17407, 1, 2, "##- Please type your reply above this line -##")]
        public void TestEmail(int emailNum, int startIndex, int endIndex, params string[] lines)
        {
            var text = lines.ToList();

            var expected = new QuoteHeader(startIndex, endIndex, text);

            Check(emailNum, expected);
        }
    }
}
