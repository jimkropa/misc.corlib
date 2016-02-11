namespace MiscCorLib.Collections
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Runtime.Serialization;

	// TODO: Test JSON Serializability, test ToString();
	// TODO: Override equality operators and GetHashCode.
	[CLSCompliant(true), Serializable, DataContract]
	public struct PagingInfo
	{
		#region [ Immutable Public Fields ]

		[NonSerialized]
		private readonly PagingInfoCalculator calculator;

		private PagingInfoCalculator Calculator
		{
			get
			{
				if (this.calculator.CurrentPage.IsValid)
				{
					return this.calculator;
				}

				if (!this.CurrentPage.IsValid)
				{
					return PagingInfoCalculator.Empty;
				}

				// Some ridiculous sleight-of-hand for the sake
				// of optimizing serialization and deserialization.
				// When deserialized, only the CurrentPage and
				// TotalItems are required, then other values
				// are calculated once into a "state" object.
				PagingInfoCalculator newCalculator = new PagingInfoCalculator(this.CurrentPage, this.TotalItems);
				this = new PagingInfo(newCalculator);
				return newCalculator;
			}
		}

		[DataMember(IsRequired = true, Order = 0)]
		public readonly PageNumberAndSize CurrentPage;

		[DataMember(IsRequired = true, Order = 1)]
		public readonly int TotalItems;

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
				totalItems >= 0, "The number of items in the list must not be negative!");
		}

		public PagingInfo(int pageNumber, byte pageSize, int totalItems)
			: this(new PageNumberAndSize(pageNumber, pageSize), totalItems)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				pageNumber >= PageNumberAndSize.FirstPageNumber,
				"An ordinal page number is not a zero-based index. The number must be at least one.");

			// Beware of possible division by zero.
			Contract.Requires<ArgumentOutOfRangeException>(
				pageSize >= PageNumberAndSize.MinimumPageSize,
				"There must be at least one item per page or there could be division by zero!");
			Contract.Requires<ArgumentOutOfRangeException>(
				totalItems >= 0, "The number of items in the list must not be negative!");
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
			: this(new PagingInfoCalculator(requestedPage, totalItems))
		{
			Contract.Requires<ArgumentException>(
				requestedPage.IsValid, "The current page must have a value. \"Unbounded\" is an acceptable value.");
			Contract.Requires<ArgumentOutOfRangeException>(
				totalItems >= 0, "The number of items in the list must not be negative!");
		}

		private PagingInfo(PagingInfoCalculator calculator)
		{
			this.CurrentPage = calculator.CurrentPage;
			this.TotalItems = calculator.TotalItems;
			this.calculator = calculator;
		}

		#endregion

		#region [ Public Read-Only Properties ]

		[DataMember(IsRequired = false, Order = 2)]
		public int TotalPages { get { return this.Calculator.TotalPages; } }

		[DataMember(IsRequired = false, Order = 3)]
		public int FirstItemNumber { get { return this.Calculator.FirstItemNumber; } }

		[DataMember(IsRequired = false, Order = 4)]
		public int LastItemNumber { get { return this.Calculator.LastItemNumber; } }

		/// <summary>
		/// Gets the zero-based index of an item within a "paged" collection of items,
		/// equal to the value of <see cref="FirstItemNumber"/> minus one.
		/// </summary>
		[DataMember(IsRequired = false, Order = 5)]
		public int FirstItemIndex
		{
			get { return this.FirstItemNumber - 1; }
		}

		/// <summary>
		/// Gets the zero-based index of an item within a "paged" collection of items,
		/// equal to the value of <see cref="LastItemNumber"/> minus one.
		/// </summary>
		[DataMember(IsRequired = false, Order = 6)]
		public int LastItemIndex
		{
			get { return this.LastItemNumber - 1; }
		}

		[DataMember(IsRequired = false, Order = 7)]
		public int ItemCount { get { return this.Calculator.ItemCount; } }

		[DataMember(IsRequired = false, Order = 8)]
		public PageNumberAndSize NextPage { get { return this.Calculator.NextPage; } }

		[DataMember(IsRequired = false, Order = 9)]
		public PageNumberAndSize PreviousPage { get { return this.Calculator.PreviousPage; } }

		[DataMember(IsRequired = false, Order = 10)]
		public PageNumberAndSize FirstPage { get { return this.Calculator.FirstPage; } }

		[DataMember(IsRequired = false, Order = 11)]
		public PageNumberAndSize LastPage { get { return this.Calculator.LastPage; } }

		[DataMember(IsRequired = false, Order = 12)]
		public bool IsFirstPage { get { return this.Calculator.IsFirstPage; } }

		[DataMember(IsRequired = false, Order = 13)]
		public bool IsLastPage { get { return this.Calculator.IsLastPage; } }

		#endregion

		#region [ Public TurnToPage Method ]

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
				pageNumber >= PageNumberAndSize.FirstPageNumber,
				"An ordinal page number is not a zero-based index. The number must be at least one.");

			// Always return the unbounded page if the current page is unbounded.
			return this.CurrentPage.IsUnbounded ? PageNumberAndSize.Unbounded
				: new PageNumberAndSize(pageNumber, this.CurrentPage.Size);
		}

		#endregion


		public override string ToString()
		{
			return string.Format("PagingInfo[{0},TotalItems={1}]", this.CurrentPage, this.TotalItems);
		}

	}
}