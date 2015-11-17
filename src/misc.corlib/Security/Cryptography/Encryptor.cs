namespace MiscCorLib.Security.Cryptography
{
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;
	using System.Text;

	using JetBrains.Annotations;

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// <para>
	/// This one is slightly more memory efficient.
	/// Allows recycling of a single algorithm instance.
	/// </para>
	/// </remarks>
	public sealed class Encryptor : Encryptor<SymmetricAlgorithm>
	{
		internal Encryptor(
			[NotNull] SymmetricAlgorithm algorithm,
			[NotNull] byte[] encryptionKey,
			out byte[] initializationVector)
			: base(algorithm, encryptionKey, out initializationVector)
		{
			Contract.Requires(algorithm != null);
			Contract.Requires(encryptionKey != null);
			Contract.Ensures(initializationVector != null);
		}

		internal Encryptor(
		[NotNull] SymmetricAlgorithm algorithm,
		[NotNull] byte[] encryptionKey,
		[NotNull] byte[] initializationVector)
			: base(algorithm, encryptionKey, initializationVector)
		{
			Contract.Requires(algorithm != null);
			Contract.Requires(encryptionKey != null);
			Contract.Requires(initializationVector != null);
		}
	}

	public class Encryptor<T> : SymmetricTransformer<T>
		where T : SymmetricAlgorithm
	{
		internal Encryptor(
			[NotNull] T algorithm,
			[NotNull] byte[] encryptionKey,
			out byte[] initializationVector)
			: base(algorithm, true, encryptionKey, null)
		{
			Contract.Requires(algorithm != null);
			Contract.Requires(encryptionKey != null);

			// Set output parameter.
			initializationVector = this.Algorithm.IV;

			////	Contract.Ensures(initializationVector != null);
		}

		internal Encryptor(
			[NotNull] T algorithm,
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] initializationVector)
			: base(algorithm, true, encryptionKey, initializationVector)
		{
			Contract.Requires(algorithm != null);
			Contract.Requires(encryptionKey != null);
			Contract.Requires(initializationVector != null);
		}

		internal Encryptor(
			[NotNull] byte[] encryptionKey,
			out byte[] initializationVector)
			: base(true, encryptionKey, null)
		{
			Contract.Requires(encryptionKey != null);

			// Set output parameter.
			initializationVector = this.Algorithm.IV;

			////	Contract.Ensures(initializationVector != null);
		}

		internal Encryptor(
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] initializationVector)
			: base(true, encryptionKey, initializationVector)
		{
			Contract.Requires(encryptionKey != null);
			Contract.Requires(initializationVector != null);
		}

		public byte[] Encrypt([NotNull] byte[] plaintextBytes)
		{
			return this.Transform(plaintextBytes);
		}

		public string EncryptToString(string plaintext)
		{
			return this.EncryptToString(
				plaintext, Encryption.DefaultTextEncoding, Encryption.DefaultCipherEncoding);
		}

		public string EncryptToString(string plaintext, CipherEncoding encoding)
		{
			return this.EncryptToString(
				plaintext, Encryption.DefaultTextEncoding, encoding);
		}

		public string EncryptToString(string plaintext, [NotNull] Encoding encoding)
		{
			return this.EncryptToString(
				plaintext, encoding, Encryption.DefaultCipherEncoding);
		}
		
		public string EncryptToString(
			string plaintext, [NotNull] Encoding textEncoding, CipherEncoding cipherEncoding)
		{
			return this.EncryptToString(
				textEncoding.GetBytes(plaintext), cipherEncoding);
		}

		public string EncryptToString([NotNull] byte[] plaintextBytes)
		{
			return this.EncryptToString(
				plaintextBytes, Encryption.DefaultCipherEncoding);
		}

		public string EncryptToString(
			[NotNull] byte[] plaintextBytes, CipherEncoding encoding)
		{
			return encoding == CipherEncoding.Hexadecimal
				? this.Transform(plaintextBytes).ToHexadecimalString()
				: this.Transform(plaintextBytes).ToBase64String();
		}
	}
}