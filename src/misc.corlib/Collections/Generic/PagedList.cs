using System;
using System.Collections.Generic;

namespace MiscCorLib.Collections.Generic
{
	/// <summary>
	/// Default implementation of <see cref="IPagedList{T}" />,
	/// derived from the base generic <see cref="List{T}" />
	/// and adding the <see cref="PagingState" /> property.
	/// A collection of elements that can be accessed by index,
	/// and which are grouped onto one "page" in a longer list
	/// of items which is broken into multiple pages.
	/// </summary>
	/// <typeparam name="T">
	/// The type of elements in the paged list.
	/// </typeparam>
	[Serializable]
	public class PagedList<T> : List<T>, IPagedList<T>
	{
		#region [ Private ReadOnly Field and Constructor Overloads ]

		/// <summary>
		/// Private backing field for the public
		/// <see cref="PagingState" /> property.
		/// </summary>
		private readonly PagingState pagingState;

		[NonSerialized]
		private readonly PagingInfo pagingInfo;

		/// <summary>
		/// Initializes a new instance of the <see cref="PagedList{T}" /> class
		/// that contains elements copied from the specified collection and has
		/// sufficient capacity to accommodate the number of elements copied.
		/// </summary>
		/// <param name="collection">
		/// The collection whose elements are copied to the new list.
		/// </param>
		/// <param name="pagingState">
		/// Metadata about this "page" of a longer list which spans multiple pages.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="collection" /> is null.
		/// </exception>
		public PagedList(IEnumerable<T> collection, PagingState pagingState)
			: base(collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException(nameof(collection));
			}

			this.pagingInfo = new PagingInfo(pagingState);
			if (this.Count != pagingInfo.ItemCount)
			{
				throw new ArgumentException(
					$"The number of items in the given collection ({this.Count}) does not match the expected number of items for the current page ({pagingInfo.ItemCount}) based on the PagingState value.");
			}

			this.pagingState = pagingState;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PagedList{T}" /> class
		/// that is empty and has the specified initial capacity.
		/// </summary>
		/// <param name="capacity">
		/// The number of elements that the new list can initially store.
		/// </param>
		/// <param name="pagingState">
		/// Metadata about this "page" of a longer list which spans multiple pages.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="capacity" /> is less than zero.
		/// </exception>
		protected PagedList(int capacity, PagingState pagingState)
			: base(capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Capacity must not be negative.");
			}

			this.pagingInfo = new PagingInfo(pagingState);
			if (capacity != pagingInfo.ItemCount)
			{
				throw new ArgumentException(
					$"The given capacity ({capacity}) does not match the expected number of items for the current page ({pagingInfo.ItemCount}) based on the PagingInfo value.");
			}

			this.pagingState = pagingState;
		}

		#endregion

		/// <inheritdoc />
		public PagingState PagingState => this.pagingState;

		/// <inheritdoc />
		public PagingInfo PagingInfo => this.pagingInfo;
	}
}