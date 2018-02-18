using System;
using System.Security.Cryptography;
using System.Text;

namespace MiscCorLib.Security.Cryptography
{
	public static class StringHashing
	{
		/// <summary>
		/// Unless a different <see cref="Encoding" /> is specified,
		/// when a string is hashed using one of the extension
		/// methods in this class, the string is assumed to be
		/// encoded as <see cref="Encoding.UTF8" />, the same
		/// as <see cref="Encryption.DefaultTextEncoding" />
		/// for <see cref="Encryption" />.
		/// </summary>
		public static readonly Encoding DefaultEncoding = Encryption.DefaultTextEncoding;

		#region [ ComputeHash Overloads returning Byte Array ]

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T">
		/// A type of <see cref="HashAlgorithm" />
		/// used to hash the <paramref name="input" />.
		/// </typeparam>
		/// <param name="input">
		/// A UTF-8 string to be hashed using
		/// a type of <see cref="HashAlgorithm" />
		/// specified as <typeparamref name="T" />.
		/// </param>
		/// <returns>
		/// 
		/// </returns>
		public static byte[] ComputeHash<T>(this string input)
			where T : HashAlgorithm
		{
			return ComputeHash<T>(input, DefaultEncoding);
		}

		public static byte[] ComputeHash<T>(this string input, Encoding encoding)
			where T : HashAlgorithm
		{
			if (encoding == null)
			{
				throw new ArgumentNullException(nameof(encoding));
			}

			/*
				if (input == null)
				{
					// Suppress ArgumentNullException which would
					// otherwise be thrown by Encoding.GetBytes()?
					return new byte[0];
				}
			*/

			return encoding.GetBytes(input).ComputeHash<T>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T">
		/// A type of <see cref="KeyedHashAlgorithm" />
		/// used to hash the <paramref name="input" />.
		/// </typeparam>
		/// <param name="input">
		/// A UTF-8 string to be hashed using
		/// a type of <see cref="KeyedHashAlgorithm" />
		/// specified as <typeparamref name="T" />.
		/// </param>
		/// <param name="key">
		/// 
		/// </param>
		/// <returns>
		/// 
		/// </returns>
		public static byte[] ComputeHash<T>(this string input, byte[] key)
			where T : KeyedHashAlgorithm
		{
			return ComputeHash<T>(input, DefaultEncoding, key);
		}

		public static byte[] ComputeHash<T>(this string input, Encoding encoding, byte[] key)
			where T : KeyedHashAlgorithm
		{
			if (encoding == null)
			{
				throw new ArgumentNullException(nameof(encoding));
			}

			return encoding.GetBytes(input).ComputeHash<T>(key);
		}

		public static byte[] ComputeHash<T>(this string input, Encoding encoding, string key)
			where T : KeyedHashAlgorithm
		{
			if (encoding == null)
			{
				throw new ArgumentNullException(nameof(encoding));
			}

			return encoding.GetBytes(input).ComputeHash<T>(key);
		}

		public static byte[] ComputeHash<T>(this string input, Encoding encoding, string key, Encoding keyEncoding)
			where T : KeyedHashAlgorithm
		{
			if (encoding == null)
			{
				throw new ArgumentNullException(nameof(encoding));
			}

			return encoding.GetBytes(input).ComputeHash<T>(key, keyEncoding);
		}

		#endregion

		#region [ HashToHexadecimalString Overloads ]

		public static string HashToHexadecimalString<T>(this string input)
			where T : HashAlgorithm
		{
			return HashToHexadecimalString<T>(input, DefaultEncoding);
		}

		public static string HashToHexadecimalString<T>(this string input, Encoding encoding)
			where T : HashAlgorithm
		{
			return ComputeHash<T>(input, encoding).ToHexadecimalString();
		}

		public static string HashToHexadecimalString<T>(this string input, byte[] key)
			where T : KeyedHashAlgorithm
		{
			return HashToHexadecimalString<T>(input, DefaultEncoding, key);
		}

		public static string HashToHexadecimalString<T>(this string input, Encoding encoding, byte[] key)
			where T : KeyedHashAlgorithm
		{
			return ComputeHash<T>(input, encoding, key).ToHexadecimalString();
		}

		public static string HashToHexadecimalString<T>(this string input, Encoding encoding, string key)
			where T : KeyedHashAlgorithm
		{
			return ComputeHash<T>(input, encoding, key).ToHexadecimalString();
		}

		public static string HashToHexadecimalString<T>(this string input, Encoding encoding, string key, Encoding keyEncoding)
			where T : KeyedHashAlgorithm
		{
			return ComputeHash<T>(input, encoding, key, keyEncoding).ToHexadecimalString();
		}

		#endregion

		#region [ HashToBase64String Overloads ]

		public static string HashToBase64String<T>(this string input)
			where T : HashAlgorithm
		{
			return HashToBase64String<T>(input, DefaultEncoding);
		}

		public static string HashToBase64String<T>(this string input, Encoding encoding)
			where T : HashAlgorithm
		{
			return ComputeHash<T>(input, encoding).ToBase64String();
		}

		public static string HashToBase64String<T>(this string input, byte[] key)
			where T : KeyedHashAlgorithm
		{
			return HashToBase64String<T>(input, DefaultEncoding, key);
		}

		public static string HashToBase64String<T>(this string input, Encoding encoding, byte[] key)
			where T : KeyedHashAlgorithm
		{
			return ComputeHash<T>(input, encoding, key).ToBase64String();
		}

		public static string HashToBase64String<T>(this string input, Encoding encoding, string key)
			where T : KeyedHashAlgorithm
		{
			return ComputeHash<T>(input, encoding, key).ToBase64String();
		}

		public static string HashToBase64String<T>(this string input, Encoding encoding, string key, Encoding keyEncoding)
			where T : KeyedHashAlgorithm
		{
			return ComputeHash<T>(input, encoding, key, keyEncoding).ToBase64String();
		}

		#endregion
	}
}