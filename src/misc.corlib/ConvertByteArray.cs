namespace MiscCorLib
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Text;

	using JetBrains.Annotations;

	using MiscCorLib.Security.Cryptography;

	public static class ConvertByteArray
	{
		private const string NullBytesString = @"(null)";

		public static string ToBase64String([NotNull] this byte[] inArray)
		{
			Contract.Requires<ArgumentNullException>(inArray != null);

			return Convert.ToBase64String(inArray);
		}

		public static string ToBase64String(this byte[] inArray, bool allowNulls)
		{
			if (allowNulls && (inArray == null))
			{
				return NullBytesString;
			}

			return inArray.ToBase64String();
		}

		public static string ToHexadecimalString([NotNull] this byte[] inArray)
		{
			Contract.Requires<ArgumentNullException>(inArray != null);

			// For performance analysis, try here:
			// https://github.com/patridge/PerformanceStubs
			// http://stackoverflow.com/questions/311165/how-do-you-convert-byte-array-to-hexadecimal-string-and-vice-versa
			//
			// This is one alternative to using the StringBuilder and looping:
			////	return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();

			// Create a new StringBuilder to collect the bytes.
			StringBuilder hashedStringBuilder = new StringBuilder();

			// Loop through each byte of the hashed data
			// and format each one as a hexadecimal string.
			foreach (byte t in inArray)
			{
				hashedStringBuilder.Append(t.ToString("x2"));
			}

			// Return the hexadecimal string.
			return hashedStringBuilder.ToString().ToLowerInvariant();
		}

		public static string ToEncodedString([NotNull] this byte[] inArray, CipherEncoding encoding)
		{
			Contract.Requires<ArgumentNullException>(inArray != null);

			// ReSharper disable once ConvertIfStatementToSwitchStatement
			if (encoding == CipherEncoding.Base64)
			{
				return inArray.ToBase64String();
			}

			if (encoding == CipherEncoding.Hexadecimal)
			{
				return inArray.ToHexadecimalString();
			}

			throw new ArgumentOutOfRangeException("encoding", encoding, "Invalid value for CipherEncoding enumeration.");
		}

		public static string ToText([NotNull] this byte[] inArray, Encoding encoding)
		{
			Contract.Requires<ArgumentNullException>(inArray != null);
			Contract.Requires<ArgumentNullException>(encoding != null);

			// Simple double-dispatch.
			// TODO: Loop over buffer if array is large.
			return encoding.GetString(inArray);
		}

		// Below works for syntactic sugar, but an abstract factory like
		// this is not efficient for looking up singleton Encoding types.
		// Better to send Encoding as a parameter, as above.
		/*
			public static string ToText<T>([NotNull] this byte[] inArray)
				where T : Encoding
			{
				return CreateEncoding<T>().GetString(inArray);
			}

			private static T CreateEncoding<T>()
				where T : Encoding
			{
				Contract.Ensures(Contract.Result<T>() != null);

				T encoding = Encoding.GetEncoding((typeof(T)).ToString()) as T;
				if (encoding == null)
				{
					throw new InvalidOperationException(string.Concat(typeof(T).FullName, " is not an encoding!"));
				}

				return encoding;
			}
		*/
	}
}