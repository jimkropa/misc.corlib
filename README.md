This is a **work in progress** as of March,&nbsp;2016.

The complete draft of the readme follows these summary lists of completed and incomplete parts. Check back here for updates and thanks for your interest!

### More or Less Ready for Release:

- Tests, documentation, and implementation of PagingInfo and related classes
- Tests, documentation, and implementation of Collections.Generic classes
- Implementation of ConvertByteArray and ConvertEncodedString static extension methods (pending&nbsp;tests)
- Documentation and implementation of FormatString static extension methods (pending&nbsp;tests)

### TODO:

- Tests and documentation of ConvertByteArray and ConvertEncodedString
- Final decisions about the contracts for Encryptor and Decryptor and their factory methods
- Tests, documentation, and implementation of the rest of the Cryptography namespace
- Tests of FormatString
- Tests, documentation, and implementation of NameValueCollectionExtensions
- Tests, documentation, and implementation of Enum utilities
- Refactor parts of Cryptography to use bitwise functions from Enum utilities
- Create builds for .NET&nbsp;2.0, 4.0, 5.0, and Mono
- Deploy as [public NuGet&nbsp;package](https://www.nuget.org/)



# misc.corlib

is a small assortment of adapters and extensions from <tt>mscorlib.dll</tt>

> These are some useful types that I've rewritten
> enough times to not want to write them over again,
> so I have composed them into this open source project.
> *&#8209;&#8209;KRopa.*

It is intended for eventual free distribution as a public NuGet&nbsp;package,
but remains a **pre&#8209;release work&nbsp;in&nbsp;progress** for&nbsp;now.


## Features

This library consists of:

- **[Security.Cryptography](https://github.com/jimkropa/misc.corlib/tree/master/src/misc.corlib/Security/Cryptography):** Simplified generic encryption, decryption, and hashing of strings or byte arrays, as flexible as you need, but guiding you to the&nbsp;most&nbsp;secure&nbsp;implementation
- **[Collections.PagingInfo](https://github.com/jimkropa/misc.corlib/tree/master/src/misc.corlib/Collections):** Contracts and structs for managing "paged"&nbsp;lists
- **[Collections.Generic](https://github.com/jimkropa/misc.corlib/tree/master/src/misc.corlib/Collections/Generic):** Transformations back and forth between comma&#8209;delimited strings and arrays&nbsp;of&nbsp;primitives
- **[Collections.Specialized](https://github.com/jimkropa/misc.corlib/tree/master/src/misc.corlib/Collections/Specialized):** Extraction of primitive values from [NameValueCollection](https://msdn.microsoft.com/en-us/library/system.collections.specialized.namevaluecollection.aspx), useful for old&#8209;school [HttpRequest.QueryString](https://msdn.microsoft.com/en-us/library/system.web.httprequestbase.querystring.aspx)&nbsp;parsing
- **[ConvertByteArray](https://github.com/jimkropa/misc.corlib/blob/master/src/misc.corlib/ConvertByteArray.cs)** and **[ConvertEncodedString](https://github.com/jimkropa/misc.corlib/blob/master/src/misc.corlib/ConvertEncodedString.cs):** Conversions between byte arrays and compact encoded strings
- **[FormatString](https://github.com/jimkropa/misc.corlib/blob/master/src/misc.corlib/FormatString.cs):** String formatting for various output or sanitization
- **ConvertEnum:** (TBD) Tools for working with and converting [Enum](https://msdn.microsoft.com/en-us/library/system.enum.aspx) types, including simplified bitwise&nbsp;operations

Iterations of this library have a longer history than its public GitHub edition,
and along the way parts of it have been made obsolete by successive releases
of the .NET&nbsp;Framework.

Obsolescence has occurred when a new version of the framework includes a new type or
method having uncanny resemblance (even down to the names!) to ones I had written
from&nbsp;scratch prior to their appearance in the framework. Historical examples
include [TimeZoneInfo](https://msdn.microsoft.com/en-us/library/system.timezoneinfo.aspx),
[IReadOnlyList<T>](https://msdn.microsoft.com/en-us/library/hh192385.aspx), and the
[IsNullOrWhiteSpace method](https://msdn.microsoft.com/en-us/library/system.string.isnullorwhitespace.aspx).

If my past ability to guess what Microsoft will add to <tt>mscorlib.dll</tt> is any guide,
perhaps we'll be seeing [Encryptor](https://github.com/jimkropa/misc.corlib/blob/master/src/misc.corlib/Security/Cryptography/Encryptor.cs)
and [Decryptor](https://github.com/jimkropa/misc.corlib/blob/master/src/misc.corlib/Security/Cryptography/Decryptor.cs) classes
added by the time ASP.NET&nbps;5 is released!


## Usage Notes

The types in this library are for core infrastructure and aim
to be as stable and reliable as the .NET&nbsp;Framework itself,
including optimized performance, full test&nbsp;coverage,
[coding by contract](http://research.microsoft.com/en-us/projects/contracts/),
and detailed inline documentation.
You&nbsp;should be able to consider a reference to **misc.corlib**
as [what Mark&nbsp;Seemann would call a "stable"
(non&#8209;volatile)&nbsp;depency](http://blogs.msdn.com/b/ploeh/archive/2006/08/24/718828.aspx).

As&nbsp;such, there is no particular emphasis on abstraction
or points of dependency injection. The&nbsp;concrete
[Encryptor](https://github.com/jimkropa/misc.corlib/blob/master/src/misc.corlib/Security/Cryptography/Encryptor.cs) and
[Decryptor](https://github.com/jimkropa/misc.corlib/blob/master/src/misc.corlib/Security/Cryptography/Decryptor.cs) classes
in the [Security.Cryptography](https://github.com/jimkropa/misc.corlib/tree/master/src/misc.corlib/Security/Cryptography) namespace,
for&nbsp;example, do&nbsp;not specify an abstract contract
because they are basically thin facades coordinating the
cooperation of a few concrete .NET&nbsp;Framework types.
The facades for cryptographic operations in this library are
designed to guide an optimum implementation with a bit less ceremony,
so it's more practical to simply treat them as concrete types themselves.
Besides, they are already amply variable by their internal use
of the [abstract factory within the SymmetricAlgorithm
class](https://msdn.microsoft.com/en-us/library/k74a682y.aspx)
in&nbsp;<tt>mscorlib.dll</tt>

> A pro tip, by the way: If you'd like to use dependency injection
> to specify which algorithms to use for encryption or hashing, the
> [AlgorithmFactory](https://github.com/jimkropa/misc.corlib/blob/master/src/misc.corlib/Security/Cryptography/AlgorithmFactory.cs)
> class provides the injection point.

Since the types in this library target specific parts of the
.NET&nbsp;Framework, their namespaces mimic the&nbsp;same&nbsp;structure.

I'm happy to make this library available publicly under the Apache&nbsp;2&nbsp;License,
and am open to suggestions for improvement. Please feel to fork away, use as much or
as little as you like, and submit&nbsp;pull&nbsp;requests.


## Documentation

Is in the form of a [compiled help file](https://github.com/jimkropa/misc.corlib/blob/master/docs/Help/MiscCorLib.chm),
and inline within the code itself. It will be included in Visual&nbsp;Studio intellisense if the XML documentation
comment file <tt>misc.corlib.XML</tt> is included adjacent to the reference.
