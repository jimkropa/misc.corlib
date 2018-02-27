using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MiscCorLib.Collections.Paged
{
	/// <summary>
	/// A simple struct representing the one-based ordinal
	/// number of a page within a "paged" collection of items,
	/// and the first and last ordinal numbers of items on the page.
	/// </summary>
	[Serializable, DataContract]
	public struct PageItemNumbers
		: IEquatable<PageItemNumbers>, IComparable<PageItemNumbers>, IHasValue
	{
		#region [ Public Fields and Internal Constructor ]

		/// <summary>
		/// A value of <see cref="PageNumberAndSize" />
		/// which is not valid, indicating an unspecified value.
		/// </summary>
		public static readonly PageItemNumbers Empty
			= new PageItemNumbers();
		
		/// <summary>
		/// The one-based ordinal number of
		/// this page within a "paged" collection.
		/// </summary>
		[DataMember(Order = 0, EmitDefaultValue = true)]
		public readonly int PageNumber;

		/// <summary>
		/// This is <em>only</em> for serialization
		/// and logic, not for sorting or equality.
		/// </summary>
		[DataMember(Order = 1, EmitDefaultValue = false)]
		public readonly bool IsCurrentPage;

		/// <summary>
		/// The one-based ordinal number
		/// of the first item on this page.
		/// </summary>
		[DataMember(Order = 2, EmitDefaultValue = true)]
		public readonly int FirstItemNumber;

		/// <summary>
		/// The one-based ordinal number
		/// of the last item on this page.
		/// </summary>
		[DataMember(Order = 3, EmitDefaultValue = true)]
		public readonly int LastItemNumber;

		/// <summary>
		/// Initializes a new instance of the <see cref="PageItemNumbers" /> struct.
		/// </summary>
		/// <param name="pageNumber">
		/// Initial value for the <see cref="PageNumber" /> field.
		/// </param>
		/// <param name="pageSize">
		/// The <see cref="PageNumberAndSize.Size" />
		/// of each page in a "paged" collection.
		/// </param>
		/// <param name="totalItems">
		/// The total number of items in a "paged" collection.
		/// </param>
		/// <param name="isLastPage">
		/// Optional signal for setting the <see cref="LastItemNumber" />
		/// value, whether to calculate if <paramref name="pageNumber" />
		/// is the last page or to use this value.
		/// </param>
		internal PageItemNumbers(
			PagingState pagingState, bool isCurrent = false, bool? isLastPage = null)
			: this(pagingState.CurrentPage, pagingState.TotalItems, isCurrent, isLastPage)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PageItemNumbers" /> struct.
		/// </summary>
		/// <param name="pageNumber">
		/// Initial value for the <see cref="PageNumber" /> field.
		/// </param>
		/// <param name="pageSize">
		/// The <see cref="PageNumberAndSize.Size" />
		/// of each page in a "paged" collection.
		/// </param>
		/// <param name="totalItems">
		/// The total number of items in a "paged" collection.
		/// </param>
		/// <param name="isLastPage">
		/// Optional signal for setting the <see cref="LastItemNumber" />
		/// value, whether to calculate if <paramref name="pageNumber" />
		/// is the last page or to use this value.
		/// </param>
		internal PageItemNumbers(
			PageNumberAndSize page, int totalItems, bool isCurrent = false, bool? isLastPage = null)
			: this(page.Number, page.Size, totalItems, isCurrent, isLastPage.HasValue
				? isLastPage.Value : Paging.CalculateTotalPages(page.Size, totalItems) == page.Number)
		{
		}

		internal PageItemNumbers(
			int pageNumber, byte pageSize, int totalItems, bool isCurrent, bool isLastPage)
		{
			if (pageSize >= PageNumberAndSize.MinimumPageSize && (totalItems > 0))
			{
				// A page of fixed size which has items.
				this.PageNumber = pageNumber;
				this.LastItemNumber = pageNumber * pageSize;
				this.FirstItemNumber = this.LastItemNumber - pageSize + 1;

				// Determine whether this is the last page,
				// either by a parameter value or by calculation.
				if (isLastPage)
				{
					// If is the last page, replace the
					// calculated LastItemNumber with
					// the totalItems value.
					this.LastItemNumber = totalItems;
				}
			}
			else
			{
				// An unbounded or empty page.
				this.PageNumber = PageNumberAndSize.FirstPageNumber;
				this.FirstItemNumber = totalItems > 0 ? 1 : 0;
				this.LastItemNumber = totalItems;
			}

			this.IsCurrentPage = isCurrent;
		}

		/// <summary>
		/// Gets a value indicating whether the
		/// <see cref="PageNumber" /> value is valid.
		/// </summary>
		public bool HasValue => this.PageNumber >= PageNumberAndSize.FirstPageNumber
			&& this.FirstItemNumber >= 0 && this.LastItemNumber >= 0;

		#endregion

		#region [ Public Static Overrides of Comparison and Equality Operators ]

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageItemNumbers" /> value is less than
		/// another specified <see cref="PageItemNumbers" /> value.
		/// </summary>
		/// <param name="left">
		/// The first value to compare.
		/// </param>
		/// <param name="right">
		/// The second value to compare.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="left" /> is less than
		/// <paramref name="right" />; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator <(PageItemNumbers left, PageItemNumbers right)
		{
			return left.LastItemNumber < right.LastItemNumber;
		}

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageItemNumbers" /> value is greater than
		/// another specified <see cref="PageItemNumbers" /> value.
		/// </summary>
		/// <param name="left">
		/// The first value to compare.
		/// </param>
		/// <param name="right">
		/// The second value to compare.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="left" /> is greater than
		/// <paramref name="right" />; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator >(PageItemNumbers left, PageItemNumbers right)
		{
			return left.LastItemNumber > right.LastItemNumber;
		}

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageItemNumbers" /> value
		/// is less than or equal to another specified
		/// <see cref="PageItemNumbers" /> value.
		/// </summary>
		/// <param name="left">
		/// The first value to compare.
		/// </param>
		/// <param name="right">
		/// The second value to compare.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="left" /> is
		/// less than or equal to <paramref name="right" />;
		/// otherwise, <c>false</c>.
		/// </returns>
		public static bool operator <=(PageItemNumbers left, PageItemNumbers right)
		{
			return left.LastItemNumber <= right.LastItemNumber;
		}

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageItemNumbers" /> value
		/// is greater than or equal to another specified
		/// <see cref="PageItemNumbers" /> value.
		/// </summary>
		/// <param name="left">
		/// The first value to compare.
		/// </param>
		/// <param name="right">
		/// The second value to compare.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="left" /> is
		/// greater than or equal to <paramref name="right" />;
		/// otherwise, <c>false</c>.
		/// </returns>
		public static bool operator >=(PageItemNumbers left, PageItemNumbers right)
		{
			return left.LastItemNumber >= right.LastItemNumber;
		}

		/// <summary>
		/// Indicates whether the values of two
		/// specified <see cref="PageItemNumbers" />
		/// objects are equal.
		/// </summary>
		/// <param name="left">
		/// The first object to compare.
		/// </param>
		/// <param name="right">
		/// The second object to compare.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="left" />
		/// and <paramref name="right" /> are equal;
		/// otherwise <c>false</c>.
		/// </returns>
		public static bool operator ==(PageItemNumbers left, PageItemNumbers right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Indicates whether the values of two
		/// specified <see cref="PageItemNumbers" />
		/// objects are not equal.
		/// </summary>
		/// <param name="left">
		/// The first object to compare.
		/// </param>
		/// <param name="right">
		/// The second object to compare.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="left" />
		/// and <paramref name="right" /> are not equal;
		/// otherwise <c>false</c>.
		/// </returns>
		public static bool operator !=(PageItemNumbers left, PageItemNumbers right)
		{
			return !left.Equals(right);
		}

		#endregion

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

			return this.Equals((PageItemNumbers)obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return this.PageNumber.GetHashCode()
				+ this.FirstItemNumber.GetHashCode()
				+ this.LastItemNumber.GetHashCode();
		}

		#endregion

		#region [ Implementation of IComparable<PageNumberAndItemNumbers> and IEquatable<PageNumberAndItemNumbers> ]

		/// <summary>
		/// Compares the current value with another
		/// <see cref="PageItemNumbers" /> value
		/// and returns an integer that indicates whether
		/// the current instance precedes, follows, or occurs
		/// in the same position in the sort order
		/// as the <paramref name="other" /> value.
		/// </summary>
		/// <param name="other">
		/// An object to compare with this instance.
		/// </param>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared,
		/// to implement <see cref="IComparable{T}.CompareTo" />.
		/// </returns>
		/// <remarks>
		/// In this case, the comparison is based on the
		/// <see cref="LastItemNumber" /> value.
		/// </remarks>
		public int CompareTo(PageItemNumbers other)
		{
			return this.LastItemNumber.CompareTo(other.LastItemNumber);
		}

		/// <summary>
		/// Indicates whether this value and another
		/// specified <see cref="PageItemNumbers" />
		/// value are equal.
		/// </summary>
		/// <param name="other">
		/// The <see cref="PageItemNumbers" /> value
		/// to compare with the current value.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="other" /> and this
		/// value have the same <see cref="PageNumber" />,
		/// <see cref="FirstItemNumber" />, and <see cref="LastItemNumber" />
		/// values; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(PageItemNumbers other)
		{
			return (this.PageNumber == other.PageNumber)
				&& (this.FirstItemNumber == other.FirstItemNumber)
				&& (this.LastItemNumber == other.LastItemNumber);
		}

		int IComparable<PageItemNumbers>.CompareTo(PageItemNumbers other)
		{
			// Return the public method.
			// Using an explicit implementation is a way
			// to avoid accidental boxing or unboxing.
			return this.CompareTo(other);
		}

		bool IEquatable<PageItemNumbers>.Equals(PageItemNumbers other)
		{
			// Return the public method.
			// Using an explicit implementation is a way
			// to avoid accidental boxing or unboxing.
			return this.Equals(other);
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
			return $"Page[Number={this.PageNumber},FirstItemNumber={this.FirstItemNumber},LastItemNumber={this.LastItemNumber}]";
		}
	}
}