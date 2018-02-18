using System;

namespace MiscCorLib.Security.Cryptography
{
	[Flags]
	public enum EncryptionOptions : byte
	{
		/// <summary>
		/// 
		/// </summary>
		None = 0x0, // 0

		/// <summary>
		/// 
		/// </summary>
		AllowNullInput = 0x1, // 1

		/// <summary>
		/// 
		/// </summary>
		SuppressCryptographicExceptions = 0x2, // 2

		/// <summary>
		/// 
		/// </summary>
		SuppressDecryptionChecksumExceptions = 0x4, // 4

		// Initial allocation for this "flags"
		// enumeration is only 8 bits:
		////	Bit4 = 0x8, // 8
		////	Bit5 = 0x10, // 16
		////	Bit6 = 0x20, // 32
		////	Bit7 = 0x40, // 64
		////	Bit8 = 0x80 // 128
	}
}