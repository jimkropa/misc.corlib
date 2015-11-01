namespace MiscCorLib.Security.Cryptography
{
	using System.Text;

	public static class StringEncryption
	{
		/// <summary>
		/// Unless a different <see cref="Encoding"/> is specified,
		/// when a string is encrypted or decrypted using one of the
		/// extension methods in this class, the string is assumed
		/// to be encoded as <see cref="Encoding.UTF8"/>.
		/// </summary>
		public static readonly Encoding DefaultEncoding = Encoding.UTF8;
	}
}