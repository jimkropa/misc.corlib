namespace MiscCorLib.Collections
{
	using System;
	using System.Text;

	/// <summary>
	/// A set of static methods for converting
	/// byte arrays into other types.
	/// </summary>
	[CLSCompliant(true)]
	public static class ConvertByteArray
	{
		/// <summary>
		/// Converts a byte array to a string.
		/// </summary>
		/// <param name="byteArray">
		/// A byte array.
		/// </param>
		/// <returns>
		/// Returns an empty string if the length of the byte array is 0.
		/// Otherwise converts the byte array to string.
		/// </returns>
		public static string ToASCIIEncodedString(byte[] byteArray)
		{
			if (byteArray == null)
			{
				throw new ArgumentNullException("byteArray", "The byte array cannot be null.");
			}

			if (byteArray.Length == 0)
			{
				return string.Empty;
			}

			ASCIIEncoding encoding = new ASCIIEncoding();

			return encoding.GetString(byteArray);
		}
	}
}