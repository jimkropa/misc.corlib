namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;
	using System.Text;

	using JetBrains.Annotations;

	[CLSCompliant(true)]
	public static class ByteArrayEncryptionExtensions
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
		/// <see cref="ComputeHash{T}(byte[],string)"/>
		/// to be used as the <see cref="SymmetricAlgorithm.Key"/>
		/// for a <see cref="SymmetricAlgorithm"/>, unless
		/// another <see cref="Encoding"/> is provided,
		/// the string key is presumed to be encoded
		/// as <see cref="Encoding.ASCII"/>.
		/// </summary>
		public static readonly Encoding DefaultIVEncoding = Encoding.ASCII;

		public static byte[] Encrypt<T>([NotNull] this byte[] plaintextBytes, [NotNull] byte[] key, [NotNull] byte[] iv)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(plaintextBytes != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(iv != null);

			byte[] encryptedBytes;

			// Use an abstract factory to create an instance
			// of a specific given type of SymmetricAlgorithm.
			using (T encryption = CreateAlgorithm<T>())
			{
				ValidateKeyAndBlockSizes(encryption, key, iv);

				using (ICryptoTransform encryptor = encryption.CreateEncryptor(key, iv))
				{
					encryptedBytes = TransformBytes(encryptor, plaintextBytes);
				}
			}

			return encryptedBytes;
		}

		public static byte[] Encrypt<T>([NotNull] this byte[] plaintextBytes, [NotNull] string key, [NotNull] string iv)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(plaintextBytes != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(iv != null);

			return Encrypt<T>(plaintextBytes, key, DefaultKeyEncoding, iv, DefaultIVEncoding);
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

			// Use an abstract factory to create an instance
			// of a specific given type of SymmetricAlgorithm.
			using (T encryption = CreateAlgorithm<T>())
			{
				ValidateKeyAndBlockSizes(encryption, key, iv);

				using (ICryptoTransform decryptor = encryption.CreateDecryptor(key, iv))
				{
					decryptedBytes = TransformBytes(decryptor, encryptedBytes);
				}
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

		private static T CreateAlgorithm<T>()
			where T : SymmetricAlgorithm
		{
			T encryption = SymmetricAlgorithm.Create((typeof(T)).ToString()) as T;
			if (encryption == null)
			{
				throw new InvalidOperationException(string.Concat(typeof(T).FullName, " is not a symmetric encryption algorithm!"));
			}

			return encryption;
		}

		private static void ValidateKeyAndBlockSizes<T>([NotNull] T encryption, [NotNull] byte[] key, [NotNull] byte[] iv)
			where T : SymmetricAlgorithm
		{
		}

		private static byte[] TransformBytes([NotNull] ICryptoTransform transformer, [NotNull] byte[] bytes)
		{
			// TODO: Loop over a block and such...
			////	return transformer.TransformFinalBlock(TransformFinal(, 0, 0));
			throw new NotImplementedException();
		}
	}
}