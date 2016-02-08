namespace MiscCorLib.Collections.Generic
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// A set of static extension methods for converting typed
	/// collections of <see cref="ValueType"/> values into string arrays
	/// and delimited strings, optionally preserving duplicate entries.
	/// </summary>
	[CLSCompliant(true)]
	public static class ConvertValueTypeCollection
	{
		/// <summary>
		/// A value to use when sorting a set of strings
		/// to a generic typed collection, indicating to remove
		/// duplicate values from the resulting collection by default.
		/// </summary>
		internal const bool DefaultPreserveDuplicates = false;

		#region [ Overloads of ToStringArray Extension Method ]

		/// <summary>
		/// Converts a generic collection to a
		/// one-dimensional array of <see cref="string"/>,
		/// by calling the <see cref="ValueType.ToString"/>
		/// method on each item in the source collection,
		/// and removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of items.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of item, derived from <see cref="ValueType"/>.
		/// </typeparam>
		/// <returns>
		/// An array of strings with no duplicates
		/// and no empty or null values.
		/// </returns>
		public static string[] ToStringArray<T>(
			this IEnumerable<T> collection)
			where T : struct
		{
			return ToStringArray(collection, DefaultPreserveDuplicates);
		}

		/// <summary>
		/// Converts a generic collection to a
		/// one-dimensional array of <see cref="string"/>,
		/// by calling the <see cref="ValueType.ToString"/>
		/// method on each item in the source collection,
		/// and optionally preserving any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of items.
		/// </param>
		/// <param name="preserveDuplicates">
		/// Whether to preserve duplicate items in the array returned.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of item, derived from <see cref="ValueType"/>.
		/// </typeparam>
		/// <returns>
		/// An array of strings with null or empty values omitted,
		/// and duplicates omitted unless the
		/// <paramref name="preserveDuplicates"/>
		/// value is <c>true</c>.
		/// </returns>
		public static string[] ToStringArray<T>(
			this IEnumerable<T> collection,
			bool preserveDuplicates)
			where T : struct
		{
			return ToStringArray(collection, preserveDuplicates, null);
		}

		/// <summary>
		/// Converts a generic collection to a
		/// one-dimensional array of <see cref="string"/>,
		/// by calling the <paramref name="toStringMethod"/>
		/// delegate on each item in the source collection,
		/// and removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of items.
		/// </param>
		/// <param name="toStringMethod">
		/// A delegate function which accepts a <typeparamref name="T"/>
		/// value and returns a string. Pass this parameter if a custom
		/// format string is needed. If not specified, uses the default
		/// <see cref="ValueType.ToString"/> method.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of item, derived from <see cref="ValueType"/>.
		/// </typeparam>
		/// <returns>
		/// An array of strings created by the
		/// <paramref name="toStringMethod"/>,
		/// with no duplicates and no empty or null values.
		/// </returns>
		public static string[] ToStringArray<T>(
			this IEnumerable<T> collection,
			Func<T, string> toStringMethod)
			where T : struct
		{
			return ToStringArray(collection, DefaultPreserveDuplicates, toStringMethod);
		}

		/// <summary>
		/// Converts a generic collection to a
		/// one-dimensional array of <see cref="string"/>,
		/// by calling the <paramref name="toStringMethod"/>
		/// delegate on each item in the source collection,
		/// and optionally preserving any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of items.
		/// </param>
		/// <param name="preserveDuplicates">
		/// Whether to preserve duplicate items in the array returned.
		/// </param>
		/// <param name="toStringMethod">
		/// A delegate function which accepts a <typeparamref name="T"/>
		/// value and returns a string. Pass this parameter if a custom
		/// format string is needed. If not specified, uses the default
		/// <see cref="ValueType.ToString"/> method.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of item, derived from <see cref="ValueType"/>.
		/// </typeparam>
		/// <returns>
		/// An array of strings created by the <paramref name="toStringMethod"/>,
		/// with null or empty values omitted, and duplicates omitted unless
		/// the <paramref name="preserveDuplicates"/> value is <c>true</c>.
		/// </returns>
		public static string[] ToStringArray<T>(
			this IEnumerable<T> collection,
			bool preserveDuplicates,
			Func<T, string> toStringMethod)
			where T : struct
		{
			if (collection == null)
			{
				return new string[0];
			}

			ICollection<string> list = new List<string>();

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (T item in collection)
			{
				string str = toStringMethod == null ? item.ToString() : toStringMethod(item);
				if (string.IsNullOrWhiteSpace(str))
				{
					continue;
				}

				// The base List<string> class uses "Ordinal" comparison by default,
				// so applying the Linq method is not necessary:
				if (preserveDuplicates || !list.Contains(str)) // !list.Contains(str, StringComparer.Ordinal)
				{
					list.Add(str);
				}
			}

			string[] ary = new string[list.Count];

			list.CopyTo(ary, 0);

			return ary;
		}

		#endregion

		#region [ Overloads of ToDelimitedString Extension Method ]

		/// <summary>
		/// Converts a generic collection to a
		/// comma-delimited <see cref="string"/>,
		/// by calling the <see cref="ValueType.ToString()"/>
		/// method on each item in the source collection,
		/// and removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of items.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of item, derived from <see cref="ValueType"/>.
		/// </typeparam>
		/// <returns>
		/// A comma-delimited string.
		/// </returns>
		public static string ToDelimitedString<T>(
			this IEnumerable<T> collection)
			where T : struct
		{
			return ToDelimitedString(collection, DefaultPreserveDuplicates);
		}

		/// <summary>
		/// Converts a generic collection to a
		/// comma-delimited <see cref="string"/>,
		/// by calling the <see cref="ValueType.ToString()"/>
		/// method on each item in the source collection,
		/// and optionally removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of items.
		/// </param>
		/// <param name="preserveDuplicates">
		/// Whether to preserve duplicate items in the array returned.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of item, derived from <see cref="ValueType"/>.
		/// </typeparam>
		/// <returns>
		/// A comma-delimited string.
		/// </returns>
		public static string ToDelimitedString<T>(
			this IEnumerable<T> collection,
			bool preserveDuplicates)
			where T : struct
		{
			return ToDelimitedString(collection, ConvertDelimitedString.DefaultSeparator, preserveDuplicates, null);
		}

		/// <summary>
		/// Converts a generic collection to a
		/// <see cref="string"/> delimited by the value
		/// of the <paramref name="separator"/> parameter,
		/// by calling the <see cref="ValueType.ToString()"/>
		/// method on each item in the source collection,
		/// and removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of items.
		/// </param>
		/// <param name="separator">
		/// The delimiter to use.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of item, derived from <see cref="ValueType"/>.
		/// </typeparam>
		/// <returns>
		/// A <paramref name="separator"/>-delimited string.
		/// </returns>
		public static string ToDelimitedString<T>(
			this IEnumerable<T> collection,
			string separator) where T : struct
		{
			return ToDelimitedString(collection, separator, DefaultPreserveDuplicates, null);
		}

		/// <summary>
		/// Converts a generic collection to a
		/// <see cref="string"/> delimited by the value
		/// of the <paramref name="separator"/> parameter,
		/// by calling the <see cref="ValueType.ToString()"/>
		/// method on each item in the source collection,
		/// and optionally removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of items.
		/// </param>
		/// <param name="separator">
		/// The delimiter to use.
		/// </param>
		/// <param name="preserveDuplicates">
		/// Whether to preserve duplicate items in the array returned.
		/// </param>
		/// <param name="toStringMethod">
		/// A delegate function which accepts a <typeparamref name="T"/>
		/// value and returns a string. Pass this parameter if a custom
		/// format string is needed. If not specified, uses the default
		/// <see cref="ValueType.ToString"/> method.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of item, derived from <see cref="ValueType"/>.
		/// </typeparam>
		/// <returns>
		/// A <paramref name="separator"/>-delimited string.
		/// </returns>
		public static string ToDelimitedString<T>(this IEnumerable<T> collection, string separator, bool preserveDuplicates, Func<T, string> toStringMethod)
			where T : struct
		{
			return string.Join(separator, ToStringArray(collection, preserveDuplicates));
		}

		#endregion
	}
}