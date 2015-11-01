namespace MiscCorLib.Collections.Generic
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;

	/// <summary>
	/// A set of static methods for converting collections of strings
	/// into typed collections of <see cref="IComparable"/> or delimited strings,
	/// with options for sorting and removing duplicate entries.
	/// </summary>
	[CLSCompliant(true)]
	public static class ConvertStrings
	{
		/// <summary>
		/// A value to use when sorting a set of strings
		/// to a generic typed collection, indicating to remove
		/// duplicate values from the resulting collection by default.
		/// </summary>
		internal const bool DefaultPreserveDuplicates = false;

		/// <summary>
		/// A value to use when sorting a set of strings
		/// to a generic typed collection, indicating
		/// to sort the resulting collection by default.
		/// </summary>
		internal const bool DefaultSort = true;

		#region [ Overloads of Public Static ToArray<T> Method ]

		/// <summary>
		/// Returns a typed array from a collection of strings,
		/// sorted and with duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="values">
		/// A collection of strings which may be converted to the
		/// <see cref="IComparable"/> type <typeparamref name="T"/>.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>,
		/// sorted and with duplicates removed.
		/// </returns>
		public static T[] ToArray<T>(this IEnumerable<string> values) where T : IComparable
		{
			return ToArray<T>(values, DefaultPreserveDuplicates, DefaultSort);
		}

		/// <summary>
		/// Returns a typed array from a collection of strings,
		/// optionally sorting the array and removing duplicates.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="values">
		/// A collection of strings which may be converted to the
		/// <see cref="IComparable"/> type <typeparamref name="T"/>.
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
		public static T[] ToArray<T>(this IEnumerable<string> values, bool preserveDuplicates, bool sort)
			where T : IComparable
		{
			IList<T> list = ToList<T>(values, preserveDuplicates, sort);
			T[] ret = new T[list.Count];

			list.CopyTo(ret, 0);

			return ret;
		}

		#endregion

		#region [ Overloads of Public Static ToEnumerable<T> Method ]

		/// <summary>
		/// Returns a typed collection from a collection of strings,
		/// sorted and with duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="values">
		/// A collection of strings which may be converted to the
		/// <see cref="IComparable"/> type <typeparamref name="T"/>.
		/// </param>
		/// <returns>
		/// A collection of type <typeparamref name="T"/>,
		/// sorted and with duplicates removed.
		/// </returns>
		public static IEnumerable<T> ToEnumerable<T>(this IEnumerable<string> values) where T : IComparable
		{
			return ToEnumerable<T>(values, DefaultPreserveDuplicates, DefaultSort);
		}

		/// <summary>
		/// Returns a typed collection from a collection of strings,
		/// optionally sorting the array and removing duplicates.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="values">
		/// A collection of strings which may be converted to the
		/// <see cref="IComparable"/> type <typeparamref name="T"/>.
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
		public static IEnumerable<T> ToEnumerable<T>(this IEnumerable<string> values, bool preserveDuplicates, bool sort)
			where T : IComparable
		{
			IList<T> list = ToList<T>(values, preserveDuplicates, sort);

			foreach (T t in list)
			{
				yield return t;
			}
		}

		#endregion

		#region [ Public Static ToList<T> Method, invoked by other Static Methods ]

		/// <summary>
		/// Returns a typed list from a collection of strings,
		/// sorted and with duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="values">
		/// A collection of strings which may be converted to the
		/// <see cref="IComparable"/> type <typeparamref name="T"/>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>,
		/// sorted and with duplicates removed.
		/// </returns>
		public static IList<T> ToList<T>(this IEnumerable<string> values) where T : IComparable
		{
			return ToList<T>(values, DefaultPreserveDuplicates, DefaultSort);
		}

		/// <summary>
		/// Returns a typed list from a collection of strings,
		/// optionally sorting the list and removing duplicates.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="values">
		/// A collection of strings which may be converted to the
		/// <see cref="IComparable"/> type <typeparamref name="T"/>.
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
		public static IList<T> ToList<T>(this IEnumerable<string> values, bool preserveDuplicates, bool sort)
			where T : IComparable
		{
			if (values == null)
			{
				throw new ArgumentNullException("values", "The list of strings to convert is null.");
			}

			List<T> list = new List<T>();
			Type t = typeof(T);
			TypeConverter converter = TypeDescriptor.GetConverter(t);

			if (converter == null)
			{
				throw new NullReferenceException(string.Format(
					"No TypeConverter could be resolved for the \"{0}\" type.", t.Name));
			}

			foreach (string value in values)
			{
				if (string.IsNullOrEmpty(value))
				{
					continue;
				}

				T instance;

				try
				{
					object converted = converter.ConvertFrom(value);

					instance = (T)converted;
				}
				catch (FormatException)
				{
					continue;
				}
				catch (NotSupportedException)
				{
					continue;
				}

				if ((!preserveDuplicates) && list.Contains(instance))
				{
					continue;
				}

				list.Add(instance);
			}

			if (sort)
			{
				list.Sort();
			}

			return list;
		}

		#endregion
	}
}