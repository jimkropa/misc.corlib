namespace MiscCorLib.Security.Cryptography
{
	using System;
	using System.Collections;
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
		public const CipherEncoding DefaultCipherEncoding = CipherEncoding.Base64;
		public static readonly Encoding DefaultTextEncoding = Encoding.UTF8;
		public static readonly Encoding DefaultKeyEncoding = Encoding.ASCII;




	}
}