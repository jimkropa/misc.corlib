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
		/// and a specified number of items on the list.
		/// </summary>
		public static PagingInfo WithTotalItems(
			this PagingInfo currentPage, int totalItems)
		{
			return new PagingInfo(
				new PagingState(currentPage.State.CurrentPage, totalItems));
		}

		/// <summary>
		/// From a given <see cref="PagingInfo" />,
		/// returns a copy with the same page number
		/// and a specified number of items per page.
		/// </summary>
		public static PagingInfo WithItemsPerPage(
			this PagingInfo currentPage, byte pageSize)
		{
			return currentPage.WithItemsPerPage(pageSize, currentPage.TotalItems);
		}

		/// <summary>
		/// From a given <see cref="PagingInfo" />,
		/// returns a copy with the same page number
		/// and a specified number of items per page.
		/// </summary>
		public static PagingInfo WithItemsPerPage(
			this PagingInfo currentPage, byte pageSize, int totalItems)
		{
			return new PagingInfo(
				new PagingState(currentPage.State.CurrentPage.ItemsPerPage(pageSize), totalItems));
		}
	}
}