using System;
using System.Security.Cryptography;
using System.Text;

namespace MiscCorLib.Security.Cryptography
{
	public static class ByteArrayEncryption
	{
		/// <summary>
		/// When a string is passed to the overload of
		/// Encrypt
		/// to be used as the <see cref="SymmetricAlgorithm.Key" />
		/// for a <see cref="SymmetricAlgorithm" />, unless
		/// another <see cref="Encoding" /> is provided,
		/// the string key is presumed to be encoded
		/// as <see cref="Encoding.ASCII" />.
		/// </summary>
		public static readonly Encoding DefaultKeyEncoding = Encoding.ASCII;

		/// <summary>
		/// When a string is passed to the overload of
		/// ComputeHash
		/// to be used as the <see cref="SymmetricAlgorithm.Key" />
		/// for a <see cref="SymmetricAlgorithm" />, unless
		/// another <see cref="Encoding" /> is provided,
		/// the string key is presumed to be encoded
		/// as <see cref="Encoding.ASCII" />.
		/// </summary>
		public static readonly Encoding DefaultIVEncoding = Encoding.ASCII;

		public static byte[] Encrypt<T>(
			this byte[] plaintextBytes,
			byte[] encryptionKey,
			byte[] salt,
			EncryptionOptions options = Encryption.DefaultOptions)
			where T : SymmetricAlgorithm
		{
			if (plaintextBytes == null)
			{
				throw new ArgumentNullException(nameof(plaintextBytes));
			}

			if (encryptionKey == null)
			{
				throw new ArgumentNullException(nameof(encryptionKey));
			}

			if (salt == null)
			{
				throw new ArgumentNullException(nameof(salt));
			}

			byte[] encryptedBytes;

			// TODO: Stop hiding the generated IV.
			byte[] initializationVector;
			using (Encryptor<T> encryptor = new Encryptor<T>(encryptionKey, out initializationVector, options))
			{
				encryptedBytes = encryptor.Encrypt(plaintextBytes);
			}

			return encryptedBytes;
		}

		public static byte[] Encrypt<T>(
			this byte[] plaintextBytes,
			string encryptionKey,
			string iv)
			where T : SymmetricAlgorithm
		{
			if (plaintextBytes == null)
			{
				throw new ArgumentNullException(nameof(plaintextBytes));
			}

			if (encryptionKey == null)
			{
				throw new ArgumentNullException(nameof(encryptionKey));
			}

			if (iv == null)
			{
				throw new ArgumentNullException(nameof(iv));
			}

			return Encrypt<T>(plaintextBytes, encryptionKey, DefaultKeyEncoding, iv, DefaultIVEncoding);
		}

		public static byte[] Encrypt<T>(
			this byte[] plaintextBytes,
			string key,
			Encoding keyEncoding,
			string iv,
			Encoding ivEncoding)
			where T : SymmetricAlgorithm
		{
			if (plaintextBytes == null)
			{
				throw new ArgumentNullException(nameof(plaintextBytes));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (keyEncoding == null)
			{
				throw new ArgumentNullException(nameof(keyEncoding));
			}

			if (iv == null)
			{
				throw new ArgumentNullException(nameof(iv));
			}

			if (ivEncoding == null)
			{
				throw new ArgumentNullException(nameof(ivEncoding));
			}

			return Encrypt<T>(plaintextBytes, keyEncoding.GetBytes(key), ivEncoding.GetBytes(iv));
		}

		public static byte[] Decrypt<T>(
			this byte[] encryptedBytes,
			byte[] key,
			byte[] iv,
			EncryptionOptions options = Encryption.DefaultOptions)
			where T : SymmetricAlgorithm
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (iv == null)
			{
				throw new ArgumentNullException(nameof(iv));
			}

			byte[] decryptedBytes;
			using (Decryptor<T> decryptor = new Decryptor<T>(key, iv, options))
			{
				decryptedBytes = decryptor.Decrypt(encryptedBytes);
			}

			return decryptedBytes;
		}

		public static byte[] Decrypt<T>(this byte[] plaintextBytes, string key, string iv)
			where T : SymmetricAlgorithm
		{
			if (plaintextBytes == null)
			{
				throw new ArgumentNullException(nameof(plaintextBytes));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (iv == null)
			{
				throw new ArgumentNullException(nameof(iv));
			}

			return Decrypt<T>(plaintextBytes, key, DefaultKeyEncoding, iv, DefaultIVEncoding);
		}

		public static byte[] Decrypt<T>(this byte[] plaintextBytes, string key, Encoding keyEncoding, string iv, Encoding ivEncoding)
			where T : SymmetricAlgorithm
		{
			if (plaintextBytes == null)
			{
				throw new ArgumentNullException(nameof(plaintextBytes));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (keyEncoding == null)
			{
				throw new ArgumentNullException(nameof(keyEncoding));
			}

			if (iv == null)
			{
				throw new ArgumentNullException(nameof(iv));
			}

			if (ivEncoding == null)
			{
				throw new ArgumentNullException(nameof(ivEncoding));
			}

			return Decrypt<T>(plaintextBytes, keyEncoding.GetBytes(key), ivEncoding.GetBytes(iv));
		}
	}
}