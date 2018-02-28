using System;
using System.Collections.Generic;

namespace MiscCorLib.Collections.Paged
{
	// Find documentation in main partial class: Paging.cs
	// This part contains extension methods for PageNumberAndSize values.
	public static partial class Paging
	{
		/// <summary>
		/// Calculates <see cref="PagingInfo" />
		/// based on page number, page size,
		/// and total items on the paged list.
		/// </summary>
		public static PagingInfo WithTotalItems(
			this PageNumberAndSize currentPage, int totalItems)
		{
			if (!currentPage.HasValue)
			{
				return PagingInfo.Empty;
			}

			// Always calculate PagingInfo to
			// ensure a valid page number for the
			// given page size and total items.
			return new PagingInfo(
				new PagingState(currentPage, totalItems));
		}

		/// <summary>
		/// From a given <see cref="PageNumberAndSize" />,
		/// returns a copy with the same page number
		/// and a specified number of items per page.
		/// </summary>
		/// <remarks>
		/// Works from <see cref="PageNumberAndSize.Empty" />,
		/// starts on page one.
		/// </remarks>
		public static PageNumberAndSize ItemsPerPage(
			this PageNumberAndSize currentPage, byte pageSize)
		{
			return new PageNumberAndSize(
				currentPage.Number > 0
					? currentPage.Number
					: PageNumberAndSize.PageOne,
				pageSize);
		}

		/// <summary>
		/// From a given <see cref="PageNumberAndSize" />,
		/// returns a copy with the same page size
		/// and a specified page number.
		/// </summary>
		/// <remarks>
		/// Works from <see cref="PageNumberAndSize.Empty" />,
		/// assuming <see cref="PageNumberAndSize.DefaultPageSize" />.
		/// </remarks>
		public static PageNumberAndSize OnPage(
			this PageNumberAndSize currentPage, int pageNumber)
		{
			return new PageNumberAndSize(
				pageNumber,
				currentPage.Size > 0
					? currentPage.Size
					: PageNumberAndSize.DefaultPageSize);
		}
	}
}