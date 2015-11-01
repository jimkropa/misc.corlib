namespace MiscCorLib.Collections
{
	using System;
	using System.Collections;
	using System.Collections.Generic;

	/// <summary>
	/// A set of static methods for converting typed collections
	/// of <see cref="object"/>s into string arrays and
	/// delimited strings, optionally removing duplicate entries.
	/// </summary>
	/// <remarks>
	/// This static class is for converting "reference types"
	/// including stings and any objects. For converting
	/// "value types" such as numbers, identifiers and dates,
	/// use the <see cref="ConvertCollection"/> class.
	/// </remarks>
	[CLSCompliant(true)]
	public static class ConvertObjectCollection
	{
		#region [ ToStringArray and ToString Extension Methods for Generic IEnumerable<T> of Reference Types ]

		/// <summary>
		/// Converts a generic collection to a
		/// one-dimensional array of <see cref="string"/>,
		/// by calling the <see cref="object.ToString()"/>
		/// method on each item in the source collection,
		/// and removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of items.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of item, derived from <see cref="object"/>.
		/// </typeparam>
		/// <returns>
		/// An array of strings with no duplicates.
		/// </returns>
		public static string[] ToStringArray<T>(this IEnumerable<T> collection) where T : class
		{
			return ToStringArray(collection, ConvertStrings.DefaultPreserveDuplicates);
		}

		/// <summary>
		/// Converts a generic collection to a
		/// one-dimensional array of <see cref="string"/>,
		/// by calling the <see cref="object.ToString()"/>
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
		/// The generic type of item, derived from <see cref="object"/>.
		/// </typeparam>
		/// <returns>
		/// An array of strings.
		/// </returns>
		public static string[] ToStringArray<T>(this IEnumerable<T> collection, bool preserveDuplicates) where T : class
		{
			if (collection == null)
			{
				return new string[0];
			}

			ICollection<string> list = new List<string>();

			foreach (T item in collection)
			{
				if (item != null)
				{
					string str = item.ToString();

					if (!string.IsNullOrEmpty(str))
					{
						if (preserveDuplicates)
						{
							list.Add(str);
						}
						else if (!list.Contains(str))
						{
							list.Add(str);
						}
					}
				}
			}

			string[] ary = new string[list.Count];

			list.CopyTo(ary, 0);

			return ary;
		}

		/// <summary>
		/// Converts a generic collection to a
		/// comma-delimited <see cref="string"/>,
		/// by calling the <see cref="object.ToString()"/>
		/// method on each item in the source collection,
		/// and removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of items.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of item, derived from <see cref="object"/>.
		/// </typeparam>
		/// <returns>
		/// A comma-delimited string.
		/// </returns>
		public static string ToString<T>(this IEnumerable<T> collection) where T : class
		{
			return ToString(collection, ConvertStrings.DefaultPreserveDuplicates);
		}

		/// <summary>
		/// Converts a generic collection to a
		/// comma-delimited <see cref="string"/>,
		/// by calling the <see cref="object.ToString()"/>
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
		/// The generic type of item, derived from <see cref="object"/>.
		/// </typeparam>
		/// <returns>
		/// A comma-delimited string.
		/// </returns>
		public static string ToString<T>(this IEnumerable<T> collection, bool preserveDuplicates) where T : class
		{
			return ToString(collection, ConvertString.DefaultSeparator, preserveDuplicates);
		}

		/// <summary>
		/// Converts a generic collection to a
		/// <see cref="string"/> delimited by the value
		/// of the <paramref name="separator"/> parameter,
		/// by calling the <see cref="object.ToString()"/>
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
		/// The generic type of item, derived from <see cref="object"/>.
		/// </typeparam>
		/// <returns>
		/// A <paramref name="separator"/>-delimited string.
		/// </returns>
		public static string ToString<T>(this IEnumerable<T> collection, string separator) where T : class
		{
			return ToString(collection, separator, ConvertStrings.DefaultPreserveDuplicates);
		}

		/// <summary>
		/// Converts a generic collection to a
		/// <see cref="string"/> delimited by the value
		/// of the <paramref name="separator"/> parameter,
		/// by calling the <see cref="object.ToString()"/>
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
		/// <typeparam name="T">
		/// The generic type of item, derived from <see cref="object"/>.
		/// </typeparam>
		/// <returns>
		/// A <paramref name="separator"/>-delimited string.
		/// </returns>
		public static string ToString<T>(this IEnumerable<T> collection, string separator, bool preserveDuplicates)
			where T : class
		{
			return string.Join(separator, ToStringArray(collection, preserveDuplicates));
		}

		#endregion

		#region [ ToStringArray and ToString Methods for IEnumerable, pass thru to IEnumerableExtensions ]

		/// <summary>
		/// Converts a collection to a
		/// one-dimensional array of <see cref="string"/>,
		/// by calling the <see cref="object.ToString()"/>
		/// method on each item in the source collection,
		/// and removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A collection of items.
		/// </param>
		/// <returns>
		/// An array of strings with no duplicates.
		/// </returns>
		/// <remarks>
		/// This method is simply an invocation of
		/// <see cref="IEnumerableExtensions.ToStringArray(IEnumerable)"/>.
		/// </remarks>
		public static string[] ToStringArray(IEnumerable collection)
		{
			return collection.ToStringArray();
		}

		/// <summary>
		/// Converts a collection to a
		/// one-dimensional array of <see cref="string"/>,
		/// by calling the <see cref="object.ToString()"/>
		/// method on each item in the source collection,
		/// and optionally removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A collection of items.
		/// </param>
		/// <param name="preserveDuplicates">
		/// Whether to preserve duplicate items in the array returned.
		/// </param>
		/// <returns>
		/// An array of strings.
		/// </returns>
		/// <remarks>
		/// This method is simply an invocation of
		/// <see cref="IEnumerableExtensions.ToStringArray(IEnumerable,bool)"/>.
		/// </remarks>
		public static string[] ToStringArray(IEnumerable collection, bool preserveDuplicates)
		{
			return collection.ToStringArray(preserveDuplicates);
		}

		/// <summary>
		/// Converts a collection to a
		/// comma-delimited <see cref="string"/>,
		/// by calling the <see cref="object.ToString()"/>
		/// method on each item in the source collection,
		/// and removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A collection of items.
		/// </param>
		/// <returns>
		/// A comma-delimited string.
		/// </returns>
		/// <remarks>
		/// This method is simply an invocation of
		/// <see cref="IEnumerableExtensions.ToString(IEnumerable)"/>.
		/// </remarks>
		public static string ToString(IEnumerable collection)
		{
			return IEnumerableExtensions.ToString(collection);
		}

		/// <summary>
		/// Converts a collection to a
		/// comma-delimited <see cref="string"/>,
		/// by calling the <see cref="object.ToString()"/>
		/// method on each item in the source collection,
		/// and optionally removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A collection of items.
		/// </param>
		/// <param name="preserveDuplicates">
		/// Whether to preserve duplicate items in the array returned.
		/// </param>
		/// <returns>
		/// A comma-delimited string.
		/// </returns>
		/// <remarks>
		/// This method is simply an invocation of
		/// <see cref="IEnumerableExtensions.ToString(IEnumerable,bool)"/>.
		/// </remarks>
		public static string ToString(IEnumerable collection, bool preserveDuplicates)
		{
			return collection.ToString(preserveDuplicates);
		}

		/// <summary>
		/// Converts a collection to a
		/// <see cref="string"/> delimited by the value
		/// of the <paramref name="separator"/> parameter,
		/// by calling the <see cref="object.ToString()"/>
		/// method on each item in the source collection,
		/// and removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A collection of items.
		/// </param>
		/// <param name="separator">
		/// The delimiter to use.
		/// </param>
		/// <returns>
		/// A <paramref name="separator"/>-delimited string.
		/// </returns>
		/// <remarks>
		/// This method is simply an invocation of
		/// <see cref="IEnumerableExtensions.ToString(IEnumerable,string)"/>.
		/// </remarks>
		public static string ToString(IEnumerable collection, string separator)
		{
			return collection.ToString(separator);
		}

		/// <summary>
		/// Converts a collection to a
		/// <see cref="string"/> delimited by the value
		/// of the <paramref name="separator"/> parameter,
		/// by calling the <see cref="object.ToString()"/>
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
		/// <returns>
		/// A <paramref name="separator"/>-delimited string.
		/// </returns>
		/// <remarks>
		/// This method is simply an invocation of
		/// <see cref="IEnumerableExtensions.ToString(IEnumerable,string,bool)"/>.
		/// </remarks>
		public static string ToString(IEnumerable collection, string separator, bool preserveDuplicates)
		{
			return collection.ToString(separator, preserveDuplicates);
		}

		#endregion

		#region [ Overloads of Public Static ToArray<T> Method, pass thru to IEnumerableExtensions ]

		/// <summary>
		/// Returns a typed array from any collection,
		/// sorted and with duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="values">
		/// A collection whose elements, when converted to string,
		/// may be converted to the <see cref="IComparable"/>
		/// type <typeparamref name="T"/>.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>,
		/// sorted and with duplicates removed.
		/// </returns>
		/// <remarks>
		/// This method is simply an invocation of
		/// <see cref="IEnumerableExtensions.ToArray{T}(IEnumerable)"/>.
		/// </remarks>
		public static T[] ToArray<T>(IEnumerable values) where T : IComparable
		{
			return values.ToArray<T>();
		}

		/// <summary>
		/// Returns a typed array from any collection,
		/// optionally sorting the array and removing duplicates.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="values">
		/// A collection whose elements, when converted to string,
		/// may be converted to the <see cref="IComparable"/>
		/// type <typeparamref name="T"/>.
		/// </param>
		/// <param name="preserveDuplicates">
		/// Whether to keep duplicate entries, or to
		/// remove duplicates from the array returned.
		/// </param>
		/// <param name="sort">
		/// Whether to sort the entries in the array returned.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>.
		/// </returns>
		/// <remarks>
		/// This method is simply an invocation of
		/// <see cref="IEnumerableExtensions.ToArray{T}(IEnumerable,bool,bool)"/>.
		/// </remarks>
		public static T[] ToArray<T>(IEnumerable values, bool preserveDuplicates, bool sort) where T : IComparable
		{
			return values.ToArray<T>(preserveDuplicates, sort);
		}

		#endregion

		#region [ Overloads of Public Static ToEnumerable<T> Method, pass thru to IEnumerableExtensions ]

		/// <summary>
		/// Returns a typed collection from any collection,
		/// sorted and with duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="values">
		/// A collection whose elements, when converted to string,
		/// may be converted to the <see cref="IComparable"/>
		/// type <typeparamref name="T"/>.
		/// </param>
		/// <returns>
		/// A collection of type <typeparamref name="T"/>,
		/// sorted and with duplicates removed.
		/// </returns>
		/// <remarks>
		/// This method is simply an invocation of
		/// <see cref="IEnumerableExtensions.ToEnumerable{T}(IEnumerable)"/>.
		/// </remarks>
		public static IEnumerable<T> ToEnumerable<T>(IEnumerable values) where T : IComparable
		{
			return values.ToEnumerable<T>();
		}

		/// <summary>
		/// Returns a typed collection from any collection,
		/// optionally sorting the array and removing duplicates.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="values">
		/// A collection whose elements, when converted to string,
		/// may be converted to the <see cref="IComparable"/>
		/// type <typeparamref name="T"/>.
		/// </param>
		/// <param name="preserveDuplicates">
		/// Whether to keep duplicate entries, or to
		/// remove duplicates from the collection returned.
		/// </param>
		/// <param name="sort">
		/// Whether to sort the entries in the collection returned.
		/// </param>
		/// <returns>
		/// An collection of type <typeparamref name="T"/>.
		/// </returns>
		/// <remarks>
		/// This method is simply an invocation of
		/// <see cref="IEnumerableExtensions.ToEnumerable{T}(IEnumerable,bool,bool)"/>.
		/// </remarks>
		public static IEnumerable<T> ToEnumerable<T>(IEnumerable values, bool preserveDuplicates, bool sort)
			where T : IComparable
		{
			return values.ToEnumerable<T>(preserveDuplicates, sort);
		}

		#endregion

		#region [ Overloads of Public Static ToList<T> Method, pass thru to IEnumerableExtensions ]

		/// <summary>
		/// Returns a typed list from any collection,
		/// sorted and with duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="values">
		/// A collection whose elements, when converted to string,
		/// may be converted to the <see cref="IComparable"/>
		/// type <typeparamref name="T"/>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>,
		/// sorted and with duplicates removed.
		/// </returns>
		/// <remarks>
		/// This method is simply an invocation of
		/// <see cref="IEnumerableExtensions.ToList{T}(IEnumerable)"/>.
		/// </remarks>
		public static IList<T> ToList<T>(IEnumerable values) where T : IComparable
		{
			return values.ToList<T>();
		}

		/// <summary>
		/// Returns a typed list from any collection,
		/// optionally sorting the list and removing duplicates.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="values">
		/// A collection whose elements, when converted to string,
		/// may be converted to the <see cref="IComparable"/>
		/// type <typeparamref name="T"/>.
		/// </param>
		/// <param name="preserveDuplicates">
		/// Whether to keep duplicate entries, or to
		/// remove duplicates from the list returned.
		/// </param>
		/// <param name="sort">
		/// Whether to sort the entries in the list returned.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		/// <remarks>
		/// This method is simply an invocation of
		/// <see cref="IEnumerableExtensions.ToList{T}(IEnumerable,bool,bool)"/>.
		/// </remarks>
		public static IList<T> ToList<T>(IEnumerable values, bool preserveDuplicates, bool sort) where T : IComparable
		{
			return values.ToList<T>(preserveDuplicates, sort);
		}

		#endregion
	}
}