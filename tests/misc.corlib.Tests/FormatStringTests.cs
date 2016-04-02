#region [ license and copyright boilerplate ]
/*
	MiscCorLib
	FormatStringTests.cs

	Copyright (c) 2016 Jim Kropa (https://github.com/jimkropa)

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

		http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/
#endregion

namespace MiscCorLib
{
	using NUnit.Framework;

	/// <summary>
	/// Automated unit tests of the <see cref="FormatString"/> extention methods.
	/// </summary>
	[TestFixture]
	public sealed class FormatStringTests
	{
		[TestFixture]
		public sealed class ToEmptyIfNull
		{
			[Test]
			public void Returns_Empty_From_Null()
			{
				Assert.IsEmpty(FormatString.ToEmptyIfNull(null));
			}

			[Test]
			public void Returns_Empty_From_Empty()
			{
				Assert.IsEmpty(string.Empty.ToEmptyIfNull());
			}

			[Test]
			public void Returns_Empty_From_WhiteSpace()
			{
				Assert.IsEmpty(@" 
  ".ToEmptyIfNull());
			}

			[Test]
			public void Returns_Trimmed_From_String()
			{
				Assert.AreEqual("hello, world", @"  hello, world 
  ".ToEmptyIfNull());
			}
		}

		[TestFixture]
		public sealed class ToNullIfEmpty
		{
			[Test]
			public void Returns_Null_From_Null()
			{
				Assert.IsNull(FormatString.ToNullIfEmpty(null));
			}

			[Test]
			public void Returns_Null_From_Empty()
			{
				Assert.IsNull(string.Empty.ToNullIfEmpty());
			}

			[Test]
			public void Returns_Null_From_WhiteSpace()
			{
				Assert.IsNull(@" 
  ".ToNullIfEmpty());
			}

			[Test]
			public void Returns_Trimmed_From_String()
			{
				Assert.AreEqual("hello, world", @"  hello, world 
  ".ToNullIfEmpty());
			}
		}

		[TestFixture]
		public sealed class ToCompactWhiteSpace
		{
			[Test]
			public void Returns_EmptyOrNull_From_Null()
			{
				Assert.IsEmpty(FormatString.ToCompactWhiteSpace(null));
				Assert.IsNull(FormatString.ToCompactWhiteSpace(null, true));
			}

			[Test]
			public void Returns_EmptyOrNull_From_Empty()
			{
				Assert.IsEmpty(string.Empty.ToCompactWhiteSpace());
				Assert.IsNull(string.Empty.ToCompactWhiteSpace(true));
			}

			[Test]
			public void Returns_EmptyOrNull_From_WhiteSpace()
			{
				Assert.IsEmpty(@" 
  ".ToCompactWhiteSpace());
				Assert.IsNull(@" 
  ".ToCompactWhiteSpace(true));
			}

			[Test]
			public void Returns_Trimmed_And_Compacted_From_String()
			{
				Assert.AreEqual("hello, world", @"    
 hello,     
 world   
  ".ToCompactWhiteSpace());

				Assert.AreEqual("hello, world",
					@"  hello,                                                                              world    ".ToCompactWhiteSpace());
			}
		}

		[TestFixture]
		public sealed class ToHtmlParagraph
		{
			[Test]
			public void Returns_EmptyOrNull_From_Null()
			{
				Assert.IsEmpty(FormatString.ToHtmlParagraph(null));
				Assert.IsEmpty(FormatString.ToHtmlParagraph(null, false));
				Assert.IsNull(FormatString.ToHtmlParagraph(null, true, true));
				Assert.IsNull(FormatString.ToHtmlParagraph(null, false, true));
			}

			[Test]
			public void Returns_EmptyOrNull_From_Empty()
			{
				Assert.IsEmpty(string.Empty.ToHtmlParagraph());
				Assert.IsEmpty(string.Empty.ToHtmlParagraph(false));
				Assert.IsNull(string.Empty.ToHtmlParagraph(true, true));
				Assert.IsNull(string.Empty.ToHtmlParagraph(false, true));
			}

			[Test]
			public void Returns_EmptyOrNull_From_WhiteSpace()
			{
				Assert.IsEmpty(@" 
  ".ToHtmlParagraph());
				Assert.IsEmpty(@" 
  ".ToHtmlParagraph(false));
				Assert.IsNull(@" 
  ".ToHtmlParagraph(true, true));
				Assert.IsNull(@" 
  ".ToHtmlParagraph(false, true));
			}

			[Test]
			public void Returns_Html_From_MultiLine_String()
			{
				Assert.AreEqual(@"<p>
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