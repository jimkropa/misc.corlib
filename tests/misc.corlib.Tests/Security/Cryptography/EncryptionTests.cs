#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Security.Cryptography
	EncryptionTests.cs

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

namespace MiscCorLib.Security.Cryptography
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

			[Fact]
			public void Derives_Same_Key_Using_Original_Salt()
			{
				byte[] salt;
				byte[] originalKey = Encryption.DeriveEncryptionKeyAndSaltFromPassword(
					this.testPassword, KeySize, BlockSize, out salt);

				byte[] recreatedKey = Encryption.DeriveEncryptionKeyFromPasswordAndSalt(
					this.testPassword, KeySize, salt);

				Assert.AreEqual(originalKey, recreatedKey);
			}

			[Fact]
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

			[Fact]
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