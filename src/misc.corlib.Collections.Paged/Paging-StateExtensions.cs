using System;
using System.Collections.Generic;

namespace MiscCorLib.Collections.Paged
{
	// Find documentation in main partial class: Paging.cs
	// This part contains extension methods for PageNumberAndSize values.
	public static partial class Paging
	{
		/// <summary>
		/// From a given <see cref="PagingInfo" />,
		/// returns a copy with the same page number and size,
		/// and a specified number of total items on the list.
		/// </summary>
		public static PagingInfo WithTotalItems(
			this PagingInfo pagingInfo, int totalItems)
		{
			return pagingInfo.IsUnbounded
					? PageNumberAndSize.Unbounded.WithTotalItems(totalItems)
					: OnPage(pagingInfo.PageNumber > 0
							? pagingInfo.PageNumber
							: PageNumberAndSize.PageOne,
						pagingInfo.PageSize).WithTotalItems(totalItems);
		}

		/// <summary>
		/// From a given <see cref="PagingInfo" />,
		/// returns the first page of a paged list with the
		/// given number of items per page, optionally assuming
		/// the same total number of items on the list.
		/// </summary>
		public static PagingInfo WithItemsPerPage(
			this PagingInfo pagingInfo, byte pageSize, int? totalItems = null)
		{
			return OnPage(PageNumberAndSize.PageOne, pageSize)
					.WithTotalItems(totalItems ?? pagingInfo.TotalItems);
		}
	}
}