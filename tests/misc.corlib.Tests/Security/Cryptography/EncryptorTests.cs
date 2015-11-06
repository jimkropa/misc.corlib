namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Security.Cryptography;

	using NUnit.Framework;

	[TestFixture]
	public sealed class EncryptorTests
	{
		[TestFixture]
		public sealed class Encrypt
		{
			[Test]
			public void ReturnsInitializationVector()
			{
				using (SymmetricAlgorithm algorithm = TestObjectFactory.CreateAlgorithm())
				{
					Console.Write("algorithm.Key: ");
					Console.WriteLine(algorithm.Key.ToBase64String(true));
					Console.Write("algorithm.Key.Length: ");
					Console.WriteLine(algorithm.Key.Length);
					Console.Write("algorithm.IV: ");
					Console.WriteLine(algorithm.IV.ToBase64String(true));
					Console.Write("algorithm.IV.Length: ");
					Console.WriteLine(algorithm.IV.Length);

					byte[] encryptionKey = TestObjectFactory.CreateEncryptionKey();
					byte[] initializationVector;
					using (Encryptor encryptor = new Encryptor(algorithm, encryptionKey, out initializationVector))
					{
						Console.WriteLine();
						Console.Write("encryptionKey: ");
						Console.WriteLine(encryptionKey.ToBase64String(true));

						Console.WriteLine();
						Console.Write("algorithm.Key: ");
						Console.WriteLine(algorithm.Key.ToBase64String(true));
						////Console.Write("encryptor.EncryptionKey: ");
						////Console.WriteLine(encryptor.EncryptionKey.ToBase64String(true));

						Console.WriteLine();
						Console.Write("algorithm.IV: ");
						Console.WriteLine(algorithm.IV.ToBase64String(true));
						Console.Write("encryptor.InitializationVector: ");
						Console.WriteLine(initializationVector.ToBase64String(true));
					}
				}

				using (Encryptor<SymmetricAlgorithm> encryptor = TestObjectFactory.CreateEncryptor())
				{
					
				}
			}
		}


	}
}