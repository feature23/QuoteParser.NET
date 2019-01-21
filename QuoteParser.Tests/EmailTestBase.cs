using System;
using System.IO;
using Xunit;

namespace QuoteParser.Tests
{
    public abstract class EmailTestBase
    {
        private readonly string _folder;

        private readonly Lazy<QuoteParser> _parser;

        protected EmailTestBase(string folder)
        {
            _folder = folder;
            _parser = new Lazy<QuoteParser>(CreateQuoteParser);
        }

        protected void Check(int emailNum, QuoteHeader expectedQuoteHeader)
        {
            using (var stream = GetResourceStream(emailNum))
            {
                var content = _parser.Value.Parse(stream);
                Assert.Equal(expectedQuoteHeader, content.Header);
            }
        }

        protected Stream GetResourceStream(int emailNum)
        {
            var asm = typeof(ABTests).Assembly;
            return asm.GetManifestResourceStream($"{asm.GetName().Name}.Resources.testEmls.{_folder}.{emailNum}.eml");
        }

        protected virtual QuoteParser CreateQuoteParser()
        {
            return new QuoteParser.Builder()
                .DeleteQuoteMarks(false)
                .Build();
        }

        protected QuoteParser Parser => _parser.Value;
    }
}
