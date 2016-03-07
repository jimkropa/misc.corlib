using System;

namespace MiscCorLib.Security.Cryptography
{
	[Flags]
	public enum EncryptionOptions : byte
	{
		None = 0x0, // 0
		AllowNullInput = 0x1, // 1
		SuppressCryptographicExceptions = 0x2, // 2
		SuppressDecryptionChecksumExceptions = 0x4, // 4
		Bit4 = 0x8, // 8
		Bit5 = 0x10, // 16
		Bit6 = 0x20, // 32
		Bit7 = 0x40, // 64
		Bit8 = 0x80 // 128
	}
}