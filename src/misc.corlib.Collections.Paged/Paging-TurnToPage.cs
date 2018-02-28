using System;
using System.Collections.Generic;

namespace MiscCorLib.Collections.Paged
{
	// Find documentation in main partial class: Paging.cs
	// This part contains overloads of a "TurnToPage" method.
	public static partial class Paging
	{
		/// <summary>
		/// Calculates a new <see cref="PagingInfo" /> for
		/// an arbitrary page on a paged list, optionally assuming
		/// the same total number of items on the list.
		/// </summary>
		public static PagingInfo TurnToPage(
			this PagingInfo pagingInfo, int pageNumber, int? totalItems = null)
		{
			return pagingInfo.State.TurnToPage(pageNumber, totalItems);
		}

		/// <summary>
		/// Calculates a new <see cref="PagingInfo" /> for
		/// an arbitrary page on a paged list, optionally assuming
		/// the same total number of items on the list.
		/// </summary>
		public static PagingInfo TurnToPage(
			this PagingState pagingState, int pageNumber, int? totalItems = null)
		{
			if (!pagingState.HasValue)
			{
				return PagingInfo.Empty;
			}

			return pagingState.CurrentPage.TurnToPage(pageNumber).WithTotalItems(totalItems ?? pagingState.TotalItems);
		}

		/// <summary>
		/// Calculates a <see cref="PageNumberAndSize" /> for an
		/// arbitrary page <see cref="PageNumberAndSize.Number" />
		/// with the same <see cref="PageNumberAndSize.Size" /> of page.
		/// </summary>
		/// <param name="pageNumber">
		/// The one-based ordinal
		/// <see cref="PageNumberAndSize.Number" />
		/// of the page to fetch.
		/// </param>
		/// <returns>
		/// A <see cref="PageNumberAndSize" /> value calculated by
		/// using the same <see cref="PageNumberAndSize.Size" />
		/// as the <see cref="CurrentPage" /> and the page
		/// <see cref="PageNumberAndSize.Number" /> given
		/// as the <paramref name="pageNumber" /> parameter.
		/// </returns>
		public static PageNumberAndSize TurnToPage(
			this PageNumberAndSize currentPage, int pageNumber)
		{
			if (!currentPage.HasValue)
			{
				return PageNumberAndSize.Empty;
			}

			// Return the unbounded page if the current page is unbounded.
			return currentPage.IsUnbounded ? PageNumberAndSize.Unbounded
				// Otherwise, use the current page size
				// and the given page number.
				: new PageNumberAndSize(pageNumber, currentPage.Size);
		}
	}
}