namespace MiscCorLib.Collections.Generic
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;

	[Serializable]
	public class PagedList<T> : List<T>, IPagedList<T>
	{
		private readonly PagingInfo pagingInfo;

		/// <summary>
		/// Initializes a new instance of the <see cref="PagedList{T}"/> class
		/// that contains elements copied from the specified collection and has
		/// sufficient capacity to accommodate the number of elements copied.
		/// </summary>
		public PagedList(PagingInfo pagingInfo)
		{
			Contract.Requires<ArgumentException>(pagingInfo.CurrentPage.IsValid);

			this.pagingInfo = pagingInfo;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PagedList{T}"/> class
		/// that contains elements copied from the specified collection and has
		/// sufficient capacity to accommodate the number of elements copied.
		/// </summary>
		/// <param name="collection">
		/// The collection whose elements are copied to the new list.
		/// </param>
		/// <param name="pagingInfo">
		/// 
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="collection"/> is null.
		/// </exception>
		public PagedList(IEnumerable<T> collection, PagingInfo pagingInfo)
			: base(collection)
		{
			Contract.Requires<ArgumentNullException>(collection != null);
			Contract.Requires<ArgumentException>(pagingInfo.CurrentPage.IsValid);

			this.pagingInfo = pagingInfo;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PagedList{T}"/> class
		/// that is empty and has the specified initial capacity.
		/// </summary>
		/// <param name="capacity">
		/// The number of elements that the new list can initially store.
		/// </param>
		/// <param name="pagingInfo">
		/// 
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="capacity"/> is less than zero.
		/// </exception>
		public PagedList(int capacity, PagingInfo pagingInfo)
			: base(capacity)
		{
			Contract.Requires<ArgumentOutOfRangeException>(capacity > 0);
			Contract.Requires<ArgumentException>(pagingInfo.CurrentPage.IsValid);

			this.pagingInfo = pagingInfo;
		}

		public PagingInfo PagingInfo { get { return this.pagingInfo; } }
	}
}