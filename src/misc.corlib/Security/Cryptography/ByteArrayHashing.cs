namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;
	using System.Text;

	using JetBrains.Annotations;

	public static class ByteArrayHashing
	{
		/// <summary>
		/// When a string is passed to the overload of
		/// <see cref="ComputeHash{T}(byte[],string)"/>
		/// to be used as the <see cref="KeyedHashAlgorithm.Key"/>
		/// for a <see cref="KeyedHashAlgorithm"/>, unless
		/// another <see cref="Encoding"/> is provided,
		/// the string key is presumed to be encoded
		/// as <see cref="Encoding.ASCII"/>.
		/// </summary>
		public static readonly Encoding DefaultHashKeyEncoding = Encoding.ASCII;

		public static byte[] ComputeHash<T>([NotNull] this byte[] bytes)
			where T : HashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(bytes != null);

			byte[] hashedBytes;

			// Use an abstract factory to create an instance
			// of a specific given type of HashAlgorithm.
			using (T hasher = HashAlgorithm.Create((typeof(T)).ToString()) as T)
			{
				// warning CC1024: CodeContracts: Contract section within try block.
				////	Contract.Requires(hasher != null);

				if (hasher == null)
				{
					throw new InvalidOperationException(string.Concat(typeof(T).FullName, " is not a cryptographic hash algorithm!"));
				}

				hashedBytes = hasher.ComputeHash(bytes);
			}

			return hashedBytes;
		}

		public static byte[] ComputeHash<T>([NotNull] this byte[] bytes, [NotNull] byte[] key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(bytes != null);
			Contract.Requires<ArgumentNullException>(key != null);

			byte[] hashedBytes;

			// Use an abstract factory to create an instance
			// of a specific given type of KeyedHashAlgorithm.
			using (T hasher = KeyedHashAlgorithm.Create((typeof(T)).ToString()) as T)
			{
				// warning CC1024: CodeContracts: Contract section within try block.
				////	Contract.Requires(hasher != null);

				if (hasher == null)
				{
					throw new InvalidOperationException(string.Concat(typeof(T).FullName, " is not a keyed cryptographic hash algorithm!"));
				}

				hasher.Key = key;

				hashedBytes = hasher.ComputeHash(bytes);
			}

			return hashedBytes;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="bytes"></param>
		/// <param name="key">
		/// An ASCII-encoded string to be used as the
		/// <see cref="KeyedHashAlgorithm.Key"/>
		/// for the <see cref="KeyedHashAlgorithm"/>
		/// specified as <typeparamref name="T"/>.
		/// </param>
		/// <returns></returns>
		public static byte[] ComputeHash<T>([NotNull] this byte[] bytes, [NotNull] string key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(bytes != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return ComputeHash<T>(bytes, key, DefaultHashKeyEncoding);
		}

		public static byte[] ComputeHash<T>([NotNull] this byte[] bytes, [NotNull] string key, [NotNull] Encoding keyEncoding)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(bytes != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(keyEncoding != null);

			return ComputeHash<T>(bytes, keyEncoding.GetBytes(key));
		}
	}
}