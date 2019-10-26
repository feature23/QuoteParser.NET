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
            var content = _parser.Value.Parse(GetResourceText(emailNum));
            Assert.Equal(expectedQuoteHeader, content.Header);
        }

        protected string GetResourceText(int emailNum)
        {
            var asm = typeof(ABTests).Assembly;
            using (var streamReader = new StreamReader(asm.GetManifestResourceStream($"{asm.GetName().Name}.Resources.testEmls.{_folder}.{emailNum}.txt"))) {
                return streamReader.ReadToEnd();
            }
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
