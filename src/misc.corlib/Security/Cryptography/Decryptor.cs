namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;

	using JetBrains.Annotations;

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
			[NotNull] SymmetricAlgorithm algorithm,
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] initializationVector)
			: base(algorithm, encryptionKey, initializationVector)
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
			[NotNull] T algorithm,
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] initializationVector)
			: base(algorithm, true, encryptionKey, initializationVector)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(initializationVector != null);
		}

		public Decryptor(
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] initializationVector)
			: base(true, encryptionKey, initializationVector)
		{
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(initializationVector != null);
		}

		public byte[] Decrypt(
			[NotNull] byte[] ciphertextBytes)
		{
			Contract.Requires<ArgumentNullException>(ciphertextBytes != null);

			return this.Transform(ciphertextBytes);
		}
	}
}