namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;

	using JetBrains.Annotations;

	// TODO: Test whether this functionality is even needed...
	// If decryption fails, is there an exception or does it just produce nonsense?
	public sealed class EncryptorWithChecksum : EncryptorWithChecksum<SymmetricAlgorithm, KeyedHashAlgorithm>
	{
		internal EncryptorWithChecksum(
			[NotNull] SymmetricAlgorithm symmetricAlgorithm,
			[NotNull] KeyedHashAlgorithm checksumHasher,
			[NotNull] byte[] encryptionKey,
			out byte[] initializationVector,
			bool allowNulls)
			: base(symmetricAlgorithm, checksumHasher, encryptionKey, out initializationVector, allowNulls)
		{
			Contract.Requires<ArgumentNullException>(symmetricAlgorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
		}

		internal EncryptorWithChecksum(
			[NotNull] SymmetricAlgorithm symmetricAlgorithm,
			[NotNull] KeyedHashAlgorithm checksumHasher,
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] initializationVector,
			bool allowNulls)
			: base(symmetricAlgorithm, checksumHasher, encryptionKey, initializationVector, allowNulls)
		{
			Contract.Requires<ArgumentNullException>(symmetricAlgorithm != null);
			Contract.Requires<ArgumentNullException>(encryptionKey != null);
			Contract.Requires<ArgumentNullException>(initializationVector != null);
		}
	}

	/// <summary>
	/// Decorator of a <see cref="Encryptor{T}"/>?
	/// </summary>
	/// <typeparam name="TEncryptor"></typeparam>
	/// <typeparam name="THasher"></typeparam>
	public class EncryptorWithChecksum<TEncryptor, THasher> : Encryptor<TEncryptor>
		where TEncryptor: SymmetricAlgorithm
		where THasher: KeyedHashAlgorithm
	{
		private Hasher<THasher> hasher;

		internal EncryptorWithChecksum(
			[NotNull] TEncryptor symmetricAlgorithm,
			[NotNull] THasher checksumHasher,
			[NotNull] byte[] encryptionKey,
			out byte[] initializationVector,
			bool allowNulls)
			: base(symmetricAlgorithm, encryptionKey, out initializationVector, allowNulls)
		{
			this.hasher = new Hasher<THasher>(checksumHasher, initializationVector, allowNulls);
		}

		internal EncryptorWithChecksum(
			[NotNull] TEncryptor algorithm,
			[NotNull] THasher checksumHasher,
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] initializationVector,
			bool allowNulls)
			: base(algorithm, encryptionKey, initializationVector, allowNulls)
		{
			this.hasher = new Hasher<THasher>(checksumHasher, initializationVector, allowNulls);
		}

		internal EncryptorWithChecksum(
			[NotNull] byte[] encryptionKey,
			out byte[] initializationVector,
			bool allowNulls)
			: base(encryptionKey, out initializationVector, allowNulls)
		{
			this.hasher = new Hasher<THasher>(initializationVector, allowNulls);
		}

		internal EncryptorWithChecksum(
			[NotNull] byte[] encryptionKey,
			[NotNull] byte[] initializationVector,
			bool allowNulls)
			: base(encryptionKey, initializationVector, allowNulls)
		{
			this.hasher = new Hasher<THasher>(initializationVector, allowNulls);
		}
	}
}
