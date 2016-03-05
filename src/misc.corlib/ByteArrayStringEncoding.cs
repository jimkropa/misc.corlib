﻿namespace MiscCorLib
{
	/// <summary>
	/// Enumeration of options for encoding
	/// byte arrays to and from strings.
	/// </summary>
	public enum ByteArrayStringEncoding : byte
	{
		/// <summary>
		/// Indication to use
		/// <see cref="ConvertByteArray.ToBase64String(byte[])"/>
		/// when encoding a byte array to string.
		/// </summary>
		Base64 = byte.MinValue,

		/// <summary>
		/// Indication to use
		/// <see cref="ConvertByteArray.ToHexadecimalString(byte[])"/>
		/// when encoding a byte array to string.
		/// </summary>
		Hexadecimal = byte.MaxValue
	}
}