namespace MiscCorLib
{
	using System;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Xml;
	using Xml;

	/// <summary>
	/// A set of static methods for manipulating strings.
	/// </summary>
	[CLSCompliant(true)]
	public static class FormatString
	{
		#region [ Public Constant and Static Regex for Guid Conversion (commented, obsolete in .NET 4) ]
		/*

			/// <summary>
			/// A string constant used to construct the
			/// <see cref="GuidRegex"/> regular expression.
			/// </summary>
			/// <remarks>
			/// <para>
			/// This string is ultimately used by the (obsolete) TryParseToGuid
			/// method as the basis of a regular expression for validating the format
			/// of a <see cref="Guid"/> prior to attempting to convert from a string.
			/// </para>
			/// <para>
			/// It handles all of the acceptable formats for the <see cref="Guid"/>
			/// constructor, and is documented here:
			/// </para>
			/// <list type="bullet">
			/// <item><description><a href="http://geekswithblogs.net/colinbo/archive/2006/01/18/66307.aspx">http://geekswithblogs.net/colinbo/archive/2006/01/18/66307.aspx</a></description></item>
			/// <item><description><a href="http://www.aspnetsource.com/blog/article.aspx?articleId=73790b68-946c-4734-9b3a-ff45e6025442">http://www.aspnetsource.com/blog/article.aspx?articleId=73790b68-946c-4734-9b3a-ff45e6025442</a></description></item>
			/// <item><description><a href="http://msdn.microsoft.com/en-us/library/96ff78dc.aspx">Guid Constructor (String) at MSDN</a></description></item>
			/// </list>.
			/// </remarks>
			public const string GuidPattern = @"^[A-Fa-f0-9]{32}$|^({|\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\))?$|^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$";

			/// <summary>
			/// A regular expression used by the (obsolete) TryParseToGuid
			/// method for validating the format of a <see cref="Guid"/>
			/// prior to attempting to convert from a string.
			/// </summary>
			/// <remarks>
			/// For more details, refer to the documentation remarks
			/// for the <see cref="GuidPattern"/> constant.
			/// </remarks>
			public static readonly Regex GuidRegex = new Regex(GuidPattern, (RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));

		*/
		#endregion

		#region [ Private String Constants for Character Escape Sequences ]

		/// <summary>
		/// An escape sequence representing a carriage return.
		/// </summary>
		private const string Cr = "\r";

		/// <summary>
		/// An escape sequence representing a
		/// carriage return followed by a line break.
		/// </summary>
		private const string CrLf = "\r\n";

		/// <summary>
		/// An escape sequence representing two carriage returns.
		/// </summary>
		private const string DoubleCr = "\r\r";

		/// <summary>
		/// An escape sequence representing a
		/// carriage return followed by a line break,
		/// followed by another carriage return
		/// and another line break.
		/// </summary>
		private const string DoubleCrlf = "\r\n\r\n";

		/// <summary>
		/// An escape sequence representing two line breaks.
		/// </summary>
		private const string DoubleLf = "\n\n";

		/// <summary>
		/// An escape sequence representing a line break.
		/// </summary>
		private const string Lf = "\n";

		/// <summary>
		/// A string with a single space.
		/// </summary>
		private const string SingleSpace = " ";

		/// <summary>
		/// An string with two spaces.
		/// </summary>
		private const string Spaces2 = "  ";

		/// <summary>
		/// An string with three spaces.
		/// </summary>
		private const string Spaces3 = "   ";

		/// <summary>
		/// An string with four spaces.
		/// </summary>
		private const string Spaces4 = "    ";

		/// <summary>
		/// An string with five spaces.
		/// </summary>
		private const string Spaces5 = "     ";

		/// <summary>
		/// An string with six spaces.
		/// </summary>
		private const string Spaces6 = "      ";

		/// <summary>
		/// An escape sequence representing a tab character.
		/// </summary>
		private const string Tab = "\t";

		#endregion

		#region [ Public Static ToEmptyIfNull and ToSingleLine and IsEqualTo Extension Methods ]

