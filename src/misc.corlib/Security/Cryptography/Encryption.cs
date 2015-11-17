namespace MiscCorLib.Security.Cryptography
{
	using System.Security.Cryptography;
	using System.Text;

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
		private const int DefaultSaltSize = 8;
		public const CipherEncoding DefaultCipherEncoding = CipherEncoding.Base64;
		public static readonly Encoding DefaultTextEncoding = Encoding.UTF8;
		public static readonly Encoding DefaultKeyEncoding = Encoding.ASCII;

		private static byte[] DeriveEncryptionKeyFromPassword(string password, int keySize, int saltSize, out byte[] salt)
		{
			byte[] encryptionKey;
			using (Rfc2898DeriveBytes keyBytes = new Rfc2898DeriveBytes(password, saltSize))
			{
				encryptionKey = keyBytes.GetBytes(keySize);
				salt = keyBytes.Salt;
			}

			return encryptionKey;
		}

		private static byte[] DeriveDecryptionKeyFromPassword(string password, int keySize, byte[] salt)
		{
			byte[] decryptionKey;
			using (Rfc2898DeriveBytes keyBytes = new Rfc2898DeriveBytes(password, salt))
			{
				decryptionKey = keyBytes.GetBytes(keySize);
			}

			return decryptionKey;
		}
	}
}