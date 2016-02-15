﻿namespace MiscCorLib.Collections
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;

	/// <summary>
	/// An internal value backing the public properties
	/// of the <see cref="PagingInfo"/> class,
	/// for optimizing deserialization.
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
		internal static PagingInfoCalculator Empty = new PagingInfoCalculator();

		internal readonly bool CalculateAllPagesAndItemNumbers;

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

		public readonly IReadOnlyList<PageNumberAndItemNumbers> Pages;

		internal PagingInfoCalculator(PageNumberAndSize currentPage, int totalItems, bool calculateAllPagesAndItemNumbers)
		{
			Contract.Requires<ArgumentException>(
				currentPage.HasValue, "The current page must have a value. \"Unbounded\" is an acceptable value.");
			Contract.Requires<ArgumentOutOfRangeException>(
				totalItems >= 0, "The number of items in the list must not be negative!");

			this.CalculateAllPagesAndItemNumbers = calculateAllPagesAndItemNumbers;
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
				this.CurrentPage = PageNumberAndSize.Unbounded;
				this.TotalPages = 1;
				this.FirstItemNumber = this.TotalItems > 0 ? 1 : 0;
				this.LastItemNumber = this.TotalItems;
				this.ItemCount = this.TotalItems;
				this.IsFirstPage = true;
				this.IsLastPage = true;
				this.PreviousPage = PageNumberAndSize.Empty;
				this.NextPage = PageNumberAndSize.Empty;
				this.FirstPage = this.CurrentPage;
				this.LastPage = this.CurrentPage;

				if (this.CalculateAllPagesAndItemNumbers)
				{
					
				}
				else
				{
					this.Pages = null;
				}
			}
			else
			{
				this.CurrentPage = currentPage;

				if ((this.TotalItems > 0) && (this.CurrentPage.Size > 0))
				{


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

					this.Pages = CreatePageItemNumbersList(this.CurrentPage.Size, this.TotalPages, this.TotalItems);
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
					this.Pages = new List<PageNumberAndItemNumbers>
					{
						new PageNumberAndItemNumbers(
							this.CurrentPage.Number, this.CurrentPage.Size, this.TotalItems)
					};
				}
			}
		}

		private static IReadOnlyList<PageNumberAndItemNumbers> AllPagesAndItemNumbers(
			byte pageSize, int totalItems, int totalPages)
		{
			Contract.Requires<ArgumentOutOfRangeException>(pageSize >= PageNumberAndSize.MinimumPageSize);
			Contract.Requires<ArgumentOutOfRangeException>(totalItems > 0);
			Contract.Requires<ArgumentOutOfRangeException>(totalPages > 0);

			List<PageNumberAndItemNumbers> list = new List<PageNumberAndItemNumbers>();
			for (int pageNumber = PageNumberAndSize.FirstPageNumber; pageNumber <= totalPages; pageNumber++)
			{
				list.Add(new PageNumberAndItemNumbers(pageNumber, pageSize, totalItems));
			}

			return list;
		}

		internal static IReadOnlyList<PageNumberAndItemNumbers> AllPagesAndItemNumbers(
			byte pageSize, int totalItems)
		{
			if ((pageSize >= PageNumberAndSize.MinimumPageSize) && (totalItems > 0))
			{
				return AllPagesAndItemNumbers(pageSize, totalItems, CalculateTotalPages(pageSize, totalItems));
			}

			if (pageSize >= PageNumberAndSize.MinimumPageSize)
			{
				// This is an empty list of a fixed size.
				return new List<PageNumberAndItemNumbers>
				{
					new PageNumberAndItemNumbers(
						PageNumberAndSize.FirstPageNumber, pageSize, totalItems)
				};
			}

			// The one unbounded
			return new List<PageNumberAndItemNumbers>
					{
						new PageNumberAndItemNumbers(
							this.CurrentPage.Number, this.CurrentPage.Size, this.TotalItems)
					};
		}


		internal static IReadOnlyList<PageNumberAndItemNumbers> AllPagesAndItemNumbers(
			PagingInfo pagingInfo)
		{
			return AllPagesAndItemNumbers(pagingInfo.CurrentPage.Size, pagingInfo.TotalItems);
		}

		internal static int CalculateTotalPages(byte pageSize, int totalItems)
		{
			Contract.Requires<ArgumentOutOfRangeException>(pageSize >= PageNumberAndSize.MinimumPageSize);
			Contract.Requires<ArgumentOutOfRangeException>(totalItems >= 0);

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
	}
}