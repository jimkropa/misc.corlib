using System;
using System.Collections.Generic;

namespace MiscCorLib.Collections
{
	/// <summary>
	/// An internal value backing the public properties
	/// of the <see cref="PagingState" /> class, for
	/// optimizing deserialization. Calculates and
	/// holds values all based on initial <see cref="PagingState.CurrentPage" />
	/// and <see cref="PagingState.TotalItems" /> values.
	/// </summary>
	/// <remarks>
	/// <para>
	/// To understand how this works, refer to the
	/// <see cref="PagingState.Calculator" /> property
	/// and the <see cref="PagingState(Paging)" />
	/// constructor.
	/// </para>
	/// </remarks>
	public static class Paging
	{
		/// <summary>
		/// Calculates the total number of pages in
		/// a "paged" collection given the number of
		/// items on each page and the total number
		/// of items in the collection.
		/// </summary>
		/// <param name="pageSize">
		/// The <see cref="PageNumberAndSize.Size" />
		/// of each page in a "paged" collection.
		/// Must be greater than zero so that there
		/// is no risk of division by zero.
		/// </param>
		/// <param name="totalItems">
		/// The total number of items in a "paged" collection.
		/// Must be greater than zero, since this method
		/// ought never be invoked for an empty collection.
		/// </param>
		/// <returns>
		/// The total number of pages, approximately
		/// <paramref name="totalItems" /> divided by
		/// <paramref name="pageSize" />, rounding up.
		/// </returns>
		public static int CalculateTotalPages(byte pageSize, int totalItems)
		{
			// Prevent possibility of division by zero.
			if (pageSize < PageNumberAndSize.MinimumPageSize)
			{
				throw new ArgumentOutOfRangeException(
					nameof(pageSize),
					pageSize,
					"There must be at least one item per page or there could be division by zero!");
			}

			if (totalItems < 0)
			{
				throw new ArgumentOutOfRangeException(
					nameof(totalItems),
					totalItems,
					"The number of items in the list must not be negative!");
			}

			// This calculation is part of the calculation of total pages,
			// which will produce an invalid value if the number of total items
			// plus the page size exceeds the capacity of a 32-bit integer.
			int extendedTotalItems = totalItems + pageSize - 1;
			if (extendedTotalItems <= 0)
			{
				throw new OverflowException(
					"There has been an integer overflow in the calculation of paging. Please reduce the number of total items or the page size so that their sum is less than the maximum value of a 32-bit integer.");
			}

			// There is no division by zero here, due to validation and logic
			// above and within the PageNumberAndSize struct itself.
			// The method of calculating total pages is shown here:
			// http://stackoverflow.com/questions/17944/how-to-round-up-the-result-of-integer-division
			return extendedTotalItems / pageSize;
		}

		#region [ Public TurnToPage Method ]

		/// <summary>
		/// Calculates a new <see cref="PagingState" /> for
		/// an arbitrary page paged list's current page, assuming that the
		/// total number of items has not changed since
		/// the .
		/// </summary>
		/// <param name="pagingInfo"></param>
		/// <param name="pageNumber"></param>
		/// <param name="totalPages"></param>
		/// <returns></returns>
		public static PagingInfo TurnToPage(
			this PagingInfo pagingInfo, int pageNumber)
		{
			return pagingInfo.TurnToPage(pageNumber, pagingInfo.TotalItems);
		}

		/// <summary>
		/// Calculates a new <see cref="PagingState" /> for
		/// a paged list's current page and total number of items.
		/// </summary>
		/// <param name="pagingInfo"></param>
		/// <param name="pageNumber"></param>
		/// <param name="totalPages"></param>
		/// <returns></returns>
		public static PagingInfo TurnToPage(
			this PagingInfo pagingInfo, int pageNumber, int totalItems)
		{
			return pagingInfo.State.TurnToPage(pageNumber, totalItems);
		}

		public static PagingInfo TurnToPage(
			this PagingState pagingState, int pageNumber)
		{
			return pagingState.TurnToPage(pageNumber, pagingState.TotalItems);
		}

		public static PagingInfo TurnToPage(
			this PagingState pagingState, int pageNumber, int totalItems)
		{
			return pagingState.CurrentPage.TurnToPage(pageNumber).WithTotalItems(totalItems);
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
				// Return empty if uninitialized.
				return PageNumberAndSize.Empty;
			}

