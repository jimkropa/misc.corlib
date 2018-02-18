using System;
using System.Security.Cryptography;

namespace MiscCorLib.Security.Cryptography
{
	// Refer to Encryption.cs for documentation comments of partial class.
	public static partial class Encryption
	{
		// These methods return EncryptorWithChecksum objects.

		#region [ Overloads of CreateEncryptor Extension Method for SymmetricAlgorithm ]

		public static EncryptorWithChecksum CreateEncryptor(
			this SymmetricAlgorithm algorithm,
			KeyedHashAlgorithm checksumHasher,
			byte[] encryptionKey,
			out byte[] randomSalt,
			EncryptionOptions options = DefaultOptions)
		{
			return new EncryptorWithChecksum(algorithm, checksumHasher, encryptionKey, out randomSalt, options);
		}

		public static EncryptorWithChecksum CreateEncryptor(
			this SymmetricAlgorithm algorithm,
			KeyedHashAlgorithm checksumHasher,
			string password,
			out byte[] randomSalt,
			EncryptionOptions options = DefaultOptions)
		{
			// In this case, use the more robustly random
			// key derivation algorithm to create the salt,
			// instead of the SymmetricAlgorithm.
			return algorithm.CreateEncryptorWithGivenSalt(
				checksumHasher,
				DeriveEncryptionKeyAndSaltFromPassword(password, algorithm.KeySize, algorithm.BlockSize, out randomSalt),
				randomSalt,
				options);
		}

		public static EncryptorWithChecksum CreateEncryptor(
			this SymmetricAlgorithm algorithm,
			KeyedHashAlgorithm checksumHasher,
			string password,
			out string randomSalt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			EncryptionOptions options = DefaultOptions)
		{
			byte[] randomSaltBytes;
			EncryptorWithChecksum encryptor = algorithm.CreateEncryptor(
				checksumHasher, password, out randomSaltBytes, options);

			randomSalt = randomSaltBytes.ToEncodedString(saltEncoding);

			return encryptor;
		}

		#endregion

		#region [ Overloads of CreateEncryptor Extension Method using a given Salt ]

		public static EncryptorWithChecksum CreateEncryptorWithGivenSalt(
			this SymmetricAlgorithm algorithm,
			KeyedHashAlgorithm checksumHasher,
			byte[] encryptionKey,
			byte[] salt,
			EncryptionOptions options = DefaultOptions)
		{
			return new EncryptorWithChecksum(algorithm, checksumHasher, encryptionKey, salt, options);
		}

		public static EncryptorWithChecksum CreateEncryptorWithGivenSalt(
			this SymmetricAlgorithm algorithm,
			KeyedHashAlgorithm checksumHasher,
			string password,
			byte[] salt,
			EncryptionOptions options = DefaultOptions)
		{
			return algorithm.CreateEncryptorWithGivenSalt(
				checksumHasher,
				DeriveEncryptionKeyFromPasswordAndSalt(password, algorithm.KeySize, salt),
				salt,
				options);
		}

		public static EncryptorWithChecksum CreateEncryptorWithGivenSalt(
			this SymmetricAlgorithm algorithm,
			KeyedHashAlgorithm checksumHasher,
			string password,
			string salt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			EncryptionOptions options = DefaultOptions)
		{
			return algorithm.CreateEncryptorWithGivenSalt(
				checksumHasher, password, salt.ToByteArray(saltEncoding), options);
		}

		#endregion

		#region [ Overloads of Generic CreateEncryptor Method ]

		public static EncryptorWithChecksum<TEncryptor, THasher> CreateEncryptor<TEncryptor, THasher>(
			byte[] encryptionKey,
			out byte[] randomSalt,
			EncryptionOptions options = DefaultOptions)
			where TEncryptor : SymmetricAlgorithm
			where THasher : KeyedHashAlgorithm
		{
			return new EncryptorWithChecksum<TEncryptor, THasher>(encryptionKey, out randomSalt, options);
		}

