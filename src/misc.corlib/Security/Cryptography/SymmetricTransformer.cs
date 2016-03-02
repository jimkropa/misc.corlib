namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Security.Cryptography;
	using System.Threading;

	using JetBrains.Annotations;

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

		public readonly bool AllowNulls;

		private readonly byte[] encryptionKey;
		private readonly byte[] initializationVector;

		private T algorithm;
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
		/// <param name="allowNulls"></param>
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
			[NotNull] T algorithm,
			bool isEncryptor,
			[NotNull] byte[] encryptionKey,
			byte[] initializationVector,
			bool allowNulls)
			: this(isEncryptor, encryptionKey, initializationVector, allowNulls)
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
		/// <param name="allowNulls"></param>
		protected SymmetricTransformer(
			bool isEncryptor,
			[NotNull] byte[] encryptionKey,
			byte[] initializationVector,
			bool allowNulls)
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			this.isEncryptor = isEncryptor;
			this.encryptionKey = encryptionKey;
			this.initializationVector = initializationVector;
			this.AllowNulls = allowNulls;
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

		public void Dispose()
		{
			if (this.transform != null)
			{
				this.transform.Dispose();
			}

			if ((this.algorithm != null) && !this.preserveAlgorithm)
			{
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
			Contract.Requires<ArgumentNullException>(this.AllowNulls || originalBytes != null);
			if (originalBytes == null) return null;

			// TODO: Here is where to implement looping over a buffer.
			// The other place is in ConvertByteArray.ToText
			byte[] transformedBytes;
			using (MemoryStream backingStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(
					backingStream, this.Transformer, CryptoStreamMode.Write))
				{
					cryptoStream.Write(originalBytes, 0, originalBytes.Length);
					cryptoStream.FlushFinalBlock();
					cryptoStream.Close();
				}

				transformedBytes = backingStream.ToArray();
			}

			return transformedBytes;
		}

		#region [ Private Methods to Create a SymmetricAlgorithm Instance based on its Generic Type Name ]

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
			////	Contract.Requires<ArgumentOutOfRangeException>(
			////		algorithm.ValidKeySize(encryptionKey.Count),
			////		"The size of the given encryption key does not match the valid key size for the algorithm.");

			if (!algorithm.ValidKeySize(encryptionKey.Count))
			{
				throw new TypeInitializationException(
					"MiscCorLib.Security.Cryptography.SymmetricTransformer",
					new ArgumentOutOfRangeException(
						"encryptionKey",
						string.Format(
							"The size of the given encryption key ({0}) does not match a valid key size for the \"{1}\" algorithm.",
							encryptionKey.Count,
							algorithm.GetType().Name)));
			}
		}

		private static void ValidateBlockSize(
			SymmetricAlgorithm algorithm, IReadOnlyCollection<byte> initializationVector)
		{
			////	Contract.Requires<ArgumentOutOfRangeException>(
			////		algorithm.ValidKeySize(initializationVector.Count),
			////		"The size of the given initialization vector does not match the valid block size for the algorithm.");

			if (!algorithm.ValidKeySize(initializationVector.Count))
			{
				throw new TypeInitializationException(
					"MiscCorLib.Security.Cryptography.SymmetricTransformer",
					new ArgumentOutOfRangeException(
						"initializationVector",
						string.Format(
							"The size of the given initialization vector ({0}) does not match a valid block size for the \"{1}\" algorithm.",
							initializationVector.Count,
							algorithm.GetType().Name)));
			}
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