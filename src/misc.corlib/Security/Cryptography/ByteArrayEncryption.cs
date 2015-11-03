namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;
	using System.Text;

	using JetBrains.Annotations;

	public static class ByteArrayEncryption
	{
		/// <summary>
		/// When a string is passed to the overload of
		/// <see cref="ComputeHash{T}(byte[],string)"/>
		/// to be used as the <see cref="SymmetricAlgorithm.Key"/>
		/// for a <see cref="SymmetricAlgorithm"/>, unless
		/// another <see cref="Encoding"/> is provided,
		/// the string key is presumed to be encoded
		/// as <see cref="Encoding.ASCII"/>.
		/// </summary>
		public static readonly Encoding DefaultKeyEncoding = Encoding.ASCII;

		// ReSharper disable once InconsistentNaming
		/// <summary>
		/// When a string is passed to the overload of
		/// <see cref="ComputeHash{T}(byte[],string,string)"/>
		/// to be used as the <see cref="SymmetricAlgorithm.Key"/>
		/// for a <see cref="SymmetricAlgorithm"/>, unless
		/// another <see cref="Encoding"/> is provided,
		/// the string key is presumed to be encoded
		/// as <see cref="Encoding.ASCII"/>.
		/// </summary>
		public static readonly Encoding DefaultIVEncoding = Encoding.ASCII;

		public static byte[] Encrypt<T>([NotNull] this byte[] plaintextBytes, [NotNull] byte[] encryptionKey, [NotNull] byte[] salt)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(plaintextBytes != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			byte[] encryptedBytes;
			using (Encryptor<T> encryptor = new Encryptor<T>(encryptionKey, salt))
			{
				encryptedBytes = encryptor.Encrypt(plaintextBytes);
			}

			return encryptedBytes;
		}

		public static byte[] Encrypt<T>([NotNull] this byte[] plaintextBytes, [NotNull] string encryptionKey, [NotNull] string iv)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(plaintextBytes != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(iv != null);

			return Encrypt<T>(plaintextBytes, encryptionKey, DefaultKeyEncoding, iv, DefaultIVEncoding);
		}

		public static byte[] Encrypt<T>([NotNull] this byte[] plaintextBytes, [NotNull] string key, [NotNull] Encoding keyEncoding, [NotNull] string iv, [NotNull] Encoding ivEncoding)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(plaintextBytes != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(keyEncoding != null);
			Contract.Requires<ArgumentNullException>(iv != null);
			Contract.Requires<ArgumentNullException>(ivEncoding != null);

			return Encrypt<T>(plaintextBytes, keyEncoding.GetBytes(key), ivEncoding.GetBytes(iv));
		}

		public static byte[] Decrypt<T>([NotNull] this byte[] encryptedBytes, [NotNull] byte[] key, [NotNull] byte[] iv)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(encryptedBytes != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(iv != null);

			byte[] decryptedBytes;
			using (Decryptor<T> decryptor = new Decryptor<T>(key, iv))
			{
				decryptedBytes = decryptor.Decrypt(encryptedBytes);
			}

			return decryptedBytes;
		}

		public static byte[] Decrypt<T>([NotNull] this byte[] plaintextBytes, [NotNull] string key, [NotNull] string iv)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(plaintextBytes != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(iv != null);

			return Decrypt<T>(plaintextBytes, key, DefaultKeyEncoding, iv, DefaultIVEncoding);
		}

		public static byte[] Decrypt<T>([NotNull] this byte[] plaintextBytes, [NotNull] string key, [NotNull] Encoding keyEncoding, [NotNull] string iv, [NotNull] Encoding ivEncoding)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(plaintextBytes != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(keyEncoding != null);
			Contract.Requires<ArgumentNullException>(iv != null);
			Contract.Requires<ArgumentNullException>(ivEncoding != null);

			return Decrypt<T>(plaintextBytes, keyEncoding.GetBytes(key), ivEncoding.GetBytes(iv));
		}
	}
}