#region [ license and copyright boilerplate ]
/*
	MiscCorLib
	FormatString.cs

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

namespace MiscCorLib
{
	using System;
	using System.Text.RegularExpressions;

	/// <summary>
	/// A set of static methods for manipulating strings.
	/// </summary>
	[CLSCompliant(true)]
	public static class FormatString
	{
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

		#region [ Public Static ToEmptyIfNull and ToSingleLine Extension Methods ]

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
		/// The <paramref name="value"/> parameter,
		/// or null if the value is null or empty.
		/// </returns>
		public static string ToNullIfEmpty(this string value)
		{
			// If the value is null, we're done.
			// Otherwise, return the original value.
			return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
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
			if (string.IsNullOrWhiteSpace(value))
			{
				return string.Empty;
			}

			// Create and initialize a value to return.
			string singleLine = value.Trim();

			// Remove any tabs, line breaks, and extra characters.
			singleLine = string.Join(SingleSpace, singleLine.Split(new[] { Tab }, StringSplitOptions.RemoveEmptyEntries));
			singleLine = string.Join(SingleSpace, singleLine.Split(new[] { Environment.NewLine, CrLf, Cr, Lf }, StringSplitOptions.RemoveEmptyEntries));
			singleLine = string.Join(SingleSpace, singleLine.Split(new[] { Spaces6, Spaces5, Spaces4, Spaces3, Spaces2 }, StringSplitOptions.RemoveEmptyEntries));

			// Compact any remaining internal spaces.
			while (singleLine.IndexOf(Spaces2, StringComparison.Ordinal) >= 0)
			{
				singleLine = string.Join(SingleSpace, singleLine.Split(new[] { Spaces2 }, StringSplitOptions.RemoveEmptyEntries));
			}

			// Finally, return the formatted string.
			return singleLine.Trim();
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
		/// <param name="includeOuterTag">
		/// Whether to enclose the HTML markup within an outer &lt;p&gt; paragraph element.
		/// If setting the InnerHtml property of a server-side &lt;p&gt; element,
		/// for instance, send "false" to this parameter.
		/// </param>
		/// <returns>
		/// A string containing the HTML marked-up version
		/// of the <paramref name="value"/> parameter.
		/// </returns>
		public static string ToHtmlParagraph(this string value, bool includeOuterTag = true)
		{
			// If the value is null, we're done.
			if (string.IsNullOrWhiteSpace(value))
			{
				return string.Empty;
			}

			// Create and initialize a value to return.
			string html = value.Trim();

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
			// Replace invalid characters with empty strings.
			return string.IsNullOrWhiteSpace(value) ? string.Empty
				: Regex.Replace(value, @"[^\w]", string.Empty).Replace("_", string.Empty);
		}
	}
}