using System;
using Xunit;

namespace MiscCorLib.Security.Cryptography
{
	public sealed class EncryptionTests
	{
		public sealed class DeriveEncryptionKey
		{
			private const int KeySize = 192;
			private const int BlockSize = 64;

			// Use NewGuid to generate a semi-random
			// string value to use as a test password.
			private string testPassword = Guid.NewGuid().ToString("N").ToLowerInvariant();

			// TODO: Xunit [TestFixtureSetUp]
			public void SetRandomPassword()
			{
				this.testPassword = Guid.NewGuid().ToString("N").ToLowerInvariant();
			}

			[Fact]
			public void Derives_Same_Key_Using_Original_Salt()
			{
				byte[] originalKey = Encryption.DeriveEncryptionKeyAndSaltFromPassword(
					this.testPassword, KeySize, BlockSize, out var salt);

				byte[] recreatedKey = Encryption.DeriveEncryptionKeyFromPasswordAndSalt(
					this.testPassword, KeySize, salt);

				Assert.Equal(originalKey, recreatedKey);
			}

			[Fact]
			public void Derives_Different_Key_Using_Different_Salt()
			{
				byte[] originalKey = Encryption.DeriveEncryptionKeyAndSaltFromPassword(
					this.testPassword, KeySize, BlockSize, out var salt);

				byte[] keyFromDifferentSalt = Encryption.DeriveEncryptionKeyAndSaltFromPassword(
					this.testPassword, KeySize, BlockSize, out var differentSalt);

				Assert.Equal(salt, differentSalt);
				Assert.Equal(originalKey, keyFromDifferentSalt);
			}

			[Fact]
			public void Uses_Case_Sensitive_Password()
			{
				byte[] originalKey = Encryption.DeriveEncryptionKeyAndSaltFromPassword(
					this.testPassword, KeySize, BlockSize, out var salt);

				byte[] keyFromUpperCasePassword = Encryption.DeriveEncryptionKeyFromPasswordAndSalt(
					this.testPassword.ToUpperInvariant(), KeySize, salt);

				Assert.Equal(originalKey, keyFromUpperCasePassword);
			}
		}
	}
}