#region [ license and copyright boilerplate ]
/*
	MiscCorLib
	ByteArrayStringEncoding.cs

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
	/// <summary>
	/// Enumeration of options for encoding
	/// byte arrays to and from strings.
	/// </summary>
	public enum ByteArrayStringEncoding : byte
	{
		/// <summary>
		/// Indication to use
		/// <see cref="ConvertByteArray.ToBase64String(byte[])" />
		/// when encoding a byte array to string.
		/// </summary>
		Base64 = byte.MinValue,

		/// <summary>
		/// Indication to use
		/// <see cref="ConvertByteArray.ToHexadecimalString(byte[])" />
		/// when encoding a byte array to string.
		/// </summary>
		Hexadecimal = byte.MaxValue
	}
}