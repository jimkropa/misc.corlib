namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;
	using System.Text;

	[CLSCompliant(true)]
	public static class StringHashingExtensions
	{
		public static readonly Encoding DefaultEncoding = Encoding.UTF8;

		#region [ CalculateHash Overloads returning Byte Array ]

		public static byte[] CalculateHash<T>(this string input)
			where T : HashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);

			return CalculateHash<T>(input, DefaultEncoding);
		}

		public static byte[] CalculateHash<T>(this string input, Encoding encoding)
			where T : HashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);

			return encoding.GetBytes(input).CalculateHash<T>();
		}

		public static byte[] CalculateHash<T>(this string input, byte[] key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return CalculateHash<T>(input, DefaultEncoding, key);
		}

		public static byte[] CalculateHash<T>(this string input, Encoding encoding, byte[] key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return encoding.GetBytes(input).CalculateHash<T>(key);
		}

		public static byte[] CalculateHash<T>(this string input, Encoding encoding, string key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return encoding.GetBytes(input).CalculateHash<T>(key);
		}

		public static byte[] CalculateHash<T>(this string input, Encoding encoding, string key, Encoding keyEncoding)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(keyEncoding != null);

			return encoding.GetBytes(input).CalculateHash<T>(key, keyEncoding);
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

			return CalculateHash<T>(input, encoding).ToHexadecimalString();
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

			return CalculateHash<T>(input, encoding, key).ToHexadecimalString();
		}

		public static string HashToHexadecimalString<T>(this string input, Encoding encoding, string key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return CalculateHash<T>(input, encoding, key).ToHexadecimalString();
		}

		public static string HashToHexadecimalString<T>(this string input, Encoding encoding, string key, Encoding keyEncoding)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(keyEncoding != null);

			return CalculateHash<T>(input, encoding, key, keyEncoding).ToHexadecimalString();
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

			return CalculateHash<T>(input, encoding).ToBase64String();
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

			return CalculateHash<T>(input, encoding, key).ToBase64String();
		}

		public static string HashToBase64String<T>(this string input, Encoding encoding, string key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return CalculateHash<T>(input, encoding, key).ToBase64String();
		}

		public static string HashToBase64String<T>(this string input, Encoding encoding, string key, Encoding keyEncoding)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(keyEncoding != null);

			return CalculateHash<T>(input, encoding, key, keyEncoding).ToBase64String();
		}

		#endregion
	}
}