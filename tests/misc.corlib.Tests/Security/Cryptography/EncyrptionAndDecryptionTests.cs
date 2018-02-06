#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Security.Cryptography
	EncyrptionAndDecryptionTests.cs

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
	using System.Security.Cryptography;
	using System.Text;

	using NUnit.Framework;

	[TestFixture]
	public sealed class EncyrptionAndDecryptionTests
	{
		[TestFixture]
		public sealed class SameEncryptionKey
		{
			[Fact]
			public void DecryptsToOriginalPlainText()
			{
				byte[] plaintextBytes = Encoding.UTF8.GetBytes("This is a test! It needs to be 128 characters long at least. This is a test! It needs to be 128 characters long at least. This is a test! It needs to be 128 characters long at least. This is a test! It needs to be 128 characters long at least. This is a test! It needs to be 128 characters long at least. This is a test! It needs to be 128 characters long at least. This is a test! It needs to be 128 characters long at least. This is a test! It needs to be 128 characters long at least.");
				byte[] decryptedBytes;

				using (SymmetricAlgorithm algorithm = new AesManaged())
				{
					byte[] wrongDecryptionKey = algorithm.Key;

					algorithm.GenerateKey();

					byte[] encryptionKey = algorithm.Key;

					Assert.AreNotEqual(encryptionKey, wrongDecryptionKey);

					byte[] ciphertextBytes, iv;
					using (Encryptor encryptor = algorithm.CreateEncryptor(encryptionKey, out iv))
					{
						Assert.AreEqual(encryptionKey, encryptor.Algorithm.Key);
						Assert.AreEqual(iv, encryptor.Algorithm.IV);

						ciphertextBytes = encryptor.Encrypt(plaintextBytes);
					}

					using (Decryptor decryptor = new Decryptor(algorithm, encryptionKey, iv, Encryption.DefaultOptions))
					{
						Assert.AreEqual(encryptionKey, decryptor.Algorithm.Key);
						Assert.AreEqual(iv, decryptor.Algorithm.IV);

						decryptedBytes = decryptor.Decrypt(ciphertextBytes);
					}
				}

				Assert.AreEqual(plaintextBytes, decryptedBytes);
			}
		}

		[TestFixture]
		public sealed class UsingDifferentEncryptionKey 
		{
			[Fact]
			public void CausesDecryptionToReturnNonsense()
			{
				byte[] plaintextBytes = Encoding.UTF8.GetBytes("This is a test!");
				byte[] decryptedBytes, decryptedBytesFromWrongKey = null;

				using (SymmetricAlgorithm algorithm = new AesManaged())
				{
					byte[] wrongDecryptionKey = algorithm.Key;

					algorithm.GenerateKey();

					byte[] encryptionKey = algorithm.Key;

					Assert.AreNotEqual(encryptionKey, wrongDecryptionKey);

					byte[] ciphertextBytes, iv;
					using (Encryptor encryptor = algorithm.CreateEncryptor(encryptionKey, out iv))
					{
						Assert.AreEqual(encryptionKey, encryptor.Algorithm.Key);
						Assert.AreEqual(iv, encryptor.Algorithm.IV);

						ciphertextBytes = encryptor.Encrypt(plaintextBytes);
					}

					using (Decryptor decryptorWithWrongKey = new Decryptor(algorithm, wrongDecryptionKey, iv, Encryption.DefaultOptions))
					{
						Assert.AreEqual(wrongDecryptionKey, decryptorWithWrongKey.Algorithm.Key);
						Assert.AreEqual(iv, decryptorWithWrongKey.Algorithm.IV);

						try
						{
							decryptedBytesFromWrongKey = decryptorWithWrongKey.Decrypt(ciphertextBytes);
						}
						catch (CryptographicException e)
						{
							// "Padding is invalid and cannot be removed."
							Assert.IsNull(decryptedBytesFromWrongKey);

							Console.WriteLine(e.Message);
						}
					}

					using (Decryptor decryptor = new Decryptor(algorithm, encryptionKey, iv, Encryption.DefaultOptions))
					{
						Assert.AreEqual(encryptionKey, decryptor.Algorithm.Key);
						Assert.AreEqual(iv, decryptor.Algorithm.IV);

						decryptedBytes = decryptor.Decrypt(ciphertextBytes);
					}
				}

				Assert.AreNotEqual(decryptedBytes, decryptedBytesFromWrongKey);
				Assert.AreEqual(plaintextBytes, decryptedBytes);
			}
		}
	}
}