		/// <summary>
		/// This method compares <paramref name="value"/> to null and <see cref="string.Empty"/>.
		/// If <paramref name="value"/> is null or empty, then false is returned.
		/// otherwise the Method <see cref="IsEqualTo"/> is invoked.
		/// </summary>
		/// <param name="value">
		/// A string object or null.
		/// </param>
		/// <param name="valueToCompare">
		/// A string object or null to compare.
		/// </param>
		/// <returns>
		/// True if both string parameters are null or empty.  Otherwise, the outcome
		/// of the <see cref="string.Equals(string,System.StringComparison)"/>
		/// method using he <see cref="StringComparison.Ordinal"/> option
		/// for <see cref="StringComparison"/>.
		/// </returns>
		public static bool HasValueAndIsEqualTo(this string value, string valueToCompare)
		{
			return !string.IsNullOrEmpty(value) && IsEqualTo(value, valueToCompare);
		}

		/// <summary>
		/// This method compares two strings to determine if they are equal,
		/// using the <see cref="StringComparison.Ordinal"/> option for
		/// <see cref="StringComparison"/>. It also varies from the
		/// <see cref="string.Equals(string,string)"/> method
		/// in that comparing null to an empty string returns true.
		/// </summary>
		/// <param name="value">
		/// A string object or null.
		/// </param>
		/// <param name="valueToCompare">
		/// A string object or null to compare.
		/// </param>
		/// <returns>
		/// True if both string parameters are null or empty.  Otherwise, the outcome
		/// of the <see cref="string.Equals(string,System.StringComparison)"/>
		/// method using he <see cref="StringComparison.Ordinal"/> option
		/// for <see cref="StringComparison"/>.
		/// </returns>
		public static bool IsEqualTo(this string value, string valueToCompare)
		{
			if (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(valueToCompare))
			{
				return true;
			}

			if (value == null)
			{
				return false;
			}

			return value.Equals(valueToCompare, StringComparison.Ordinal);
		}

		/// <summary>
		/// Returns an empty string if the string value is null.
		/// </summary>
		/// <param name="value">
		/// A string which may be a null reference.
		/// </param>
		/// <returns>
		/// The <paramref name="value"/> parameter,
		/// or an empty string if the value is null.
		/// </returns>
		public static string ToEmptyIfNull(this string value)
		{
			// If the value is null, we're done.
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}

			// Otherwise, return the original value.
			return value.Trim();
		}

		/// <summary>
		/// Returns null if the string value is empty.
		/// </summary>
		/// <param name="value">
		/// A string which may be a null reference.
		/// </param>
		/// <returns>
		/// The <paramref name="value"/> parameter,
		/// or null if the value is null or empty.
		/// </returns>
		public static string ToNullIfEmpty(this string value)
		{
			// If the value is null, we're done.
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}

			// Try again after trimming the value.
			value = value.Trim();

			// If the value is not empty, we're done.
			if (value.Length > 0)
			{
				return value;
			}

