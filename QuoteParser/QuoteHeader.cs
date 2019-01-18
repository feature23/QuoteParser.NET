using System.Collections.Generic;
using System.Linq;

namespace QuoteParser
{
    public class QuoteHeader
    {
        public QuoteHeader(int startIndex, IList<string> text)
            : this(startIndex, startIndex, text)
        {
        }

        public QuoteHeader(int startIndex, int endIndex, IList<string> text)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
            Text = text;
        }

        public int StartIndex { get; }

        public int EndIndex { get; }

        public IList<string> Text { get; }

        public override string ToString()
        {
            return Text.JoinToString(separator: "\n");
        }

        public override bool Equals(object other)
        {
            if (other == null) return false;
            if (!(other is QuoteHeader qh)) return false;
            if (StartIndex != qh.StartIndex) return false;
            if (EndIndex != qh.EndIndex) return false;
            if (!Text.SequenceEqual(qh.Text)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + StartIndex;
            result = prime * result + EndIndex;
            result = prime * result + Text.GetHashCode();
            return result;
        }
    }
}