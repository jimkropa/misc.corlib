namespace MiscCorLib.Collections
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;

	/// <summary>
	/// An internal value backing the public properties
	/// of the <see cref="PagingInfo"/> class, for
	/// optimizing deserialization. Calculates and
	/// holds values all based on initial <see cref="PagingInfo.CurrentPage"/>
	/// and <see cref="PagingInfo.TotalItems"/> values.
	/// </summary>
	/// <remarks>
	/// <para>
	/// To understand how this works, refer to the
	/// <see cref="PagingInfo.Calculator"/> property
	/// and the <see cref="PagingInfo(PagingInfoCalculator)"/>
	/// constructor.
	/// </para>
	/// </remarks>
	internal struct PagingInfoCalculator
	{
		#region [ Fields and Constructor ]

		public readonly PageNumberAndSize CurrentPage;
		public readonly int TotalItems;

		public readonly int TotalPages;
		public readonly int FirstItemNumber;
		public readonly int LastItemNumber;
		public readonly int ItemCount;
		public readonly PageNumberAndSize NextPage;
		public readonly PageNumberAndSize PreviousPage;
		public readonly PageNumberAndSize FirstPage;
		public readonly PageNumberAndSize LastPage;
		public readonly bool IsFirstPage;
		public readonly bool IsLastPage;

		public readonly IReadOnlyList<PageNumberAndItemNumbers> AllPages;

		/// <summary>
		/// Used by the <see cref="PagingInfo.Calculator"/> property
		/// </summary>
		internal static PagingInfoCalculator Empty = new PagingInfoCalculator();

		/// <summary>
		/// Relays a value sent to the constructor back to the
		/// <see cref="PagingInfo"/> which initialized this value.
		/// </summary>
		internal readonly bool IncludeAllPagesAndItemNumbers;

		/// <summary>
		/// Initializes a new instance of the <see cref="PagingInfoCalculator"/> struct.
		/// Invoked by the private <see cref="PagingInfo.Calculator"/>
		/// property of an owner <see cref="PagingInfo"/> value,
		/// calculates metadata for paging UI, optionally including
		/// a list of all pages and item numbers. For effective lazy
		/// initialization following deserialization from bare essentials.
		/// </summary>
		/// <param name="currentPage">
		/// The page <see cref="PageNumberAndSize.Number"/>
		/// and <see cref="PageNumberAndSize.Size"/>.
		/// If <see cref="PageNumberAndSize.Unbounded"/> is sent,
		/// all of the items are returned on a single page as large
		/// as the number of <paramref name="totalItems"/>.
		/// </param>
		/// <param name="totalItems">
		/// The total number of items in the collection to be paged,
		/// initial value for the immutable <see cref="TotalItems"/> field.
		/// </param>
		/// <param name="includeAllPagesAndItemNumbers">
		/// Whether to fill the set of <see cref="AllPages"/>
		/// including the item numbers on each page,
		/// which may be useful for some paging UI.
		/// Relayed back to the <see cref="PagingInfo"/> via the
		/// <see cref="IncludeAllPagesAndItemNumbers"/> field,
		/// adds the private <see cref="PagingInfo.AllPages"/>
		/// property to the serialization output for JSON.
		/// </param>
		internal PagingInfoCalculator(
			PageNumberAndSize currentPage, int totalItems, bool includeAllPagesAndItemNumbers)
		{
			Contract.Requires<ArgumentException>(
				currentPage.HasValue, "The current page must have a value. \"Unbounded\" is an acceptable value.");
			Contract.Requires<ArgumentOutOfRangeException>(
				totalItems >= 0, "The number of items in the list must not be negative!");

			this.IncludeAllPagesAndItemNumbers = includeAllPagesAndItemNumbers;
			this.CurrentPage = currentPage;
			this.TotalItems = totalItems;

			if (currentPage.IsUnbounded)
			{
				// This is the case where all of the items are returned on
				// the list, and there is just one unbounded page of items.
				// The total number of items may exceed the maximum allowed
				// value of a byte, so the "page size" value remains as zero.
				// Beware of division by zero!
				// There are no calculations here based on the page size!
				this.TotalPages = 1;
				this.CurrentPage = PageNumberAndSize.Unbounded;
				this.FirstItemNumber = this.TotalItems > 0 ? 1 : 0;
				this.LastItemNumber = this.TotalItems;
				this.ItemCount = this.TotalItems;
				this.IsFirstPage = true;
				this.IsLastPage = true;
				this.PreviousPage = PageNumberAndSize.Empty;
				this.NextPage = PageNumberAndSize.Empty;
				this.FirstPage = this.CurrentPage;
				this.LastPage = this.CurrentPage;
			}
			else
			{
				this.CurrentPage = currentPage;

				if ((this.TotalItems > 0) && (this.CurrentPage.Size > 0))
				{
					// Calculate the total pages for a fixed page size and at least one result.
					this.TotalPages = CalculateTotalPages(this.CurrentPage.Size, this.TotalItems);

					// Handle the situation if someone turns past the last page.
					if (this.CurrentPage.Number > this.TotalPages)
					{
						// Reset the current page to be the number of the last possible page.
						this.CurrentPage = new PageNumberAndSize(this.TotalPages, this.CurrentPage.Size);
					}

					this.LastItemNumber = this.CurrentPage.Number * this.CurrentPage.Size;
					this.FirstItemNumber = this.LastItemNumber - this.CurrentPage.Size + 1;
					this.ItemCount = this.LastItemNumber - this.FirstItemNumber + 1;
					this.IsFirstPage = this.CurrentPage.Number == PageNumberAndSize.FirstPageNumber;
					this.IsLastPage = this.CurrentPage.Number == this.TotalPages;

					if (this.IsFirstPage)
					{
						this.FirstPage = this.CurrentPage;
						this.PreviousPage = PageNumberAndSize.Empty;
					}
					else
					{
						this.FirstPage = new PageNumberAndSize(
							PageNumberAndSize.FirstPageNumber, this.CurrentPage.Size);

						this.PreviousPage = new PageNumberAndSize(
							this.CurrentPage.Number - 1, this.CurrentPage.Size);
					}

					if (this.IsLastPage)
					{
						this.LastPage = this.CurrentPage;
						this.NextPage = PageNumberAndSize.Empty;

						// The number of items shown on the last page
						// may be smaller than the number of items per page.
						this.LastItemNumber = this.TotalItems;
					}
					else
					{
						this.LastPage = new PageNumberAndSize(
							this.TotalPages, this.CurrentPage.Size);

						this.NextPage = new PageNumberAndSize(
							this.CurrentPage.Number + 1, this.CurrentPage.Size);
					}
				}
				else
				{
					// This is the case where the count of TotalItems is zero,
					// so reset the page number back to the first page.
					this.CurrentPage = new PageNumberAndSize(
						PageNumberAndSize.FirstPageNumber, this.CurrentPage.Size);

					// There is just one page of results, with no items.
					this.TotalPages = 1;
					this.FirstItemNumber = 0;
					this.LastItemNumber = 0;
					this.ItemCount = 0;
					this.IsFirstPage = true;
					this.IsLastPage = true;
					this.PreviousPage = PageNumberAndSize.Empty;
					this.NextPage = PageNumberAndSize.Empty;
					this.FirstPage = this.CurrentPage;
					this.LastPage = this.CurrentPage;
				}
			}

			this.AllPages = this.IncludeAllPagesAndItemNumbers
				? AllPagesAndItemNumbers(this.CurrentPage.Size, this.TotalItems, this.TotalPages)
				: null;
		}

		#endregion

		#region [ Static Methods ]

		/// <summary>
		/// Calculates the total number of pages in
		/// a "paged" collection given the number of
		/// items on each page and the total number
		/// of items in the collection.
		/// </summary>
		/// <param name="pageSize">
		/// The <see cref="PageNumberAndSize.Size"/>
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
		/// <paramref name="totalItems"/> divided by
		/// <paramref name="pageSize"/>, rounding up.
		/// </returns>
		internal static int CalculateTotalPages(byte pageSize, int totalItems)
		{
			Contract.Requires<ArgumentOutOfRangeException>(pageSize >= PageNumberAndSize.MinimumPageSize);
			Contract.Requires<ArgumentOutOfRangeException>(totalItems > 0);

			// This calculation is part of the calculation of total pages,
			// which will produce an invalid value if the number of total items
			// plus the page size exceeds the capacity of a 32-bit integer.
			int extendedTotalItems = totalItems + pageSize - 1;

			// CodeContracts gives a warning here, but that's
			// expected in this extraordinary situation.
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

		/// <summary>
		/// Calculates the full set of page numbers and item numbers
		/// from parameters relayed by the public static
		/// <see cref="PageNumberAndItemNumbers.Calculate"/>
		/// method of <see cref="PageNumberAndItemNumbers"/>.
		/// </summary>
		/// <param name="pageSize">
		/// The <see cref="PageNumberAndSize.Size"/>
		/// of each page in a "paged" collection.
		/// </param>
		/// <param name="totalItems">
		/// The total number of items in a "paged" collection.
		/// </param>
		/// <returns>
		/// The full set of page numbers and item numbers.
		/// </returns>
		internal static IReadOnlyList<PageNumberAndItemNumbers> AllPagesAndItemNumbers(
			byte pageSize, int totalItems)
		{
			Contract.Requires<ArgumentOutOfRangeException>(totalItems >= 0);

			if ((pageSize >= PageNumberAndSize.MinimumPageSize) && (totalItems > 0))
			{
				return AllPagesAndItemNumbers(
					pageSize, totalItems, CalculateTotalPages(pageSize, totalItems));
			}

			// If the paged collection contains no items or
			// is unbounded, return a list with a single item,
			// representing the empty or unbounded page.
			return new List<PageNumberAndItemNumbers>
			{
				new PageNumberAndItemNumbers(
					PageNumberAndSize.FirstPageNumber, pageSize, totalItems)
			};
		}

		/// <summary>
		/// Calculates the full set of page numbers and item numbers
		/// for the <see cref="PagingInfo.AllPages"/> property of a
		/// given <see cref="PagingInfo"/> value.
		/// </summary>
		/// <param name="pagingInfo">
		/// A <see cref="PagingInfo"/> value from which to gather
		/// page <see cref="PageNumberAndSize.Size"/>,
		/// <see cref="PagingInfo.TotalItems"/>, and
		/// <see cref="PagingInfo.TotalPages"/> values.
		/// </param>
		/// <returns>
		/// The full set of page numbers and item numbers.
		/// </returns>
		internal static IReadOnlyList<PageNumberAndItemNumbers> AllPagesAndItemNumbers(
			PagingInfo pagingInfo)
		{
			Contract.Requires<ArgumentException>(pagingInfo.HasValue);

			return AllPagesAndItemNumbers(
				pagingInfo.CurrentPage.Size, pagingInfo.TotalItems, pagingInfo.TotalPages);
		}

		/// <summary>
		/// Private overload shared by the two internal overloads,
		/// assumes that <paramref name="totalPages"/> value
		/// is correct, having already been calculated by the
		/// <see cref="CalculateTotalPages"/> method.
		/// </summary>
		/// <param name="pageSize">
		/// The <see cref="PageNumberAndSize.Size"/>
		/// of each page in a "paged" collection.
		/// </param>
		/// <param name="totalItems">
		/// The total number of items in a "paged" collection,
		/// corresponding to the <see cref="PagingInfo.TotalItems"/>
		/// of a <see cref="PagingInfo"/> value.
		/// </param>
		/// <param name="totalPages">
		/// The total number of items in a "paged" collection,
		/// corresponding to the <see cref="PagingInfo.TotalPages"/>
		/// of a <see cref="PagingInfo"/> value, or otherwise calculated
		/// by the <see cref="CalculateTotalPages"/> method.
		/// </param>
		/// <returns>
		/// The full set of page numbers and item numbers.
		/// </returns>
		private static IReadOnlyList<PageNumberAndItemNumbers> AllPagesAndItemNumbers(
			byte pageSize, int totalItems, int totalPages)
		{
			Contract.Requires<ArgumentOutOfRangeException>(totalItems >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(totalPages >= 0);

			// ReSharper disable once InvertIf
			if ((pageSize >= PageNumberAndSize.MinimumPageSize)
				&& (totalItems > 0) && (totalPages > 0))
			{
				// Assume that the "totalPages" value is correct,
				// having been calculated by the "CalculateTotalPages"
				// function by some private operation within the calculator.
				// That is, the only routes here are through "CalculateTotalPages"
				List<PageNumberAndItemNumbers> list = new List<PageNumberAndItemNumbers>();
				for (int pageNumber = PageNumberAndSize.FirstPageNumber; pageNumber <= totalPages; pageNumber++)
				{
					list.Add(new PageNumberAndItemNumbers(pageNumber, pageSize, totalItems, pageNumber == totalPages));
				}

				return list;
			}

			// If the paged collection contains no items or
			// is unbounded, return a list with a single item,
			// representing the empty (totalItems == 0)
			// or unbounded (pageSize == 0) page.
			return new List<PageNumberAndItemNumbers>
			{
				new PageNumberAndItemNumbers(
					PageNumberAndSize.FirstPageNumber, pageSize, totalItems)
			};
		}

		#endregion
	}
}