namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading;

	using JetBrains.Annotations;

	/// <summary>
	/// Common base class for generic <see cref="Encryptor{T}"/>
	/// and <see cref="Decryptor{T}"/> types, with an internal
	/// abstract factory which can automatically create an instance
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
		private readonly bool isEncryptor;
		private readonly byte[] encryptionKey;
		private readonly byte[] initializationVector;
		private readonly bool preserveAlgorithm;

		private T algorithm;
		private ICryptoTransform transform;

		public bool IsEncryptor { get { return this.isEncryptor; } }

		public bool IsDecryptor { get { return !this.IsEncryptor; } }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="algorithm"></param>
		/// <param name="isEncryptor"></param>
		/// <param name="encryptionKey"></param>
		/// <param name="initializationVector"></param>
		/// <remarks>
		/// <para>
		/// Beware of side effects. If a <see cref="SymmetricAlgorithm"/>
		/// instanceis passed to this constructor, the values of
		/// its <see cref="SymmetricAlgorithm.Key"/>
		/// and <see cref="SymmetricAlgorithm.IV"/>
		/// properties will be changed by code inside
		/// the constructor.
		/// </para>
		/// </remarks>
		protected SymmetricTransformer(
			[NotNull] T algorithm, bool isEncryptor, [NotNull] byte[] encryptionKey, byte[] initializationVector)
			: this(isEncryptor, encryptionKey, initializationVector)
		{
			Contract.Requires(algorithm != null);
			Contract.Requires(encryptionKey != null);

			this.algorithm = algorithm;
			this.preserveAlgorithm = true;

			ValidateKeySize(this.algorithm, this.encryptionKey);

			this.algorithm.Key = this.encryptionKey;

			if (initializationVector == null)
			{
				// Always generate a new vector
				// to use for the data to be
				// encrypted by this instance.
				this.algorithm.GenerateIV();
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
		protected SymmetricTransformer(
			bool isEncryptor, [NotNull] byte[] encryptionKey, byte[] initializationVector)
		{
			Contract.Requires(encryptionKey != null);

			this.isEncryptor = isEncryptor;
			this.encryptionKey = encryptionKey;
			this.initializationVector = initializationVector;
		}

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

		public void Dispose()
		{
			if (this.transform != null)
			{
				this.transform.Dispose();
			}

			if ((this.algorithm != null) && (!this.preserveAlgorithm))
			{
				this.algorithm.Dispose();
			}
		}

		protected byte[] Transform(byte[] plaintextBytes)
		{
			// TODO: Here is where to implement looping over a buffer.
			return this.Transformer.TransformFinalBlock(plaintextBytes, 0, 0);
		}

		protected byte[] Transform(string plaintext, Encoding encoding)
		{
			return Transform(encoding.GetBytes(plaintext));
		}

		// Method for lazy initialization of algorithm field.
		private T CreateValidAlgorithm()
		{
			T validAlgorithm = CreateAlgorithm();

			ValidateKeySize(validAlgorithm, this.encryptionKey);

			validAlgorithm.Key = this.encryptionKey;

			if (this.initializationVector == null)
			{
				// If encrypting, use a different initialization vector
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

		private static T CreateAlgorithm()
		{
			T algorithm = SymmetricAlgorithm.Create((typeof(T)).ToString()) as T;
			if (algorithm == null)
			{
				throw new InvalidOperationException(string.Concat(typeof(T).FullName, " is not a symmetric encryption algorithm!"));
			}

			return algorithm;
		}

		private static void ValidateKeySize(
			SymmetricAlgorithm algorithm, byte[] encryptionKey)
		{
			Contract.Requires(algorithm.ValidKeySize(encryptionKey.Length));
		}

		private static void ValidateBlockSize(
			SymmetricAlgorithm algorithm, byte[] initializationVector)
		{
			Contract.Requires(algorithm.ValidKeySize(initializationVector.Length));
		}
	}
}