namespace MiscCorLib.Security.Cryptography
{
	using System.Security.Cryptography;

	using JetBrains.Annotations;

	public sealed class Encryptor<T> : SymmetricTransformer<T>
		where T : SymmetricAlgorithm
	{
		public Encryptor([NotNull] T algorithm, byte[] encryptionKey, byte[] salt)
			: base(algorithm, true, encryptionKey, salt)
		{
		}

		public Encryptor(byte[] encryptionKey, byte[] salt)
			: base(true, encryptionKey, salt)
		{
		}

		public byte[] Encrypt(byte[] plaintextBytes)
		{
			return this.Transform(plaintextBytes);
		}
	}
}