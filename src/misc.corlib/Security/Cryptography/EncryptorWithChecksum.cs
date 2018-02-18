using System;
using System.Security.Cryptography;
using System.Text;

namespace MiscCorLib.Security.Cryptography
{
	// TODO: Test whether this functionality is even needed...
	// If decryption fails, is there an exception or does it just produce nonsense?
	public sealed class EncryptorWithChecksum : EncryptorWithChecksum<SymmetricAlgorithm, KeyedHashAlgorithm>
	{
		internal EncryptorWithChecksum(
			SymmetricAlgorithm symmetricAlgorithm,
			KeyedHashAlgorithm checksumHasher,
			byte[] encryptionKey,
			out byte[] initializationVector,
			EncryptionOptions options)
			: base(symmetricAlgorithm, checksumHasher, encryptionKey, out initializationVector, options)
		{
		}

		internal EncryptorWithChecksum(
			SymmetricAlgorithm symmetricAlgorithm,
			KeyedHashAlgorithm checksumHasher,
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
			: base(symmetricAlgorithm, checksumHasher, encryptionKey, initializationVector, options)
		{
		}
	}

	/// <summary>
	/// Decorator of a <see cref="Encryptor{T}" />?
	/// </summary>
	/// <typeparam name="TEncryptor"></typeparam>
	/// <typeparam name="THasher"></typeparam>
	public class EncryptorWithChecksum<TEncryptor, THasher> : Encryptor<TEncryptor>
		where TEncryptor: SymmetricAlgorithm
		where THasher: KeyedHashAlgorithm
	{
		private readonly KeyedHasher<THasher> hasher;

		internal EncryptorWithChecksum(
			TEncryptor symmetricAlgorithm,
			THasher checksumHasher,
			byte[] encryptionKey,
			out byte[] initializationVector,
			EncryptionOptions options)
			: base(symmetricAlgorithm, encryptionKey, out initializationVector, options)
		{
			this.hasher = new KeyedHasher<THasher>(
				checksumHasher, encryptionKey, initializationVector, this.AllowsNulls);
		}

		internal EncryptorWithChecksum(
			TEncryptor algorithm,
			THasher checksumHasher,
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
			: base(algorithm, encryptionKey, initializationVector, options)
		{
			this.hasher = new KeyedHasher<THasher>(
				checksumHasher, encryptionKey, initializationVector, this.AllowsNulls);
		}

		internal EncryptorWithChecksum(
			byte[] encryptionKey,
			out byte[] initializationVector,
			EncryptionOptions options)
			: base(encryptionKey, out initializationVector, options)
		{
			this.hasher = new KeyedHasher<THasher>(
				encryptionKey, initializationVector, this.AllowsNulls);
		}

		internal EncryptorWithChecksum(
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
			: base(encryptionKey, initializationVector, options)
		{
			this.hasher = new KeyedHasher<THasher>(
				encryptionKey, initializationVector, this.AllowsNulls);
		}

		public byte[] Encrypt(byte[] plaintextBytes, out byte[] checksum)
		{
			checksum = this.hasher.ComputeHash(plaintextBytes);

			return this.Encrypt(plaintextBytes);
		}

		public byte[] Encrypt(string plaintext, out byte[] checksum)
		{
			return this.Encrypt(plaintext, Encryption.DefaultTextEncoding, out checksum);
		}

		public byte[] Encrypt(
			string plaintext,
			Encoding plaintextEncoding,
			out byte[] checksum)
		{
			if (plaintextEncoding == null)
			{
				throw new ArgumentNullException(nameof(plaintextEncoding));
			}

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
			return this.EncryptToString(
				plaintext, Encryption.DefaultTextEncoding, out checksum, cipherTextAndChecksumEncoding);
		}

		public string EncryptToString(
			string plaintext,
			Encoding plaintextEncoding,
			out string checksum,
			ByteArrayStringEncoding ciphertextEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			if (plaintextEncoding == null)
			{
				throw new ArgumentNullException(nameof(plaintextEncoding));
			}

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
			byte[] checksumBytes;
			string encryptedString = this.Encrypt(plaintextBytes, out checksumBytes).ToEncodedString(cipherTextAndChecksumEncoding);

			checksum = checksumBytes.ToEncodedString(cipherTextAndChecksumEncoding);

			return encryptedString;
		}
	}
}