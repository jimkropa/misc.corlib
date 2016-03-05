﻿namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;

	using JetBrains.Annotations;

	// Refer to Encryption.cs for documentation comments of partial class.
	public static partial class Encryption
	{
		// These methods return EncryptorWithChecksum objects.

		#region [ Overloads of CreateEncryptor Extension Method for SymmetricAlgorithm ]

		public static EncryptorWithChecksum CreateEncryptor(
			[NotNull] this SymmetricAlgorithm algorithm,
			[NotNull] KeyedHashAlgorithm checksumHasher,
			[NotNull] byte[] encryptionKey,
			out byte[] randomSalt,
			bool allowNulls = DefaultAllowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(checksumHasher != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			return new EncryptorWithChecksum(algorithm, checksumHasher, encryptionKey, out randomSalt, allowNulls);
		}

		public static EncryptorWithChecksum CreateEncryptor(
			[NotNull] this SymmetricAlgorithm algorithm,
			[NotNull] KeyedHashAlgorithm checksumHasher,
			[NotNull] string password,
			out byte[] randomSalt,
			bool allowNulls = DefaultAllowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(checksumHasher != null);
			Contract.Requires<ArgumentNullException>(password != null);

			// In this case, use the more robustly random
			// key derivation algorithm to create the salt,
			// instead of the SymmetricAlgorithm.
			return algorithm.CreateEncryptorWithGivenSalt(
				checksumHasher,
				DeriveEncryptionKeyAndSaltFromPassword(password, algorithm.KeySize, algorithm.BlockSize, out randomSalt),
				randomSalt,
				allowNulls);
		}

		public static EncryptorWithChecksum CreateEncryptor(
			[NotNull] this SymmetricAlgorithm algorithm,
			[NotNull] KeyedHashAlgorithm checksumHasher,
			[NotNull] string password,
			out string randomSalt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			bool allowNulls = DefaultAllowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(checksumHasher != null);
			Contract.Requires<ArgumentNullException>(password != null);

			byte[] randomSaltBytes;
			EncryptorWithChecksum encryptor = algorithm.CreateEncryptor(
				checksumHasher, password, out randomSaltBytes, allowNulls);

			randomSalt = randomSaltBytes.ToEncodedString(saltEncoding);

			return encryptor;
		}

		#endregion

		#region [ Overloads of CreateEncryptor Extension Method using a given Salt ]

		public static EncryptorWithChecksum CreateEncryptorWithGivenSalt(
			[NotNull] this SymmetricAlgorithm algorithm,
			[NotNull] KeyedHashAlgorithm checksumHasher,
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] salt,
			bool allowNulls = DefaultAllowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(checksumHasher != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			return new EncryptorWithChecksum(algorithm, checksumHasher, encryptionKey, salt, allowNulls);
		}

		public static EncryptorWithChecksum CreateEncryptorWithGivenSalt(
			[NotNull] this SymmetricAlgorithm algorithm,
			[NotNull] KeyedHashAlgorithm checksumHasher,
			[NotNull] string password,
			[NotNull] byte[] salt,
			bool allowNulls = DefaultAllowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(checksumHasher != null);
			Contract.Requires<ArgumentNullException>(password != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			return algorithm.CreateEncryptorWithGivenSalt(
				checksumHasher,
				DeriveEncryptionKeyFromPasswordAndSalt(password, algorithm.KeySize, salt),
				salt,
				allowNulls);
		}

		public static EncryptorWithChecksum CreateEncryptorWithGivenSalt(
			[NotNull] this SymmetricAlgorithm algorithm,
			[NotNull] KeyedHashAlgorithm checksumHasher,
			[NotNull] string password,
			[NotNull] string salt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			bool allowNulls = DefaultAllowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(checksumHasher != null);
			Contract.Requires<ArgumentNullException>(password != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			return algorithm.CreateEncryptorWithGivenSalt(
				checksumHasher, password, salt.ToByteArray(saltEncoding), allowNulls);
		}

		#endregion

		#region [ Overloads of Generic CreateEncryptor Method ]

		public static EncryptorWithChecksum<TEncryptor, THasher> CreateEncryptor<TEncryptor, THasher>(
			[NotNull] byte[] encryptionKey,
			out byte[] randomSalt,
			bool allowNulls = DefaultAllowNulls)
			where TEncryptor : SymmetricAlgorithm
			where THasher : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			return new EncryptorWithChecksum<TEncryptor, THasher>(encryptionKey, out randomSalt, allowNulls);
		}

		public static EncryptorWithChecksum<TEncryptor, THasher> CreateEncryptor<TEncryptor, THasher>(
			[NotNull] string password,
			out byte[] randomSalt,
			bool allowNulls = DefaultAllowNulls)
			where TEncryptor : SymmetricAlgorithm
			where THasher : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(password != null);

			int keySize, blockSize;
			using (TEncryptor algorithm = SymmetricTransformer<TEncryptor>.CreateAlgorithm())
			{
				keySize = algorithm.KeySize;
				blockSize = algorithm.BlockSize;
			}

			return new EncryptorWithChecksum<TEncryptor, THasher>(
				DeriveEncryptionKeyAndSaltFromPassword(password, keySize, blockSize, out randomSalt),
				randomSalt,
				allowNulls);
		}

		public static EncryptorWithChecksum<TEncryptor, THasher> CreateEncryptor<TEncryptor, THasher>(
			[NotNull] string password,
			out string randomSalt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			bool allowNulls = DefaultAllowNulls)
			where TEncryptor : SymmetricAlgorithm
			where THasher : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(password != null);

			byte[] randomSaltBytes;
			EncryptorWithChecksum<TEncryptor, THasher> encryptor = CreateEncryptor<TEncryptor, THasher>(
				password, out randomSaltBytes, allowNulls);

			randomSalt = randomSaltBytes.ToEncodedString(saltEncoding);

			return encryptor;
		}

		#endregion

		#region [ Overloads of Generic CreateEncryptor Method using a given Salt ]

		public static EncryptorWithChecksum<TEncryptor, THasher> CreateEncryptorWithGivenSalt<TEncryptor, THasher>(
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] salt,
			bool allowNulls = DefaultAllowNulls)
			where TEncryptor : SymmetricAlgorithm
			where THasher : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			return new EncryptorWithChecksum<TEncryptor, THasher>(encryptionKey, salt, allowNulls);
		}

		public static EncryptorWithChecksum<TEncryptor, THasher> CreateEncryptorWithGivenSalt<TEncryptor, THasher>(
			[NotNull] string password,
			[NotNull] byte[] salt,
			bool allowNulls = DefaultAllowNulls)
			where TEncryptor : SymmetricAlgorithm
			where THasher : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(password != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			int keySize;
			using (TEncryptor algorithm = SymmetricTransformer<TEncryptor>.CreateAlgorithm())
			{
				keySize = algorithm.KeySize;
			}

			return new EncryptorWithChecksum<TEncryptor, THasher>(
				DeriveEncryptionKeyFromPasswordAndSalt(password, keySize, salt),
				salt,
				allowNulls);
		}

		public static EncryptorWithChecksum<TEncryptor, THasher> CreateEncryptorWithGivenSalt<TEncryptor, THasher>(
			[NotNull] string password,
			[NotNull] string salt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			bool allowNulls = DefaultAllowNulls)
			where TEncryptor : SymmetricAlgorithm
			where THasher : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(password != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			return CreateEncryptorWithGivenSalt<TEncryptor, THasher>(password, salt.ToByteArray(allowNulls, saltEncoding));
		}

		#endregion
	}
}