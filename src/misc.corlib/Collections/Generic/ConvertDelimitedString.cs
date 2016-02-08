namespace MiscCorLib.Collections.Generic
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// A set of static methods for converting delimited strings
	/// into typed collections of <see cref="IComparable"/>,
	/// with options for sorting and removing duplicate entries.
	/// </summary>
	[CLSCompliant(true)]
	public static class ConvertDelimitedString
	{
		/// <summary>
		/// A constant value to use when converting to
		/// and from a delimited string, always a comma.
		/// </summary>
		internal const string DefaultSeparator = ",";

		#region [ Overloads of Public Static ToArray<T> Method ]

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
				value, separator, ConvertValueTypeCollection.DefaultPreserveDuplicates);
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
				value, separator, ConvertValueTypeCollection.DefaultPreserveDuplicates);
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

		#endregion
	}
}