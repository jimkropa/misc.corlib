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
		/// <param name="delimitedString">
		/// A comma-delimited string containing
		/// <typeparamref name="T"/> values to parse.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the default <see cref="TypeConverter"/>.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(
			this string delimitedString,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates,
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
		/// <param name="delimitedString">
		/// A comma-delimited string containing
		/// <typeparamref name="T"/> values to parse.
		/// </param>
		/// <param name="converter">
		/// A <see cref="TypeConverter"/> to convert
		/// strings to values of <typeparamref name="T"/>.
		/// If <c>null</c>, the default converter will be used.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the given <paramref name="converter"/>.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(
			this string delimitedString,
			TypeConverter converter,
			bool throwTypeConversionExceptions,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
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
		/// <param name="delimitedString">
		/// A comma-delimited string containing
		/// <typeparamref name="T"/> values to parse.
		/// </param>
		/// <param name="tryParseDelegate">
		/// A delegate method conforming to the
		/// <see cref="ConvertStrings.TryParseFromString{T}"/>
		/// signature, for optimized type coversion
		/// via a "TryParse" method common to many
		/// primitive <see cref="ValueType"/> classes.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(
			this string delimitedString,
			ConvertStrings.TryParseFromString<T> tryParseDelegate,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return delimitedString.ToList(
				tryParseDelegate, removeDuplicates).ToArray();
		}

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
		/// <param name="delimitedString">
		/// A comma-delimited string containing
		/// <typeparamref name="T"/> values to parse.
		/// </param>
		/// <param name="separator">
		/// A string delimiter to use instead of a comma.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the default <see cref="TypeConverter"/>.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(
			this string delimitedString,
			string separator,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates,
			bool throwTypeConversionExceptions = ConvertStrings.DefaultThrowTypeConversionExceptions)
			where T : struct
		{
			return delimitedString.ToList<T>(
				separator, removeDuplicates, throwTypeConversionExceptions).ToArray();
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
		/// <param name="delimitedString">
		/// A comma-delimited string containing
		/// <typeparamref name="T"/> values to parse.
		/// </param>
		/// <param name="separator">
		/// A string delimiter to use instead of a comma.
		/// </param>
		/// <param name="converter">
		/// A <see cref="TypeConverter"/> to convert
		/// strings to values of <typeparamref name="T"/>.
		/// If <c>null</c>, the default converter will be used.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the given <paramref name="converter"/>.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(
			this string delimitedString,
			string separator,
			TypeConverter converter,
			bool throwTypeConversionExceptions,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return delimitedString.ToList<T>(
				separator, converter, throwTypeConversionExceptions, removeDuplicates).ToArray();
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
		/// <param name="delimitedString">
		/// A comma-delimited string containing
		/// <typeparamref name="T"/> values to parse.
		/// </param>
		/// <param name="separator">
		/// A string delimiter to use instead of a comma.
		/// </param>
		/// <param name="tryParseDelegate">
		/// A delegate method conforming to the
		/// <see cref="ConvertStrings.TryParseFromString{T}"/>
		/// signature, for optimized type coversion
		/// via a "TryParse" method common to many
		/// primitive <see cref="ValueType"/> classes.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(
			this string delimitedString,
			string separator,
			ConvertStrings.TryParseFromString<T> tryParseDelegate,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return delimitedString.ToList(
				separator, tryParseDelegate, removeDuplicates).ToArray();
		}

		#endregion

		#region [ Overloads of Public Static ToEnumerable<T> Method ]

		/// <summary>
		/// Returns a generic <see cref="IReadOnlyList{T}"/>
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
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the default <see cref="TypeConverter"/>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static IReadOnlyList<T> ToEnumerable<T>(
			this string delimitedString,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates,
			bool throwTypeConversionExceptions = ConvertStrings.DefaultThrowTypeConversionExceptions)
			where T : struct
		{
			return delimitedString.SplitDelimitedString().ToEnumerable<T>(
				removeDuplicates, throwTypeConversionExceptions);
		}

		/// <summary>
		/// Returns a generic <see cref="IReadOnlyList{T}"/>
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
		/// A <see cref="TypeConverter"/> to convert
		/// strings to values of <typeparamref name="T"/>.
		/// If <c>null</c>, the default converter will be used.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the given <paramref name="converter"/>.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static IReadOnlyList<T> ToEnumerable<T>(
			this string delimitedString,
			TypeConverter converter,
			bool throwTypeConversionExceptions,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return delimitedString.SplitDelimitedString().ToEnumerable<T>(
				converter, throwTypeConversionExceptions, removeDuplicates);
		}

		/// <summary>
		/// Returns a generic <see cref="IReadOnlyList{T}"/>
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
		/// A delegate method conforming to the
		/// <see cref="ConvertStrings.TryParseFromString{T}"/>
		/// signature, for optimized type coversion
		/// via a "TryParse" method common to many
		/// primitive <see cref="ValueType"/> classes.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static IReadOnlyList<T> ToEnumerable<T>(
			this string delimitedString,
			ConvertStrings.TryParseFromString<T> tryParseDelegate,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return delimitedString.SplitDelimitedString().ToEnumerable(
				tryParseDelegate, removeDuplicates);
		}

		/// <summary>
		/// Returns a generic <see cref="IReadOnlyList{T}"/>
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
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <param name="separator">
		/// A string delimiter to use instead of a comma.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the default <see cref="TypeConverter"/>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static IReadOnlyList<T> ToEnumerable<T>(
			this string delimitedString,
			string separator,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates,
			bool throwTypeConversionExceptions = ConvertStrings.DefaultThrowTypeConversionExceptions)
			where T : struct
		{
			return delimitedString.SplitDelimitedString(separator).ToEnumerable<T>(
				removeDuplicates, throwTypeConversionExceptions);
		}

		/// <summary>
		/// Returns a generic <see cref="IReadOnlyList{T}"/>
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
		/// <param name="separator">
		/// A string delimiter to use instead of a comma.
		/// </param>
		/// <param name="converter">
		/// A <see cref="TypeConverter"/> to convert
		/// strings to values of <typeparamref name="T"/>.
		/// If <c>null</c>, the default converter will be used.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the given <paramref name="converter"/>.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static IReadOnlyList<T> ToEnumerable<T>(
			this string delimitedString,
			string separator,
			TypeConverter converter,
			bool throwTypeConversionExceptions,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return delimitedString.SplitDelimitedString(separator).ToEnumerable<T>(
				converter, throwTypeConversionExceptions, removeDuplicates);
		}

		/// <summary>
		/// Returns a generic <see cref="IReadOnlyList{T}"/>
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
		/// <param name="delimitedString">
		/// A comma-delimited string containing
		/// <typeparamref name="T"/> values to parse.
		/// </param>
		/// <param name="separator">
		/// A string delimiter to use instead of a comma.
		/// </param>
		/// <param name="tryParseDelegate">
		/// A delegate method conforming to the
		/// <see cref="ConvertStrings.TryParseFromString{T}"/>
		/// signature, for optimized type coversion
		/// via a "TryParse" method common to many
		/// primitive <see cref="ValueType"/> classes.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static IReadOnlyList<T> ToEnumerable<T>(
			this string delimitedString,
			string separator,
			ConvertStrings.TryParseFromString<T> tryParseDelegate,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return delimitedString.SplitDelimitedString(separator).ToEnumerable(
				tryParseDelegate, removeDuplicates);
		}

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
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the default <see cref="TypeConverter"/>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static List<T> ToList<T>(
			this string delimitedString,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates,
			bool throwTypeConversionExceptions = ConvertStrings.DefaultThrowTypeConversionExceptions)
			where T : struct
		{
			return delimitedString.SplitDelimitedString().ToList<T>(
				removeDuplicates, throwTypeConversionExceptions);
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
		/// A <see cref="TypeConverter"/> to convert
		/// strings to values of <typeparamref name="T"/>.
		/// If <c>null</c>, the default converter will be used.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the given <paramref name="converter"/>.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static List<T> ToList<T>(
			this string delimitedString,
			TypeConverter converter,
			bool throwTypeConversionExceptions,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return delimitedString.SplitDelimitedString().ToList<T>(
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
		/// A delegate method conforming to the
		/// <see cref="ConvertStrings.TryParseFromString{T}"/>
		/// signature, for optimized type coversion
		/// via a "TryParse" method common to many
		/// primitive <see cref="ValueType"/> classes.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static List<T> ToList<T>(
			this string delimitedString,
			ConvertStrings.TryParseFromString<T> tryParseDelegate,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return delimitedString.SplitDelimitedString().ToList(
				tryParseDelegate, removeDuplicates);
		}

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
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <param name="separator">
		/// A string delimiter to use instead of a comma.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the default <see cref="TypeConverter"/>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static List<T> ToList<T>(
			this string delimitedString,
			string separator,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates,
			bool throwTypeConversionExceptions = ConvertStrings.DefaultThrowTypeConversionExceptions)
			where T : struct
		{
			return delimitedString.SplitDelimitedString(separator).ToList<T>(
				removeDuplicates, throwTypeConversionExceptions);
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
		/// <param name="separator">
		/// A string delimiter to use instead of a comma.
		/// </param>
		/// <param name="converter">
		/// A <see cref="TypeConverter"/> to convert
		/// strings to values of <typeparamref name="T"/>.
		/// If <c>null</c>, the default converter will be used.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the given <paramref name="converter"/>.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static List<T> ToList<T>(
			this string delimitedString,
			string separator,
			TypeConverter converter,
			bool throwTypeConversionExceptions,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return delimitedString.SplitDelimitedString(separator).ToList<T>(
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
		/// <param name="delimitedString">
		/// A comma-delimited string containing
		/// <typeparamref name="T"/> values to parse.
		/// </param>
		/// <param name="separator">
		/// A string delimiter to use instead of a comma.
		/// </param>
		/// <param name="tryParseDelegate">
		/// A delegate method conforming to the
		/// <see cref="ConvertStrings.TryParseFromString{T}"/>
		/// signature, for optimized type coversion
		/// via a "TryParse" method common to many
		/// primitive <see cref="ValueType"/> classes.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static List<T> ToList<T>(
			this string delimitedString,
			string separator,
			ConvertStrings.TryParseFromString<T> tryParseDelegate,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return delimitedString.SplitDelimitedString(separator).ToList(
				tryParseDelegate, removeDuplicates);
		}

		#endregion

		private static IEnumerable<string> SplitDelimitedString(
			this string delimitedString, string separator = null)
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