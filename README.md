# misc.corlib

is a small assortment of adapters and extensions from <tt>mscorlib.dll</tt>

> These are some useful types that I've rewritten
> enough times to not want to write them over again,
> so I have composed them into this open source project.

The types in this library are for core infrastructure and aim
to be as stable and reliable as the .NET&nbsp;Framework itself,
including optimized performance, test&nbsp;coverage, and detailed inline documentation.
You&nbsp;should be able to consider a reference to **misc.corlib**
as [what Mark&nbsp;Seemann would call a "stable"
(non&#8209;volatile)&nbsp;depency](http://blogs.msdn.com/b/ploeh/archive/2006/08/24/718828.aspx).

As&nbsp;such, there is no particular emphasis on abstraction
or points of dependency injection. The&nbsp;concrete Encryptor and
Decryptor classes in the "Security.Cryptography" namespace,
for&nbsp;example, do&nbsp;not specify an abstract contract because
they are basically thin facades over a pairing of .NET&nbsp;Framework types,
designed to guide an optimum implementation with a bit less ceremony, so it's
more practical to treat them as concrete types.
Besides, they are already amply variable by their internal use
of the [abstract factory within the SymmetricAlgorithm
class](https://msdn.microsoft.com/en-us/library/k74a682y(v=vs.110).aspx)
in&nbsp;<tt>mscorlib.dll</tt>

Since the types in this library target specific parts of the
.NET&nbsp;Framework, their namespaces mimic the&nbsp;same&nbsp;structure.


## Features

This library consists of:

- Simplified generic encryption, decryption, and hashing of strings or byte arrays, as flexible as you need it to be, but guiding you to the&nbsp;most&nbsp;secure&nbsp;implementation
- Contracts and structs for managing "paged"&nbsp;lists
- Transformations back and forth between comma-delimited strings and arrays&nbsp;of&nbsp;primitives
- Extraction of primitive values from NameValueCollection, useful for old&#8209;school HttpRequest.QueryString&nbsp;parsing
- String formatting for various output or sanitization

I'm happy to make this library available publicly under the Apache&nbsp;2&nbsp;License,
and am open to suggestions for improvement. Please feel to fork away, use as much or
as little as you like, and submit&nbsp;pull&nbsp;requests.
