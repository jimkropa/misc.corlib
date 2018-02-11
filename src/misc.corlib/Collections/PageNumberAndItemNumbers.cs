using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MiscCorLib.Collections
{
	/// <summary>
	/// A simple struct representing the one-based ordinal
	/// number of a page within a "paged" collection of items,
	/// and the first and last ordinal numbers of items on the page.
	/// </summary>
	[Serializable, DataContract]
	public struct PageNumberAndItemNumbers
		: IEquatable<PageNumberAndItemNumbers>, IComparable<PageNumberAndItemNumbers>
	{
		#region [ Public Fields and Internal Constructor ]

		/// <summary>
		/// A value of <see cref="PageNumberAndSize" />
		/// which is not valid, indicating an unspecified value.
		/// </summary>
		public static readonly PageNumberAndItemNumbers Empty
			= new PageNumberAndItemNumbers();
		
		/// <summary>
		/// The one-based ordinal number of
		/// this page within a "paged" collection.
		/// </summary>
		[DataMember(IsRequired = true, Order = 0)]
		public readonly int PageNumber;

		/// <summary>
		/// The one-based ordinal number
		/// of the first item on this page.
		/// </summary>
		[DataMember(IsRequired = true, Order = 1)]
		public readonly int FirstItemNumber;

		/// <summary>
		/// The one-based ordinal number
		/// of the last item on this page.
		/// </summary>
		[DataMember(IsRequired = true, Order = 2)]
		public readonly int LastItemNumber;

		/// <summary>
		/// Initializes a new instance of the <see cref="PageNumberAndItemNumbers" /> struct.
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
		internal PageNumberAndItemNumbers(
			int pageNumber, byte pageSize, int totalItems, bool? isLastPage = null)
		{
			if (pageNumber < PageNumberAndSize.FirstPageNumber)
			{
				throw new ArgumentOutOfRangeException(
					nameof(pageNumber),
					pageNumber,
					"An ordinal page number is not a zero-based index. The number must be at least one.");
			}

			if (totalItems < 0)
			{
				throw new ArgumentOutOfRangeException(
					nameof(totalItems),
					totalItems,
					"The number of items in the list must not be negative!");
			}

			if (pageSize >= PageNumberAndSize.MinimumPageSize && (totalItems > 0))
			{
				// A page of fixed size which has items.
				this.PageNumber = pageNumber;
				this.LastItemNumber = pageNumber * pageSize;
				this.FirstItemNumber = this.LastItemNumber - pageSize + 1;

				// Determine whether this is the last page,
				// either by a parameter value or by calculation.
				if ((isLastPage.HasValue && isLastPage.Value)
					|| (PagingInfoCalculator.CalculateTotalPages(pageSize, totalItems) == this.PageNumber))
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
		}

		/// <summary>
		/// Gets a value indicating whether the
		/// <see cref="PageNumber" /> value is valid.
		/// </summary>
		////	[NonSerialized] // (this is applicable only to fields, not properties)
		public bool HasValue
		{
			get
			{
				return this.PageNumber >= PageNumberAndSize.FirstPageNumber
					&& this.FirstItemNumber >= 0 && this.LastItemNumber >= 0;
			}
		}

		#endregion

		#region [ Public Static Overrides of Comparison and Equality Operators ]

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageNumberAndItemNumbers" /> value is less than
		/// another specified <see cref="PageNumberAndItemNumbers" /> value.
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
		public static bool operator <(PageNumberAndItemNumbers left, PageNumberAndItemNumbers right)
		{
			return left.LastItemNumber < right.LastItemNumber;
		}

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageNumberAndItemNumbers" /> value is greater than
		/// another specified <see cref="PageNumberAndItemNumbers" /> value.
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
		public static bool operator >(PageNumberAndItemNumbers left, PageNumberAndItemNumbers right)
		{
			return left.LastItemNumber > right.LastItemNumber;
		}

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageNumberAndItemNumbers" /> value
		/// is less than or equal to another specified
		/// <see cref="PageNumberAndItemNumbers" /> value.
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
		public static bool operator <=(PageNumberAndItemNumbers left, PageNumberAndItemNumbers right)
		{
			return left.LastItemNumber <= right.LastItemNumber;
		}

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageNumberAndItemNumbers" /> value
		/// is greater than or equal to another specified
		/// <see cref="PageNumberAndItemNumbers" /> value.
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
		public static bool operator >=(PageNumberAndItemNumbers left, PageNumberAndItemNumbers right)
		{
			return left.LastItemNumber >= right.LastItemNumber;
		}

		/// <summary>
		/// Indicates whether the values of two
		/// specified <see cref="PageNumberAndItemNumbers" />
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
		public static bool operator ==(PageNumberAndItemNumbers left, PageNumberAndItemNumbers right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Indicates whether the values of two
		/// specified <see cref="PageNumberAndItemNumbers" />
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
		public static bool operator !=(PageNumberAndItemNumbers left, PageNumberAndItemNumbers right)
		{
			return !left.Equals(right);
		}

		#endregion

		/// <summary>
		/// Calculates the full set of page numbers and item
		/// numbers from given <paramref name="pageSize" />
		/// and <paramref name="totalItems" /> values.
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
		/// <remarks>
		/// Relays to the internal <see cref="PagingInfoCalculator.AllPagesAndItemNumbers(byte,int)" />
		/// method of <see cref="PagingInfoCalculator" />.
		/// </remarks>
		public static IReadOnlyList<PageNumberAndItemNumbers> Calculate(
			byte pageSize, int totalItems)
		{
			// Zero as page size is acceptable,
			// indicating a single "unbounded" page.
			if (totalItems < 0)
			{
				throw new ArgumentOutOfRangeException(
					nameof(totalItems),
					totalItems,
					"The number of items in the list must not be negative!");
			}

			return PagingInfoCalculator.AllPagesAndItemNumbers(pageSize, totalItems);
		}

		/// <summary>
		/// Converts this value to its equivalent string representation.
		/// </summary>
		/// <returns>
		/// The string representation of this value.
		/// </returns>
		public override string ToString()
		{
			return string.Format(
				"Page[Number={0},FirstItemNumber={1},LastItemNumber={2}]",
				this.PageNumber,
				this.FirstItemNumber,
				this.LastItemNumber);
		}

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

			// ReSharper disable once ConvertIfStatementToReturnStatement
			if (obj.GetType() != this.GetType())
			{
				return false;
			}

			return this.Equals((PageNumberAndItemNumbers)obj);
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

		#region [ Implementation of IComparable<PageNumberAndItemNumbers> and IEquatable<ITenantIdentifier> ]

		/// <summary>
		/// Compares the current value with another
		/// <see cref="PageNumberAndItemNumbers" /> value
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
		public int CompareTo(PageNumberAndItemNumbers other)
		{
			return this.LastItemNumber.CompareTo(other.LastItemNumber);
		}

		/// <summary>
		/// Indicates whether this value and another
		/// specified <see cref="PageNumberAndItemNumbers" />
		/// value are equal.
		/// </summary>
		/// <param name="other">
		/// The <see cref="PageNumberAndItemNumbers" /> value
		/// to compare with the current value.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="other" /> and this
		/// value have the same <see cref="PageNumber" />,
		/// <see cref="FirstItemNumber" />, and <see cref="LastItemNumber" />
		/// values; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(PageNumberAndItemNumbers other)
		{
			return (this.PageNumber == other.PageNumber)
				&& (this.FirstItemNumber == other.FirstItemNumber)
				&& (this.LastItemNumber == other.LastItemNumber);
		}

		int IComparable<PageNumberAndItemNumbers>.CompareTo(PageNumberAndItemNumbers other)
		{
			// Return the public method.
			// Using an explicit implementation is a way
			// to avoid accidental boxing or unboxing.
			return this.CompareTo(other);
		}

		bool IEquatable<PageNumberAndItemNumbers>.Equals(PageNumberAndItemNumbers other)
		{
			// Return the public method.
			// Using an explicit implementation is a way
			// to avoid accidental boxing or unboxing.
			return this.Equals(other);
		}

		#endregion
	}
}