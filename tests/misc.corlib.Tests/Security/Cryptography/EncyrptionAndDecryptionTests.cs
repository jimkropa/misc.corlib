using System;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace MiscCorLib.Security.Cryptography
{
	public sealed class EncyrptionAndDecryptionTests
	{
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

					Assert.NotEqual(encryptionKey, wrongDecryptionKey);

					byte[] ciphertextBytes, iv;
					using (Encryptor encryptor = algorithm.CreateEncryptor(encryptionKey, out iv))
					{
						Assert.Equal(encryptionKey, encryptor.Algorithm.Key);
						Assert.Equal(iv, encryptor.Algorithm.IV);

						ciphertextBytes = encryptor.Encrypt(plaintextBytes);
					}

					using (Decryptor decryptor = new Decryptor(algorithm, encryptionKey, iv, Encryption.DefaultOptions))
					{
						Assert.Equal(encryptionKey, decryptor.Algorithm.Key);
						Assert.Equal(iv, decryptor.Algorithm.IV);

						decryptedBytes = decryptor.Decrypt(ciphertextBytes);
					}
				}

				Assert.Equal(plaintextBytes, decryptedBytes);
			}
		}

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

					Assert.NotEqual(encryptionKey, wrongDecryptionKey);

					byte[] ciphertextBytes, iv;
					using (Encryptor encryptor = algorithm.CreateEncryptor(encryptionKey, out iv))
					{
						Assert.Equal(encryptionKey, encryptor.Algorithm.Key);
						Assert.Equal(iv, encryptor.Algorithm.IV);

						ciphertextBytes = encryptor.Encrypt(plaintextBytes);
					}

					using (Decryptor decryptorWithWrongKey = new Decryptor(algorithm, wrongDecryptionKey, iv, Encryption.DefaultOptions))
					{
						Assert.Equal(wrongDecryptionKey, decryptorWithWrongKey.Algorithm.Key);
						Assert.Equal(iv, decryptorWithWrongKey.Algorithm.IV);

						try
						{
							decryptedBytesFromWrongKey = decryptorWithWrongKey.Decrypt(ciphertextBytes);
						}
						catch (CryptographicException e)
						{
							// "Padding is invalid and cannot be removed."
							Assert.Null(decryptedBytesFromWrongKey);

							Console.WriteLine(e.Message);
						}
					}

					using (Decryptor decryptor = new Decryptor(algorithm, encryptionKey, iv, Encryption.DefaultOptions))
					{
						Assert.Equal(encryptionKey, decryptor.Algorithm.Key);
						Assert.Equal(iv, decryptor.Algorithm.IV);

						decryptedBytes = decryptor.Decrypt(ciphertextBytes);
					}
				}

				Assert.NotEqual(decryptedBytes, decryptedBytesFromWrongKey);
				Assert.Equal(plaintextBytes, decryptedBytes);
			}
		}
	}
}