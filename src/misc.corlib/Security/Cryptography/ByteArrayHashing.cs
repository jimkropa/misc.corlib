#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Security.Cryptography
	ByteArrayHashing.cs

	Copyright (c) 2016 Jim Kropa (https://github.com/jimkropa)

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

		http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/
#endregion

namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;
	using System.Text;

	public static class ByteArrayHashing
	{

		/// <summary>
		/// Use this to generate random salts for ByteArrayHashing.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		internal static byte[] GenerateRandomBytes(int size)
		{
			byte[] bytes = new byte[size];
			using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
			{
				rng.GetBytes(bytes);
			}

			return bytes;
		}

		/// <summary>
		/// When a string is passed to the overload of
		/// <see cref="ComputeHash{T}(byte[],string)" />
		/// to be used as the <see cref="KeyedHashAlgorithm.Key" />
		/// for a <see cref="KeyedHashAlgorithm" />, unless
		/// another <see cref="Encoding" /> is provided,
		/// the string key is presumed to be encoded
		/// as <see cref="Encoding.ASCII" />.
		/// </summary>
		public static readonly Encoding DefaultHashKeyEncoding = Encoding.ASCII;

		public static byte[] ComputeHash<T>(this byte[] bytes)
			where T : HashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(bytes != null);

			byte[] hashedBytes;

			// Use an abstract factory to create an instance
			// of a specific given type of HashAlgorithm.
			using (T hasher = HashAlgorithm.Create((typeof(T)).ToString()) as T)
			{
				// warning CC1024: CodeContracts: Contract section within try block.
				////	Contract.Requires(hasher != null);

				if (hasher == null)
				{
					throw new InvalidOperationException(string.Concat(typeof(T).FullName, " is not a cryptographic hash algorithm!"));
				}

				hashedBytes = hasher.ComputeHash(bytes);
			}

			return hashedBytes;
		}

		public static byte[] ComputeHash<T>(this byte[] bytes, byte[] key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(bytes != null);
			Contract.Requires<ArgumentNullException>(key != null);

			byte[] hashedBytes;

			// Use an abstract factory to create an instance
			// of a specific given type of KeyedHashAlgorithm.
			using (T hasher = KeyedHashAlgorithm.Create((typeof(T)).ToString()) as T)
			{
				// warning CC1024: CodeContracts: Contract section within try block.
				////	Contract.Requires(hasher != null);

				if (hasher == null)
				{
					throw new InvalidOperationException(string.Concat(typeof(T).FullName, " is not a keyed cryptographic hash algorithm!"));
				}

				hasher.Key = key;

				hashedBytes = hasher.ComputeHash(bytes);
			}

			return hashedBytes;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="bytes"></param>
		/// <param name="key">
		/// An ASCII-encoded string to be used as the
		/// <see cref="KeyedHashAlgorithm.Key" />
		/// for the <see cref="KeyedHashAlgorithm" />
		/// specified as <typeparamref name="T" />.
		/// </param>
		/// <returns></returns>
		public static byte[] ComputeHash<T>(this byte[] bytes, string key)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(bytes != null);
			Contract.Requires<ArgumentNullException>(key != null);

			return ComputeHash<T>(bytes, key, DefaultHashKeyEncoding);
		}

		public static byte[] ComputeHash<T>(
			this byte[] bytes,
			string key,
			Encoding keyEncoding)
			where T : KeyedHashAlgorithm
		{
			Contract.Requires<ArgumentNullException>(bytes != null);
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(keyEncoding != null);

			return ComputeHash<T>(bytes, keyEncoding.GetBytes(key));
		}
	}
}