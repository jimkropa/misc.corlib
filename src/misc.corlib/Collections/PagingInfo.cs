namespace MiscCorLib.Collections
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Runtime.Serialization;

	// TODO: Test JSON Serializability, test ToString();

	/// <summary>
	/// A simple struct with a robust set of metadata
	/// about a page within a "paged" collection of items,
	/// for rendering user interface widgets for moving
	/// through pages, and optimized for simple serialization.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Initialize using values of the <see cref="CurrentPage"/>
	/// number and size, and the number of <see cref="TotalItems"/>
	/// in the collection. All other values, accessible as the
	/// public read-only properties of this struct, are calculated
	/// and held by an internal lazy-initialized value.
	/// </para>
	/// <para>
	/// When serialized this value contains a wealth of information.
	/// When sent for deserialization, this value needs only the
	/// page number, page size, and total number of items.
	/// </para>
	/// </remarks>
	[CLSCompliant(true), Serializable, DataContract]
	public struct PagingInfo : IEquatable<PagingInfo>
	{
		#region [ Private Field and Property backing the Public Read-Only Properties ]

		public const bool DefaultCalculateAllPagesAndItemNumbers = false;

		public static readonly PagingInfo Empty = new PagingInfo();

		[NonSerialized]
		private readonly bool calculateAllPagesAndItemNumbers;

		/// <summary>
		/// Backing field of the internal
		/// <see cref="Calculator"/>,
		/// also used as a semaphore
		/// indicating whether calculated
		/// values have been initialized.
		/// </summary>
		[NonSerialized]
		private readonly PagingInfoCalculator calculator;

		/// <summary>
		/// Gets an internal reference to all of the values
		/// calculated based on initial <see cref="CurrentPage"/>
		/// and <see cref="TotalItems"/> values.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property does some clever sleight-of-hand
		/// for the sake of optimizing serialization and deserialization.
		/// When deserialized, only the <see cref="CurrentPage"/>
		/// and <see cref="TotalItems"/> are required, then other
		/// values are calculated once into a "state" object.
		/// </para>
		/// <para>
		/// Internally, the first access of this property
		/// also has the effect of replacing the parent
		/// <see cref="PagingInfo"/> value.  It's a "lazy"
		/// initialization optimized for <see cref="ValueType"/>
		/// requiring this serialization feature.
		/// </para>
		/// <para>
		/// This property backs all of the serialized public
		/// read-only properties of <see cref="PagingInfo"/>.
		/// </para>
		/// </remarks>
		private PagingInfoCalculator Calculator
		{
			get
			{
				if (this.calculator.CurrentPage.HasValue)
				{
					return this.calculator;
				}

				if (!this.CurrentPage.HasValue)
				{
					return PagingInfoCalculator.Empty;
				}

				// Some ridiculous sleight-of-hand for the sake
				// of optimizing serialization and deserialization.
				// When deserialized, only the CurrentPage and
				// TotalItems are required, then other values
				// are calculated once into a "state" object.
				PagingInfoCalculator newCalculator = new PagingInfoCalculator(
					this.CurrentPage, this.TotalItems, this.calculateAllPagesAndItemNumbers);

				// Having initialized the values,
				// replace the original PagingInfo
				// with a fully-initialized value.
				this = new PagingInfo(newCalculator);

				return newCalculator;
			}
		}

		#endregion

		#region [ Immutable Public Fields CurrentPage and TotalItems ]

		/// <summary>
		/// The current page number and size.
		/// </summary>
		[DataMember(IsRequired = true, Order = 0)]
		public readonly PageNumberAndSize CurrentPage;

		/// <summary>
		/// The total number of items paged.
		/// </summary>
		[DataMember(IsRequired = true, Order = 1)]
		public readonly int TotalItems;

		#endregion

		#region [ Constructor Overloads ]

		/// <summary>
		/// Initializes a new instance of the <see cref="PagingInfo"/> class
		/// for having all of the items in a collection on a single page
		/// as large as the number of <paramref name="totalItems"/>.
		/// </summary>
		/// <param name="totalItems">
		/// The total number of items in the collection to be paged,
		/// initial value for the immutable <see cref="TotalItems"/> field.
		/// </param>
		/// <param name="calculateAllPagesAndItemNumbers">
		/// Indicates whether to include a representation of every
		/// page and its item numbers in the serialized version of
		/// this <see cref="PagingInfo"/>, as a list of
		/// <see cref="PageNumberAndItemNumbers"/>,
		/// for a paging widget which may want to use them.
		/// </param>
		public PagingInfo(
			int totalItems, bool calculateAllPagesAndItemNumbers = DefaultCalculateAllPagesAndItemNumbers)
			: this(PageNumberAndSize.Unbounded, totalItems, calculateAllPagesAndItemNumbers)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				totalItems >= 0, "The number of items in the list must not be negative!");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PagingInfo"/> class
		/// for pages of a fixed size between 1 and 255.
		/// </summary>
		/// <param name="pageNumber">
		/// The requested page <see cref="PageNumberAndSize.Number"/>.
		/// </param>
		/// <param name="pageSize">
		/// The requested page <see cref="PageNumberAndSize.Size"/>.
		/// </param>
		/// <param name="totalItems">
		/// The total number of items in the collection to be paged,
		/// initial value for the immutable <see cref="TotalItems"/> field.
		/// </param>
		/// <param name="calculateAllPagesAndItemNumbers">
		/// Indicates whether to include a representation of every
		/// page and its item numbers in the serialized version of
		/// this <see cref="PagingInfo"/>, as a list of
		/// <see cref="PageNumberAndItemNumbers"/>,
		/// for a paging widget which may want to use them.
		/// </param>
		public PagingInfo(
			int pageNumber, byte pageSize, int totalItems, bool calculateAllPagesAndItemNumbers = DefaultCalculateAllPagesAndItemNumbers)
			: this(new PageNumberAndSize(pageNumber, pageSize), totalItems, calculateAllPagesAndItemNumbers)
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
		/// based on a given <see cref="PageNumberAndSize"/> value.
		/// </summary>
		/// <param name="requestedPage">
		/// The requested page <see cref="PageNumberAndSize.Number"/>
		/// and <see cref="PageNumberAndSize.Size"/>.
		/// If <see cref="PageNumberAndSize.Unbounded"/> is sent,
		/// all of the items are returned on a single page as large
		/// as the number of <paramref name="totalItems"/>.
		/// </param>
		/// <param name="totalItems">
		/// The total number of items in the collection to be paged,
		/// initial value for the immutable <see cref="TotalItems"/> field.
		/// </param>
		/// <param name="calculateAllPagesAndItemNumbers">
		/// Indicates whether to include a representation of every
		/// page and its item numbers in the serialized version of
		/// this <see cref="PagingInfo"/>, as a list of
		/// <see cref="PageNumberAndItemNumbers"/>,
		/// for a paging widget which may want to use them.
		/// </param>
		public PagingInfo(
			PageNumberAndSize requestedPage, int totalItems, bool calculateAllPagesAndItemNumbers = DefaultCalculateAllPagesAndItemNumbers)
			: this(new PagingInfoCalculator(requestedPage, totalItems, calculateAllPagesAndItemNumbers))
		{
			Contract.Requires<ArgumentException>(
				requestedPage.HasValue, "The current page must have a value. \"Unbounded\" is an acceptable value.");
			Contract.Requires<ArgumentOutOfRangeException>(
				totalItems >= 0, "The number of items in the list must not be negative!");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PagingInfo"/> class
		/// upon deserialization of another <see cref="PagingInfo"/> which
		/// this one replaces, used for lazy initializtion by the internal
		/// <see cref="Calculator"/> property.
		/// </summary>
		/// <param name="calculator">
		/// A <see cref="PagingInfoCalculator"/> value initialized from
		/// the <see cref="CurrentPage"/> and <see cref="TotalItems"/>
		/// values of a <see cref="PagingInfo"/> value to be replaced
		/// by this new instance. To understand how this works,
		/// refer to the <see cref="Calculator"/> property.
		/// </param>
		private PagingInfo(PagingInfoCalculator calculator)
		{
			this.CurrentPage = calculator.CurrentPage;
			this.TotalItems = calculator.TotalItems;
			this.calculator = calculator;
			this.calculateAllPagesAndItemNumbers = calculator.IncludeAllPagesAndItemNumbers;
		}

		#endregion

		#region [ Public Read-Only Properties ]

		/// <summary>
		/// Gets a value indicating whether
		/// the <see cref="CurrentPage"/>
		/// and <see cref="TotalItems"/>
		/// values are valid.
		/// </summary>
		////	[NonSerialized] // (this is applicable only to fields, not properties)
		public bool HasValue
		{
			get { return this.CurrentPage.HasValue && this.TotalItems >= 0; }
		}

		[DataMember(IsRequired = false, Order = 2)]
		public int TotalPages { get { return this.Calculator.TotalPages; } }

		[DataMember(IsRequired = false, Order = 3)]
		public bool IsFirstPage { get { return this.Calculator.IsFirstPage; } }

		[DataMember(IsRequired = false, Order = 4)]
		public bool IsLastPage { get { return this.Calculator.IsLastPage; } }

		[DataMember(IsRequired = false, Order = 5)]
		public int FirstItemNumber { get { return this.Calculator.FirstItemNumber; } }

		[DataMember(IsRequired = false, Order = 6)]
		public int LastItemNumber { get { return this.Calculator.LastItemNumber; } }

		/// <summary>
		/// Gets the zero-based index of an item within a "paged" collection of items,
		/// equal to the value of <see cref="FirstItemNumber"/> minus one.
		/// </summary>
		[DataMember(IsRequired = false, Order = 7)]
		public int FirstItemIndex { get { return this.FirstItemNumber - 1; } }

		/// <summary>
		/// Gets the zero-based index of an item within a "paged" collection of items,
		/// equal to the value of <see cref="LastItemNumber"/> minus one.
		/// </summary>
		[DataMember(IsRequired = false, Order = 8)]
		public int LastItemIndex { get { return this.LastItemNumber - 1; } }

		[DataMember(IsRequired = false, Order = 9)]
		public int ItemCount { get { return this.Calculator.ItemCount; } }

		[DataMember(IsRequired = false, Order = 10)]
		public PageNumberAndSize NextPage { get { return this.Calculator.NextPage; } }

		[DataMember(IsRequired = false, Order = 11)]
		public PageNumberAndSize PreviousPage { get { return this.Calculator.PreviousPage; } }

		[DataMember(IsRequired = false, Order = 12)]
		public PageNumberAndSize FirstPage { get { return this.Calculator.FirstPage; } }

		[DataMember(IsRequired = false, Order = 13)]
		public PageNumberAndSize LastPage { get { return this.Calculator.LastPage; } }

		/// <summary>
		/// 
		/// </summary>
		[DataMember(IsRequired = false, Name="AllPages", EmitDefaultValue = false, Order = 14)]
		private IReadOnlyList<PageNumberAndItemNumbers> AllPages { get { return this.Calculator.AllPages; } }

		#endregion

		#region [ Public TurnToPage and CalculateAllPagesAndItemNumbers Methods ]

		/// <summary>
		/// 
		/// </summary>
		/// <returns>
		/// 
		/// </returns>
		public IReadOnlyList<PageNumberAndItemNumbers> CalculateAllPagesAndItemNumbers()
		{
			return this.AllPages ?? PagingInfoCalculator.AllPagesAndItemNumbers(this);
		}

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

		/// <summary>
		/// Converts this value to its equivalent string representation.
		/// </summary>
		/// <returns>
		/// The string representation of this value.
		/// </returns>
		public override string ToString()
		{
			return string.Format("PagingInfo[{0},TotalItems={1}]", this.CurrentPage, this.TotalItems);
		}

		#region [ Public Equality Overrides for Memory Optimization ]

		/// <summary>
		/// Indicates whether this instance
		/// and a specified object are equal.
		/// </summary>
		/// <returns>
		/// <c>true</c> if <paramref name="obj"/>
		/// and this instance are the same type
		/// and represent the same value;
		/// otherwise, <c>false</c>.
		/// </returns>
		/// <param name="obj">
		/// The object to compare with the current instance.
		/// </param>
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			// ReSharper disable once ConvertIfStatementToReturnStatement
			if (obj.GetType() != this.GetType())
			{
				return false;
			}

			return this.Equals((PagingInfo)obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return this.CurrentPage.GetHashCode() + this.TotalItems.GetHashCode();
		}

		#endregion

		#region [ Implementation of IEquatable<PageNumberAndSize> ]

		/// <summary>
		/// Indicates whether this value and another
		/// specified <see cref="PagingInfo"/>
		/// value are equal.
		/// </summary>
		/// <param name="other">
		/// The <see cref="PagingInfo"/> value
		/// to compare with the current value.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="other"/> and this
		/// value have the same <see cref="CurrentPage"/> and
		/// <see cref="TotalItems"/> values; otherwise, <c>false</c>.
		/// </returns>
		[Pure]
		public bool Equals(PagingInfo other)
		{
			return this.CurrentPage.Equals(other.CurrentPage)
				&& this.TotalItems == other.TotalItems;
		}

		bool IEquatable<PagingInfo>.Equals(PagingInfo other)
		{
			// Return the public method.
			// Using an explicit implementation is a way
			// to avoid accidental boxing or unboxing.
			return this.Equals(other);
		}

		#endregion
	}
}