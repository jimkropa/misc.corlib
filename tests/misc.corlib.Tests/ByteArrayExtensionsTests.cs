namespace MiscCorLib
{
	using System;

	using NUnit.Framework;

	[TestFixture]
	public sealed class ByteArrayExtensionsTests
	{
		[TestFixture]
		public sealed class ToBase64String
		{
			[Test]
			public void ThrowsExceptionForNullArray()
			{
				// ReSharper disable once AssignNullToNotNullAttribute
				Assert.Throws<ArgumentNullException>(
					() => ByteArrayExtensions.ToBase64String(null));
			}
		}

		[TestFixture]
		public sealed class ToHexadecimalString
		{
			[Test]
			public void ThrowsExceptionForNullArray()
			{
				// ReSharper disable once AssignNullToNotNullAttribute
				Assert.Throws<ArgumentNullException>(
					() => ByteArrayExtensions.ToHexadecimalString(null));
			}
		}
	}
}