			// Otherwise, return null for an empty value.
			return null;
		}

		/// <summary>
		/// Removes all carriage returns and line breaks from a string of text.
		/// </summary>
		/// <param name="value">
		/// A multiple-line string.
		/// </param>
		/// <returns>
		/// A single-line string from the <paramref name="value"/> parameter.
		/// </returns>
		public static string ToSingleLine(this string value)
		{
			// If the value is null, we're done.
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}

			// Create and initialize a value to return.
			string singleLine = value.Trim();

			// If the value is insignificant, we're done.
			if (!(singleLine.Length > 0))
			{
				return string.Empty;
			}

			// Remove any tabs, line breaks, and extra characters.
			singleLine = string.Join(SingleSpace, singleLine.Split(new[] { Tab }, StringSplitOptions.RemoveEmptyEntries));
			singleLine = string.Join(SingleSpace, singleLine.Split(new[] { Environment.NewLine, CrLf, Cr, Lf }, StringSplitOptions.RemoveEmptyEntries));
			singleLine = string.Join(SingleSpace, singleLine.Split(new[] { Spaces6, Spaces5, Spaces4, Spaces3, Spaces2 }, StringSplitOptions.RemoveEmptyEntries));

			// Finally, return the formatted string.
			return singleLine.Trim();
		}

		#endregion

		#region [ Public Static ToHtmlParagraph and ToXmlDocument Extension Methods ]

		/// <summary>
		/// Changes a string of text into HTML markup,
		/// replacing double carriage returns with &lt;p&gt; tags
		/// and single carriage returns with &lt;br/&gt; tags,
		/// and enclosing the whole string within a &lt;p&gt; tag.
		/// </summary>
		/// <param name="value">
		/// A string to convert to HTML markup.
		/// </param>
		/// <returns>
		/// A string containing the HTML marked-up version
		/// of the <paramref name="value"/> parameter,
		/// enclosed within an outer &lt;p&gt; paragraph element.
		/// </returns>
		public static string ToHtmlParagraph(this string value)
		{
			// Call the main overload of this method,
			// passing in a value for the optional parameter.
			return ToHtmlParagraph(value, true);
		}

		/// <summary>
		/// Changes a string of text into HTML markup,
		/// replacing double carriage returns with &lt;p&gt; tags
		/// and single carriage returns with &lt;br/&gt; tags,
		/// and optionally enclosing the whole string within a &lt;p&gt; tag.
		/// </summary>
		/// <param name="value">
		/// A string to convert to HTML markup.
		/// </param>
		/// <param name="includeOuterTag">
		/// Whether to enclose the HTML markup within an outer &lt;p&gt; paragraph element.
		/// If setting the InnerHtml property of a server-side &lt;p&gt; element,
		/// for instance, send "false" to this parameter.
		/// </param>
		/// <returns>
		/// A string containing the HTML marked-up version
		/// of the <paramref name="value"/> parameter.
		/// </returns>
		public static string ToHtmlParagraph(this string value, bool includeOuterTag)
		{
			// If the value is null, we're done.
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}

			// Create and initialize a value to return.
			string html = value.Trim();

			// If the value is insignificant, we're done.
			if (!(html.Length > 0))
			{
				return string.Empty;
			}

			// Replace line breaks with Html tags.
			html = string.Join(@"</p><p>", html.Split(new[] { DoubleCrlf, DoubleCr, DoubleLf }, StringSplitOptions.RemoveEmptyEntries));
			html = string.Join(@"<br />", html.Split(new[] { Environment.NewLine, CrLf, Cr, Lf }, StringSplitOptions.RemoveEmptyEntries));

			// Create values for inserting breaks between Html tags.
			string paragraphBreak = string.Concat(Environment.NewLine, @"</p>", Environment.NewLine, @"<p>", Environment.NewLine);
			string lineBreak = string.Concat(Environment.NewLine, @"<br />", Environment.NewLine);

			// Insert line breaks between Html tags.
			html = string.Join(paragraphBreak, html.Split(new[] { @"</p><p>" }, StringSplitOptions.RemoveEmptyEntries));
			html = string.Join(lineBreak, html.Split(new[] { @"<br />" }, StringSplitOptions.RemoveEmptyEntries));

			// Check whether an additional <p> tag
			// should be included surrounding the string.
			if (includeOuterTag)
			{
				html = string.Concat(@"<p>", Environment.NewLine, html.Trim(), Environment.NewLine, @"</p>");
			}

			// Finally, return the markup string.
			return html;
		}

		/// <summary>
		/// Parses a string into an
		/// <see cref="XmlDocument"/>.
		/// </summary>
		/// <param name="str">
		/// A string which is valid XML.
		/// </param>
		/// <returns>
		/// A new <see cref="XmlDocument"/> after the invocation
		/// of the <see cref="XmlDocument.LoadXml"/> method with
		/// the <paramref name="str"/> string parameter.
		/// </returns>
		public static XmlDocument ToXmlDocument(this string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return null;
			}

			return DomUtility.LoadFromString(str);
		}

		#endregion

		#region [ Public Static ToGuid and TryParseToGuid Extension Methods (commented, obsolete in .NET 4) ]
		/*

			/// <summary>
			/// Extension method to convert a string to a <see cref="Guid"/>,
			/// returning <see cref="Guid.Empty"/> if conversion fails.
			/// </summary>
			/// <param name="value">
			/// A string representing a valid <see cref="Guid"/>.
			/// </param>
			/// <returns>
			/// A <see cref="Guid"/> if <paramref name="value"/> can be
			/// converted. Otherwise, returns <see cref="Guid.Empty"/>.
			/// </returns>
			/// <remarks>
			/// For more details, refer to the documentation remarks
			/// for the <see cref="GuidPattern"/> constant.
			/// </remarks>
			[Obsolete("Just Use the one in the framework.")]
			public static Guid ToGuid(this string value)
			{
				Guid guid;
				TryParseToGuid(value, out guid);
				return guid;
			}

			/// <summary>
			/// Extension method to convert a string to a <see cref="Guid"/>,
			/// returning an indicator of whether the conversion is successful.
			/// </summary>
			/// <param name="value">
			/// A string representing a valid <see cref="Guid"/>.
			/// </param>
			/// <param name="result">
			/// When this method returns, this parameter contains
			/// a <see cref="Guid"/> to which the converted
			/// <paramref name="value"/> is assigned. If this method
			/// returns false, indicating that the string could not be
			/// converted to <see cref="Guid"/>, then this parameter
			/// will contain <see cref="Guid.Empty"/>.
			/// </param>
			/// <returns>
			/// True if <paramref name="value"/> is converted
			/// to <see cref="Guid"/>, otherwise false.
			/// </returns>
			/// <remarks>
			/// For more details, refer to the documentation remarks
			/// for the <see cref="GuidPattern"/> constant.
			/// </remarks>
			[Obsolete("Just Use the one in the framework.")]
			public static bool TryParseToGuid(this string value, out Guid result)
			{
				return Guid.TryParse(value, out result);
			}

		*/
		#endregion

		/// <summary>
		/// Removes any special characters from the string,
		/// leaving only alphanumeric characters.
		/// </summary>
		/// <param name="value">
		/// A <see cref="string"/> that needs to be cleaned up.
		/// </param>
		/// <returns>
		/// A string which contains only alphanumeric characters.
		/// </returns>
		public static string ToAlphanumeric(this string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return string.Empty;
			}

			// Replace invalid characters with empty strings.
			return Regex.Replace(value, @"[^\w]", string.Empty).Replace("_", string.Empty);
		}

		/// <summary>
		/// Encodes a string to be represented as a string literal. The format
		/// is essentially a JSON string.
		/// The string returned includes outer quotes
		/// Example Output: "Hello \"World\"!".
		/// see http://www.west-wind.com/weblog/posts/114530.aspx.
		/// </summary>
		/// <param name="value">
		/// A string that needs to be encoded for use in Javascript.
		/// </param>
		/// <returns>
		/// Returns a encoded string to be used in javascript.
		/// </returns>
		public static string ToJsEncodedString(this string value)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("\"");
			foreach (char c in value)
			{
				switch (c)
				{
					case '\"':
						sb.Append("\\\"");
						break;
					case '\\':
						sb.Append("\\\\");
						break;
					case '\b':
						sb.Append("\\b");
						break;
					case '\f':
						sb.Append("\\f");
						break;
					case '\n':
						sb.Append("\\n");
						break;
					case '\r':
						sb.Append("\\r");
						break;
					case '\t':
						sb.Append("\\t");
						break;
					default:
						int i = c;
						if (i < 32 || i > 127)
						{
							sb.AppendFormat("\\u{0:X04}", i);
						}
						else
						{
							sb.Append(c);
						}

						break;
				}
			}

			sb.Append("\"");

			return sb.ToString();
		}
	}
}