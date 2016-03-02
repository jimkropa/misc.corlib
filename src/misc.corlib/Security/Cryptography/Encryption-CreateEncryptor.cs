namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;

	using JetBrains.Annotations;

	// Refer to Encryption.cs for documentation comments of partial class.
	public static partial class Encryption
	{
		#region [ Overloads of CreateEncryptor Extension Method for SymmetricAlgorithm ]

		public static Encryptor CreateEncryptor(
			[NotNull] this SymmetricAlgorithm algorithm,
			[NotNull] byte[] encryptionKey,
			out byte[] salt,
			bool allowNulls = DefaultAllowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			return new Encryptor(algorithm, encryptionKey, out salt, allowNulls);
		}

		public static Encryptor CreateEncryptor(
			[NotNull] this SymmetricAlgorithm algorithm,
			[NotNull] string password,
			out byte[] randomSalt,
			bool allowNulls = DefaultAllowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(password != null);

			// In this case, use the more robustly random
			// key derivation algorithm to create the salt,
			// instead of the SymmetricAlgorithm.
			return algorithm.CreateEncryptorWithGivenSalt(
				DeriveEncryptionKeyAndSaltFromPassword(password, algorithm.KeySize, algorithm.BlockSize, out randomSalt),
				randomSalt,
				allowNulls);
		}

		public static Encryptor CreateEncryptor(
			[NotNull] this SymmetricAlgorithm algorithm,
			[NotNull] string password,
			out string randomSalt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			bool allowNulls = DefaultAllowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(password != null);

			byte[] randomSaltBytes;
			Encryptor encryptor = algorithm.CreateEncryptor(password, out randomSaltBytes, allowNulls);

			randomSalt = randomSaltBytes.ToEncodedString(saltEncoding);

			return encryptor;
		}

		#endregion

		#region [ Overloads of CreateEncryptor Extension Method using a given Salt ]

		public static Encryptor CreateEncryptorWithGivenSalt(
			[NotNull] this SymmetricAlgorithm algorithm,
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] salt,
			bool allowNulls = DefaultAllowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			return new Encryptor(algorithm, encryptionKey, salt, allowNulls);
		}

		public static Encryptor CreateEncryptorWithGivenSalt(
			[NotNull] this SymmetricAlgorithm algorithm,
			[NotNull] string password,
			[NotNull] byte[] salt,
			bool allowNulls = DefaultAllowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(password != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			return algorithm.CreateEncryptorWithGivenSalt(
				DeriveEncryptionKeyFromPasswordAndSalt(password, algorithm.KeySize, salt),
				salt,
				allowNulls);
		}

		public static Encryptor CreateEncryptorWithGivenSalt(
			[NotNull] this SymmetricAlgorithm algorithm,
			[NotNull] string password,
			[NotNull] string salt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			bool allowNulls = DefaultAllowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(password != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			return algorithm.CreateEncryptorWithGivenSalt(password, salt.ToByteArray(saltEncoding), allowNulls);
		}

		#endregion

		#region [ Overloads of Generic CreateEncryptor Method ]

		public static Encryptor<T> CreateEncryptor<T>(
			[NotNull] byte[] encryptionKey,
			out byte[] salt,
			bool allowNulls = DefaultAllowNulls)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			return new Encryptor<T>(encryptionKey, out salt, allowNulls);
		}

		public static Encryptor<T> CreateEncryptor<T>(
			[NotNull] string password,
			out byte[] randomSalt,
			bool allowNulls = DefaultAllowNulls)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(password != null);

			int keySize, blockSize;
			using (T algorithm = SymmetricTransformer<T>.CreateAlgorithm())
			{
				keySize = algorithm.KeySize;
				blockSize = algorithm.BlockSize;
			}

			return new Encryptor<T>(
				DeriveEncryptionKeyAndSaltFromPassword(password, keySize, blockSize, out randomSalt),
				randomSalt,
				allowNulls);
		}

		public static Encryptor<T> CreateEncryptor<T>(
			[NotNull] string password,
			out string randomSalt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			bool allowNulls = DefaultAllowNulls)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(password != null);

			byte[] randomSaltBytes;
			Encryptor<T> encryptor = CreateEncryptor<T>(password, out randomSaltBytes, allowNulls);

			randomSalt = randomSaltBytes.ToEncodedString(saltEncoding);

			return encryptor;
		}

		#endregion

		#region [ Overloads of Generic CreateEncryptor Method using a given Salt ]

		public static Encryptor<T> CreateEncryptorWithGivenSalt<T>(
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] salt,
			bool allowNulls = DefaultAllowNulls)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			return new Encryptor<T>(encryptionKey, salt, allowNulls);
		}

		public static Encryptor<T> CreateEncryptorWithGivenSalt<T>(
			[NotNull] string password,
			[NotNull] byte[] salt,
			bool allowNulls = DefaultAllowNulls)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(password != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			int keySize;
			using (T algorithm = SymmetricTransformer<T>.CreateAlgorithm())
			{
				keySize = algorithm.KeySize;
			}

			return new Encryptor<T>(
				DeriveEncryptionKeyFromPasswordAndSalt(password, keySize, salt),
				salt,
				allowNulls);
		}

		public static Encryptor<T> CreateEncryptorWithGivenSalt<T>(
			[NotNull] string password,
			[NotNull] string salt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			bool allowNulls = DefaultAllowNulls)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(password != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			return CreateEncryptorWithGivenSalt<T>(password, salt.ToByteArray(allowNulls, saltEncoding));
		}

		#endregion
	}
}