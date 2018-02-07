#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Security.Cryptography
	Encryption.cs

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

	/// <summary>
	/// Factory producting <see cref="Encryptor" />
	/// and <see cref="Decryptor" /> instances.
	/// </summary>
	/// <remarks>
	/// <para>
	/// An abstract factory could be extracted, an IEncryption
	/// factory producing IEncryptor and IDecryptor instances.
	/// </para>
	/// </remarks>
	public static partial class Encryption
	{
		public const EncryptionOptions DefaultOptions = EncryptionOptions.AllowNullInput;
			////	| EncryptionOptions.SuppressCryptographicExceptions
			////	| EncryptionOptions.SuppressDecryptionChecksumExceptions;

		public static readonly Encoding DefaultTextEncoding = Encoding.UTF8;

		public static byte[] DeriveEncryptionKeyAndSaltFromPassword(
			string password, int keySize, int saltSize, out byte[] randomSalt)
		{
			Contract.Requires<ArgumentNullException>(password != null);

			byte[] encryptionKey;
			using (Rfc2898DeriveBytes keyBytes = new Rfc2898DeriveBytes(password, saltSize))
			{
				// Get an encryption key of pseudo-random
				// bytes derived from the given password.
				encryptionKey = keyBytes.GetBytes(keySize);

				// The salt is a random value,
				// uniquely identifying this instance
				// of the encryption operations,
				// and needed for decryption.
				randomSalt = keyBytes.Salt;
			}

			return encryptionKey;
		}

		public static byte[] DeriveEncryptionKeyFromPasswordAndSalt(
			string password, int keySize, byte[] salt)
		{
			Contract.Requires<ArgumentNullException>(password != null);

			byte[] decryptionKey;
			using (DeriveBytes keyBytes = new Rfc2898DeriveBytes(password, salt))
			{
				decryptionKey = keyBytes.GetBytes(keySize);
			}

			return decryptionKey;
		}
	}
}