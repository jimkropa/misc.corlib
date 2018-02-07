using System;
using System.Text.RegularExpressions;

namespace MiscCorLib
{
	/// <summary>
	/// A set of static methods for manipulating strings.
	/// </summary>
	public static class FormatString
	{
		#region [ Private String Constants for Character Escape Sequences ]

		/// <summary>
		/// An escape sequence representing a carriage return.
		/// </summary>
		public const string Cr = "\r";

		/// <summary>
		/// An escape sequence representing a line break.
		/// </summary>
		public const string Lf = "\n";

		/// <summary>
		/// An escape sequence representing a
		/// carriage return followed by a line break.
		/// </summary>
		public const string CrLf = "\r\n";

		/// <summary>
		/// An escape sequence representing two carriage returns.
		/// </summary>
		public const string DoubleCr = "\r\r";

		/// <summary>
		/// An escape sequence representing two line breaks.
		/// </summary>
		public const string DoubleLf = "\n\n";

		/// <summary>
		/// An escape sequence representing a
		/// carriage return followed by a line break,
		/// followed by another carriage return
		/// and another line break.
		/// </summary>
		public const string DoubleCrLf = "\r\n\r\n";

		/// <summary>
		/// A concatenation of two <see cref="Environment.NewLine" /> strings.
		/// </summary>
		public static readonly string DoubleNewLine = Environment.NewLine + Environment.NewLine;

		/// <summary>
		/// An string with two spaces.
		/// </summary>
		public const string DoubleSpace = "  ";

		/// <summary>
		/// A string with a single space.
		/// </summary>
		public const string SingleSpace = " ";

		/// <summary>
		/// An escape sequence representing a tab character.
		/// </summary>
		public const string Tab = "\t";

		/// <summary>
		/// Static allocation of an array for splitting a string using the
		/// <see cref="string.Split(string[],StringSplitOptions)" /> method,
		/// to avoid the expensive allocation each time it is used by the
		/// <see cref="ToCompactWhiteSpace" /> method.
		/// </summary>
		public static readonly string[] TabSplitter = { Tab };

		/// <summary>
		/// Static allocation of an array for splitting a string using the
		/// <see cref="string.Split(string[],StringSplitOptions)" /> method,
		/// to avoid the expensive allocation each time it is used by the
		/// <see cref="ToCompactWhiteSpace" /> method.
		/// </summary>
		public static readonly string[] NewLineSplitter = { Environment.NewLine, CrLf, Cr, Lf };

		/// <summary>
		/// Static allocation of an array for splitting a string using the
		/// <see cref="string.Split(string[],StringSplitOptions)" /> method,
		/// to avoid the expensive allocation each time it is used by the
		/// <see cref="ToCompactWhiteSpace" /> method.
		/// </summary>
		public static readonly string[] DoubleSpaceSplitter = { DoubleSpace };

		private static readonly string[] ExcessNewLineSplitter = { DoubleNewLine + Environment.NewLine, DoubleCrLf + CrLf, DoubleCr + Cr, DoubleLf + Lf, DoubleNewLine, DoubleCrLf, DoubleCr, DoubleLf };
		private static readonly string[] ExcessSpacesSplitter = { "     ", "    ", "   ", DoubleSpace };

		private const string HtmlParagraphStart = @"<p>";
		private const string HtmlParagraphEnd = @"</p>";
		private const string HtmlParagraphDelimiter = @"</p><p>";
		private const string HtmlLineDelimiter = @"<br />";

		private static readonly string[] HtmlParagraphSplitter = { @" </p><p> ", @" </p><p>", @"</p><p> ", HtmlParagraphDelimiter };
		private static readonly string[] HtmlLineSplitter = { @" <br /> ", @" <br />", @"<br /> ", HtmlLineDelimiter };

		private static readonly string HtmlParagraphBreak = string.Concat(
			Environment.NewLine,
			HtmlParagraphEnd,
			Environment.NewLine,
			HtmlParagraphStart,
			Environment.NewLine);

		private static readonly string HtmlLineBreak = string.Concat(
			Environment.NewLine,
			HtmlLineDelimiter,
			Environment.NewLine);

		#endregion

		#region [ Public Static ToEmptyIfNull and ToNullIfEmpty Extension Methods ]

		/// <summary>
		/// Returns an empty string if the string value is null.
		/// </summary>
		/// <param name="value">
		/// A string which may be a null reference.
		/// </param>
		/// <returns>
		/// The <paramref name="value" /> parameter,
		/// or an empty string if the value is null.
		/// </returns>
		public static string ToEmptyIfNull(this string value)
		{
			// If the value is null, we're done.
			// Otherwise, return the original value.
			return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
		}

		/// <summary>
		/// Returns null if the string value is empty.
		/// </summary>
		/// <param name="value">
		/// A string which may be a null reference.
		/// </param>
		/// <returns>
		/// The <paramref name="value" /> parameter,
		/// or null if the value is null or empty.
		/// </returns>
		public static string ToNullIfEmpty(this string value)
		{
			// If the value is null, we're done.
			// Otherwise, return the original value.
			return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
		}

