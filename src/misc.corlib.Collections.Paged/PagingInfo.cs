using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MiscCorLib.Collections.Paged
{
	[Serializable, DataContract]
	public struct PagingInfo : IEquatable<PagingInfo>, IEquatable<PagingState>,
		IComparable<PagingInfo>, IComparable<PagingState>, IComparable<PageNumberAndSize>, IHasValue
	{
		/// <summary>
		/// An empty, default value.
		/// </summary>
		/// <returns>
		/// A <see cref="PagingInfo" /> value whose
		/// internal <see cref="PagingState" />'s
		/// <see cref="PagingState.HasValue" />
		/// is <c>false</c>.
		/// </returns>
		public static PagingInfo Empty = new PagingInfo();

		/// <summary>
		/// A nonempty <see cref="PagingState" />
		/// used to calculate other values which
		/// describe a paged list.
		/// </summary>
		/// <remarks>
		/// This is the only given field.
		/// All of the other fields are calculated.
		/// Some public properties relay from
		/// private calculated fields.
		/// </remarks>
		private readonly PagingState _state;

		/// <summary>
		/// Internal calculated field containing
		/// the values of the <see cref="FirstItemNumber" />
		/// and <see cref="LastItemNumber" /> properties.
		/// </summary>
		private readonly PageItemNumbers _pageAndItemNumbers; // This is a calculated field.

		/// <summary>
		/// The current page number.
		/// </summary>
		[DataMember(Order = 1, EmitDefaultValue = true)]
		public int PageNumber => _state.CurrentPage.Number;

		/// <summary>
		/// The current page size.
		/// </summary>
		[DataMember(Order = 2, EmitDefaultValue = true)]
		public int PageSize => _state.CurrentPage.Number;

		/// <summary>
		/// The total number of items paged.
		/// </summary>
		[DataMember(Order = 3, EmitDefaultValue = true)]
		public int TotalItems => _state.TotalItems;

		/// <summary>
		/// Gets a value indicating whether the
		/// <see cref="Number" /> and <see cref="Size" />
		/// values are set to indicate that all items
		/// should be shown on a single page of
		/// unbounded size.
		/// </summary>
		/// <remarks>
		/// When this value is <c>true</c>, there
		/// is a risk of division by zero because the
		/// <see cref="Size" /> value is zero.
		/// </remarks>
		[DataMember(Order = 4, EmitDefaultValue = false)]
		public PagingState State => this._state;

		/// <summary>
		/// Count of total pages in a paged list,
		/// calculated based on the current page number
		/// and total number of items on the list.
		/// </summary>
		[DataMember(Order = 5, EmitDefaultValue = true)]
		public readonly int TotalPages; // This is a calculated field.

		[DataMember(Order = 6, EmitDefaultValue = false)]
		public bool IsUnbounded => this._state.CurrentPage.IsUnbounded;

		[DataMember(Order = 7, EmitDefaultValue = true)]
		public readonly int ItemCount; // This is a calculated field.

		[DataMember(Order = 8, EmitDefaultValue = true)]
		public int FirstItemNumber => this._pageAndItemNumbers.FirstItemNumber;

		[DataMember(Order = 9, EmitDefaultValue = true)]
		public int LastItemNumber => this._pageAndItemNumbers.LastItemNumber;

		/// <summary>
		/// Gets the zero-based index of an item within a "paged" collection of items,
		/// equal to the value of <see cref="FirstItemNumber" /> minus one.
		/// </summary>
		/// <remarks>
		/// If the "paged" collection is empty,
		/// this value will be <c>-1</c>.
		/// </remarks>
		public int FirstItemIndex => this.FirstItemNumber - 1;

		/// <summary>
		/// Gets the zero-based index of an item within a "paged" collection of items,
		/// equal to the value of <see cref="LastItemNumber" /> minus one.
		/// </summary>
		/// <remarks>
		/// If the "paged" collection is empty,
		/// this value will be <c>-1</c>.
		/// </remarks>
		public int LastItemIndex => this.LastItemNumber - 1;

		[DataMember(Order = 10, EmitDefaultValue = true)]
		public readonly bool IsFirstPage;

		[DataMember(Order = 11, EmitDefaultValue = true)]
		public readonly bool IsLastPage;

		/// <summary>
		/// Initializes a new instance of the <see cref="Paging" /> struct.
		/// Invoked by the private <see cref="PagingState.Calculator" />
		/// property of an owner <see cref="PagingState" /> value,
		/// calculates metadata for paging UI, optionally including
		/// a list of all pages and item numbers. For effective lazy
		/// initialization following deserialization from bare essentials.
		/// </summary>
		/// <param name="currentPage">
		/// The page <see cref="PageNumberAndSize.Number" />
		/// and <see cref="PageNumberAndSize.Size" />.
		/// If <see cref="PageNumberAndSize.Unbounded" /> is sent,
		/// all of the items are returned on a single page as large
		/// as the number of <paramref name="totalItems" />.
		/// </param>
		/// <param name="totalItems">
		/// The total number of items in the collection to be paged,
		/// initial value for the immutable <see cref="TotalItems" /> field.
		/// </param>
		/// <param name="includeAllPagesAndItemNumbers">
		/// Whether to fill the set of <see cref="AllPages" />
		/// including the item numbers on each page,
		/// which may be useful for some paging UI.
		/// Relayed back to the <see cref="PagingState" /> via the
		/// <see cref="IncludeAllPagesAndItemNumbers" /> field,
		/// adds the private <see cref="PagingState.AllPages" />
		/// property to the serialization output for JSON.
		/// </param>
		internal PagingInfo(PagingState pagingState)
		{
			if (!pagingState.HasValue)
			{
				// Throw an exception, don't return empty.
				throw new ArgumentException(
					"A valid PagingState value is required.",
					nameof(pagingState));
			}

			if (pagingState.CurrentPage.IsUnbounded)
			{
				this.TotalPages = 1;
				this.IsFirstPage = true;
				this.IsLastPage = true;
				this._pageAndItemNumbers = new PageItemNumbers(pagingState, true);
				this.ItemCount = pagingState.TotalItems;
			}
			else if ((pagingState.TotalItems > 0)
				&& (pagingState.CurrentPage.Size > 0))
			{
				// Calculate the total pages for a fixed page size and at least one result.
				this.TotalPages = Paging.CalculateTotalPages(
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
				this._pageAndItemNumbers = new PageItemNumbers(pagingState, this.IsLastPage);
				this.ItemCount = this._pageAndItemNumbers.LastItemNumber - this._pageAndItemNumbers.FirstItemNumber + 1;
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
				this._pageAndItemNumbers = new PageItemNumbers(pagingState, true);
				this.ItemCount = 0;
			}

			this._state = pagingState;
		}

		/// <summary>
		/// Gets a value indicating whether
		/// the <see cref="CurrentPage" />
		/// and <see cref="TotalItems" />
		/// values are valid.
		/// </summary>
		public bool HasValue => this._state.HasValue;

		#region [ Public Equality Overrides for Memory Optimization ]

		/// <summary>
		/// Indicates whether this instance
		/// and a specified object are equal.
		/// </summary>
		/// <returns>
		/// <c>true</c> if <paramref name="obj" />
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
			return this._state.GetHashCode();
		}

		#endregion

		#region [ Implementation of IEquatable<PagingInfo> and IEquatable<PagingState> ]

		/// <summary>
		/// Indicates whether this value and another
		/// specified <see cref="PagingInfo" />
		/// value are equal.
		/// </summary>
		/// <param name="other">
		/// The <see cref="PagingInfo" /> value
		/// to compare with the current value.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="other" /> and this
		/// value have the same <see cref="CurrentPage" /> and
		/// <see cref="TotalItems" /> values; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(PagingInfo other)
		{
			return this._state.Equals(other._state);
		}

		bool IEquatable<PagingInfo>.Equals(PagingInfo other)
		{
			// Return the public method.
			// Using an explicit implementation is a way
			// to avoid accidental boxing or unboxing.
			return this.Equals(other);
		}

		bool IEquatable<PagingState>.Equals(PagingState other)
		{
			// Return the public method.
			// Using an explicit implementation is a way
			// to avoid accidental boxing or unboxing.
			return this._state.Equals(other);
		}

		#endregion

		#region [ Explicit Implementation of IComparable<PagingInfo> and IComparable<PagingState> ]

		int IComparable<PagingInfo>.CompareTo(PagingInfo other)
		{
			return this._state.CurrentPage.CompareTo(other._state.CurrentPage);
		}

		int IComparable<PagingState>.CompareTo(PagingState other)
		{
			return this._state.CurrentPage.CompareTo(other.CurrentPage);
		}

		#endregion

		#region [ Explicit Implementation of IComparable<PageNumberAndSize> ]

		int IComparable<PageNumberAndSize>.CompareTo(PageNumberAndSize other)
		{
			return this._state.CurrentPage.CompareTo(other);
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
			return $"PagingInfo[{this._state},TotalPages={this.TotalPages},ItemNumbers={this.FirstItemNumber}-{this.LastItemNumber}]";
		}
	}
}