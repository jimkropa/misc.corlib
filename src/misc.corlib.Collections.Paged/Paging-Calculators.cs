using System;
using System.Collections.Generic;

namespace MiscCorLib.Collections.Paged
{
	// Find documentation in main partial class: Paging.cs
	//
	// This part contains calculators of item numbers
	// and resources to move through a paged list.
	public static partial class Paging
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

		public static PagingResources CalculatePagingResources(
			this PagingInfo pagingInfo)
		{
			if (!pagingInfo.HasValue)
			{
				return PagingResources.Empty;
			}

			return new PagingResources(pagingInfo);
		}

		public static IReadOnlyDictionary<PageNumberAndSize,PageItemNumbers> CalculateAllPageResources(
			this PagingInfo pagingInfo)
		{
			if (!pagingInfo.HasValue)
			{
				return new Dictionary<PageNumberAndSize,PageItemNumbers>();
			}

			throw new NotImplementedException();
		}

		public static IEnumerable<PageItemNumbers> CalculateAllPagesAndItemNumbers(
			this PagingInfo pagingInfo)
		{
			// Relay to overload with main implementation:
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
			if (currentPage.Size >= PageNumberAndSize.MinimumPageSize && totalItems > 0)
			{
				int totalPages = CalculateTotalPages(currentPage.Size, totalItems);
				for (int pageNumber = PageNumberAndSize.PageOne; pageNumber <= totalPages; pageNumber++)
				{
					yield return new PageItemNumbers(
						pageNumber,
						currentPage.Size,
						totalItems,
						pageNumber == currentPage.Number,
						pageNumber == totalPages);
				}
			}

			// If the paged collection contains no items or
			// is unbounded, return a list with a single item,
			// representing the empty (totalItems == 0)
			// or unbounded (pageSize == 0) page.
			yield return new PageItemNumbers(
				currentPage, totalItems, totalItems > 0, true);
		}
	}
}