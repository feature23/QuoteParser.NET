# QuoteParser.NET [![Build Status](https://travis-ci.org/feature23/QuoteParser.NET.svg?branch=master)](https://travis-ci.org/feature23/QuoteParser.NET)
A .NET Standard port of JetBrains' [email-parser](https://github.com/JetBrains/email-parser) library.

## Usage Example
The `QuoteParser` class processes the plain text body content of an email message, separating the content of the latest reply from any previous quoted exchanges:

```csharp
// create an instance of the parser builder
var builder = new QuoteParser.QuoteParser.Builder();

// configure the builder using the fluent interface to override any defaults
builder = builder.MinimumQuoteBlockSize(10);

// build the parser
var parser = builder.Build();

// example reply email content that includes a quote of the original message
var emailContent =
@"this is the latest reply body text

On Tue, Oct 30, 2019, at 12:00 AM, Person A <person.a@example.com> wrote:

> this is the original message that was replied to";

// parse the email content
var content = parser.Parse(emailContent);

// write the content of the latest reply to the console
Console.Write(String.Join(Environment.NewLine, content.Body)); // this is the latest reply body text
```

### Processing raw MIME messages
If you are processing the full raw MIME message, you need to first extract the plain text body content using a MIME parser such as the one included with [MimeKit](http://www.mimekit.net/). The example below uses the `MimeMessage` class which is included with [MimeKitLite](https://www.nuget.org/packages/MimeKitLite/), [MimeKit](https://www.nuget.org/packages/MimeKit/) and [MailKit](https://www.nuget.org/packages/MailKit/).

```csharp
// same example as above only in raw MIME format
var rawEmailContent =
@"MIME-Version: 1.0
From: Person B <person.b@example.com>
To: Person A <person.a@example.com>
In-Reply-To: <random-message-id@mail.example.com>
Content-Type: multipart/alternative; boundary=""random-boundary-id text""

--random-boundary-id text
Content-Type: text/plain

this is the latest reply body text

On Tue, Oct 30, 2019, at 12:00 AM, Person A <person.a@example.com> wrote:

> this is the original message that was replied to

--random-boundary-id text
Content-Type: text/html;
Content-Transfer-Encoding: quoted-printable

<p>this is the latest reply body text<p>

<p>On Tue, Oct 30, 2019, at 12:00 AM, Person A <person.a@example.com> wrote:</p>

<blockquote><p>this is the original message that was replied to<p></blockquote>

--random-boundary-id text--
";

// parse the message using MimeKit
var message = MimeMessage.Load(new MemoryStream(Encoding.UTF8.GetBytes(rawEmailContent)));

// get the plain text body
var emailContent = message.GetTextBody(TextFormat.Plain);

// check for reply headers
var hasInReplyToHeader = message.Headers.Contains("In-Reply-To") || message.Headers.Contains("References");

// parse the text body using QuoteParser 
var content = new QuoteParser.QuoteParser
	.Builder()
	.Build()
	.Parse(emailContent, hasInReplyToHeader);

// write the content of the latest reply to the console
Console.Write(String.Join(Environment.NewLine, content.Body)); // this is the latest reply body text
```

### QuoteParser.Builder Configuration

|Builder Method|Default Value|
|---|---|
|`HeaderLinesCount`|`3`|
|`MultiLineHeaderLinesCount`|`6`|
|`MinimumQuoteBlockSize`|`7`|
|`DeleteQuoteMarks`|`true`|
|`Recursive`|`false`|
|`KeyPhrases`|`InReplyToRegex, ReplyAboveRegex, OriginalMsgRegex`|

## Version 2.0.0 Breaking Changes
Removed MimeKitLite dependency in order to avoid duplicate type/namespace conflits for projects that already included (directly or indirectly) different versions of MimeKitLite, MimeKit or MailKit. See example above for processing raw MIME messages.