#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Security.Cryptography
	TestObjectFactory.cs

	Copyright (c) 2015 Jim Kropa (http://www.kropa.net/)

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
	using System.Security.Cryptography;

	public static class TestObjectFactory
	{
		// Using the old Random class here for speed.
		// For generating numbers in production code,
		// use RNGCryptoServiceProvider.
		public static readonly Random RandomInstance = new Random();

		public static byte[] CreateEncryptionKey(int size = 32)
		{
			byte[] byteArray = new byte[size];

			RandomInstance.NextBytes(byteArray);

			return byteArray;
		}

		public static SymmetricAlgorithm CreateAlgorithm()
		{
			// Use AesManaged by default because it's easier on memory.
			// In production code use more compliant "crypto" flavor of AES.
			return new AesManaged();
		}

		public static Encryptor CreateEncryptor(out byte[] initializationVector)
		{
			return CreateEncryptor(CreateEncryptionKey(), out initializationVector);
		}

		public static Encryptor CreateEncryptor(byte[] encryptionKey, out byte[] initializationVector)
		{
			using (SymmetricAlgorithm algorithm = CreateAlgorithm())
			{
				return new Encryptor(algorithm, encryptionKey, out initializationVector, Encryption.DefaultOptions);
			}
		}
	}
}