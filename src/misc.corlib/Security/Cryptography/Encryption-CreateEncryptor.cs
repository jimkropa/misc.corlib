namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Security.Cryptography;

	// Refer to Encryption.cs for documentation comments of partial class.
	public static partial class Encryption
	{
		public static Encryptor CreateEncryptor()
		{
			throw new NotImplementedException();
		}

		public static Encryptor<T> CreateEncryptor<T>()
			where T : SymmetricAlgorithm
		{
			throw new NotImplementedException();
		}
	}
}