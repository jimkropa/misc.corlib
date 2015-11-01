namespace MiscCorLib.Collections.Generic
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;

	/// <summary>
	/// Extension methods for <see cref="IEnumerable{T}"/>.
	/// </summary>
	[CLSCompliant(true)]
	public static class EnumerableExtensions
	{
		/*
			/// <summary>
			/// Uses an internal counter to retrieve an item
			/// of type <typeparamref name="T"/> from
			/// an <see cref="IEnumerable{T}"/> based
			/// on a specified index.
			/// </summary>
			/// <typeparam name="T">
			/// The type of item enumerated by
			/// <paramref name="enumerable"/>.
			/// </typeparam>
			/// <param name="enumerable">
			/// The instance of <see cref="IEnumerable{T}"/>
			/// in which to locate an item by index.
			/// </param>
			/// <param name="index">
			/// The zero-based position of an item
			/// in the underlying colleciton.
			/// </param>
			/// <returns>
			/// The instance of <typeparamref name="T"/>
			/// whose position in the underlying collection
			/// matches <paramref name="index"/>.
			/// </returns>
			[Obsolete("Refactor to the GetItemByIndex(index) method instead.", true)]
			public static T Get<T>(this IEnumerable<T> enumerable, int index)
			{
				// TODO: Remove this obsolete method.
				return GetItemByIndex(enumerable, index);
			}
		*/

		/// <summary>
		/// This method asserts (by throwing exceptions) that there is only one not null item
		/// in the collection and returns it if the criteria is met.
		/// </summary>
		/// <typeparam name="T">The type of item in the collection.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>The first and only not null item in the collection.</returns>
		public static T ValidateOnlyOneItem<T>(this IEnumerable<T> collection)
			where T : class
		{
			collection.ValidateIsNotNullArugment("collection");
			collection = collection.Where(x => x != null);

			T first;
			IEnumerator<T> enumerator = collection.GetEnumerator();

			if (enumerator.MoveNext())
			{
				first = enumerator.Current;
			}
			else
			{
				throw new InvalidOperationException(string.Format("There is no {0} item in the collection.", typeof(T).Name));
			}

			if (enumerator.MoveNext())
			{
				throw new InvalidOperationException(string.Format("There is more than one {0} item in the collection.", typeof(T).Name));
			}

			return first;
		}

		/// <summary>
		/// Uses an internal counter to retrieve an item
		/// of type <typeparamref name="T"/> from
		/// an <see cref="IEnumerable{T}"/> based
		/// on a specified index.
		/// </summary>
		/// <typeparam name="T">
		/// The type of item enumerated by
		/// <paramref name="enumerable"/>.
		/// </typeparam>
		/// <param name="enumerable">
		/// The instance of <see cref="IEnumerable{T}"/>
		/// in which to locate an item by index.
		/// </param>
		/// <param name="index">
		/// The zero-based position of an item
		/// in the underlying colleciton.
		/// </param>
		/// <returns>
		/// The instance of <typeparamref name="T"/>
		/// whose position in the underlying collection
		/// matches <paramref name="index"/>.
		/// </returns>
		public static T GetItemByIndex<T>(this IEnumerable<T> enumerable, int index)
		{
			if (enumerable == null)
			{
				throw new ArgumentNullException("enumerable", "The instance of IEnumerable<T> is a null reference!");
			}

			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "The index must be a positive integer.");
			}

			int count = 0;
			foreach (T item in enumerable)
			{
				if (count == index)
				{
					return item;
				}

				count++;
			}

			throw new IndexOutOfRangeException("The instance of IEnumerable<T> does not enumerate to the specified index.");
		}

		/// <summary>
		/// Splits an <see cref="IEnumerable{T}"/> instance
		/// into several <see cref="IEnumerable{T}"/> instances
		/// of approximately equal length, for programmatically
		/// dividing a list into columns, for instance.
		/// </summary>
		/// <typeparam name="T">
		/// The type of item enumerated by
		/// <paramref name="enumerable"/>.
		/// </typeparam>
		/// <param name="enumerable">
		/// The instance of <see cref="IEnumerable{T}"/>
		/// to split into smaller parts.
		/// </param>
		/// <param name="count">
		/// The number of parts into which to
		/// split <paramref name="enumerable"/>.
		/// </param>
		/// <returns>
		/// A list of <see cref="IEnumerable{T}"/> instances
		/// copied from <paramref name="enumerable"/>.
		/// </returns>
		public static IList<IEnumerable<T>> Split<T>(this IEnumerable<T> enumerable, byte count)
		{
			int totalRecords;
			IList<int> splitTotalRecords;

			return Split(enumerable, count, out totalRecords, out splitTotalRecords);
		}

		/// <summary>
		/// Splits an <see cref="IEnumerable{T}"/> instance
		/// into several <see cref="IEnumerable{T}"/> instances
		/// of approximately equal length, for programmatically
		/// dividing a list into columns, for instance.
		/// </summary>
		/// <typeparam name="T">
		/// The type of item enumerated by
		/// <paramref name="enumerable"/>.
		/// </typeparam>
		/// <param name="enumerable">
		/// The instance of <see cref="IEnumerable{T}"/>
		/// to split into smaller parts.
		/// </param>
		/// <param name="count">
		/// The number of parts into which to
		/// split <paramref name="enumerable"/>.
		/// </param>
		/// <param name="totalRecords">
		/// Returns the total number of items in
		/// <paramref name="enumerable"/>.
		/// </param>
		/// <param name="splitTotalRecords">
		/// Returns the total number of items in each split,
		/// the indicies corresponding to the indicies of
		/// the <see cref="IList{T}"/> returned.
		/// </param>
		/// <returns>
		/// A list of <see cref="IEnumerable{T}"/> instances
		/// copied from <paramref name="enumerable"/>.
		/// </returns>
		public static IList<IEnumerable<T>> Split<T>(
			this IEnumerable<T> enumerable,
			byte count,
			out int totalRecords,
			out IList<int> splitTotalRecords)
		{
			// Verify that the first parameter is okay.
			if (enumerable == null)
			{
				throw new ArgumentNullException("enumerable", "The enumerable instance is a null reference!");
			}

			// Verify that the second parameter is okay.
			if (count < 1)
			{
				throw new ArgumentException("The number of parts for splitting the list must be greater than zero.", "count");
			}

			// Declare a variable allowing access to
			// the number of items in the original list.
			// Use the List implementation of IEnumerable
			// to expose the CopyTo() overload which,
			// includes the starting index, invoked below.
			List<T> initialList = new List<T>(enumerable);

			// Declare variables which will be the basis of the
			// return value, and of an output parameter.
			IList<IEnumerable<T>> returnList = new List<IEnumerable<T>>();
			IList<int> returnTotalRecordsList = new List<int>();

			// Set the value of one of the output parameters.
			totalRecords = initialList.Count;

			// If the original list is empty, return empty values.
			if (!(totalRecords > 0))
			{
				for (int i = 0; i < count; i++)
				{
					returnList.Add(new T[0]);
					returnTotalRecordsList.Add(0);
				}

				splitTotalRecords = new ReadOnlyCollection<int>(returnTotalRecordsList);

				return new ReadOnlyCollection<IEnumerable<T>>(returnList);
			}

			// If not splitting into more than one part,
			// return a list containing the original list.
			if (!(count > 1))
			{
				returnList.Add(initialList);
				returnTotalRecordsList.Add(totalRecords);

				splitTotalRecords = new ReadOnlyCollection<int>(returnTotalRecordsList);

				return new ReadOnlyCollection<IEnumerable<T>>(returnList);
			}

			// Declare local variables which will be used for looping.
			bool remainingListCountDividesEvenly = false;
			int remainingListCount = totalRecords;
			int remainingSplitCount = count;

			// Declare a variable for the number
			// of items to add to each split.
			int splitListCount = 0;

			do
			{
				// First, check whether any more division is necessary.
				if (!remainingListCountDividesEvenly)
				{
					// Calculate the quotient and the remainder
					// from dividing the number of remaining items
					// by the number of remaining parts.
					// Applied to two integers, the division operator will
					// always return an integer, truncating the fractional part.
					// The while condition assures no division by zero.
					int quotient = remainingListCount / remainingSplitCount;
					int remainder = remainingListCount % remainingSplitCount;

					// Check whether the number of remaining items
					// divides evenly by the number of remaining parts.
					if (remainder == 0)
					{
						remainingListCountDividesEvenly = true;
						splitListCount = quotient;
					}
					else
					{
						splitListCount = quotient + 1;
					}
				}

				T[] array = new T[splitListCount];

				initialList.CopyTo(totalRecords - remainingListCount, array, 0, splitListCount);
				returnList.Add(array);
				returnTotalRecordsList.Add(splitListCount);

				remainingListCount = remainingListCount - splitListCount;
				remainingSplitCount = remainingSplitCount - 1;
			}
			while (remainingSplitCount > 0);

			splitTotalRecords = new ReadOnlyCollection<int>(returnTotalRecordsList);

			return new ReadOnlyCollection<IEnumerable<T>>(returnList);
		}
	}
}