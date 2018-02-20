/*
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MiscCorLib.Collections
{
	internal struct PagingStateMetadata
	{
		public readonly int TotalPages;

		public readonly bool IsFirstPage;

		public readonly bool IsLastPage;

		public readonly PageNumberAndItemNumbers PageAndItemNumbers;

		public readonly int ItemCount;

		public PagingStateMetadata(PagingState pagingState)
		{
			if (!pagingState.CurrentPage.HasValue)
			{
				throw new ArgumentException(
					"A valid PagingState value is required. \"Unbounded\" is an acceptable value.",
					nameof(pagingState));
			}

			if (pagingState.CurrentPage.IsUnbounded)
			{
				this.TotalPages = 1;
				this.IsFirstPage = true;
				this.IsLastPage = true;
				this.ItemCount = pagingState.TotalItems;
				this.PageAndItemNumbers = new PageNumberAndItemNumbers(pagingState, true);
			}
			else
			{
				if ((pagingState.TotalItems > 0) && (pagingState.CurrentPage.Size > 0))
				{
					// Calculate the total pages for a fixed page size and at least one result.
					this.TotalPages = PagingCalculator.CalculateTotalPages(
						pagingState.CurrentPage.Size, pagingState.TotalItems);

					// Handle the situation if someone turns past the last page.
					if (pagingState.CurrentPage.Number > this.TotalPages)
					{
						// Reset the current page to be the number of the last possible page.
						pagingState = new PagingState(
							new PageNumberAndSize(this.TotalPages, pagingState.CurrentPage.Size),
							pagingState.TotalItems);
					}

					this.IsFirstPage = pagingState.CurrentPage.Number == PageNumberAndSize.FirstPageNumber;
					this.IsLastPage = pagingState.CurrentPage.Number == this.TotalPages;

					this.PageAndItemNumbers = new PageNumberAndItemNumbers(pagingState, this.IsLastPage);
					this.ItemCount = this.PageAndItemNumbers.LastItemNumber - this.PageAndItemNumbers.FirstItemNumber + 1;
				}
				else
				{
					// This is the case where the count of TotalItems is zero,
					// so reset the page number back to the first page.
					pagingState = new PagingState(
						new PageNumberAndSize(PageNumberAndSize.FirstPageNumber, pagingState.CurrentPage.Size),
						pagingState.TotalItems);

					// There is just one page of results, with no items.
					this.TotalPages = 1;
					this.IsFirstPage = true;
					this.IsLastPage = true;

					this.PageAndItemNumbers = new PageNumberAndItemNumbers(pagingState, true);
					this.ItemCount = 0;
				}
			}
		}
	}
}
*/