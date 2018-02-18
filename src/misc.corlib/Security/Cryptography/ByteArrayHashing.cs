using System;
using System.Security.Cryptography;
using System.Text;

namespace MiscCorLib.Security.Cryptography
{
	public static class ByteArrayHashing
	{

		/// <summary>
		/// Use this to generate random salts for ByteArrayHashing.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		internal static byte[] GenerateRandomBytes(int size)
		{
			byte[] bytes = new byte[size];
			using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
			{
				rng.GetBytes(bytes);
			}

			return bytes;
		}

		/// <summary>
		/// When a string is passed to the overload of
		/// <see cref="ComputeHash{T}(byte[],string)" />
		/// to be used as the <see cref="KeyedHashAlgorithm.Key" />
		/// for a <see cref="KeyedHashAlgorithm" />, unless
		/// another <see cref="Encoding" /> is provided,
		/// the string key is presumed to be encoded
		/// as <see cref="Encoding.ASCII" />.
		/// </summary>
		public static readonly Encoding DefaultHashKeyEncoding = Encoding.ASCII;

		public static byte[] ComputeHash<T>(this byte[] bytes)
			where T : HashAlgorithm
		{
			if (bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes));
			}

			byte[] hashedBytes;

			// Use an abstract factory to create an instance
			// of a specific given type of HashAlgorithm.
			// https://github.com/dotnet/corefx/issues/22626
			//  OLD: T hasher = HashAlgorithm.Create((typeof(T)).ToString()) as T
			//  NEW: (HashAlgorithm) CryptoConfig.CreateFromName(hashName)
			using (T hasher = (HashAlgorithm) CryptoConfig.CreateFromName((typeof(T)).ToString()) as T)
			{
				if (hasher == null)
				{
					throw new InvalidOperationException(string.Concat(typeof(T).FullName, " is not a cryptographic hash algorithm!"));
				}

				hashedBytes = hasher.ComputeHash(bytes);
			}

			return hashedBytes;
		}

		public static byte[] ComputeHash<T>(this byte[] bytes, byte[] key)
			where T : KeyedHashAlgorithm
		{
			if (bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			byte[] hashedBytes;

			// Use an abstract factory to create an instance
			// of a specific given type of KeyedHashAlgorithm.
			using (T hasher = KeyedHashAlgorithm.Create((typeof(T)).ToString()) as T)
			{
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
		/// <see cref="KeyedHashAlgorithm.Key" />
		/// for the <see cref="KeyedHashAlgorithm" />
		/// specified as <typeparamref name="T" />.
		/// </param>
		/// <returns></returns>
		public static byte[] ComputeHash<T>(this byte[] bytes, string key)
			where T : KeyedHashAlgorithm
		{
			if (bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			return ComputeHash<T>(bytes, key, DefaultHashKeyEncoding);
		}

		public static byte[] ComputeHash<T>(
			this byte[] bytes,
			string key,
			Encoding keyEncoding)
			where T : KeyedHashAlgorithm
		{
			if (bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (keyEncoding == null)
			{
				throw new ArgumentNullException(nameof(keyEncoding));
			}

			return ComputeHash<T>(bytes, keyEncoding.GetBytes(key));
		}
	}
}