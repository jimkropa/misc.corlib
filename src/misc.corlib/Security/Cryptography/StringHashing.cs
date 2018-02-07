#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Security.Cryptography
	StringHashing.cs

	Copyright (c) 2016 Jim Kropa (https://github.com/jimkropa)

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

		http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/
#endregion

namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;
	using System.Text;

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
			Contract.Requires<ArgumentNullException>(input != null);

			return ComputeHash<T>(input, DefaultEncoding);
		}

		public static byte[] ComputeHash<T>(this string input, Encoding encoding)
			where T : HashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);

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
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return ComputeHash<T>(input, DefaultEncoding, key);
		}

		public static byte[] ComputeHash<T>(this string input, Encoding encoding, byte[] key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return encoding.GetBytes(input).ComputeHash<T>(key);
		}

		public static byte[] ComputeHash<T>(this string input, Encoding encoding, string key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return encoding.GetBytes(input).ComputeHash<T>(key);
		}

		public static byte[] ComputeHash<T>(this string input, Encoding encoding, string key, Encoding keyEncoding)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(keyEncoding != null);

			return encoding.GetBytes(input).ComputeHash<T>(key, keyEncoding);
		}

		#endregion

		#region [ HashToHexadecimalString Overloads ]

		public static string HashToHexadecimalString<T>(this string input)
			where T : HashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);

			return HashToHexadecimalString<T>(input, DefaultEncoding);
		}

		public static string HashToHexadecimalString<T>(this string input, Encoding encoding)
			where T : HashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);

			return ComputeHash<T>(input, encoding).ToHexadecimalString();
		}

		public static string HashToHexadecimalString<T>(this string input, byte[] key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return HashToHexadecimalString<T>(input, DefaultEncoding, key);
		}

		public static string HashToHexadecimalString<T>(this string input, Encoding encoding, byte[] key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return ComputeHash<T>(input, encoding, key).ToHexadecimalString();
		}

		public static string HashToHexadecimalString<T>(this string input, Encoding encoding, string key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return ComputeHash<T>(input, encoding, key).ToHexadecimalString();
		}

		public static string HashToHexadecimalString<T>(this string input, Encoding encoding, string key, Encoding keyEncoding)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(keyEncoding != null);

			return ComputeHash<T>(input, encoding, key, keyEncoding).ToHexadecimalString();
		}

		#endregion

		#region [ HashToBase64String Overloads ]

		public static string HashToBase64String<T>(this string input)
			where T : HashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);

			return HashToBase64String<T>(input, DefaultEncoding);
		}

		public static string HashToBase64String<T>(this string input, Encoding encoding)
			where T : HashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);

			return ComputeHash<T>(input, encoding).ToBase64String();
		}

		public static string HashToBase64String<T>(this string input, byte[] key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return HashToBase64String<T>(input, DefaultEncoding, key);
		}

		public static string HashToBase64String<T>(this string input, Encoding encoding, byte[] key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return ComputeHash<T>(input, encoding, key).ToBase64String();
		}

		public static string HashToBase64String<T>(this string input, Encoding encoding, string key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return ComputeHash<T>(input, encoding, key).ToBase64String();
		}

		public static string HashToBase64String<T>(this string input, Encoding encoding, string key, Encoding keyEncoding)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(keyEncoding != null);

			return ComputeHash<T>(input, encoding, key, keyEncoding).ToBase64String();
		}

		#endregion
	}
}