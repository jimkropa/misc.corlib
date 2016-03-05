namespace MiscCorLib.Security.Cryptography
{
	using System.Security.Cryptography;
	using System.Text;

	using NUnit.Framework;

	[TestFixture]
	public sealed class EncyrptionAndDecryptionTests
	{
		[TestFixture]
		public sealed class UsingDifferentEncryptionKey 
		{
			[Test]
			public void CausesDecryptionToReturnNonsense()
			{
				byte[] plaintextBytes = Encoding.UTF8.GetBytes("This is a test!");
				byte[] decryptedBytes, decryptedBytesFromWrongKey;

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

						decryptedBytesFromWrongKey = decryptorWithWrongKey.Decrypt(ciphertextBytes);
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