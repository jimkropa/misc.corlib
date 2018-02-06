#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Security.Cryptography
	EncryptorWithChecksum.cs

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

	// TODO: Test whether this functionality is even needed...
	// If decryption fails, is there an exception or does it just produce nonsense?
	public sealed class EncryptorWithChecksum : EncryptorWithChecksum<SymmetricAlgorithm, KeyedHashAlgorithm>
	{
		internal EncryptorWithChecksum(
			SymmetricAlgorithm symmetricAlgorithm,
			KeyedHashAlgorithm checksumHasher,
			byte[] encryptionKey,
			out byte[] initializationVector,
			EncryptionOptions options)
			: base(symmetricAlgorithm, checksumHasher, encryptionKey, out initializationVector, options)
		{
			Contract.Requires<ArgumentNullException>(symmetricAlgorithm != null);
			Contract.Requires<ArgumentNullException>(checksumHasher != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
		}

		internal EncryptorWithChecksum(
			SymmetricAlgorithm symmetricAlgorithm,
			KeyedHashAlgorithm checksumHasher,
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
			: base(symmetricAlgorithm, checksumHasher, encryptionKey, initializationVector, options)
		{
			Contract.Requires<ArgumentNullException>(symmetricAlgorithm != null);
			Contract.Requires<ArgumentNullException>(checksumHasher != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(initializationVector != null);
		}
	}

	/// <summary>
	/// Decorator of a <see cref="Encryptor{T}"/>?
	/// </summary>
	/// <typeparam name="TEncryptor"></typeparam>
	/// <typeparam name="THasher"></typeparam>
	public class EncryptorWithChecksum<TEncryptor, THasher> : Encryptor<TEncryptor>
		where TEncryptor: SymmetricAlgorithm
		where THasher: KeyedHashAlgorithm
	{
		// This delegate really belongs to DecryptorWithChecksum...
		//public delegate void HandleChecksumFailure()

		private readonly KeyedHasher<THasher> hasher;

		internal EncryptorWithChecksum(
			TEncryptor symmetricAlgorithm,
			THasher checksumHasher,
			byte[] encryptionKey,
			out byte[] initializationVector,
			EncryptionOptions options)
			: base(symmetricAlgorithm, encryptionKey, out initializationVector, options)
		{
			this.hasher = new KeyedHasher<THasher>(
				checksumHasher, encryptionKey, initializationVector, this.AllowsNulls);
		}

		internal EncryptorWithChecksum(
			TEncryptor algorithm,
			THasher checksumHasher,
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
			: base(algorithm, encryptionKey, initializationVector, options)
		{
			this.hasher = new KeyedHasher<THasher>(
				checksumHasher, encryptionKey, initializationVector, this.AllowsNulls);
		}

		internal EncryptorWithChecksum(
			byte[] encryptionKey,
			out byte[] initializationVector,
			EncryptionOptions options)
			: base(encryptionKey, out initializationVector, options)
		{
			this.hasher = new KeyedHasher<THasher>(
				encryptionKey, initializationVector, this.AllowsNulls);
		}

		internal EncryptorWithChecksum(
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
			: base(encryptionKey, initializationVector, options)
		{
			this.hasher = new KeyedHasher<THasher>(
				encryptionKey, initializationVector, this.AllowsNulls);
		}

		public byte[] Encrypt(byte[] plaintextBytes, out byte[] checksum)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintextBytes != null);

			checksum = this.hasher.ComputeHash(plaintextBytes);

			return this.Encrypt(plaintextBytes);
		}

		public byte[] Encrypt(string plaintext, out byte[] checksum)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintext != null);

			return this.Encrypt(plaintext, Encryption.DefaultTextEncoding, out checksum);
		}

		public byte[] Encrypt(
			string plaintext,
			Encoding plaintextEncoding,
			out byte[] checksum)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintext != null);
			Contract.Requires<ArgumentNullException>(plaintextEncoding != null);

			// ReSharper disable once InvertIf
			if (plaintext == null)
			{
				checksum = null;

				return null;
			}

			return this.Encrypt(plaintextEncoding.GetBytes(plaintext), out checksum);
		}

		public string EncryptToString(
			string plaintext,
			out string checksum,
			ByteArrayStringEncoding cipherTextAndChecksumEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintext != null);

			return this.EncryptToString(
				plaintext, Encryption.DefaultTextEncoding, out checksum, cipherTextAndChecksumEncoding);
		}

		public string EncryptToString(
			string plaintext,
			Encoding plaintextEncoding,
			out string checksum,
			ByteArrayStringEncoding ciphertextEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintext != null);
			Contract.Requires<ArgumentNullException>(plaintextEncoding != null);

			// ReSharper disable once InvertIf
			if (plaintext == null)
			{
				checksum = null;

				return null;
			}

			return this.EncryptToString(
				plaintextEncoding.GetBytes(plaintext), out checksum, ciphertextEncoding);
		}

		public string EncryptToString(
			byte[] plaintextBytes,
			out string checksum,
			ByteArrayStringEncoding cipherTextAndChecksumEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintextBytes != null);

			byte[] checksumBytes;
			string encryptedString = this.Encrypt(plaintextBytes, out checksumBytes).ToEncodedString(cipherTextAndChecksumEncoding);

			checksum = checksumBytes.ToEncodedString(cipherTextAndChecksumEncoding);

			return encryptedString;
		}
	}
}