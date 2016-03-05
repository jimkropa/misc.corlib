﻿namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;
	using System.Text;

	using JetBrains.Annotations;

	// TODO: Test whether this functionality is even needed...
	// If decryption fails, is there an exception or does it just produce nonsense?
	public sealed class EncryptorWithChecksum : EncryptorWithChecksum<SymmetricAlgorithm, KeyedHashAlgorithm>
	{
		internal EncryptorWithChecksum(
			[NotNull] SymmetricAlgorithm symmetricAlgorithm,
			[NotNull] KeyedHashAlgorithm checksumHasher,
			[NotNull] byte[] encryptionKey,
			out byte[] initializationVector,
			bool allowNulls)
			: base(symmetricAlgorithm, checksumHasher, encryptionKey, out initializationVector, allowNulls)
		{
			Contract.Requires<ArgumentNullException>(symmetricAlgorithm != null);
			Contract.Requires<ArgumentNullException>(checksumHasher != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
		}

		internal EncryptorWithChecksum(
			[NotNull] SymmetricAlgorithm symmetricAlgorithm,
			[NotNull] KeyedHashAlgorithm checksumHasher,
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] initializationVector,
			bool allowNulls)
			: base(symmetricAlgorithm, checksumHasher, encryptionKey, initializationVector, allowNulls)
		{
			Contract.Requires<ArgumentNullException>(symmetricAlgorithm != null);
			Contract.Requires<ArgumentNullException>(checksumHasher != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(initializationVector != null);
		}
	}

	/// <summary>
	/// Decorator of a <see cref="Encryptor{T}"/>?
	/// </summary>
	/// <typeparam name="TEncryptor"></typeparam>
	/// <typeparam name="THasher"></typeparam>
	public class EncryptorWithChecksum<TEncryptor, THasher> : Encryptor<TEncryptor>
		where TEncryptor: SymmetricAlgorithm
		where THasher: KeyedHashAlgorithm
	{
		// This delegate really belongs to DecryptorWithChecksum...
		//public delegate void HandleChecksumFailure()

		private readonly KeyedHasher<THasher> hasher;

		internal EncryptorWithChecksum(
			[NotNull] TEncryptor symmetricAlgorithm,
			[NotNull] THasher checksumHasher,
			[NotNull] byte[] encryptionKey,
			out byte[] initializationVector,
			bool allowNulls)
			: base(symmetricAlgorithm, encryptionKey, out initializationVector, allowNulls)
		{
			this.hasher = new KeyedHasher<THasher>(
				checksumHasher, encryptionKey, initializationVector, allowNulls);
		}

		internal EncryptorWithChecksum(
			[NotNull] TEncryptor algorithm,
			[NotNull] THasher checksumHasher,
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] initializationVector,
			bool allowNulls)
			: base(algorithm, encryptionKey, initializationVector, allowNulls)
		{
			this.hasher = new KeyedHasher<THasher>(
				checksumHasher, encryptionKey, initializationVector, allowNulls);
		}

		internal EncryptorWithChecksum(
			[NotNull] byte[] encryptionKey,
			out byte[] initializationVector,
			bool allowNulls)
			: base(encryptionKey, out initializationVector, allowNulls)
		{
			this.hasher = new KeyedHasher<THasher>(
				encryptionKey, initializationVector, allowNulls);
		}

		internal EncryptorWithChecksum(
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] initializationVector,
			bool allowNulls)
			: base(encryptionKey, initializationVector, allowNulls)
		{
			this.hasher = new KeyedHasher<THasher>(
				encryptionKey, initializationVector, allowNulls);
		}

		public byte[] Encrypt(byte[] plaintextBytes, out byte[] checksum)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintextBytes != null);

			checksum = this.hasher.ComputeHash(plaintextBytes);

			return this.Encrypt(plaintextBytes);
		}

		public byte[] Encrypt(string plaintext, out byte[] checksum)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintext != null);

			return this.Encrypt(plaintext, Encryption.DefaultTextEncoding, out checksum);
		}

		public byte[] Encrypt(
			string plaintext,
			[NotNull] Encoding plaintextEncoding,
			out byte[] checksum)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintext != null);
			Contract.Requires<ArgumentNullException>(plaintextEncoding != null);

			// ReSharper disable once InvertIf
			if (plaintext == null)
			{
				checksum = null;

				return null;
			}

			return this.Encrypt(plaintextEncoding.GetBytes(plaintext), out checksum);
		}

		public string EncryptToString(
			string plaintext,
			out string checksum,
			ByteArrayStringEncoding cipherTextAndChecksumEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintext != null);

			return this.EncryptToString(
				plaintext, Encryption.DefaultTextEncoding, out checksum, cipherTextAndChecksumEncoding);
		}

		public string EncryptToString(
			string plaintext,
			[NotNull] Encoding plaintextEncoding,
			out string checksum,
			ByteArrayStringEncoding ciphertextEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintext != null);
			Contract.Requires<ArgumentNullException>(plaintextEncoding != null);

			// ReSharper disable once InvertIf
			if (plaintext == null)
			{
				checksum = null;

				return null;
			}

			return this.EncryptToString(
				plaintextEncoding.GetBytes(plaintext), out checksum, ciphertextEncoding);
		}

		public string EncryptToString(
			byte[] plaintextBytes,
			out string checksum,
			ByteArrayStringEncoding cipherTextAndChecksumEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintextBytes != null);

			byte[] checksumBytes;
			string encryptedString = this.Encrypt(plaintextBytes, out checksumBytes).ToEncodedString(cipherTextAndChecksumEncoding);

			checksum = checksumBytes.ToEncodedString(cipherTextAndChecksumEncoding);

			return encryptedString;
		}
	}
}