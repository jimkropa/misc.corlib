namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Security.Cryptography;

	using NUnit.Framework;

	[TestFixture]
	public sealed class StringHashingExtensionsTests
	{
		[TestFixture]
		public sealed class CalculateHash
		{
		}

		[TestFixture]
		public sealed class HashToHexadecimalString
		{
			// https://www.dlitz.net/crypto/shad256-test-vectors/
			// https://www.dlitz.net/crypto/shad256-test-vectors/SHAd256_Test_Vectors.txt
			// http://www.di-mgt.com.au/sha_testvectors.html
			// http://www.febooti.com/products/filetweak/members/hash-and-crc/test-vectors/
			// ReSharper disable InconsistentNaming
			private const string EmptySHA1 = @"da39a3ee5e6b4b0d3255bfef95601890afd80709";
			private const string EmptySHA256 = @"e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
			private const string EmptySHA384 = @"38b060a751ac96384cd9327eb1b1e36a21fdb71114be07434c0cc7bf63f6e1da274edebfe76f65fbd51ad2f14898b95b";
			private const string EmptySHA512 = @"cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e";
			// ReSharper restore InconsistentNaming

			[Test, Ignore]
			public void CalculatesCorrectHashesForKnownTestVectors()
			{
			}

			[Test]
			public void CalculatesKnownHashesForEmptyString()
			{
				Assert.AreEqual(EmptySHA1, string.Empty.HashToHexadecimalString<SHA1>());
				Assert.AreEqual(EmptySHA256, string.Empty.HashToHexadecimalString<SHA256>());
				Assert.AreEqual(EmptySHA384, string.Empty.HashToHexadecimalString<SHA384>());
				Assert.AreEqual(EmptySHA512, string.Empty.HashToHexadecimalString<SHA512>());

				Assert.AreEqual(EmptySHA1, string.Empty.HashToHexadecimalString<SHA1Managed>());
				Assert.AreEqual(EmptySHA256, string.Empty.HashToHexadecimalString<SHA256Managed>());
				Assert.AreEqual(EmptySHA384, string.Empty.HashToHexadecimalString<SHA384Managed>());
				Assert.AreEqual(EmptySHA512, string.Empty.HashToHexadecimalString<SHA512Managed>());
			}

			[Test]
			public void ThrowsExceptionForNullString()
			{
				Assert.Throws<ArgumentNullException>(
					() => StringHashing.HashToHexadecimalString<SHA1>(null));
			}
		}

		[TestFixture]
		public sealed class HashToBase64String
		{
			// ReSharper disable InconsistentNaming
			private const string EmptySHA1 = @"da39a3ee5e6b4b0d3255bfef95601890afd80709";
			private const string EmptySHA256 = @"e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
			private const string EmptySHA384 = @"38b060a751ac96384cd9327eb1b1e36a21fdb71114be07434c0cc7bf63f6e1da274edebfe76f65fbd51ad2f14898b95b";
			private const string EmptySHA512 = @"cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e";
			// ReSharper restore InconsistentNaming

			public void CalculatesCorrectHashesForKnownTestVectors()
			{
			}

			[Test]
			public void CalculatesKnownHashesForEmptyString()
			{
				Assert.AreEqual(EmptySHA1, string.Empty.HashToBase64String<SHA1>());
				Assert.AreEqual(EmptySHA256, string.Empty.HashToBase64String<SHA256>());
				Assert.AreEqual(EmptySHA384, string.Empty.HashToBase64String<SHA384>());
				Assert.AreEqual(EmptySHA512, string.Empty.HashToBase64String<SHA512>());

				Assert.AreEqual(EmptySHA1, string.Empty.HashToBase64String<SHA1Managed>());
				Assert.AreEqual(EmptySHA256, string.Empty.HashToBase64String<SHA256Managed>());
				Assert.AreEqual(EmptySHA384, string.Empty.HashToBase64String<SHA384Managed>());
				Assert.AreEqual(EmptySHA512, string.Empty.HashToBase64String<SHA512Managed>());
			}

			public void ThrowsExceptionForNullString()
			{
				Assert.Throws<ArgumentNullException>(
					() => StringHashing.HashToBase64String<SHA1>(null));
			}
		}
	}
}