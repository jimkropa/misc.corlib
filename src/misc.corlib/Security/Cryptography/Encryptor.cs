#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Security.Cryptography
	Encryptor.cs

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
	/// Encapsulation of an encryption operation
	/// with a simple contract: An encryptor encrypts
	/// plaintext bytes using any implementation of
	/// <see cref="SymmetricAlgorithm" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This type is derived from the generic
	/// <see cref="Encryptor{T}" />. Refer to
	/// remarks there for more details.
	/// </para>
	/// </remarks>
	public sealed class Encryptor : Encryptor<SymmetricAlgorithm>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Encryptor" /> class
		/// which generates a random initialization vector using the
		/// <see cref="SymmetricAlgorithm.GenerateIV" /> method
		/// of its embedded <see cref="SymmetricAlgorithm" />.
		/// The new <paramref name="initializationVector" /> must be
		/// preserved, in addition to the <paramref name="encryptionKey" />,
		/// to decrypt ciphertext created by this encryptor.
		/// </summary>
		/// <param name="algorithm">
		/// An instance of <see cref="SymmetricAlgorithm" />
		/// to use for an encryption operation.
		/// </param>
		/// <param name="encryptionKey">
		/// A value for the <see cref="SymmetricAlgorithm.Key" />
		/// of the embedded <see cref="SymmetricAlgorithm" />.
		/// </param>
		/// <param name="initializationVector">
		/// Returns a new pseudo-random value which has been
		/// generated for the <see cref="SymmetricAlgorithm.IV" />
		/// of the embedded <see cref="SymmetricAlgorithm" />.
		/// This value must be preserved, in addition to the
		/// <paramref name="encryptionKey" />, to decrypt
		/// ciphertext created by this <see cref="Encryptor" />.
		/// </param>
		/// <param name="options">
		/// 
		/// </param>
		internal Encryptor(
			SymmetricAlgorithm algorithm,
			byte[] encryptionKey,
			out byte[] initializationVector,
			EncryptionOptions options)
			: base(algorithm, encryptionKey, out initializationVector, options)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
		}

		internal Encryptor(
			SymmetricAlgorithm algorithm,
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
			: base(algorithm, encryptionKey, initializationVector, options)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(initializationVector != null);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <remarks>
	/// <para>
	/// , adding a layer of security by preventing
	/// cryptographic dictionary attacks.
	/// </para>
	/// </remarks>
	public class Encryptor<T> : SymmetricTransformer<T>
		where T : SymmetricAlgorithm
	{
		#region [ Internal Constructor Overloads used by Encryption.CreateEncryptor ]

		// Do not use optional parameters...
		// ...save that for the factory method.

		internal Encryptor(
			T algorithm,
			byte[] encryptionKey,
			out byte[] initializationVector,
			EncryptionOptions options)
			: base(algorithm, true, encryptionKey, null, options)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			// Set output parameter.
			initializationVector = this.Algorithm.IV;
		}

		internal Encryptor(
			T algorithm,
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
			: base(algorithm, true, encryptionKey, initializationVector, options)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(initializationVector != null);
		}

		internal Encryptor(
			byte[] encryptionKey,
			out byte[] initializationVector,
			EncryptionOptions options)
			: base(true, encryptionKey, null, options)
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			// Set output parameter.
			initializationVector = this.Algorithm.IV;
		}

		internal Encryptor(
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
			: base(true, encryptionKey, initializationVector, options)
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(initializationVector != null);
		}

		#endregion

		public byte[] Encrypt(byte[] plaintextBytes)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintextBytes != null);

			return this.Transform(plaintextBytes);
		}

		public byte[] Encrypt(string plaintext)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintext != null);

			return this.Encrypt(
				plaintext, Encryption.DefaultTextEncoding);
		}

		public byte[] Encrypt(
			string plaintext,
			Encoding plaintextEncoding)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintext != null);
			Contract.Requires<ArgumentNullException>(plaintextEncoding != null);

			return plaintext == null ? null
				: this.Transform(plaintextEncoding.GetBytes(plaintext));
		}

		public string EncryptToString(
			string plaintext,
			ByteArrayStringEncoding cipherTextEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintext != null);

			return this.EncryptToString(
				plaintext, Encryption.DefaultTextEncoding, cipherTextEncoding);
		}

		public string EncryptToString(
			string plaintext,
			Encoding plaintextEncoding,
			ByteArrayStringEncoding ciphertextEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintext != null);
			Contract.Requires<ArgumentNullException>(plaintextEncoding != null);

			return plaintext == null ? null
				: this.EncryptToString(plaintextEncoding.GetBytes(plaintext), ciphertextEncoding);
		}

		public string EncryptToString(
			byte[] plaintextBytes,
			ByteArrayStringEncoding cipherTextEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || plaintextBytes != null);

			return this.Encrypt(plaintextBytes).ToEncodedString(cipherTextEncoding);
		}
	}
}