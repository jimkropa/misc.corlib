﻿namespace MiscCorLib.Security.Cryptography
{
	using System;

	using NUnit.Framework;

	[TestFixture]
	public sealed class EncryptionTests
	{
		[TestFixture]
		public sealed class DeriveEncryptionKey
		{
			private const int KeySize = 192;
			private const int BlockSize = 64;

			// Use NewGuid to generate a semi-random
			// string value to use as a test password.
			private string testPassword = Guid.NewGuid().ToString("N").ToLowerInvariant();

			[TestFixtureSetUp]
			public void SetRandomPassword()
			{
				this.testPassword = Guid.NewGuid().ToString("N").ToLowerInvariant();
			}

			[Test]
			public void Derives_Same_Key_Using_Original_Salt()
			{
				byte[] salt;
				byte[] originalKey = Encryption.DeriveEncryptionKeyAndSaltFromPassword(
					this.testPassword, KeySize, BlockSize, out salt);

				byte[] recreatedKey = Encryption.DeriveEncryptionKeyFromPasswordAndSalt(
					this.testPassword, KeySize, salt);

				Assert.AreEqual(originalKey, recreatedKey);
			}

			[Test]
			public void Derives_Different_Key_Using_Different_Salt()
			{
				byte[] salt;
				byte[] originalKey = Encryption.DeriveEncryptionKeyAndSaltFromPassword(
					this.testPassword, KeySize, BlockSize, out salt);
				
				byte[] differentSalt;
				byte[] keyFromDifferentSalt = Encryption.DeriveEncryptionKeyAndSaltFromPassword(
					this.testPassword, KeySize, BlockSize, out differentSalt);

				Assert.AreNotEqual(salt, differentSalt);
				Assert.AreNotEqual(originalKey, keyFromDifferentSalt);
			}

			[Test]
			public void Uses_Case_Sensitive_Password()
			{
				byte[] salt;
				byte[] originalKey = Encryption.DeriveEncryptionKeyAndSaltFromPassword(
					this.testPassword, KeySize, BlockSize, out salt);

				byte[] keyFromUpperCasePassword = Encryption.DeriveEncryptionKeyFromPasswordAndSalt(
					this.testPassword.ToUpperInvariant(), KeySize, salt);

				Assert.AreNotEqual(originalKey, keyFromUpperCasePassword);
			}
		}
	}
}