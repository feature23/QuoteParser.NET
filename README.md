# QuoteParser.NET [![Build Status](https://travis-ci.org/feature23/QuoteParser.NET.svg?branch=master)](https://travis-ci.org/feature23/QuoteParser.NET)
A .NET Standard port of JetBrains' [email-parser](https://github.com/JetBrains/email-parser) library.

## Version 2.0.0 Breaking Changes
Removed MimeKitLite dependency in order to avoid duplicate type/namespace conflits for projects that already included (directly or indirectly) different versions of MimeKitLite, MimeKit or MailKit. The QuoteParser class processes the plain text body content of an email message. A MIME parser (such as MimeKit) is required to process raw email messages and extract the plain text body content for quote parsing.