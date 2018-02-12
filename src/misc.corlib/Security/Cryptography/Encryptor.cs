using System;
using System.Security.Cryptography;
using System.Text;

namespace MiscCorLib.Security.Cryptography
{
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
			if (algorithm == null)
			{
				throw new ArgumentNullException(nameof(algorithm));
			}

			if (encryptionKey == null)
			{
				throw new ArgumentNullException(nameof(encryptionKey));
			}
		}

		internal Encryptor(
			SymmetricAlgorithm algorithm,
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
			: base(algorithm, encryptionKey, initializationVector, options)
		{
			if (algorithm == null)
			{
				throw new ArgumentNullException(nameof(algorithm));
			}

			if (encryptionKey == null)
			{
				throw new ArgumentNullException(nameof(encryptionKey));
			}

			if (initializationVector == null)
			{
				throw new ArgumentNullException(nameof(initializationVector));
			}
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
			if (algorithm == null)
			{
				throw new ArgumentNullException(nameof(algorithm));
			}

			if (encryptionKey == null)
			{
				throw new ArgumentNullException(nameof(encryptionKey));
			}

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
			if (algorithm == null)
			{
				throw new ArgumentNullException(nameof(algorithm));
			}

			if (encryptionKey == null)
			{
				throw new ArgumentNullException(nameof(encryptionKey));
			}

			if (initializationVector == null)
			{
				throw new ArgumentNullException(nameof(initializationVector));
			}
		}

		internal Encryptor(
			byte[] encryptionKey,
			out byte[] initializationVector,
			EncryptionOptions options)
			: base(true, encryptionKey, null, options)
		{
			if (encryptionKey == null)
			{
				throw new ArgumentNullException(nameof(encryptionKey));
			}

			// Set output parameter.
			initializationVector = this.Algorithm.IV;
		}

		internal Encryptor(
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
			: base(true, encryptionKey, initializationVector, options)
		{
			if (encryptionKey == null)
			{
				throw new ArgumentNullException(nameof(encryptionKey));
			}

			if (initializationVector == null)
			{
				throw new ArgumentNullException(nameof(initializationVector));
			}
		}

		#endregion

		public byte[] Encrypt(byte[] plaintextBytes)
		{
			if ((!this.AllowsNulls) && plaintextBytes == null)
			{
				throw new ArgumentNullException(
					nameof(plaintextBytes),
					"Requires a non-null plaintextBytes value, or set AllowsNulls=true.");
			}

			return this.Transform(plaintextBytes);
		}

		public byte[] Encrypt(string plaintext)
		{
			if ((!this.AllowsNulls) && plaintext == null)
			{
				throw new ArgumentNullException(
					nameof(plaintext),
					"Requires a non-null plaintext value, or set AllowsNulls=true.");
			}

			return this.Encrypt(
				plaintext, Encryption.DefaultTextEncoding);
		}

		public byte[] Encrypt(
			string plaintext,
			Encoding plaintextEncoding)
		{
			if ((!this.AllowsNulls) && plaintext == null)
			{
				throw new ArgumentNullException(
					nameof(plaintext),
					"Requires a non-null plaintext value, or set AllowsNulls=true.");
			}

			if (plaintextEncoding == null)
			{
				throw new ArgumentNullException(nameof(plaintextEncoding));
			}

			return plaintext == null ? null
				: this.Transform(plaintextEncoding.GetBytes(plaintext));
		}

		public string EncryptToString(
			string plaintext,
			ByteArrayStringEncoding cipherTextEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			if ((!this.AllowsNulls) && plaintext == null)
			{
				throw new ArgumentNullException(
					nameof(plaintext),
					"Requires a non-null plaintext value, or set AllowsNulls=true.");
			}

			return this.EncryptToString(
				plaintext, Encryption.DefaultTextEncoding, cipherTextEncoding);
		}

		public string EncryptToString(
			string plaintext,
			Encoding plaintextEncoding,
			ByteArrayStringEncoding ciphertextEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			if ((!this.AllowsNulls) && plaintext == null)
			{
				throw new ArgumentNullException(
					nameof(plaintext),
					"Requires a non-null plaintext value, or set AllowsNulls=true.");
			}

			if (plaintextEncoding == null)
			{
				throw new ArgumentNullException(nameof(plaintextEncoding));
			}

			return plaintext == null ? null
				: this.EncryptToString(plaintextEncoding.GetBytes(plaintext), ciphertextEncoding);
		}

		public string EncryptToString(
			byte[] plaintextBytes,
			ByteArrayStringEncoding cipherTextEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			if ((!this.AllowsNulls) && plaintextBytes == null)
			{
				throw new ArgumentNullException(
					nameof(plaintextBytes),
					"Requires a non-null plaintextBytes value, or set AllowsNulls=true.");
			}

			return this.Encrypt(plaintextBytes).ToEncodedString(cipherTextEncoding);
		}
	}
}