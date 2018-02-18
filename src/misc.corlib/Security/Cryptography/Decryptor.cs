using System;
using System.Security.Cryptography;

namespace MiscCorLib.Security.Cryptography
{
	/// <summary>
	/// Encapsulation of a decryption operation
	/// with a simple contract: A decryptor decrypts
	/// ciphertext bytes using any implementation of
	/// <see cref="SymmetricAlgorithm" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This type is derived from the generic
	/// <see cref="Decryptor{T}" />. Refer to
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
		}

		public Decryptor(
			byte[] encryptionKey,
			byte[] initializationVector,
			EncryptionOptions options)
			: base(false, encryptionKey, initializationVector, options)
		{
		}

		public byte[] Decrypt(
			byte[] ciphertextBytes)
		{
			return this.Transform(ciphertextBytes);
		}
	}
}