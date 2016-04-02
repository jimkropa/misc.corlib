#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Security.Cryptography
	EncryptorTests.cs

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
	using System.Security.Cryptography;

	using NUnit.Framework;

	[TestFixture]
	public sealed class EncryptorTests
	{
		[TestFixture]
		public sealed class Constructor
		{
			
		}

		[TestFixture]
		public sealed class Encrypt
		{
			[Test]
			public void Returns_InitializationVector()
			{
				using (SymmetricAlgorithm algorithm = TestObjectFactory.CreateAlgorithm())
				{
					Console.Write("algorithm.Key: ");
					Console.WriteLine(algorithm.Key.ToBase64String(true));
					Console.Write("algorithm.Key.Length: ");
					Console.WriteLine(algorithm.Key.Length);
					Console.Write("algorithm.IV: ");
					Console.WriteLine(algorithm.IV.ToBase64String(true));
					Console.Write("algorithm.IV.Length: ");
					Console.WriteLine(algorithm.IV.Length);

					byte[] encryptionKey = TestObjectFactory.CreateEncryptionKey();
					byte[] initializationVector;
					using (Encryptor encryptor = new Encryptor(
						algorithm, encryptionKey, out initializationVector, EncryptionOptions.AllowNullInput))
					{
						Console.WriteLine();
						Console.Write("encryptionKey: ");
						Console.WriteLine(encryptionKey.ToBase64String(true));

						Console.WriteLine();
						Console.Write("algorithm.Key: ");
						Console.WriteLine(algorithm.Key.ToBase64String(true));
						////Console.Write("encryptor.EncryptionKey: ");
						////Console.WriteLine(encryptor.EncryptionKey.ToBase64String(true));

						Console.WriteLine();
						Console.Write("algorithm.IV: ");
						Console.WriteLine(algorithm.IV.ToBase64String(true));
						Console.Write("encryptor.InitializationVector: ");
						Console.WriteLine(initializationVector.ToBase64String(true));
					}
				}

				////	using (Encryptor<SymmetricAlgorithm> encryptor = TestObjectFactory.CreateEncryptor())
				////	{
				////		
				////	}
			}
		}
	}
}