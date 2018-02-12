using System;
using System.Security.Cryptography;
using System.Threading;

namespace MiscCorLib.Security.Cryptography
{
	public sealed class Hasher : Hasher<HashAlgorithm>
	{
		internal Hasher(
			HashAlgorithm algorithm,
			byte[] salt,
			bool allowNulls)
			: base(algorithm, salt, allowNulls)
		{
		}
	}

	/// <summary>
	/// Common base class for generic <see cref="Encryptor{T}" />
	/// and <see cref="Decryptor{T}" /> types, with an internal
	/// abstract factory which automatically creates an instance
	/// of a specific <see cref="SymmetricAlgorithm" /> based on
	/// the generic type parameter <typeparamref name="T" />.
	/// </summary>
	/// <typeparam name="T">
	/// The type of <see cref="SymmetricAlgorithm" />
	/// to use for encryption and decryption.
	/// </typeparam>
	/// <remarks>
	/// <para>
	/// This class is optimized for memory.
	/// Special implementation of <see cref="IDisposable" />.
	/// </para>
	/// </remarks>
	public class Hasher<T> : IDisposable
		where T : HashAlgorithm
	{
		/// <summary>
		/// An internal semaphore indicating responsibility for disposing
		/// an internal instance of <see cref="SymmetricAlgorithm" />.
		/// </summary>
		/// <remarks>
		/// <para>
		/// If <c>true</c>, the internal instance of
		/// <see cref="SymmetricAlgorithm" />
		/// should <em>not</em> be disposed, even when
		/// <see cref="Dispose" /> is invoked.
		/// The instance of <see cref="SymmetricAlgorithm" />
		/// was passed to a constructor overload, and its
		/// creator assumes responsibility for its disposal.
		/// If it were disposed here, the absence of the object
		/// might cause other trouble later.
		/// </para>
		/// <para>
		/// If <c>false</c>, the <see cref="SymmetricAlgorithm" />
		/// was created internally by the lazy initializer of
		/// the <see cref="Algorithm" /> property, so this
		/// object is responsible for disposing it.
		/// </para>
		/// </remarks>
		private readonly bool preserveAlgorithm;

		/// <summary>
		/// Indicates whether the cryptographic operation
		/// allows null input
		/// returns <c></c>
		/// </summary>
		public readonly bool AllowsNulls;

		private readonly byte[] salt;

		/// <summary>
		/// The <see cref="HashAlgorithm" />
		/// used for cryptographic hashing.
		/// </summary>
		/// <remarks>
		/// This field cannot be readonly
		/// because it is lazily initialized,
		/// allowing a generic specification.
		/// </remarks>
		private T algorithm;

		/// <summary>
		/// Initializes a new instance of the <see cref="SymmetricTransformer{T}" /> class
		/// using a given <see cref="SymmetricAlgorithm" /> instance.
		/// </summary>
		/// <param name="algorithm">
		/// An instance of <see cref="SymmetricAlgorithm" />
		/// to use for encryption or decryption.
		/// Beware of side effect: The values
		/// of its <see cref="SymmetricAlgorithm.Key" />
		/// and <see cref="SymmetricAlgorithm.IV" />
		/// properties will be changed.
		/// </param>
		/// <param name="salt"></param>
		/// <param name="allowNulls"></param>
		/// <remarks>
		/// <para>
		/// This is <em>not a pure method</em>, so beware of this side effect:
		/// When the given <see cref="SymmetricAlgorithm" />
		/// instance is passed to this constructor, the values
		/// of its <see cref="SymmetricAlgorithm.Key" />
		/// and <see cref="SymmetricAlgorithm.IV" />
		/// properties will be changed by code inside
		/// this constructor.
		/// </para>
		/// <para>
		/// Also note that the instance of <see cref="SymmetricAlgorithm" />
		/// will not be disposed when this transformer object's
		/// <see cref="Dispose" /> method is invoked, so the
		/// same algorithm object may be re-used later, and its
		/// creator assumes responsibility to dispose of it.
		/// </para>
		/// </remarks>
		internal Hasher(
			T algorithm,
			byte[] salt,
			bool allowNulls)
			: this(salt, allowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			this.algorithm = algorithm;
			this.preserveAlgorithm = true;
		}

		/// <summary>
		/// Initializes a new instance using an abstract factory
		/// to resolve an algorithm based on the generic type.
		/// </summary>
		/// <param name="salt"></param>
		/// <param name="allowNulls"></param>
		internal Hasher(
			byte[] salt,
			bool allowNulls)
		{
			Contract.Requires<ArgumentNullException>(salt != null);

			this.salt = salt;
			this.AllowsNulls = allowNulls;
		}

		/// <summary>
		/// Lazy initializer of an internal instance
		/// of a <see cref="HashAlgorithm" />
		/// of type <typeparamref name="T" />,
		/// created by <see cref="CreateAlgorithm" />.
		/// </summary>
		protected internal T Algorithm
		{
			get
			{
				// Use an abstract factory to create an instance
				// of a specific given type of SymmetricAlgorithm,
				// based on the generic type parameter.
				return LazyInitializer.EnsureInitialized(
					ref this.algorithm, this.CreateAlgorithm);
			}
		}

		public void Dispose()
		{
			if ((this.algorithm != null) && !this.preserveAlgorithm)
			{
				this.algorithm.Dispose();
			}
		}



		public byte[] ComputeHash(byte[] inputStream)
		{
			Contract.Requires<ArgumentNullException>(this.AllowsNulls || inputStream != null);
			if (inputStream == null) return null;

			// TODO: Here is where to implement looping over a buffer.
			//byte[] transformedBytes;
			//using (MemoryStream backingStream = new MemoryStream())
			//{
			//	using (CryptoStream cryptoStream = new CryptoStream(
			//		backingStream, this.Transformer, CryptoStreamMode.Write))
			//	{
			//		cryptoStream.Write(originalBytes, 0, originalBytes.Length);
			//		cryptoStream.FlushFinalBlock();
			//		cryptoStream.Close();
			//	}
			//
			//	transformedBytes = backingStream.ToArray();
			//}

			// TODO: First add the salt!
			return this.algorithm.ComputeHash(inputStream);

				//				// Compute the hash of the input file.
				//	byte[] hashValue = hmac.ComputeHash(inStream);
				//	// Reset inStream to the beginning of the file.
				//	inStream.Position = 0;
				//	// Write the computed hash value to the output file.
				//	outStream.Write(hashValue, 0, hashValue.Length);
				//	// Copy the contents of the sourceFile to the destFile.
				//	int bytesRead;
				//	// read 1K at a time
				//	byte[] buffer = new byte[1024];
				//	do
				//	{
				//		// Read from the wrapping CryptoStream.
				//		bytesRead = inStream.Read(buffer, 0, 1024);
				//		outStream.Write(buffer, 0, bytesRead);
				//	} while (bytesRead > 0);
				//}
		}

		#region [ Private Method to Create a HashAlgorithm Instance based on its Generic Type Name ]

		protected virtual T CreateAlgorithm()
		{
			T hashAlgorithm = HashAlgorithm.Create(typeof(T).ToString()) as T;
			if (hashAlgorithm == null)
			{
				throw new InvalidOperationException(
					string.Concat(typeof(T).FullName, " is not a hashing algorithm!"));
			}

			return hashAlgorithm;
		}

		#endregion
	}
}