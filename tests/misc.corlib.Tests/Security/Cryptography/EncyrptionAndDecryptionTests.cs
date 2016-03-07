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
			[Test]
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

					using (Decryptor decryptor = new Decryptor(algorithm, encryptionKey, iv))
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
			[Test]
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

					using (Decryptor decryptorWithWrongKey = new Decryptor(algorithm, wrongDecryptionKey, iv))
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

					using (Decryptor decryptor = new Decryptor(algorithm, encryptionKey, iv))
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