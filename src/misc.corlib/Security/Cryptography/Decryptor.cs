namespace MiscCorLib.Security.Cryptography
{
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;

	using JetBrains.Annotations;

	public sealed class Decryptor : Decryptor<SymmetricAlgorithm>
	{
		public Decryptor(SymmetricAlgorithm algorithm, byte[] encryptionKey, byte[] initializationVector)
			: base(algorithm, encryptionKey, initializationVector)
		{
		}
	}

	public class Decryptor<T> : SymmetricTransformer<T>
		where T : SymmetricAlgorithm
	{
		public Decryptor([NotNull] T algorithm, byte[] encryptionKey, byte[] initializationVector)
			: base(algorithm, true, encryptionKey, initializationVector)
		{
			Contract.Requires(algorithm != null);
			Contract.Requires(encryptionKey != null);
			Contract.Requires(initializationVector != null);
		}

		public Decryptor(byte[] encryptionKey, byte[] initializationVector)
			: base(true, encryptionKey, initializationVector)
		{
			Contract.Requires(encryptionKey != null);
			Contract.Requires(initializationVector != null);
		}

		public byte[] Decrypt(byte[] ciphertextBytes)
		{
			return this.Transform(ciphertextBytes);
		}
	}
}