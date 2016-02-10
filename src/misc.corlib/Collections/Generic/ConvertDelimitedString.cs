namespace MiscCorLib.Collections.Generic
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;

	/// <summary>
	/// A set of static methods for converting delimited strings
	/// into typed collections of <see cref="IComparable"/>,
	/// with options for sorting and removing duplicate entries.
	/// </summary>
	[CLSCompliant(true)]
	public static class ConvertDelimitedString
	{
		#region [ Constants Default String Delimiter (a comma) ]

		/// <summary>
		/// A constant value to use when converting to
		/// and from a delimited string, always a comma,
		/// used by <see cref="string.Join(string,string[])"/>.
		/// </summary>
		public const string DefaultSeparator = ",";

		/// <summary>
		/// A constant value to use when converting to
		/// and from a delimited string, always a comma,
		/// used by <see cref="string.Split(char[])"/>.
		/// </summary>
		public static readonly char[] DefaultStringSplitter = {','};

		#endregion

		#region [ Overloads of Public Static ToArray<T> Method ]

		/// <summary>
		/// Returns a typed array of <see cref="ValueType"/>
		/// values parsed from a collection of strings
		/// using a default <see cref="TypeConverter"/>,
		/// optionally having duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, must
		/// be a <see cref="ValueType"/>.
		/// </typeparam>
		/// <param name="delimitedString"></param>
		/// <param name="removeDuplicates">
		///     Whether to remove duplicate items in the array returned.
		///     Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		///     Whether to throw any <see cref="FormatException"/>
		///     or <see cref="NotSupportedException"/> encountered
		///     during <see cref="TypeConverter.ConvertFrom(object)"/>
		///     using the default <see cref="TypeConverter"/>.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(
			this string delimitedString,
			bool removeDuplicates = ConvertValueTypeCollection.DefaultRemoveDuplicates,
			bool throwTypeConversionExceptions = ConvertStrings.DefaultThrowTypeConversionExceptions)
			where T : struct
		{
			return delimitedString.ToList<T>(
				removeDuplicates, throwTypeConversionExceptions).ToArray();
		}

		/// <summary>
		/// Returns a typed array of <see cref="ValueType"/>
		/// values parsed from a collection of strings
		/// using a given <see cref="TypeConverter"/>,
		/// optionally having duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, must
		/// be a <see cref="ValueType"/>.
		/// </typeparam>
		/// <param name="delimitedString"></param>
		/// <param name="converter">
		///     A <see cref="TypeConverter"/> to convert
		///     strings to values of <typeparamref name="T"/>.
		///     If <c>null</c>, the default converter will be used.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		///     Whether to throw any <see cref="FormatException"/>
		///     or <see cref="NotSupportedException"/> encountered
		///     during <see cref="TypeConverter.ConvertFrom(object)"/>
		///     using the given <paramref name="converter"/>.
		/// </param>
		/// <param name="removeDuplicates">
		///     Whether to remove duplicate items in the array returned.
		///     Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(
			this string delimitedString,
			TypeConverter converter,
			bool throwTypeConversionExceptions,
			bool removeDuplicates = ConvertValueTypeCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return delimitedString.ToList<T>(
				converter, throwTypeConversionExceptions, removeDuplicates).ToArray();
		}

		/// <summary>
		/// Returns a typed array of <see cref="ValueType"/> values
		/// parsed from a collection of strings using a specified
		/// <see cref="ConvertStrings.TryParseFromString{T}"/> delegate,
		/// ignoring any values which cannot be parsed
		/// and optionally having duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, must
		/// be a <see cref="ValueType"/>.
		/// </typeparam>
		/// <param name="delimitedString"></param>
		/// <param name="tryParseDelegate">
		///     A delegate method conforming to the
		///     <see cref="ConvertStrings.TryParseFromString{T}"/>
		///     signature, for optimized type coversion
		///     via a "TryParse" method common to many
		///     primitive <see cref="ValueType"/> classes.
		/// </param>
		/// <param name="removeDuplicates">
		///     Whether to remove duplicate items in the array returned.
		///     Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(
			this string delimitedString,
			ConvertStrings.TryParseFromString<T> tryParseDelegate,
			bool removeDuplicates = ConvertValueTypeCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			throw new NotImplementedException();
			//return delimitedString.ToList(
			//	tryParseDelegate, removeDuplicates).ToArray();
		}

		#endregion

		#region [ Overloads of Public Static ToEnumerable<T> Method ]


		#endregion

		#region [ Public Static ToList<T> Method Overloads, invoked by other Static Methods ]

		/// <summary>
		/// Returns a generic <see cref="List{T}"/>
		/// of <see cref="ValueType"/> values parsed
		/// from a collection of strings using a default
		/// <see cref="TypeConverter"/>, optionally
		/// having duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, must
		/// be a <see cref="ValueType"/>.
		/// </typeparam>
		/// <param name="delimitedString"></param>
		/// <param name="removeDuplicates">
		///     Whether to remove duplicate items in the array returned.
		///     Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		///     Whether to throw any <see cref="FormatException"/>
		///     or <see cref="NotSupportedException"/> encountered
		///     during <see cref="TypeConverter.ConvertFrom(object)"/>
		///     using the default <see cref="TypeConverter"/>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static List<T> ToList<T>(
			this string delimitedString,
			bool removeDuplicates = ConvertValueTypeCollection.DefaultRemoveDuplicates,
			bool throwTypeConversionExceptions = ConvertStrings.DefaultThrowTypeConversionExceptions)
			where T : struct
		{
			return delimitedString.ToList<T>(null, throwTypeConversionExceptions, removeDuplicates);
		}

		/// <summary>
		/// Returns a generic <see cref="List{T}"/>
		/// of <see cref="ValueType"/> values parsed
		/// from a collection of strings using a given
		/// <see cref="TypeConverter"/>, optionally
		/// having duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, must
		/// be a <see cref="ValueType"/>.
		/// </typeparam>
		/// <param name="delimitedString"></param>
		/// <param name="converter">
		///     A <see cref="TypeConverter"/> to convert
		///     strings to values of <typeparamref name="T"/>.
		///     If <c>null</c>, the default converter will be used.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		///     Whether to throw any <see cref="FormatException"/>
		///     or <see cref="NotSupportedException"/> encountered
		///     during <see cref="TypeConverter.ConvertFrom(object)"/>
		///     using the given <paramref name="converter"/>.
		/// </param>
		/// <param name="removeDuplicates">
		///     Whether to remove duplicate items in the array returned.
		///     Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static List<T> ToList<T>(
			this string delimitedString,
			TypeConverter converter,
			bool throwTypeConversionExceptions,
			bool removeDuplicates = ConvertValueTypeCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return delimitedString.SplitDelimitedString(DefaultSeparator).ToList<T>(
				converter, throwTypeConversionExceptions, removeDuplicates);
		}

		/// <summary>
		/// Returns a generic <see cref="List{T}"/>
		/// of <see cref="ValueType"/> values parsed
		/// from a collection of strings using a specified
		/// <see cref="ConvertStrings.TryParseFromString{T}"/> delegate,
		/// ignoring any values which cannot be parsed
		/// and optionally having duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, must
		/// be a <see cref="ValueType"/>.
		/// </typeparam>
		/// <param name="delimitedString"></param>
		/// <param name="tryParseDelegate">
		///     A delegate method conforming to the
		///     <see cref="ConvertStrings.TryParseFromString{T}"/>
		///     signature, for optimized type coversion
		///     via a "TryParse" method common to many
		///     primitive <see cref="ValueType"/> classes.
		/// </param>
		/// <param name="separator"></param>
		/// <param name="removeDuplicates">
		///     Whether to remove duplicate items in the array returned.
		///     Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static List<T> ToList<T>(
			this string delimitedString,
			ConvertStrings.TryParseFromString<T> tryParseDelegate,
			string separator,
			bool removeDuplicates = ConvertValueTypeCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return delimitedString.SplitDelimitedString(separator).ToList(
				tryParseDelegate, removeDuplicates);
		}

		#endregion

		#region [ Overloads of Public Static ToArray<T> Method ]
		/*
		/// <summary>
		/// Returns a typed array from a comma-delimited string,
		/// with the resulting array sorted and duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="value">
		/// A comma-delimited string containing values for the array.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>,
		/// sorted and without duplicate entries.
		/// </returns>
		public static T[] ToArray<T>(
			string value)
			where T : struct
		{
			return ToArray<T>(value, DefaultSeparator);
		}

		/// <summary>
		/// Returns a typed array from a delimited string,
		/// with the resulting array sorted and duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="value">
		/// A string, delimited by the <paramref name="separator"/>
		/// string, containing values for the array.
		/// </param>
		/// <param name="separator">
		/// The delimiter for the string in the
		/// <paramref name="value"/> parameter.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>,
		/// sorted and without duplicate entries.
		/// </returns>
		public static T[] ToArray<T>(
			string value, string separator
			) where T : struct
		{
			return ToArray<T>(
				value, separator, ConvertValueTypeCollection.DefaultRemoveDuplicates);
		}

		/// <summary>
		/// Returns a typed array from a comma-delimited
		/// string, optionally sorting and removing
		/// duplicates from the resulting array.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="value">
		/// A comma-delimited string containing values for the array.
		/// </param>
		/// <param name="preserveDuplicates">
		/// Whether to keep duplicate entries, or to
		/// remove duplicates from the array returned.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(
			string value, bool preserveDuplicates)
			where T : struct
		{
			return ToArray<T>(value, DefaultSeparator, preserveDuplicates);
		}

		/// <summary>
		/// Returns a typed array from a delimited
		/// string, optionally sorting and removing
		/// duplicates from the resulting array.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="value">
		/// A string, delimited by the <paramref name="separator"/>
		/// string, containing values for the array.
		/// </param>
		/// <param name="separator">
		/// The delimiter for the string in the
		/// <paramref name="value"/> parameter.
		/// </param>
		/// <param name="preserveDuplicates">
		/// Whether to keep duplicate entries, or to
		/// remove duplicates from the array returned.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(string value, string separator, bool preserveDuplicates)
			where T : struct
		{
			IList<T> list = ToList<T>(value, separator, preserveDuplicates);
			T[] ret = new T[list.Count];
			list.CopyTo(ret, 0);
			return ret;
		}

		#endregion

		#region [ Overloads of Public Static ToEnumerable<T> Method ]

		/// <summary>
		/// Returns a typed collection from a comma-delimited string,
		/// with the resulting collection sorted and duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of collection to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="value">
		/// A comma-delimited string containing values for the collection.
		/// </param>
		/// <returns>
		/// An collection of type <typeparamref name="T"/>,
		/// sorted and without duplicate entries.
		/// </returns>
		public static IEnumerable<T> ToEnumerable<T>(
			string value)
			where T : struct
		{
			return ToEnumerable<T>(value, DefaultSeparator);
		}

		/// <summary>
		/// Returns a typed collection from a delimited string,
		/// with the resulting collection sorted and duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of collection to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="value">
		/// A string, delimited by the <paramref name="separator"/>
		/// string, containing values for the collection.
		/// </param>
		/// <param name="separator">
		/// The delimiter for the string in the
		/// <paramref name="value"/> parameter.
		/// </param>
		/// <returns>
		/// An collection of type <typeparamref name="T"/>,
		/// sorted and without duplicate entries.
		/// </returns>
		public static IEnumerable<T> ToEnumerable<T>(
			string value, string separator)
			where T : struct
		{
			return ToEnumerable<T>(
				value, separator, ConvertValueTypeCollection.DefaultRemoveDuplicates);
		}

		/// <summary>
		/// Returns a typed collection from a comma-delimited
		/// string, optionally sorting and removing
		/// duplicates from the resulting collection.
		/// </summary>
		/// <typeparam name="T">
		/// The type of collection to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="value">
		/// A comma-delimited string containing values for the collection.
		/// </param>
		/// <param name="preserveDuplicates">
		/// Whether to keep duplicate entries, or to
		/// remove duplicates from the collection returned.
		/// </param>
		/// <returns>
		/// An collection of type <typeparamref name="T"/>.
		/// </returns>
		public static IEnumerable<T> ToEnumerable<T>(
			string value, bool preserveDuplicates)
			where T : struct
		{
			return ToEnumerable<T>(value, DefaultSeparator, preserveDuplicates);
		}

		/// <summary>
		/// Returns a typed collection from a delimited
		/// string, optionally sorting and removing
		/// duplicates from the resulting collection.
		/// </summary>
		/// <typeparam name="T">
		/// The type of collection to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="value">
		/// A string, delimited by the <paramref name="separator"/>
		/// string, containing values for the collection.
		/// </param>
		/// <param name="separator">
		/// The delimiter for the string in the
		/// <paramref name="value"/> parameter.
		/// </param>
		/// <param name="preserveDuplicates">
		/// Whether to keep duplicate entries, or to
		/// remove duplicates from the collection returned.
		/// </param>
		/// <returns>
		/// An collection of type <typeparamref name="T"/>.
		/// </returns>
		public static IEnumerable<T> ToEnumerable<T>(
			string value, string separator, bool preserveDuplicates)
			where T : struct
		{
			IList<T> list = ToList<T>(value, separator, preserveDuplicates);

			foreach (T t in list)
			{
				yield return t;
			}
		}

		#endregion

		#region [ Public Static ToList<T> Method, invoked by other Static Methods ]

		/// <summary>
		/// Returns a typed list from a delimited
		/// string, optionally sorting and removing
		/// duplicates from the resulting list.
		/// </summary>
		/// <typeparam name="T">
		/// The type of collection to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="value">
		/// A string, delimited by the <paramref name="separator"/>
		/// string, containing values for the list.
		/// </param>
		/// <param name="separator">
		/// The delimiter for the string in the
		/// <paramref name="value"/> parameter.
		/// </param>
		/// <param name="preserveDuplicates">
		/// Whether to keep duplicate entries, or to
		/// remove duplicates from the list returned.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static IList<T> ToList<T>(
			string value, string separator, bool preserveDuplicates)
			where T : struct
		{
			if (value == null)
			{
				throw new ArgumentNullException("value", "The string value to split is null.");
			}

			if (string.IsNullOrEmpty(separator))
			{
				throw new ArgumentException("The delimiter for splitting the string is null.", "separator");
			}

			string[] strings = value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);

			// Invoke the internal method of the ConvertStrings class,
			// passing the string array made by splitting the string.
			return strings.ToList<T>(preserveDuplicates);
		}
		*/
		#endregion

		private static IEnumerable<string> SplitDelimitedString(
			this string delimitedString, string separator)
		{
			if (string.IsNullOrWhiteSpace(delimitedString))
			{
				return new string[0];
			}

			if (string.IsNullOrWhiteSpace(separator))
			{
				return delimitedString.Split(
					DefaultStringSplitter,
					StringSplitOptions.RemoveEmptyEntries);
			}

			return separator.Length > 1
				? delimitedString.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
				: delimitedString.Split(new[] { separator[0] }, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}