using System.Collections.Generic;
using System.Text;

namespace QuoteParser
{
    public class Content
    {
        public Content(IList<string> body, QuoteHeader header, Content quote)
        {
            Body = body;
            Header = header;
            Quote = quote;
        }
        
        public IList<string> Body { get; }

        public QuoteHeader Header { get; }

        public Content Quote { get; }

        public string ToString(bool addMarks, bool uppercaseHeader = false)
        {
            string prefix = addMarks ? "> " : "";
            string separator = addMarks ? "\n> " : "\n";

            string bodyText = (!Body.IsEmpty())
                ? Body.JoinToString(
                    separator: separator,
                    postfix: (Header != null) ? "\n" : ""
                )
                : "";

            string headerText = (Header != null && !Header.Text.IsEmpty())
                ? Header.Text.JoinToString(
                    prefix: prefix,
                    separator: separator,
                    postfix: "\n",
                    transform: it => uppercaseHeader ? it.ToUpper() : it
                )
                : "";

            string quoteText = (Quote != null && Quote.Body.IsEmpty())
                ? Quote
                    .ToString(addMarks, uppercaseHeader).Lines()
                    .JoinToString(
                        prefix: prefix,
                        separator: separator,
                        postfix: ""
                    )
                : "";

            return new StringBuilder(bodyText)
                .Append(headerText)
                .Append(quoteText)
                .ToString();
        }

        public override string ToString()
        {
            return ToString(addMarks: true, uppercaseHeader: true);
        }
    }
}