		public static EncryptorWithChecksum<TEncryptor, THasher> CreateEncryptor<TEncryptor, THasher>(
			string secretKey,
			out byte[] randomSalt,
			EncryptionOptions options = DefaultOptions)
			where TEncryptor : SymmetricAlgorithm
			where THasher : KeyedHashAlgorithm
		{
			int keySize, blockSize;
			using (TEncryptor algorithm = SymmetricTransformer<TEncryptor>.CreateAlgorithm())
			{
				keySize = algorithm.KeySize;
				blockSize = algorithm.BlockSize;
			}

			return new EncryptorWithChecksum<TEncryptor, THasher>(
				DeriveEncryptionKeyAndSaltFromPassword(secretKey, keySize, blockSize, out randomSalt),
				randomSalt,
				options);
		}

		public static EncryptorWithChecksum<TEncryptor, THasher> CreateEncryptor<TEncryptor, THasher>(
			string secretKey,
			out string randomSalt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			EncryptionOptions options = DefaultOptions)
			where TEncryptor : SymmetricAlgorithm
			where THasher : KeyedHashAlgorithm
		{
			if (secretKey == null)
			{
				throw new ArgumentNullException(nameof(secretKey));
			}

			byte[] randomSaltBytes;
			EncryptorWithChecksum<TEncryptor, THasher> encryptor = CreateEncryptor<TEncryptor, THasher>(
				secretKey, out randomSaltBytes, options);

			randomSalt = randomSaltBytes.ToEncodedString(saltEncoding);

			return encryptor;
		}

		#endregion

		#region [ Overloads of Generic CreateEncryptor Method using a given Salt ]

		public static EncryptorWithChecksum<TEncryptor, THasher> CreateEncryptorWithGivenSalt<TEncryptor, THasher>(
			byte[] encryptionKey,
			byte[] salt,
			EncryptionOptions options = DefaultOptions)
			where TEncryptor : SymmetricAlgorithm
			where THasher : KeyedHashAlgorithm
		{
			if (encryptionKey == null)
			{
				throw new ArgumentNullException(nameof(encryptionKey));
			}

			if (salt == null)
			{
				throw new ArgumentNullException(nameof(salt));
			}

			return new EncryptorWithChecksum<TEncryptor, THasher>(encryptionKey, salt, options);
		}

		public static EncryptorWithChecksum<TEncryptor, THasher> CreateEncryptorWithGivenSalt<TEncryptor, THasher>(
			string secretKey,
			byte[] salt,
			EncryptionOptions options = DefaultOptions)
			where TEncryptor : SymmetricAlgorithm
			where THasher : KeyedHashAlgorithm
		{
			if (secretKey == null)
			{
				throw new ArgumentNullException(nameof(secretKey));
			}

			if (salt == null)
			{
				throw new ArgumentNullException(nameof(salt));
			}

			int keySize;
			using (TEncryptor algorithm = SymmetricTransformer<TEncryptor>.CreateAlgorithm())
			{
				keySize = algorithm.KeySize;
			}

			return new EncryptorWithChecksum<TEncryptor, THasher>(
				DeriveEncryptionKeyFromPasswordAndSalt(secretKey, keySize, salt),
				salt,
				options);
		}

		public static EncryptorWithChecksum<TEncryptor, THasher> CreateEncryptorWithGivenSalt<TEncryptor, THasher>(
			string secretKey,
			string salt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			EncryptionOptions options = DefaultOptions)
			where TEncryptor : SymmetricAlgorithm
			where THasher : KeyedHashAlgorithm
		{
			if (secretKey == null)
			{
				throw new ArgumentNullException(nameof(secretKey));
			}

			if (salt == null)
			{
				throw new ArgumentNullException(nameof(salt));
			}

			return CreateEncryptorWithGivenSalt<TEncryptor, THasher>(secretKey, salt.ToByteArray(
				(options & EncryptionOptions.AllowNullInput) == EncryptionOptions.AllowNullInput,
				saltEncoding));
		}

		#endregion
	}
}