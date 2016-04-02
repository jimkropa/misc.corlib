#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Security.Cryptography
	EncryptionOptions.cs

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

		/*
			/// <summary>
			/// 
			/// </summary>
			/// <value>
			/// </value>
			Default = AllowNullInput | SuppressCryptographicExceptions | SuppressDecryptionChecksumExceptions,
		*/

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