#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Security.Cryptography
	SymmetricTransformer.cs

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
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Threading;

	using MiscCorLib.Collections.Generic;

	/// <summary>
	/// Common base class for generic <see cref="Encryptor{T}"/>
	/// and <see cref="Decryptor{T}"/> types, with an internal
	/// abstract factory which automatically creates an instance
	/// of a specific <see cref="SymmetricAlgorithm"/> based on
	/// the generic type parameter <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">
	/// The type of <see cref="SymmetricAlgorithm"/>
	/// to use for encryption and decryption.
	/// </typeparam>
	/// <remarks>
	/// <para>
	/// This class is optimized for memory.
	/// Special implementation of <see cref="IDisposable"/>.
	/// </para>
	/// </remarks>
	public abstract class SymmetricTransformer<T> : IDisposable
		where T : SymmetricAlgorithm
	{
		private const byte BitsPerByte = 8;

		/// <summary>
		/// Private backing field for <see cref="IsEncryptor"/>
		/// and <see cref="IsDecryptor"/> properties.
		/// </summary>
		private readonly bool isEncryptor;

		/// <summary>
		/// An internal semaphore indicating responsibility for disposing
		/// an internal instance of <see cref="SymmetricAlgorithm"/>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// If <c>true</c>, the internal instance of
		/// <see cref="SymmetricAlgorithm"/>
		/// should <em>not</em> be disposed, even when
		/// <see cref="Dispose"/> is invoked.
		/// The instance of <see cref="SymmetricAlgorithm"/>
		/// was passed to a constructor overload, and its
		/// creator assumes responsibility for its disposal.
		/// If it were disposed here, the absence of the object
		/// might cause other trouble later.
		/// </para>
		/// <para>
		/// If <c>false</c>, the <see cref="SymmetricAlgorithm"/>
		/// was created internally by the lazy initializer of
		/// the <see cref="Algorithm"/> property, so this
		/// object is responsible for disposing it.
		/// </para>
		/// </remarks>
		private readonly bool preserveAlgorithm;

		private readonly EncryptionOptions options;

		private readonly byte[] encryptionKey;
		private readonly byte[] initializationVector;

		/// <summary>
		/// The <see cref="SymmetricAlgorithm"/>
		/// used for encryption and decryption.
		/// </summary>
		/// <remarks>
		/// This field cannot be readonly
		/// because it is lazily initialized,
		/// allowing a generic specification.
		/// </remarks>
		private T algorithm;

		/// <summary>
		/// <see cref="System.Security.Cryptography.ICryptoTransform"/>
		/// defines the basic operations of cryptographic transformations.
		/// </summary>
		private ICryptoTransform transform;

		/// <summary>
		/// Initializes a new instance of the <see cref="SymmetricTransformer{T}"/> class
		/// using a given <see cref="SymmetricAlgorithm"/> instance.
		/// </summary>
		/// <param name="algorithm">
		/// An instance of <see cref="SymmetricAlgorithm"/>
		/// to use for encryption or decryption.
		/// Beware of side effect: The values
		/// of its <see cref="SymmetricAlgorithm.Key"/>
		/// and <see cref="SymmetricAlgorithm.IV"/>
		/// properties will be changed.
		/// </param>
		/// <param name="isEncryptor"></param>
		/// <param name="encryptionKey"></param>
		/// <param name="initializationVector"></param>
		/// <param name="options"></param>
		/// <remarks>
		/// <para>
		/// This is <em>not a pure method</em>, so beware of this side effect:
		/// When the given <see cref="SymmetricAlgorithm"/>
		/// instance is passed to this constructor, the values
		/// of its <see cref="SymmetricAlgorithm.Key"/>
		/// and <see cref="SymmetricAlgorithm.IV"/>
		/// properties will be changed by code inside
		/// this constructor.
		/// </para>
		/// <para>
		/// Also note that the instance of <see cref="SymmetricAlgorithm"/>
		/// will not be disposed when this transformer object's
		/// <see cref="Dispose"/> method is invoked, so the
		/// same algorithm object may be re-used later, and its
		/// creator assumes responsibility to dispose of it.
		/// </para>
		/// </remarks>
		protected SymmetricTransformer(
			T algorithm,
			bool isEncryptor,
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
			: this(isEncryptor, encryptionKey, initializationVector, options)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			ValidateKeySize(algorithm, encryptionKey);

			this.algorithm = algorithm;
			this.algorithm.Key = this.encryptionKey;
			this.preserveAlgorithm = true;

			if (this.initializationVector == null)
			{
				// Always generate a new vector
				// to use for the data to be
				// encrypted by this instance.
				this.algorithm.GenerateIV();
				this.initializationVector = this.algorithm.IV;
			}
			else
			{
				ValidateBlockSize(this.algorithm, this.initializationVector);

				this.algorithm.IV = this.initializationVector;
			}
		}

		/// <summary>
		/// Initializes a new instance using an abstract factory
		/// to resolve an algorithm based on the generic type.
		/// </summary>
		/// <param name="isEncryptor"></param>
		/// <param name="encryptionKey"></param>
		/// <param name="initializationVector"></param>
		/// <param name="options"></param>
		protected SymmetricTransformer(
			bool isEncryptor,
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			this.isEncryptor = isEncryptor;
			this.encryptionKey = encryptionKey;
			this.initializationVector = initializationVector;

			this.options = options;
		}

		/// <summary>
		/// Lazy initializer of an internal instance of
		/// a <see cref="SymmetricAlgorithm"/>
		/// of type <typeparamref name="T"/>,
		/// created by <see cref="CreateValidAlgorithm"/>.
		/// </summary>
		protected internal T Algorithm
		{
			get
			{
				// Use an abstract factory to create an instance
				// of a specific given type of SymmetricAlgorithm,
				// based on the generic type parameter.
				return LazyInitializer.EnsureInitialized(
					ref this.algorithm, this.CreateValidAlgorithm);
			}
		}

		/// <summary>
		/// Gets a reference to an <see cref="ICryptoTransform"/>
		/// used for encryption or decryption, initializing the
		/// reference if none exists.
		/// </summary>
		private ICryptoTransform Transformer
		{
			get
			{
				return LazyInitializer.EnsureInitialized(
					ref this.transform,
					() => this.IsEncryptor
						? this.Algorithm.CreateEncryptor()
						: this.Algorithm.CreateDecryptor());
			}
		}

		/// <summary>
		/// Gets a value indicating whether this object
		/// encrypts plaintext bytes to ciphertext bytes.
		/// </summary>
		public bool IsEncryptor { get { return this.isEncryptor; } }

		/// <summary>
		/// Gets a value indicating whether this object
		/// decrypts ciphertext bytes to plaintext bytes.
		/// </summary>
		public bool IsDecryptor { get { return !this.IsEncryptor; } }

		/// <summary>
		/// Indicates whether the cryptographic operation
		/// allows null input
		/// returns <c></c>
		/// </summary>
		public bool AllowsNulls
		{
			get { return (this.options & EncryptionOptions.AllowNullInput) == EncryptionOptions.AllowNullInput; }
		}

		public bool ThrowsCryptographicExceptions
		{
			get { return (this.options & EncryptionOptions.SuppressCryptographicExceptions) != EncryptionOptions.SuppressCryptographicExceptions; }
		}

		public bool ThrowsDecryptionChecksumExceptions
		{
			get { return (this.options & EncryptionOptions.SuppressDecryptionChecksumExceptions) != EncryptionOptions.SuppressDecryptionChecksumExceptions; }
		}

		public void Dispose()
		{
			if (this.transform != null)
			{
				this.transform.Dispose();
			}

			if ((this.algorithm != null) && !this.preserveAlgorithm)
			{
				this.algorithm.Clear();
				this.algorithm.Dispose();
			}
		}

		/// <summary>
		/// Encrypts or decrypts.
		/// </summary>
		/// <param name="originalBytes"></param>
		/// <returns></returns>
		protected byte[] Transform(byte[] originalBytes)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || originalBytes != null);
			if (originalBytes == null) return null;

			// Declare a local variable to be the output value.
			byte[] transformedBytes;

			// Do not place a "using" around this MemoryStream.
			// CA2202: Do not dispose objects multiple times
			// https://msdn.microsoft.com/en-us/library/ms182334.aspx
			MemoryStream backingStream = null;

			try
			{
				// This is the destination of Write operations.
				backingStream = new MemoryStream();

				using (CryptoStream cryptoStream = new CryptoStream(
					backingStream, this.Transformer, CryptoStreamMode.Write))
				{
					// Use the block size in bits as the arbitrary size of the buffer,
					// not for any particular reason except to make the estimate
					// of an appropriate size relative to the algorithm.
					int bufferSize = this.Algorithm.BlockSize;
					if (originalBytes.Length <= bufferSize)
					{
						// If the message is shorter than the buffer,
						// use one simple method call to transform.
						cryptoStream.Write(originalBytes, 0, originalBytes.Length);
					}
					else
					{
						// If the message is long, use an in-memory buffer...
						using (MemoryStream originalBytesStream
							//// ...and make sure it read-only:
							= new MemoryStream(originalBytes, false))
						{
							int bytesRead;
							byte[] buffer = new byte[bufferSize];

							// Set the position to the beginning of the stream.
							originalBytesStream.Seek(0, SeekOrigin.Begin);

							// Then read into a buffer until there is
							// no more of originalBytesStream to read.
							do
							{
								// Read from originalBytesStream into the buffer...
								bytesRead = originalBytesStream.Read(buffer, 0, bufferSize);

								// ...then write from the buffer to the destination stream.
								cryptoStream.Write(buffer, 0, bytesRead);
							}
							while (bytesRead > 0);
						}
					}

					// Truncate to avoid error about incorrect padding.
					cryptoStream.FlushFinalBlock();

					// Set return value.
					transformedBytes = backingStream.ToArray();
				}
			}
			finally
			{
				// CA2202: Do not dispose objects multiple times
				// https://msdn.microsoft.com/en-us/library/ms182334.aspx
				if (backingStream != null)
				{
					// Since a MemoryStream has no unmanaged resources
					// (unlike other implementers of Stream) this step isn't
					// strictly necessary...
					backingStream.Dispose();
				}
			}

			return transformedBytes;
		}

		#region [ Private Methods to Create a SymmetricAlgorithm Instance based on its Generic Type Name ]

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		internal static T CreateAlgorithm()
		{
			T algorithm = SymmetricAlgorithm.Create(typeof(T).ToString()) as T;
			if (algorithm == null)
			{
				throw new InvalidOperationException(
					string.Concat(typeof(T).FullName, " is not a symmetric encryption algorithm!"));
			}

			return algorithm;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private static void ValidateKeySize(
			SymmetricAlgorithm algorithm, IReadOnlyCollection<byte> encryptionKey)
		{
			// Array length is bytes, Key size measured in bits.
			int encryptionKeySize = encryptionKey.Count * BitsPerByte; // (BitsPerByte == 8)
			if (algorithm.ValidKeySize(encryptionKeySize))
			{
				return;
			}

			string message = string.Format(
				"The size of the given encryption key ({0} bytes, {1} bits) does not match a valid key size ({3}) for the \"{2}\" algorithm.",
				encryptionKey.Count,
				encryptionKeySize,
				algorithm.GetType().Name,
				algorithm.LegalKeySizes.Select(
					legalKeySize => string.Format(
						"Max{0}Min{1}Skip{2}", legalKeySize.MaxSize, legalKeySize.MinSize, legalKeySize.SkipSize))
					.ToDelimitedString(", "));

			////	Contract.Requires<ArgumentOutOfRangeException>(
			////		algorithm.ValidKeySize(encryptionKeySize), message);
			throw new TypeInitializationException(
				"MiscCorLib.Security.Cryptography.SymmetricTransformer",
				new ArgumentOutOfRangeException("encryptionKey", message));
		}

		private static void ValidateBlockSize(
			SymmetricAlgorithm algorithm, IReadOnlyCollection<byte> initializationVector)
		{
			// Array length is bytes, IV size measured in bits.
			int initializationVectorSize = initializationVector.Count * BitsPerByte; // (BitsPerByte == 8)
			if (algorithm.ValidKeySize(initializationVectorSize))
			{
				return;
			}

			string message = string.Format(
				"The size of the given initialization vector ({0}) does not match a valid block size ({2}) for the \"{1}\" algorithm.",
				initializationVector.Count,
				algorithm.GetType().Name,
				algorithm.LegalBlockSizes.Select(
					legalBlockSize => string.Format(
						"Max{0}Min{1}Skip{2}", legalBlockSize.MaxSize, legalBlockSize.MinSize, legalBlockSize.SkipSize))
					.ToDelimitedString(", "));

			////	Contract.Requires<ArgumentOutOfRangeException>(
			////		algorithm.ValidBlockSize(initializationVectorSize), message);
			throw new TypeInitializationException(
				"MiscCorLib.Security.Cryptography.SymmetricTransformer",
				new ArgumentOutOfRangeException("initializationVector", message));
		}

		/// <summary>
		/// Internal method for lazy initialization
		/// of the <see cref="algorithm"/> field by
		/// the <see cref="Algorithm"/> property.
		/// </summary>
		/// <returns>
		/// An instance of <see cref="SymmetricAlgorithm"/>
		/// initialized using the <see cref="encryptionKey"/>
		/// and <see cref="initializationVector"/> values
		/// passed to the constructor.
		/// </returns>
		private T CreateValidAlgorithm()
		{
			T validAlgorithm = CreateAlgorithm();

			ValidateKeySize(validAlgorithm, this.encryptionKey);

			validAlgorithm.Key = this.encryptionKey;

			if (this.initializationVector == null)
			{
				// If encrypting and the algorithm is lazily initialized,
				// use a different initialization vector
				// for each instance of an encryptor.
				validAlgorithm.GenerateIV();
			}
			else
			{
				// If decrypting, validate the given initialization vector.
				ValidateBlockSize(validAlgorithm, this.initializationVector);

				// Use the given initialization vector for the decryptor.
				validAlgorithm.IV = this.initializationVector;
			}

			return validAlgorithm;
		}

		#endregion
	}
}