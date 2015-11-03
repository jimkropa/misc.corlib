namespace MiscCorLib.Security.Cryptography
{
	using System.Security.Cryptography;

	using JetBrains.Annotations;

	public sealed class Decryptor<T> : SymmetricTransformer<T>
		where T : SymmetricAlgorithm
	{
		public Decryptor([NotNull] T algorithm, byte[] encryptionKey, byte[] salt)
			: base(algorithm, false, encryptionKey, salt)
		{
		}

		public Decryptor(byte[] encryptionKey, byte[] salt)
			: base(false, encryptionKey, salt)
		{
		}

		public byte[] Decrypt(byte[] ciphertextBytes)
		{
			return this.Transform(ciphertextBytes);
		}
	}
}