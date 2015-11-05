namespace MiscCorLib.Security.Cryptography
{
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;

	using JetBrains.Annotations;

	public sealed class Encryptor : Encryptor<SymmetricAlgorithm>
	{
		public Encryptor(SymmetricAlgorithm algorithm, byte[] encryptionKey, out byte[] initializationVector)
			: base(algorithm, encryptionKey, out initializationVector)
		{
			Contract.Requires(algorithm != null);
			Contract.Requires(encryptionKey != null);
			Contract.Ensures(encryptionKey != null);
		}

		public Encryptor(SymmetricAlgorithm algorithm, byte[] encryptionKey)
			: base(algorithm, encryptionKey)
		{
			Contract.Requires(algorithm != null);
			Contract.Requires(encryptionKey != null);
		}
	}

	public class Encryptor<T> : SymmetricTransformer<T>
		where T : SymmetricAlgorithm
	{
		public Encryptor([NotNull] T algorithm, byte[] encryptionKey, out byte[] initializationVector)
			: base(algorithm, encryptionKey, null)
		{
			Contract.Requires(algorithm != null);
			Contract.Requires(encryptionKey != null);
			Contract.Ensures(encryptionKey != null);

			initializationVector = algorithm.IV;
		}

		public Encryptor([NotNull] T algorithm, byte[] encryptionKey)
			: base(algorithm, encryptionKey, null)
		{
			Contract.Requires(algorithm != null);
			Contract.Requires(encryptionKey != null);
		}

		public Encryptor(byte[] encryptionKey)
			: base(encryptionKey, null)
		{
			Contract.Requires(encryptionKey != null);
		}

		public byte[] InitializationVector { get { return this.Algorithm.IV; } }

		public byte[] Encrypt(byte[] plaintextBytes)
		{
			return this.Transform(plaintextBytes);
		}
	}
}