namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;
	using System.Text;

	using JetBrains.Annotations;

	/// <summary>
	/// Factory producting <see cref="Encryptor"/>
	/// and <see cref="Decryptor"/> instances.
	/// </summary>
	/// <remarks>
	/// <para>
	/// An abstract factory could be extracted, an IEncryption
	/// factory producing IEncryptor and IDecryptor instances.
	/// </para>
	/// </remarks>
	public static partial class Encryption
	{
		public const EncryptionOptions DefaultOptions = EncryptionOptions.AllowNullInput;
			////	| EncryptionOptions.SuppressCryptographicExceptions
			////	| EncryptionOptions.SuppressDecryptionChecksumExceptions;

		public static readonly Encoding DefaultTextEncoding = Encoding.UTF8;

		public static byte[] DeriveEncryptionKeyAndSaltFromPassword(
			[NotNull] string password, int keySize, int saltSize, out byte[] randomSalt)
		{
			Contract.Requires<ArgumentNullException>(password != null);

			byte[] encryptionKey;
			using (Rfc2898DeriveBytes keyBytes = new Rfc2898DeriveBytes(password, saltSize))
			{
				// Get an encryption key of pseudo-random
				// bytes derived from the given password.
				encryptionKey = keyBytes.GetBytes(keySize);

				// The salt is a random value,
				// uniquely identifying this instance
				// of the encryption operations,
				// and needed for decryption.
				randomSalt = keyBytes.Salt;
			}

			return encryptionKey;
		}

		public static byte[] DeriveEncryptionKeyFromPasswordAndSalt(
			[NotNull] string password, int keySize, [NotNull] byte[] salt)
		{
			Contract.Requires<ArgumentNullException>(password != null);

			byte[] decryptionKey;
			using (DeriveBytes keyBytes = new Rfc2898DeriveBytes(password, salt))
			{
				decryptionKey = keyBytes.GetBytes(keySize);
			}

			return decryptionKey;
		}
	}
}