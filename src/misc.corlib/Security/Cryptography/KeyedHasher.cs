﻿namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;

	using JetBrains.Annotations;

	public sealed class KeyedHasher : KeyedHasher<KeyedHashAlgorithm>
	{
		internal KeyedHasher(
			[NotNull] KeyedHashAlgorithm algorithm,
			[NotNull] byte[] key,
			[NotNull] byte[] salt,
			bool allowNulls)
			: base(algorithm, key, salt, allowNulls)
		{
		}
	}

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
	public class KeyedHasher<T> : Hasher<T>
		where T : KeyedHashAlgorithm
	{
		private readonly byte[] key;

		internal KeyedHasher(
			[NotNull] T algorithm,
			[NotNull] byte[] key,
			[NotNull] byte[] salt,
			bool allowNulls)
			: base(algorithm, salt, allowNulls)
		{
			Contract.Requires<ArgumentNullException>(algorithm != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			this.key = key;
			this.Algorithm.Key = key;
		}

		internal KeyedHasher(
			[NotNull] byte[] key,
			[NotNull] byte[] salt,
			bool allowNulls)
			: base(salt, allowNulls)
		{
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(salt != null);

			this.key = key;
		}

		#region [ Private Method to Create a KeyedHashAlgorithm Instance based on its Generic Type Name ]

		protected override T CreateAlgorithm()
		{
			T algorithm = KeyedHashAlgorithm.Create(typeof(T).ToString()) as T;
			if (algorithm == null)
			{
				throw new InvalidOperationException(
					string.Concat(typeof(T).FullName, " is not a keyed hashing algorithm!"));
			}

			algorithm.Key = this.key;

			return algorithm;
		}

		#endregion
	}
}