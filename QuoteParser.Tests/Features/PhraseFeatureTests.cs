using QuoteParser.Features;
using Xunit;

namespace QuoteParser.Tests.Features
{
    public class PhraseFeatureTests
    {
        private readonly PhraseFeature _phraseFeature = new PhraseFeature(KeyPhrases.Default);

        [Theory]
        [InlineData("   In reply to:   ")]
        [InlineData(">> In reply to:   ")]
        [InlineData("   In    RePly To:   ")]
        [InlineData("##- Bitte geben Sie Ihre Antwort oberhalb dieser Zeile ein. -##")]
        [InlineData("##- Bitte geben Sie Ihre Antwort oberhalb dieser Zeile ein.\u200e-##")]
        [InlineData("   In  \u200e  RePly To:   ")]
        [InlineData(" ‎##- Bitte geben Sie\u00a0Ihre Antwort oberhalb dieser Zeile ein. -## ")]
        public void ShouldMatchPhraseRegex(string line)
        {
            Assert.True(_phraseFeature.Matches(line));
        }

        [Theory]
        [InlineData("> > ##- Bitte geben Sie Ihre Antwort oberhalb dieser Zeile ein. -##")]
        [InlineData(" >>>>  In    RePly To:  <<< ")]
        [InlineData("text ##- Bitte geben Sie Ihre Antwort oberhalb dieser Zeile ein. -##")]
        [InlineData("  In    RePly To:  garbage")]
        [InlineData("##- Bitte geben Sie Ihre Antwort oberhalb dieser Zeile ein. -## garbage")]
        public void ShouldNotMatchPhraseRegex(string line)
        {
            Assert.False(_phraseFeature.Matches(line));
        }
    }
}
