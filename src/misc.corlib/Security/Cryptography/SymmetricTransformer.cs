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
		private readonly bool preserveAlgorithm;
		private readonly byte[] encryptionKey;
		private readonly byte[] initializationVector;
		private T algorithm;
		private ICryptoTransform transform;

		public bool IsEncryptor { get { return (this.initializationVector == null); } }

		public bool IsDecryptor { get { return !this.IsEncryptor; } }

		protected SymmetricTransformer(
			[NotNull] T algorithm, byte[] encryptionKey, byte[] initializationVector)
			: this(encryptionKey, initializationVector)
		{
			Contract.Requires(algorithm != null);

			ValidateKeySize(algorithm, this.encryptionKey);

			this.preserveAlgorithm = true;
			this.algorithm = algorithm;

			if (initializationVector == null)
			{
				// Always generate a new vector
				// to use for the data to be
				// encrypted by this instance.
				this.algorithm.GenerateIV();
			}
		}

		protected SymmetricTransformer(
			byte[] encryptionKey, byte[] initializationVector)
		{
			Contract.Requires(encryptionKey != null);

			// If no initialization vector is given,
			// then this is an encryptor.
			this.encryptionKey = encryptionKey;
			this.initializationVector = initializationVector;
		}

		protected T Algorithm
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
						? this.Algorithm.CreateEncryptor(this.encryptionKey, this.Algorithm.IV)
						: this.Algorithm.CreateDecryptor(this.encryptionKey, this.initializationVector));
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

		private T CreateValidAlgorithm()
		{
			T validAlgorithm = CreateAlgorithm();

			ValidateKeySize(validAlgorithm, this.encryptionKey);

			validAlgorithm.Key = this.encryptionKey;

			if (this.initializationVector != null)
			{
				ValidateBlockSize(validAlgorithm, this.initializationVector);

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
			// TODO: Implement key and block size validation
			////	throw new CryptographicException();
		}

		private static void ValidateBlockSize(
			SymmetricAlgorithm algorithm, byte[] initializationVector)
		{
			// TODO: Implement key and block size validation
			////	throw new CryptographicException();
		}
	}
}