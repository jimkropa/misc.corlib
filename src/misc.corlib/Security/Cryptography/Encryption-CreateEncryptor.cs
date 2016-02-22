namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;
	using System.Text;

	using JetBrains.Annotations;

	// Refer to Encryption.cs for documentation comments of partial class.
	public static partial class Encryption
	{
		public static Encryptor CreateEncryptor(
			[NotNull] SymmetricAlgorithm algorithm,
			[NotNull] byte[] encryptionKey,
			out byte[] initializationVector)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			return new Encryptor(algorithm, encryptionKey, out initializationVector);
		}

		public static Encryptor<T> CreateEncryptor<T>(
			[NotNull] byte[] encryptionKey,
			out byte[] initializationVector)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			// Set output parameter.
			return new Encryptor<T>(encryptionKey, out initializationVector);
		}

		public static Encryptor<T> CreateEncryptor<T>(
			[NotNull] string encryptionKey,
			out string initializationVector)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			return CreateEncryptor<T>(
				DefaultKeyEncoding,
				encryptionKey,
				DefaultByteArrayStringEncoding,
				out initializationVector);
		}

		public static Encryptor<T> CreateEncryptor<T>(
			[NotNull] Encoding keyEncoding,
			[NotNull] string encryptionKey,
			ByteArrayStringEncoding cipherEncoding,
			out string initializationVector)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(keyEncoding != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			byte[] ivbytes;
			Encryptor<T> encryptor = CreateEncryptor<T>(
				keyEncoding.GetBytes(encryptionKey), out ivbytes);

			initializationVector = ivbytes.ToEncodedString(cipherEncoding);

			return encryptor;
		}

		public static Encryptor CreateEncryptorFromPassword(
			[NotNull] SymmetricAlgorithm algorithm,
			[NotNull] string password,
			out byte[] salt,
			out byte[] initializationVector)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);

			return CreateEncryptorFromPassword(algorithm, password, DefaultSaltSize, out salt, out initializationVector);
		}

		public static Encryptor CreateEncryptorFromPassword(
			[NotNull] SymmetricAlgorithm algorithm,
			[NotNull] string password,
			int saltSize,
			out byte[] salt,
			out byte[] initializationVector)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);

			return new Encryptor(
				algorithm,
				DeriveEncryptionKeyFromPassword(password, algorithm.KeySize, saltSize, out salt),
				out initializationVector);
		}

		public static Encryptor CreateEncryptorFromPassword(
			[NotNull] SymmetricAlgorithm algorithm,
			[NotNull] byte[] initializationVector,
			[NotNull] string password,
			out byte[] salt)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(initializationVector != null);

			return CreateEncryptorFromPassword(algorithm, initializationVector, password, DefaultSaltSize, out salt);
		}

		public static Encryptor CreateEncryptorFromPassword(
			[NotNull] SymmetricAlgorithm algorithm,
			[NotNull] byte[] initializationVector,
			[NotNull] string password,
			int saltSize,
			out byte[] salt)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(initializationVector != null);

			return new Encryptor(
				algorithm,
				DeriveEncryptionKeyFromPassword(password, algorithm.KeySize, saltSize, out salt),
				initializationVector);
		}
	}
}