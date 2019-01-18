using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QuoteParser.Features
{
    internal class KeyPhrases
    {
        public const string Whitespace = "[\\p{C}\\p{Z}\\s]";

        public const string InReplyToRegex = Whitespace + "*>*" + Whitespace + "*in" + Whitespace + "+" +
                                              "reply" + Whitespace + "+to:" + Whitespace + "*";

        public const string ReplyAboveRegex = Whitespace + "*>*" + Whitespace + "*##-.+-##" + Whitespace + "*";

        public const string OriginalMsgRegex = Whitespace + "*-*" + Whitespace + "*Original" + Whitespace + "+" +
                                                 "Message" + Whitespace + "*-*" + Whitespace + "*";

        public const string FwdMsgRegex = Whitespace + "*-*" + Whitespace + "Forwarded" + Whitespace + "+" +
                                            "message" + Whitespace + "*-*" + Whitespace + "*";

        public static IList<string> Default = new[]
        {
            InReplyToRegex,
            ReplyAboveRegex,
            OriginalMsgRegex
        };

    }

    internal class PhraseFeature : AbstractQuoteFeature
    {
        private readonly string _commonRegex;

        public PhraseFeature(IEnumerable<string> keyPhrases)
        {
            _commonRegex = keyPhrases.JoinToString(prefix: "(", separator: ")|(", postfix: ")");
        }

        public override string Name => "PHRASE";

        protected override Regex GetRegex()
        {
            return new Regex(_commonRegex, RegexOptions.IgnoreCase);
        }
    }
}
