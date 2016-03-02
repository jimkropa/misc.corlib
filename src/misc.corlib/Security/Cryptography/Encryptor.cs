namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;
	using System.Text;

	using JetBrains.Annotations;

	/// <summary>
	/// Encapsulation of an encryption operation
	/// with a simple contract: An encryptor encrypts
	/// plaintext bytes using any implementation of
	/// <see cref="SymmetricAlgorithm"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This type is derived from the generic
	/// <see cref="Encryptor{T}"/>. Refer to
	/// remarks there for more details.
	/// </para>
	/// </remarks>
	public sealed class Encryptor : Encryptor<SymmetricAlgorithm>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Encryptor"/> class
		/// which generates a random initialization vector using the
		/// <see cref="SymmetricAlgorithm.GenerateIV"/> method
		/// of its embedded <see cref="SymmetricAlgorithm"/>.
		/// The new <paramref name="initializationVector"/> must be
		/// preserved, in addition to the <paramref name="encryptionKey"/>,
		/// to decrypt ciphertext created by this encryptor.
		/// </summary>
		/// <param name="algorithm">
		/// An instance of <see cref="SymmetricAlgorithm"/>
		/// to use for an encryption operation.
		/// </param>
		/// <param name="encryptionKey">
		/// A value for the <see cref="SymmetricAlgorithm.Key"/>
		/// of the embedded <see cref="SymmetricAlgorithm"/>.
		/// </param>
		/// <param name="initializationVector">
		/// Returns a new pseudo-random value which has been
		/// generated for the <see cref="SymmetricAlgorithm.IV"/>
		/// of the embedded <see cref="SymmetricAlgorithm"/>.
		/// This value must be preserved, in addition to the
		/// <paramref name="encryptionKey"/>, to decrypt
		/// ciphertext created by this <see cref="Encryptor"/>.
		/// </param>
		/// <param name="allowNulls">
		/// 
		/// </param>
		internal Encryptor(
			[NotNull] SymmetricAlgorithm algorithm,
			[NotNull] byte[] encryptionKey,
			out byte[] initializationVector,
			bool allowNulls = Encryption.DefaultAllowNulls)
			: base(algorithm, encryptionKey, out initializationVector, allowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
		}

		internal Encryptor(
			[NotNull] SymmetricAlgorithm algorithm,
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] initializationVector,
			bool allowNulls = Encryption.DefaultAllowNulls)
			: base(algorithm, encryptionKey, initializationVector, allowNulls)
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

		internal Encryptor(
			[NotNull] T algorithm,
			[NotNull] byte[] encryptionKey,
			out byte[] initializationVector,
			bool allowNulls = Encryption.DefaultAllowNulls)
			: base(algorithm, true, encryptionKey, null, allowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			// Set output parameter.
			initializationVector = this.Algorithm.IV;
		}

		internal Encryptor(
			[NotNull] T algorithm,
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] initializationVector,
			bool allowNulls = Encryption.DefaultAllowNulls)
			: base(algorithm, true, encryptionKey, initializationVector, allowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(initializationVector != null);
		}

		internal Encryptor(
			[NotNull] byte[] encryptionKey,
			out byte[] initializationVector,
			bool allowNulls = Encryption.DefaultAllowNulls)
			: base(true, encryptionKey, null, allowNulls)
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);

			// Set output parameter.
			initializationVector = this.Algorithm.IV;
		}

		internal Encryptor(
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] initializationVector,
			bool allowNulls = Encryption.DefaultAllowNulls)
			: base(true, encryptionKey, initializationVector, allowNulls)
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(initializationVector != null);
		}

		#endregion

		public byte[] Encrypt(byte[] plaintextBytes)
		{
			Contract.Requires<ArgumentNullException>(this.AllowNulls || plaintextBytes != null);

			return this.Transform(plaintextBytes);
		}

		public byte[] Encrypt(string plaintext)
		{
			Contract.Requires<ArgumentNullException>(this.AllowNulls || plaintext != null);

			return this.Encrypt(
				plaintext, Encryption.DefaultTextEncoding);
		}

		public byte[] Encrypt(
			string plaintext,
			[NotNull] Encoding plaintextEncoding)
		{
			Contract.Requires<ArgumentNullException>(this.AllowNulls || plaintext != null);
			Contract.Requires<ArgumentNullException>(plaintextEncoding != null);

			return this.Transform(plaintextEncoding.GetBytes(plaintext));
		}

		public string EncryptToString(
			string plaintext,
			ByteArrayStringEncoding cipherTextEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			Contract.Requires<ArgumentNullException>(this.AllowNulls || plaintext != null);

			return this.EncryptToString(
				plaintext, Encryption.DefaultTextEncoding, cipherTextEncoding);
		}

		public string EncryptToString(
			string plaintext,
			[NotNull] Encoding plaintextEncoding,
			ByteArrayStringEncoding ciphertextEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			Contract.Requires<ArgumentNullException>(this.AllowNulls || plaintext != null);
			Contract.Requires<ArgumentNullException>(plaintextEncoding != null);

			return this.EncryptToString(
				plaintextEncoding.GetBytes(plaintext), ciphertextEncoding);
		}

		public string EncryptToString(
			byte[] plaintextBytes,
			ByteArrayStringEncoding cipherTextEncoding = ConvertByteArray.DefaultStringEncoding)
		{
			Contract.Requires<ArgumentNullException>(this.AllowNulls || plaintextBytes != null);

			return this.Encrypt(plaintextBytes).ToEncodedString(cipherTextEncoding);
		}
	}
}