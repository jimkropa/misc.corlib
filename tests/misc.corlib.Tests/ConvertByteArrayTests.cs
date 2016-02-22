namespace MiscCorLib
{
	using System;
	using NUnit.Framework;

	/// <summary>
	/// Automated unit tests of the <see cref="ConvertByteArray"/> extention methods.
	/// </summary>
	[TestFixture]
	public sealed class ConvertByteArrayTests
	{
		[TestFixture]
		public sealed class ToBase64String
		{
			[Test]
			public void DisallowsNullByDefault()
			{
				// ReSharper disable once AssignNullToNotNullAttribute
				Assert.Throws<ArgumentNullException>(
					() => ConvertByteArray.ToBase64String(null));
			}

			[Test]
			public void ReturnsNullFromNull()
			{
				Assert.IsNull(ConvertByteArray.ToBase64String(null, true));
			}
		}

		[TestFixture]
		public sealed class ToHexadecimalString
		{
			
		}

		[TestFixture]
		public sealed class ToEncodedString
		{
			
		}

		[TestFixture]
		public sealed class ToText
		{
			
		}

		// ReSharper disable once InconsistentNaming
		[TestFixture]
		public sealed class ToASCII
		{

		}
	}
}