			// Return the unbounded page if the current page is unbounded.
			return currentPage.IsUnbounded ? PageNumberAndSize.Unbounded
				// Otherwise, use the current page size
				// and the given page number.
				: new PageNumberAndSize(pageNumber, currentPage.Size);
		}

		#endregion

		public static PageNumberAndSize UnboundedSinglePage => PageNumberAndSize.Unbounded;

		public static PageNumberAndSize OnPage(int pageNumber, byte pageSize)
		{
			return new PageNumberAndSize(pageNumber, pageSize);
		}

		public static PageNumberAndSize OnPage(int pageNumber)
		{
			return new PageNumberAndSize(pageNumber);
		}

		public static PageNumberAndSize ItemsPerPage(byte pageSize)
		{
			return new PageNumberAndSize(PageNumberAndSize.FirstPageNumber, pageSize);
		}

		public static PageNumberAndSize OnPage(
			this PageNumberAndSize currentPage, byte pageSize)
		{
			return new PageNumberAndSize(currentPage.Number, pageSize);
		}

		public static PageNumberAndSize ItemsPerPage(
			this PageNumberAndSize currentPage, byte pageSize)
		{
			return new PageNumberAndSize(currentPage.Number, pageSize);
		}

		public static PagingInfo WithTotalItems(
			this PageNumberAndSize currentPage, int totalItems)
		{
			return new PagingInfo(
				new PagingState(currentPage, totalItems));
		}

		public static PagingResources CalculatePagingResources(
			this PagingInfo pagingInfo)
		{
			return new PagingResources(pagingInfo);
		}

		public static IEnumerable<PageItemNumbers> CalculateAllPagesAndItemNumbers(
			this PagingInfo pagingInfo)
		{
			// Relay to overload with main implementation:
			return pagingInfo.State.CalculateAllPagesAndItemNumbers();
		}

		public static IReadOnlyDictionary<PageNumberAndSize,PageItemNumbers> CalculateAllPageResources(
			this PagingInfo pagingInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Calculates the full set of page numbers and item numbers
		/// for the <see cref="PagingState.AllPages" /> property of a
		/// given <see cref="PagingState" /> value.
		/// </summary>
		/// <param name="pagingState">
		/// A <see cref="PagingState" /> value from which to gather
		/// page <see cref="PageNumberAndSize.Size" />,
		/// <see cref="PagingState.TotalItems" />, and
		/// <see cref="PagingState.TotalPages" /> values.
		/// </param>
		/// <returns>
		/// The full set of page numbers and item numbers.
		/// </returns>
		internal static IEnumerable<PageItemNumbers> CalculateAllPagesAndItemNumbers(
			this PagingState pagingState)
		{
			return pagingState.CurrentPage.HasValue
				? Paging.CalculateAllPagesAndItemNumbers(pagingState.CurrentPage, pagingState.TotalItems)
				: new PageItemNumbers[0];
		}

		/// <summary>
		/// Calculates the full set of page numbers and item numbers
		/// from parameters relayed by the public static
		/// <see cref="PageItemNumbers.Calculate" />
		/// method of <see cref="PageItemNumbers" />.
		/// </summary>
		/// <param name="pageSize">
		/// The <see cref="PageNumberAndSize.Size" />
		/// of each page in a "paged" collection.
		/// </param>
		/// <param name="totalItems">
		/// The total number of items in a "paged" collection.
		/// </param>
		/// <returns>
		/// The full set of page numbers and item numbers.
		/// </returns>
		private static IEnumerable<PageItemNumbers> CalculateAllPagesAndItemNumbers(
			PageNumberAndSize currentPage, int totalItems)
		{
			if ((currentPage.Size >= PageNumberAndSize.MinimumPageSize)
				&& (totalItems > 0))
			{
				int totalPages = CalculateTotalPages(currentPage.Size, totalItems);
				for (int pageNumber = PageNumberAndSize.FirstPageNumber; pageNumber <= totalPages; pageNumber++)
				{
					yield return new PageItemNumbers(
						pageNumber,
						currentPage.Size,
						totalItems,
						pageNumber == totalPages,
						pageNumber == currentPage.Number);
				}
			}

			// If the paged collection contains no items or
			// is unbounded, return a list with a single item,
			// representing the empty (totalItems == 0)
			// or unbounded (pageSize == 0) page.
			yield return new PageItemNumbers(
				currentPage, totalItems, true, true);
		}
	}
}