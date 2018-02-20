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
	/// and the <see cref="PagingState(PagingCalculator)" />
	/// constructor.
	/// </para>
	/// </remarks>
	public static class PagingCalculator
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

		public static PagingState TurnToPage(
			this PagingInfo pagingInfo, int pageNumber)
		{
			return pagingInfo.State.TurnToPage(pageNumber);
		}

		public static PagingState TurnToPage(this PagingState pagingState, int pageNumber)
		{
			return pagingState.CurrentPage.TurnToPage(pageNumber).ToPagingState(pagingState.TotalItems);
		}

		/// <summary>
		/// Calculates a <see cref="PageNumberAndSize" /> for an
		/// arbitrary page <see cref="PageNumberAndSize.Number" />
		/// using the same page <see cref="PageNumberAndSize.Size" />.
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
		/// <remarks>
		/// <para>
		/// Should this operation be closed under the set of
		/// <see cref="PagingState" /> operations? In other words,
		/// should this method return a new <see cref="PagingState" />
		/// value instead of a <see cref="PageNumberAndSize" /> value?
		/// </para>
		/// <para>
		/// The reason it should not is that the number of
		/// <see cref="TotalItems" /> cannot be assumed to
		/// remain constant between requests. That number
		/// may have changed between the time that this
		/// page was retrieved and the retrieval of a different
		/// page using the return from this function.
		/// </para>
		/// <para>
		/// That is the same reason for checking the maximum
		/// allowed page number in this function.
		/// The <see cref="PagingState" /> constructor gracefully
		/// handles the situation in which a page number
		/// is higher than the total number of pages.
		/// </para>
		/// </remarks>
		public static PageNumberAndSize TurnToPage(this PageNumberAndSize currentPage, int pageNumber)
		{
			if (currentPage.HasValue)
			{
				// Always return the unbounded page if the current page is unbounded.
				return currentPage.IsUnbounded ? PageNumberAndSize.Unbounded
					: new PageNumberAndSize(pageNumber, currentPage.Size);
			}

			// Return empty if uninitialized.
			return PageNumberAndSize.Empty;
		}

		#endregion

		public static PagingInfo ToPagingInfo(this PagingState pagingState)
		{
			return new PagingInfo(pagingState);
		}

		public static PagingState ToPagingState(
			this PageNumberAndSize currentPage, int totalItems)
		{
			return new PagingState(currentPage, totalItems);
		}

		public static PagingResources CalculatePagingResources(
			this PagingInfo pagingInfo)
		{
			return new PagingResources(pagingInfo);
		}

		public static IEnumerable<PageNumberAndItemNumbers> CalculateAllPagesAndItemNumbers(
			this PagingInfo pagingInfo)
		{
			return pagingInfo.State.CalculateAllPagesAndItemNumbers();
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
		public static IEnumerable<PageNumberAndItemNumbers> CalculateAllPagesAndItemNumbers(
			this PagingState pagingState)
		{
			return pagingState.CurrentPage.HasValue
				? PagingCalculator.CalculateAllPagesAndItemNumbers(pagingState.CurrentPage, pagingState.TotalItems)
				: new PageNumberAndItemNumbers[0];
		}

		/// <summary>
		/// Calculates the full set of page numbers and item numbers
		/// from parameters relayed by the public static
		/// <see cref="PageNumberAndItemNumbers.Calculate" />
		/// method of <see cref="PageNumberAndItemNumbers" />.
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
		private static IEnumerable<PageNumberAndItemNumbers> CalculateAllPagesAndItemNumbers(
			PageNumberAndSize currentPage, int totalItems)
		{
			if ((currentPage.Size >= PageNumberAndSize.MinimumPageSize)
				&& (totalItems > 0))
			{
				int totalPages = CalculateTotalPages(currentPage.Size, totalItems);
				for (int pageNumber = PageNumberAndSize.FirstPageNumber; pageNumber <= totalPages; pageNumber++)
				{
					yield return new PageNumberAndItemNumbers(
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
			yield return new PageNumberAndItemNumbers(
				currentPage, totalItems, true, true);
		}
	}
}