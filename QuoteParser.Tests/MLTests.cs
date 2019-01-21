using System.Linq;
using Xunit;

namespace QuoteParser.Tests
{
    public class MLTests : EmailTestBase
    {
        public MLTests()
            : base("ML")
        {
        }

        [Theory]
        [InlineData(31, 22, 26, "From: text text [mailto:text-text@text.com] ", "Sent: Monday, May 26, 2014 11:13 AM", "To: text@text.com", "Subject: [text-6432] text text text, text to text in to text.com")]
        [InlineData(38, 16, 21, "From: text text [mailto:text.text@text.com]", "Sent: Tuesday, May 27, 2014 4:05 AM", "To: text text", "Cc: text-text-v@text.com; text.text@v.com", "Subject: Re: text text v")]
        [InlineData(43, 37, 41, "От:     text text <text-text@text.com>", "Кому:   text@text.ru", "Дата:   26.05.2014 21:27", "Тема:   [text-9654] Организация text'a")]
        [InlineData(82, 14, 16, "<text-text@text.com>", "Дата:   30.05.2014 11:17")]
        [InlineData(85, 31, 34, "From: text text [mailto:text-text@text.com] ", "Sent: Thursday, May 29, 2014 5:01 PM", "Subject: Re: text text text text")]
        [InlineData(158, 2, 7, "From: text text text [mailto:text.text@text.com]", "Sent: Wednesday, June 04, 2014 12:20 PM", "To: 'text text'", "Cc: text text; text text", "Subject: FW: text text in text- can't text text text")]
        [InlineData(196, 5, 9, "From: text text", "Sent: Wednesday, June 04, 2014 2:46 PM", "To: 'text text'", "Subject: RE: text-text text text text text tex textext")]
        [InlineData(210, 26, 32, "From: text text [mailto:text.text@text.com] ", "Sent: Thursday, June 05, 2014 2:11 PM", "To: text.text@text.com", "Cc: text text; text text", "Subject: text of text text text & text", "Importance: High")]
        [InlineData(260, 15, 20, "From: test test [mailto:test-test@test.com]", "Sent: Tuesday, June 10, 2014 12:11 PM", "To: test test", "Cc: test test", "Subject: Re: test test test")]
        [InlineData(279, 26, 27, "Sent: Tue 27 May 2014 18:46")]
        [InlineData(322, 9, 13, "Da:     text text <text-text@text.com>", "Per:    text.text@text.it", "Data:   11/06/2014 19.16", "Oggetto:        Re: text text text?")]
        [InlineData(348, 21, 23, "<text-text-text@text.com>", "Data:   14/06/2014 12.41")]
        [InlineData(3697, 18, 23, "> ", "> From: test test [mailto:test@test.com <mailto:test@test.com>] ", "> Sent: Friday, November 14, 2014 8:40 AM", "> To: test@test.com <mailto:test@test.com>", "> Subject: test test test to test my test test for test test test test")]
        [InlineData(3715, 15, 23, ">", ">", ">", ">", ">", ">", "> *From:* text text [mailto:text.text@text.com]", "> *Sent:* Thursday, November 13, 2014 11:15 AM")]
        [InlineData(3926, 2, 8, "From:  test test <test.test@test.com>", "Organization:  test test", "Date:  Monday, November 24, 2014 at 4:05 PM", "To:  'test test' <test@test.net>, <test@test.com>", "Cc:  test test <test@test.com>", "Subject:  RE: test test test 1500 test test test")]
        [InlineData(4298, 10, 11, "<div>-------- Original message --------</div><div>From: text text <text-text@text.com> </div><div>Date:12/02/2014  09:48  (GMT-08:00) </div><div>To: text.text.123@text.text.text </div><div>Subject: Re: text't text or text text </div><div>")]
        [InlineData(5859, 3, 6, "From : asd.text@text.com", "Sent : Sun Nov 02 17:11:48 MSK 2014", "Subject : text text to text text text")]
        [InlineData(17326, 11, 17, "> Begin forwarded message:", "> ", "> From: text text <text@text.com>", "> Subject: text R123456. text text text text", "> Date: 19 October 2015 16:38:23 BST", "> To: text@text.text.uk")]
        [InlineData(17747, 19, 21, "text.text@text.com]", "*Sent:* Friday, October 30, 2015 2:58 PM")]
        [InlineData(18139, 85, 89, "From: text, text P.", "Sent: Friday, November 06, 2015 3:44 PM", "To: 'text text'", "Subject: RE: [text text] Re: text text text text - text text text text text")]
        public void TestEmail(int emailNum, int startIndex, int endIndex, params string[] lines)
        {
            var text = lines.ToList();

            var expected = new QuoteHeader(startIndex, endIndex, text);

            Check(emailNum, expected);
        }
    }
}
