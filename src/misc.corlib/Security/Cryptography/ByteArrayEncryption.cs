#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Security.Cryptography
	ByteArrayEncryption.cs

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

	public static class ByteArrayEncryption
	{
		/// <summary>
		/// When a string is passed to the overload of
		/// Encrypt
		/// to be used as the <see cref="SymmetricAlgorithm.Key" />
		/// for a <see cref="SymmetricAlgorithm" />, unless
		/// another <see cref="Encoding" /> is provided,
		/// the string key is presumed to be encoded
		/// as <see cref="Encoding.ASCII" />.
		/// </summary>
		public static readonly Encoding DefaultKeyEncoding = Encoding.ASCII;

		// ReSharper disable once InconsistentNaming
		/// <summary>
		/// When a string is passed to the overload of
		/// ComputeHash
		/// to be used as the <see cref="SymmetricAlgorithm.Key" />
		/// for a <see cref="SymmetricAlgorithm" />, unless
		/// another <see cref="Encoding" /> is provided,
		/// the string key is presumed to be encoded
		/// as <see cref="Encoding.ASCII" />.
		/// </summary>
		public static readonly Encoding DefaultIVEncoding = Encoding.ASCII;

		public static byte[] Encrypt<T>(
			this byte[] plaintextBytes,
			byte[] encryptionKey,
			byte[] salt,
			EncryptionOptions options = Encryption.DefaultOptions)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(plaintextBytes != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			byte[] encryptedBytes;

			// TODO: Stop hiding the generated IV.
			byte[] initializationVector;
			using (Encryptor<T> encryptor = new Encryptor<T>(encryptionKey, out initializationVector, options))
			{
				encryptedBytes = encryptor.Encrypt(plaintextBytes);
			}

			return encryptedBytes;
		}

		public static byte[] Encrypt<T>(
			this byte[] plaintextBytes,
			string encryptionKey,
			string iv)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(plaintextBytes != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(iv != null);

			return Encrypt<T>(plaintextBytes, encryptionKey, DefaultKeyEncoding, iv, DefaultIVEncoding);
		}

		public static byte[] Encrypt<T>(
			this byte[] plaintextBytes,
			string key,
			Encoding keyEncoding,
			string iv,
			Encoding ivEncoding)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(plaintextBytes != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(keyEncoding != null);
			Contract.Requires<ArgumentNullException>(iv != null);
			Contract.Requires<ArgumentNullException>(ivEncoding != null);

			return Encrypt<T>(plaintextBytes, keyEncoding.GetBytes(key), ivEncoding.GetBytes(iv));
		}

		public static byte[] Decrypt<T>(
			this byte[] encryptedBytes,
			byte[] key,
			byte[] iv,
			EncryptionOptions options = Encryption.DefaultOptions)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(iv != null);

			byte[] decryptedBytes;
			using (Decryptor<T> decryptor = new Decryptor<T>(key, iv, options))
			{
				decryptedBytes = decryptor.Decrypt(encryptedBytes);
			}

			return decryptedBytes;
		}

		public static byte[] Decrypt<T>(this byte[] plaintextBytes, string key, string iv)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(plaintextBytes != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(iv != null);

			return Decrypt<T>(plaintextBytes, key, DefaultKeyEncoding, iv, DefaultIVEncoding);
		}

		public static byte[] Decrypt<T>(this byte[] plaintextBytes, string key, Encoding keyEncoding, string iv, Encoding ivEncoding)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(plaintextBytes != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(keyEncoding != null);
			Contract.Requires<ArgumentNullException>(iv != null);
			Contract.Requires<ArgumentNullException>(ivEncoding != null);

			return Decrypt<T>(plaintextBytes, keyEncoding.GetBytes(key), ivEncoding.GetBytes(iv));
		}
	}
}