using System.IO;
using MimeKit;
using MimeKit.Text;

namespace QuoteParser
{
    public static class Parse
    {
        public static bool ContainInReplyToHeader(MimeMessage msg)
        {
            return msg.Headers.Contains("In-Reply-To") || msg.Headers.Contains("References");
        }

        public static MimeMessage GetMimeMessage(Stream emlFile)
        {
            return MimeMessage.Load(emlFile);
        }

        public static string GetEmailText(MimeMessage msg)
        {
            return msg.GetTextBody(TextFormat.Plain);
        }
    }
}
