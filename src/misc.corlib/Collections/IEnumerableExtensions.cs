namespace MiscCorLib.Collections
{
	using System;
	using System.Collections;
	using System.Collections.Generic;

	/// <summary>
	/// Extension methods for <see cref="IEnumerable"/>.
	/// </summary>
	/// <remarks>
	/// Many of these methods support static methods
	/// of the <see cref="ConvertCollection"/> and
	/// <see cref="ConvertObjectCollection"/> classes.
	/// </remarks>
	[CLSCompliant(true)]
	public static class IEnumerableExtensions
	{
		#region [ GetItemByIndex Method ]

		/// <summary>
		/// Uses an internal counter to retrieve an item from an
		/// <see cref="IEnumerable"/> based on a specified index.
		/// </summary>
		/// <param name="enumerable">
		/// The instance of <see cref="IEnumerable"/>
		/// in which to locate an item by index.
		/// </param>
		/// <param name="index">
		/// The zero-based position of an item
		/// in the underlying colleciton.
		/// </param>
		/// <returns>
		/// The item whose position in the underlying
		/// collection matches <paramref name="index"/>.
		/// </returns>
		public static object GetItemByIndex(this IEnumerable enumerable, int index)
		{
			if (enumerable == null)
			{
				throw new ArgumentNullException("enumerable", "The instance of IEnumerable is a null reference!");
			}

			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "The index must be a positive integer.");
			}

			int count = 0;
			foreach (object item in enumerable)
			{
				if (count == index)
				{
					return item;
				}

				count++;
			}

			throw new IndexOutOfRangeException("The instance of IEnumerable does not enumerate to the specified index.");
		}

		#endregion

		#region [ ToStringArray and ToString Methods for IEnumerable ]

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
		public static string[] ToStringArray(this IEnumerable collection)
		{
			return ToStringArray(collection, ConvertStrings.DefaultPreserveDuplicates);
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
		public static string[] ToStringArray(this IEnumerable collection, bool preserveDuplicates)
		{
			if (collection == null)
			{
				return new string[0];
			}

			ICollection<string> list = new List<string>();

			foreach (object item in collection)
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
		public static string ToString(this IEnumerable collection)
		{
			return ToString(collection, ConvertStrings.DefaultPreserveDuplicates);
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
		public static string ToString(this IEnumerable collection, bool preserveDuplicates)
		{
			return ToString(collection, ConvertString.DefaultSeparator, preserveDuplicates);
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
		public static string ToString(this IEnumerable collection, string separator)
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
		/// <returns>
		/// A <paramref name="separator"/>-delimited string.
		/// </returns>
		public static string ToString(this IEnumerable collection, string separator, bool preserveDuplicates)
		{
			return string.Join(separator, ToStringArray(collection, preserveDuplicates));
		}

		#endregion

		#region [ Overloads of Public Static ToArray<T> Method ]

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
		public static T[] ToArray<T>(this IEnumerable values) where T : IComparable
		{
			return ToArray<T>(values, ConvertStrings.DefaultPreserveDuplicates, ConvertStrings.DefaultSort);
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
		public static T[] ToArray<T>(this IEnumerable values, bool preserveDuplicates, bool sort) where T : IComparable
		{
			IList<T> list = ToList<T>(values, preserveDuplicates, sort);
			T[] ret = new T[list.Count];

			list.CopyTo(ret, 0);

			return ret;
		}

		#endregion

		#region [ Overloads of Public Static ToEnumerable<T> Method ]

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
		public static IEnumerable<T> ToEnumerable<T>(this IEnumerable values) where T : IComparable
		{
			return ToEnumerable<T>(values, ConvertStrings.DefaultPreserveDuplicates, ConvertStrings.DefaultSort);
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
		public static IEnumerable<T> ToEnumerable<T>(this IEnumerable values, bool preserveDuplicates, bool sort)
			where T : IComparable
		{
			IList<T> list = ToList<T>(values, preserveDuplicates, sort);

			foreach (T t in list)
			{
				yield return t;
			}
		}

		#endregion

		#region [ Overloads of Public Static ToList<T> Method ]

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
		public static IList<T> ToList<T>(this IEnumerable values) where T : IComparable
		{
			return ToList<T>(values, ConvertStrings.DefaultPreserveDuplicates, ConvertStrings.DefaultSort);
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
		public static IList<T> ToList<T>(this IEnumerable values, bool preserveDuplicates, bool sort) where T : IComparable
		{
			ICollection<string> collection = new List<string>();
			foreach (object value in values)
			{
				if (value != null)
				{
					collection.Add(value.ToString());
				}
			}

			return collection.ToList<T>(preserveDuplicates, sort);
		}

		#endregion
	}
}