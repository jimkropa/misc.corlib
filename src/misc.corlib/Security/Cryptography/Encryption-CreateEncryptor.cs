#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Security.Cryptography
	Encryption-CreateEncryptor.cs

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

	// Refer to Encryption.cs for documentation comments of partial class.
	public static partial class Encryption
	{
		#region [ Overloads of CreateEncryptor Extension Method for SymmetricAlgorithm ]

		public static Encryptor CreateEncryptor(
			this SymmetricAlgorithm algorithm,
			byte[] encryptionKey,
			out byte[] randomSalt,
			EncryptionOptions options = DefaultOptions)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			return new Encryptor(algorithm, encryptionKey, out randomSalt, options);
		}

		public static Encryptor CreateEncryptor(
			this SymmetricAlgorithm algorithm,
			string secretKey,
			out byte[] randomSalt,
			EncryptionOptions options = DefaultOptions)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(secretKey != null);

			// In this case, use the more robustly random
			// key derivation algorithm to create the salt,
			// instead of the SymmetricAlgorithm.
			return algorithm.CreateEncryptorWithGivenSalt(
				DeriveEncryptionKeyAndSaltFromPassword(secretKey, algorithm.KeySize, algorithm.BlockSize, out randomSalt),
				randomSalt,
				options);
		}

		public static Encryptor CreateEncryptor(
			this SymmetricAlgorithm algorithm,
			string secretKey,
			out string randomSalt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			EncryptionOptions options = DefaultOptions)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(secretKey != null);

			byte[] randomSaltBytes;
			Encryptor encryptor = algorithm.CreateEncryptor(secretKey, out randomSaltBytes, options);

			randomSalt = randomSaltBytes.ToEncodedString(saltEncoding);

			return encryptor;
		}

		#endregion

		#region [ Overloads of CreateEncryptor Extension Method using a given Salt ]

		public static Encryptor CreateEncryptorWithGivenSalt(
			this SymmetricAlgorithm algorithm,
			byte[] encryptionKey,
			byte[] salt,
			EncryptionOptions options = DefaultOptions)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			return new Encryptor(algorithm, encryptionKey, salt, options);
		}

		public static Encryptor CreateEncryptorWithGivenSalt(
			this SymmetricAlgorithm algorithm,
			string secretKey,
			byte[] salt,
			EncryptionOptions options = DefaultOptions)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(secretKey != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			return algorithm.CreateEncryptorWithGivenSalt(
				DeriveEncryptionKeyFromPasswordAndSalt(secretKey, algorithm.KeySize, salt),
				salt,
				options);
		}

		public static Encryptor CreateEncryptorWithGivenSalt(
			this SymmetricAlgorithm algorithm,
			string secretKey,
			string salt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			EncryptionOptions options = DefaultOptions)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(secretKey != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			return algorithm.CreateEncryptorWithGivenSalt(secretKey, salt.ToByteArray(saltEncoding), options);
		}

		#endregion

		#region [ Overloads of Generic CreateEncryptor Method ]

		public static Encryptor<T> CreateEncryptor<T>(
			byte[] encryptionKey,
			out byte[] randomSalt,
			EncryptionOptions options = DefaultOptions)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			return new Encryptor<T>(encryptionKey, out randomSalt, options);
		}

		public static Encryptor<T> CreateEncryptor<T>(
			string secretKey,
			out byte[] randomSalt,
			EncryptionOptions options = DefaultOptions)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(secretKey != null);

			int keySize, blockSize;
			using (T algorithm = SymmetricTransformer<T>.CreateAlgorithm())
			{
				keySize = algorithm.KeySize;
				blockSize = algorithm.BlockSize;
			}

			return new Encryptor<T>(
				DeriveEncryptionKeyAndSaltFromPassword(secretKey, keySize, blockSize, out randomSalt),
				randomSalt,
				options);
		}

		public static Encryptor<T> CreateEncryptor<T>(
			string secretKey,
			out string randomSalt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			EncryptionOptions options = DefaultOptions)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(secretKey != null);

			byte[] randomSaltBytes;
			Encryptor<T> encryptor = CreateEncryptor<T>(secretKey, out randomSaltBytes, options);

			randomSalt = randomSaltBytes.ToEncodedString(saltEncoding);

			return encryptor;
		}

		#endregion

		#region [ Overloads of Generic CreateEncryptor Method using a given Salt ]

		public static Encryptor<T> CreateEncryptorWithGivenSalt<T>(
			byte[] encryptionKey,
			byte[] salt,
			EncryptionOptions options = DefaultOptions)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			return new Encryptor<T>(encryptionKey, salt, options);
		}

		public static Encryptor<T> CreateEncryptorWithGivenSalt<T>(
			string secretKey,
			byte[] salt,
			EncryptionOptions options = DefaultOptions)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(secretKey != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			int keySize;
			using (T algorithm = SymmetricTransformer<T>.CreateAlgorithm())
			{
				keySize = algorithm.KeySize;
			}

			return new Encryptor<T>(
				DeriveEncryptionKeyFromPasswordAndSalt(secretKey, keySize, salt),
				salt,
				options);
		}

		public static Encryptor<T> CreateEncryptorWithGivenSalt<T>(
			string secretKey,
			string salt,
			ByteArrayStringEncoding saltEncoding = ConvertByteArray.DefaultStringEncoding,
			EncryptionOptions options = DefaultOptions)
			where T : SymmetricAlgorithm
		{
			Contract.Requires<ArgumentNullException>(secretKey != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			return CreateEncryptorWithGivenSalt<T>(secretKey, salt.ToByteArray(
				(options & EncryptionOptions.AllowNullInput) == EncryptionOptions.AllowNullInput,
				saltEncoding));
		}

		#endregion
	}
}