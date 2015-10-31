namespace MiscCorLib.Collections
{
	using System;
	using System.Diagnostics.Contracts;

	// TODO: Test JSON Serializability, test ToString();
	// TODO: Override equality operators and GetHashCode.
	[Serializable]
	public struct PagingInfo
	{
		#region [ Immutable Public Fields ]

		public readonly PageNumberAndSize CurrentPage;

		public readonly int FirstItemNumber;

		public readonly PageNumberAndSize FirstPage;

		public readonly bool IsFirstPage;

		public readonly bool IsLastPage;

		public readonly int LastItemNumber;

		public readonly PageNumberAndSize LastPage;

		public readonly PageNumberAndSize NextPage;

		public readonly PageNumberAndSize PreviousPage;

		public readonly int TotalItems;

		public readonly int TotalPages;

		#endregion

		#region [ Constructor Overloads ]

		/// <summary>
		/// 
		/// </summary>
		/// <param name="totalItems">
		/// 
		/// </param>
		public PagingInfo(int totalItems)
			: this(PageNumberAndSize.Unbounded, totalItems)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				(totalItems >= 0), "The number of items in the list must not be negative!");
		}

		public PagingInfo(int pageNumber, byte pageSize, int totalItems)
			: this(new PageNumberAndSize(pageNumber, pageSize), totalItems)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				(pageNumber >= PageNumberAndSize.FirstPageNumber),
				"An ordinal page number is not a zero-based index. The number must be at least one.");

			// Beware of possible division by zero.
			Contract.Requires<ArgumentOutOfRangeException>(
				(pageSize >= PageNumberAndSize.MinimumPageSize),
				"There must be at least one item per page or there could be division by zero!");
			Contract.Requires<ArgumentOutOfRangeException>(
				(totalItems >= 0), "The number of items in the list must not be negative!");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PagingInfo"/> class
		/// starting from a <see cref="PageNumberAndSize"/> value.
		/// </summary>
		/// <param name="requestedPage">
		/// The requested page <see cref="PageNumberAndSize.Number"/>
		/// and <see cref="PageNumberAndSize.Size"/>.
		/// If <see cref="PageNumberAndSize.Unbounded"/> is sent,
		/// all of the items are returned on a single page as large
		/// as the number of <paramref name="totalItems"/>.
		/// </param>
		/// <param name="totalItems">
		/// The total number of items on a paged list,
		/// the initial value of <see cref="TotalItems"/>.
		/// </param>
		public PagingInfo(PageNumberAndSize requestedPage, int totalItems)
		{
			Contract.Requires<ArgumentException>(
				(requestedPage.IsValid), "The current page must have a value. \"Unbounded\" is an acceptable value.");
			Contract.Requires<ArgumentOutOfRangeException>(
				(totalItems >= 0), "The number of items in the list must not be negative!");

			// With code contracts in place, the following generates compiler warnings:
			////	if (totalItems < 0)
			////	{
			////		throw new ArgumentOutOfRangeException(
			////			"totalItems", totalItems, "The number of items in the list must not be negative!");
			////	}

			if (requestedPage.IsUnbounded)
			{
				// This is the case where all of the items are returned on
				// the list, and there is just one unbounded page of items.
				// The total number of items may exceed the maximum allowed
				// value of a byte, so the "page size" value remains as zero.
				// Beware of division by zero!
				// There are no calculations here based on the page size!
				this.CurrentPage = PageNumberAndSize.Unbounded;
				this.TotalItems = totalItems;
				this.TotalPages = 1;
				this.FirstItemNumber = ((totalItems > 0) ? 1 : 0);
				this.LastItemNumber = this.TotalItems;
				this.IsFirstPage = true;
				this.IsLastPage = true;
				this.PreviousPage = PageNumberAndSize.Empty;
				this.NextPage = PageNumberAndSize.Empty;
				this.FirstPage = this.CurrentPage;
				this.LastPage = this.CurrentPage;
			}
			else
			{
				this.CurrentPage = requestedPage;
				this.TotalItems = totalItems;

				if ((this.TotalItems > 0) && (this.CurrentPage.Size > 0))
				{
					// There is no division by zero here, due to validation and logic
					// above and within the PageNumberAndSize struct itself.
					// The method of calculating total pages is shown here:
					// http://stackoverflow.com/questions/17944/how-to-round-up-the-result-of-integer-division
					this.TotalPages = ((this.TotalItems + this.CurrentPage.Size - 1) / this.CurrentPage.Size);

					// Handle the situation if someone turns past the last page.
					if (this.CurrentPage.Number > this.TotalPages)
					{
						// Reset the current page to be the number of the last possible page.
						this.CurrentPage = new PageNumberAndSize(this.TotalPages, this.CurrentPage.Size);
					}

					this.LastItemNumber = (this.CurrentPage.Number * this.CurrentPage.Size);
					this.FirstItemNumber = (this.LastItemNumber - requestedPage.Size + 1);
					this.IsFirstPage = (this.CurrentPage.Number == PageNumberAndSize.FirstPageNumber);
					this.IsLastPage = (this.CurrentPage.Number == this.TotalPages);

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
							(this.CurrentPage.Number - 1), this.CurrentPage.Size);
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
							(this.CurrentPage.Number + 1), this.CurrentPage.Size);
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
					this.IsFirstPage = true;
					this.IsLastPage = true;
					this.PreviousPage = PageNumberAndSize.Empty;
					this.NextPage = PageNumberAndSize.Empty;
					this.FirstPage = this.CurrentPage;
					this.LastPage = this.CurrentPage;
				}
			}
		}

		#endregion

		#region [ Public Read-Only Properties ]

		/// <summary>
		/// Gets the zero-based index of an item within a "paged" collection of items,
		/// equal to the value of <see cref="FirstItemNumber"/> minus one.
		/// </summary>
		public int FirstItemIndex
		{
			get { return (this.FirstItemNumber - 1); }
		}

		/// <summary>
		/// Gets the zero-based index of an item within a "paged" collection of items,
		/// equal to the value of <see cref="LastItemNumber"/> minus one.
		/// </summary>
		public int LastItemIndex
		{
			get { return (this.LastItemNumber - 1); }
		}

		#endregion

		/// <summary>
		/// Calculates a <see cref="PageNumberAndSize"/> for an
		/// arbitrary page <see cref="PageNumberAndSize.Number"/>
		/// using the same page <see cref="PageNumberAndSize.Size"/>.
		/// </summary>
		/// <param name="pageNumber">
		/// The one-based ordinal
		/// <see cref="PageNumberAndSize.Number"/>
		/// of the page to fetch.
		/// </param>
		/// <returns>
		/// A <see cref="PageNumberAndSize"/> value calculated by
		/// using the same <see cref="PageNumberAndSize.Size"/>
		/// as the <see cref="CurrentPage"/> and the page
		/// <see cref="PageNumberAndSize.Number"/> given
		/// as the <paramref name="pageNumber"/> parameter.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Should this operation be closed under the set of
		/// <see cref="PagingInfo"/> operations? In other words,
		/// should this method return a new <see cref="PagingInfo"/>
		/// value instead of a <see cref="PageNumberAndSize"/> value?
		/// </para>
		/// <para>
		/// The reason it should not is that the number of
		/// <see cref="TotalItems"/> cannot be assumed to
		/// remain constant between requests. That number
		/// may have changed between the time that this
		/// page was retrieved and the retrieval of a different
		/// page using the return from this function.
		/// </para>
		/// <para>
		/// That is the same reason for checking the maximum
		/// allowed page number in this function.
		/// The <see cref="PagingInfo"/> constructor gracefully
		/// handles the situation in which a page number
		/// is higher than the total number of pages.
		/// </para>
		/// </remarks>
		public PageNumberAndSize TurnToPage(int pageNumber)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				(pageNumber >= PageNumberAndSize.FirstPageNumber),
				"An ordinal page number is not a zero-based index. The number must be at least one.");

			return new PageNumberAndSize(pageNumber, this.CurrentPage.Size);
		}
	}
}