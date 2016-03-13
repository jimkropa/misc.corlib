namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Security.Cryptography;

	public static class TestObjectFactory
	{
		// Using the old Random class here for speed.
		// For generating numbers in production code,
		// use RNGCryptoServiceProvider.
		public static readonly Random RandomInstance = new Random();

		public static byte[] CreateEncryptionKey(int size = 32)
		{
			byte[] byteArray = new byte[size];

			RandomInstance.NextBytes(byteArray);

			return byteArray;
		}

		public static SymmetricAlgorithm CreateAlgorithm()
		{
			// Use AesManaged by default because it's easier on memory.
			// In production code use more compliant "crypto" flavor of AES.
			return new AesManaged();
		}

		public static Encryptor CreateEncryptor(out byte[] initializationVector)
		{
			return CreateEncryptor(CreateEncryptionKey(), out initializationVector);
		}

		public static Encryptor CreateEncryptor(byte[] encryptionKey, out byte[] initializationVector)
		{
			using (SymmetricAlgorithm algorithm = CreateAlgorithm())
			{
				return new Encryptor(algorithm, encryptionKey, out initializationVector, Encryption.DefaultOptions);
			}
		}
	}
}