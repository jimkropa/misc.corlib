using Xunit;

namespace MiscCorLib
{
	/// <summary>
	/// Automated unit tests of the <see cref="FormatString" /> extention methods.
	/// </summary>
	public sealed class FormatStringTests
	{
		public sealed class ToEmptyIfNull
		{
			[Fact]
			public void Returns_Empty_From_Null()
			{
				Assert.Empty(FormatString.ToEmptyIfNull(null));
			}

			[Fact]
			public void Returns_Empty_From_Empty()
			{
				Assert.Empty(string.Empty.ToEmptyIfNull());
			}

			[Fact]
			public void Returns_Empty_From_WhiteSpace()
			{
				Assert.Empty(@" 
  ".ToEmptyIfNull());
			}

			[Fact]
			public void Returns_Trimmed_From_String()
			{
				Assert.Equal("hello, world", @"  hello, world 
  ".ToEmptyIfNull());
			}
		}

		public sealed class ToNullIfEmpty
		{
			[Fact]
			public void Returns_Null_From_Null()
			{
				Assert.Null(FormatString.ToNullIfEmpty(null));
			}

			[Fact]
			public void Returns_Null_From_Empty()
			{
				Assert.Null(string.Empty.ToNullIfEmpty());
			}

			[Fact]
			public void Returns_Null_From_WhiteSpace()
			{
				Assert.Null(@" 
  ".ToNullIfEmpty());
			}

			[Fact]
			public void Returns_Trimmed_From_String()
			{
				Assert.Equal("hello, world", @"  hello, world 
  ".ToNullIfEmpty());
			}
		}

		public sealed class ToCompactWhiteSpace
		{
			[Fact]
			public void Returns_EmptyOrNull_From_Null()
			{
				Assert.Empty(FormatString.ToCompactWhiteSpace(null));
				Assert.Null(FormatString.ToCompactWhiteSpace(null, true));
			}

			[Fact]
			public void Returns_EmptyOrNull_From_Empty()
			{
				Assert.Empty(string.Empty.ToCompactWhiteSpace());
				Assert.Null(string.Empty.ToCompactWhiteSpace(true));
			}

			[Fact]
			public void Returns_EmptyOrNull_From_WhiteSpace()
			{
				Assert.Empty(@" 
  ".ToCompactWhiteSpace());
				Assert.Null(@" 
  ".ToCompactWhiteSpace(true));
			}

			[Fact]
			public void Returns_Trimmed_And_Compacted_From_String()
			{
				Assert.Equal("hello, world", @"    
 hello,     
 world   
  ".ToCompactWhiteSpace());

				Assert.Equal("hello, world",
					@"  hello,                                                                              world    ".ToCompactWhiteSpace());
			}
		}

		public sealed class ToHtmlParagraph
		{
			[Fact]
			public void Returns_EmptyOrNull_From_Null()
			{
				Assert.Empty(FormatString.ToHtmlParagraph(null));
				Assert.Empty(FormatString.ToHtmlParagraph(null, false));
				Assert.Null(FormatString.ToHtmlParagraph(null, true, true));
				Assert.Null(FormatString.ToHtmlParagraph(null, false, true));
			}

			[Fact]
			public void Returns_EmptyOrNull_From_Empty()
			{
				Assert.Empty(string.Empty.ToHtmlParagraph());
				Assert.Empty(string.Empty.ToHtmlParagraph(false));
				Assert.Null(string.Empty.ToHtmlParagraph(true, true));
				Assert.Null(string.Empty.ToHtmlParagraph(false, true));
			}

			[Fact]
			public void Returns_EmptyOrNull_From_WhiteSpace()
			{
				Assert.Empty(@" 
  ".ToHtmlParagraph());
				Assert.Empty(@" 
  ".ToHtmlParagraph(false));
				Assert.Null(@" 
  ".ToHtmlParagraph(true, true));
				Assert.Null(@" 
  ".ToHtmlParagraph(false, true));
			}

			[Fact]
			public void Returns_Html_From_MultiLine_String()
			{
				Assert.Equal(@"<p>
hello,
<br />
world
</p>
<p>
I'm a little teapot, short and stout.
</p>
<p>
Here is my handle. Here is my spout.
</p>", @"    
 hello,     
 world   




I'm a little teapot, short and   stout.

Here is my handle.     Here is my spout.
  ".ToHtmlParagraph());
			}
		}
	}
}