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
		public sealed class ToSingleLine
		{
			[Test]
			public void Returns_Empty_From_Null()
			{
				Assert.IsEmpty(FormatString.ToSingleLine(null));
			}

			[Test]
			public void Returns_Empty_From_Empty()
			{
				Assert.IsEmpty(string.Empty.ToSingleLine());
			}

			[Test]
			public void Returns_Empty_From_WhiteSpace()
			{
				Assert.IsEmpty(@" 
  ".ToSingleLine());
			}

			[Test]
			public void Returns_Trimmed_And_Compacted_From_String()
			{
				Assert.AreEqual("hello, world", @"    
 hello,     
 world   
  ".ToSingleLine());

				Assert.AreEqual("hello, world", @"  hello,                                                                              world    ".ToSingleLine());
			}
		}
	}
}