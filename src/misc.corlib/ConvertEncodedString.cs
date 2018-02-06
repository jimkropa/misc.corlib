#region [ license and copyright boilerplate ]
/*
	MiscCorLib
	ConvertEncodedString.cs

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

namespace MiscCorLib
{
	using System;
	using System.Diagnostics.Contracts;

	public static class ConvertEncodedString
	{
		#region [ Constants and Delegates describing Common Method Signatures ]

		/// <summary>
		/// Byte array value to return from <c>null</c> string input.
		/// </summary>
		/// <value><c>null</c></value>
		internal const byte[] NullByteArray = null;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encodedString"></param>
		/// <returns></returns>
		public delegate byte[] ConvertNonNullString(string encodedString);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encodedString"></param>
		/// <param name="allowNulls"></param>
		/// <returns></returns>
		public delegate byte[] ConvertString(string encodedString, bool allowNulls);

		#endregion

		public static byte[] FromBase64(this string encodedString)
		{
			Contract.Requires<ArgumentNullException>(encodedString != null);

			return Convert.FromBase64String(encodedString);
		}

		public static byte[] FromBase64(this string encodedString, bool allowNulls)
		{
			if (allowNulls && (encodedString == null))
			{
				return NullByteArray;
			}

			return encodedString.FromBase64();
		}
		
		public static byte[] FromHexadecimal(this string encodedString)
		{
			Contract.Requires<ArgumentNullException>(encodedString != null);

			int length = encodedString.Length / 2;
			byte[] outArray = new byte[length];
			for (int i = 0; i < length; i++)
			{
				outArray[i] = Convert.ToByte(encodedString.Substring(i * 2, 2), 16);
			}

			return outArray;
		}

		public static byte[] FromHexadecimal(this string encodedString, bool allowNulls)
		{
			if (allowNulls && (encodedString == null))
			{
				return NullByteArray;
			}

			return encodedString.FromHexadecimal();
		}
		
		public static byte[] ToByteArray(
			this string encodedString,
			ByteArrayStringEncoding fromEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			Contract.Requires<ArgumentNullException>(encodedString != null);

			// ReSharper disable once ConvertIfStatementToSwitchStatement
			if (fromEncoding == ByteArrayStringEncoding.Base64)
			{
				return encodedString.FromBase64();
			}
	
			// ReSharper disable once InvertIf
			if (fromEncoding == ByteArrayStringEncoding.Hexadecimal)
			{
				return encodedString.FromHexadecimal();
			}

			throw new ArgumentOutOfRangeException();
		}

		public static byte[] ToByteArray(
			this string encodedString,
			bool allowNulls,
			ByteArrayStringEncoding fromEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			if (allowNulls && (encodedString == null))
			{
				return NullByteArray;
			}

			return encodedString.ToByteArray(fromEncoding);
		}
	}
}