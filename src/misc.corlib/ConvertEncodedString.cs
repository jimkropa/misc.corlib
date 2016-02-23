namespace MiscCorLib
{
	using System;

	public static class ConvertBase64String
	{
		public static byte[] ToByteArray(this string encodedString)
		{
			return Convert.FromBase64String(encodedString);
		}
	}

	public static class ConvertHexadecimalString
	{
		public static byte[] ToByteArray(this string encodedString)
		{
			int length = encodedString.Length / 2;
			byte[] outArray = new byte[length];
			for (int i = 0; i < length; i++)
			{
				outArray[i] = Convert.ToByte(encodedString.Substring(i * 2, 2), 16);
			}

			return outArray;
		}
	}

	public static class ConvertEncodedString
	{
		public static byte[] ToByteArray(
			this string encodedString,
			ByteArrayStringEncoding fromEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			// ReSharper disable once ConvertIfStatementToSwitchStatement
			if (fromEncoding == ByteArrayStringEncoding.Base64)
			{
				return ConvertBase64String.ToByteArray(encodedString);
			}
	
			// ReSharper disable once InvertIf
			if (fromEncoding == ByteArrayStringEncoding.Hexadecimal)
			{
				return ConvertHexadecimalString.ToByteArray(encodedString);
			}

			throw new ArgumentOutOfRangeException();
		}
	}
}