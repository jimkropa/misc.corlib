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
		public static T[] ToArray<T>(
			this IEnumerable<string> values)
			where T : struct
		{
			return ToArray<T>(values, ConvertValueTypeCollection.DefaultRemoveDuplicates);
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
		/// <returns>
		/// An array of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(
			this IEnumerable<string> values, bool preserveDuplicates)
			where T : struct
		{
			return ToList<T>(values, preserveDuplicates).ToArray();
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
		public static IReadOnlyList<T> ToEnumerable<T>(
			this IEnumerable<string> values)
			where T : struct
		{
			return ToEnumerable<T>(values, ConvertValueTypeCollection.DefaultRemoveDuplicates);
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
		/// <returns>
		/// An collection of type <typeparamref name="T"/>.
		/// </returns>
		public static IReadOnlyList<T> ToEnumerable<T>(
			this IEnumerable<string> values,
			bool preserveDuplicates)
			where T : struct
		{
			List<T> list = ToList<T>(values, preserveDuplicates);

			return list;
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
		public static IList<T> ToList<T>(
			this IEnumerable<string> values)
			where T : struct
		{
			return ToList<T>(values, ConvertValueTypeCollection.DefaultRemoveDuplicates);
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
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static List<T> ToList<T>(this IEnumerable<string> values, bool preserveDuplicates)
			where T : struct
		{
			if (values == null)
			{
				throw new ArgumentNullException("values", "The list of strings to convert is null.");
			}

			List<T> list = new List<T>();
			Type t = typeof(T);
			TypeConverter converter = TypeDescriptor.GetConverter(t);

			////	if (converter == null)
			////	{
			////		throw new NullReferenceException(string.Format(
			////			"No TypeConverter could be resolved for the \"{0}\" type.", t.Name));
			////	}

			foreach (string value in values)
			{
				if (string.IsNullOrEmpty(value))
				{
					continue;
				}

				T instance;
				if (!TryParseFromString(converter, value, out instance))
				if (!preserveDuplicates && list.Contains(instance))
				{
					continue;
				}

				list.Add(instance);
			}

			return list;
		}

		#endregion

		private static bool TryParseFromString<T>(
			TypeConverter converter, string value, out T result)
			where T : struct
		{
			result = default(T);

			try
			{
				object converted = converter.ConvertFrom(value);
				if (converted == null)
				{
					return false;
				}

				result = (T)converted;

				return true;
			}
			catch (FormatException)
			{
				return false;
			}
			catch (NotSupportedException)
			{
				return false;
			}
		}
	}
}