		#endregion

		#region [ Public Static ToCompactWhiteSpace Extension Method ]

		/// <summary>
		/// Removes all tabs, carriage returns, line breaks from a string of text.
		/// </summary>
		/// <param name="value">
		/// A multiple-line string.
		/// </param>
		/// <param name="returnNullIfEmptyOrNull">
		/// 
		/// </param>
		/// <returns>
		/// A string based on the <paramref name="value" /> parameter
		/// with all of the white space compacted to single spaces.
		/// </returns>
		public static string ToCompactWhiteSpace(
			this string value, bool returnNullIfEmptyOrNull = false)
		{
			// If the value is null, we're done.
			if (string.IsNullOrWhiteSpace(value))
			{
				return returnNullIfEmptyOrNull ? null : string.Empty;
			}

			// Create and initialize a value to return.
			string singleLine = value.Trim();

			// Remove any tabs, line breaks, and extra characters.
			singleLine = string.Join(SingleSpace, singleLine.Split(TabSplitter, StringSplitOptions.RemoveEmptyEntries));
			singleLine = string.Join(SingleSpace, singleLine.Split(NewLineSplitter, StringSplitOptions.RemoveEmptyEntries));
			singleLine = string.Join(SingleSpace, singleLine.Split(ExcessSpacesSplitter, StringSplitOptions.RemoveEmptyEntries));

			// Compact any remaining internal spaces.
			while (singleLine.IndexOf(DoubleSpace, StringComparison.Ordinal) >= 0)
			{
				singleLine = string.Join(SingleSpace, singleLine.Split(DoubleSpaceSplitter, StringSplitOptions.RemoveEmptyEntries));
			}

			// Finally, return the formatted string.
			return returnNullIfEmptyOrNull ? singleLine.Trim().ToNullIfEmpty() : singleLine.Trim();
		}

		#endregion

		#region [ Public Static ToHtmlParagraph Extension Method ]

		/// <summary>
		/// Changes a string of text into HTML markup,
		/// replacing double carriage returns with &lt;p&gt; tags
		/// and single carriage returns with &lt;br/&gt; tags,
		/// and optionally enclosing the whole string within a &lt;p&gt; tag.
		/// </summary>
		/// <param name="value">
		/// A string to convert to HTML markup.
		/// </param>
		/// <param name="includeContainerElement">
		/// Whether to enclose the HTML markup within an outer &lt;p&gt; paragraph element.
		/// If setting the InnerHtml property of a server-side &lt;p&gt; element,
		/// for instance, send "false" to this parameter.
		/// </param>
		/// <param name="returnNullIfEmptyOrNull">
		/// 
		/// </param>
		/// <returns>
		/// A string containing the HTML marked-up version
		/// of the <paramref name="value" /> parameter.
		/// </returns>
		public static string ToHtmlParagraph(
			this string value, bool includeContainerElement = true, bool returnNullIfEmptyOrNull = false)
		{
			// If the value is null, we're done.
			if (string.IsNullOrWhiteSpace(value))
			{
				return returnNullIfEmptyOrNull ? null : string.Empty;
			}

			// Create and initialize a value to return.
			string html = value.Trim();

			// Replace line breaks with Html tags.
			html = string.Join(HtmlParagraphDelimiter, html.Split(ExcessNewLineSplitter, StringSplitOptions.RemoveEmptyEntries));
			html = string.Join(HtmlLineBreak, html.Split(NewLineSplitter, StringSplitOptions.RemoveEmptyEntries));
			html = html.ToCompactWhiteSpace();

			// Insert line breaks between Html tags.
			html = string.Join(HtmlParagraphBreak, html.Split(HtmlParagraphSplitter, StringSplitOptions.RemoveEmptyEntries));
			html = string.Join(HtmlLineBreak, html.Split(HtmlLineSplitter, StringSplitOptions.RemoveEmptyEntries));

			// Check whether an additional <p> tag
			// should be included surrounding the string.
			if (includeContainerElement && html.Length > 0)
			{
				html = string.Concat(HtmlParagraphStart, Environment.NewLine, html, Environment.NewLine, HtmlParagraphEnd);
			}

			// Finally, return the markup string.
			return returnNullIfEmptyOrNull ? html.Trim().ToNullIfEmpty() : html.Trim();
		}

		#endregion

		/// <summary>
		/// Removes any special characters from the string,
		/// leaving only alphanumeric characters.
		/// </summary>
		/// <param name="value">
		/// A <see cref="string" /> that needs to be cleaned up.
		/// </param>
		/// <param name="returnNullIfEmptyOrNull">
		/// 
		/// </param>
		/// <returns>
		/// A string which contains only alphanumeric characters.
		/// </returns>
		public static string ToAlphanumeric(this string value, bool returnNullIfEmptyOrNull = false)
		{
			// Replace invalid characters with empty strings.
			return string.IsNullOrWhiteSpace(value) ? returnNullIfEmptyOrNull ? null : string.Empty
				: Regex.Replace(value, @"[^\w]", string.Empty).Replace("_", string.Empty);
		}
	}
}