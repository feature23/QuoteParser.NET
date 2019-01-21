using System.Linq;
using Xunit;

namespace QuoteParser.Tests
{
    public class SLTests : EmailTestBase
    {
        public SLTests()
            : base("SL")
        {
        }

        [Theory]
        [InlineData(10, 11, 12, "On 26 May 2014, at 14:25 , Robin <robin@man.com> wrote:")]
        [InlineData(20, 5, 6, "2014-05-23 16:32 GMT+04:00 catwoman catwoman <cat@woman.com>:")]
        [InlineData(26, 18, 20, "2014-05-27 13:02 GMT+02:00 WHO IS <", "who@is.com>:")]
        [InlineData(75, 7, 8, "On May 29, 2014 at 10:01:16 AM, Xxxx Yyyy (asdasd@asdadsasd.cd) wrote:")]
        [InlineData(108, 13, 14, "On 12/05/2014 13:00, Professor X wrote:")]
        [InlineData(187, 4, 5, "On 05.июня.2014, at 14:50, Censored Cen <dd-dd@dd.com> wrote:")]
        [InlineData(214, 10, 12, "On Friday, June 6, 2014, Eddard Stark <strak.rulez@winteriscoming.com>", "wrote:")]
        [InlineData(245, 33, 34, "On 02/06/2014 11:34, Wonder Woman wrote:")]
        [InlineData(259, 35, 37, "> Capitan America <mailto:America.Capitan@avengers.com>", "> 10 Jun 2014 17:49")]
        [InlineData(374, 4, 5, "Am Dienstag, 17. Juni 2014 schrieb Xxx Yyyy :")]
        [InlineData(427, 8, 9, "-Original Message-On 20/06/2014 11:40 AM, \"Xxxxx Yyyyy\" <sdf-sdf@sdf.com" + "<mailto:sdf-sdf@sdf.com>> wrote:")]
        [InlineData(484, 2, 4, "On Jun 25, 2014 5:51 PM, \"Harry Potter\" <ads-asd@asd.com>", "wrote:")]
        [InlineData(599, 12, 13, "Am 30.06.2014 16:31, schrieb XXX YYY:")]
        [InlineData(704, 7, 8, "Dnia 3 lip 2014 o godz. 19:33 text text <text-text@text.com> napisał(a):")]
        [InlineData(800, 12, 14, "2014-07-10 12:13 GMT+02:00 asd asd <a.asd@a.com>", ":")]
        [InlineData(880, 16, 17, "On Mon, Jul 14, 2014 at 11:11 AM, Dan Banan <dan@banan.com> wrote:")]
        [InlineData(967, 3, 5, "On Fri, Jul 18, 2014 at 1:17 AM, Dart Vader <luke@imyourfather.com", "> wrote:")]
        [InlineData(1066, 19, 20, "On 7/11/14, 9:30 AM, X Y wrote:")]
        [InlineData(1077, 2, 3, "суббота, 26 июля 2014 г. пользователь Some Body написал:")]
        [InlineData(3711, 10, 11, "---- On Mon, 17 Nov 2014 13:31:52 -0800 Text Text &lt;Text.Text@Text.com&gt; wrote ---- ")]
        [InlineData(3997, 9, 10, "Op 25 nov. 2014 om 18:44 heeft text text <text-text@text.com<mailto:text-text@text.com>>" + " het volgende geschreven:")]
        [InlineData(4008, 25, 26, "Wiadomość napisana przez text text <text-text@text.com> w dniu 25 lis 2014, o godz. 21:44:")]
        [InlineData(4169, 40, 40)]
        [InlineData(11461, 25, 27, "text text <text.text@text.com> schrieb am Sa., 19. Sep.", "2015 um 12:04 Uhr:")]
        [InlineData(17444, 18, 19, ">>> text text <text-text-text@text.com> 10/22/15 21:02 >>>")]
        [InlineData(17949, 37, 38, "On Nov 2, 2015, at 10:09 AM, text text (text text) <textck.text@text.com<mailto:" + "text.text@text.com>> wrote:")]
        [InlineData(17995, 2, 3, "> 5 нояб. 2015 г., в 13:37, test test (test test) <test.test@test.com> написал(а):")]
        [InlineData(18075, 19, 21, "Il giorno ven 6 nov 2015 alle ore 12:30 text text (text text) <", "text.text@text.com> ha scritto:")]
        [InlineData(18762, 10, 11, "     On Wednesday, November 11, 2015 2:51 PM, text text (text textack) <text.text@asd." + "com> wrote:")]
        [InlineData(21086, 3, 4, "On Tue, 15 Mar 2016 12:30:19 +0000")]
        [InlineData(21593, 1, 4, "> 在 2016年4月15日，17:14，text text (text text) <text-text-text@text.com> 写道：", "> ", "> ##- Please type your reply above this line -##")]
        public void TestEmail(int emailNum, int startIndex, int endIndex, params string[] lines)
        {
            var text = lines.ToList();

            var expected = new QuoteHeader(startIndex, endIndex, text);

            Check(emailNum, expected);
        }
    }
}
