#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Security.Cryptography
	Decryptor.cs

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

	/// <summary>
	/// Encapsulation of a decryption operation
	/// with a simple contract: A decryptor decrypts
	/// ciphertext bytes using any implementation of
	/// <see cref="SymmetricAlgorithm"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This type is derived from the generic
	/// <see cref="Decryptor{T}"/>. Refer to
	/// remarks there for more details.
	/// </para>
	/// </remarks>
	public sealed class Decryptor : Decryptor<SymmetricAlgorithm>
	{
		public Decryptor(
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

	public class Decryptor<T> : SymmetricTransformer<T>
		where T : SymmetricAlgorithm
	{
		public Decryptor(
			T algorithm,
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
			: base(algorithm, false, encryptionKey, initializationVector, options)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(initializationVector != null);
		}

		public Decryptor(
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
			: base(false, encryptionKey, initializationVector, options)
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(initializationVector != null);
		}

		public byte[] Decrypt(
			byte[] ciphertextBytes)
		{
			return this.Transform(ciphertextBytes);
		}
	}
}