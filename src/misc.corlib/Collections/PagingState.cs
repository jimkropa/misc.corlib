using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MiscCorLib.Collections
{
	/// <summary>
	/// A simple struct with a robust set of metadata
	/// about a page within a "paged" collection of items,
	/// for rendering user interface widgets for moving
	/// through pages, and optimized for simple serialization.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Initialize using values of the <see cref="CurrentPage" />
	/// number and size, and the number of <see cref="TotalItems" />
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
	[Serializable, DataContract]
	public struct PagingState : IEquatable<PagingState>, IComparable<PagingState>, IComparable<PageNumberAndSize>, IHasValue
	{
		#region [ Public Constants for Default and Empty Values ]

		/// <summary>
		/// A value of <see cref="PageNumberAndSize" />
		/// which is not valid, indicating an unspecified value.
		/// </summary>
		public static readonly PagingState Empty = new PagingState();

		#endregion

		#region [ Immutable Fields CurrentPage and TotalItems ]

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
		/// Initializes a new instance of the <see cref="PagingState" /> struct
		/// for having all of the items in a collection on a single page
		/// as large as the number of <paramref name="totalItems" />.
		/// </summary>
		/// <param name="totalItems">
		/// The total number of items in the collection to be paged,
		/// initial value for the immutable <see cref="TotalItems" /> field.
		/// </param>
		public PagingState(int totalItems)
			: this(PageNumberAndSize.Unbounded, totalItems)
		{
		}

		/// <summary>
		/// Initializes a <see cref="PagingState" /> value
		/// for pages of a given size between 1 and 255.
		/// </summary>
		/// <param name="pageNumber">
		/// The requested page <see cref="PageNumberAndSize.Number" />.
		/// </param>
		/// <param name="pageSize">
		/// The requested page <see cref="PageNumberAndSize.Size" />.
		/// </param>
		/// <param name="totalItems">
		/// The total number of items in the collection to be paged,
		/// initial value for the immutable <see cref="TotalItems" /> field.
		/// </param>
		public PagingState(
			int pageNumber, byte pageSize, int totalItems)
			: this(new PageNumberAndSize(pageNumber, pageSize), totalItems)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PagingState" /> struct
		/// upon deserialization of another <see cref="PagingState" /> which
		/// this one replaces, used for lazy initialization by the internal
		/// <see cref="Calculator" /> property.
		/// </summary>
		public PagingState(PageNumberAndSize currentPage, int totalItems)
		{
			if (!currentPage.HasValue)
			{
				throw new ArgumentException(
					"The current page must have a value. \"Unbounded\" is an acceptable value.",
					nameof(currentPage));
			}

			if (totalItems < 0)
			{
				throw new ArgumentOutOfRangeException(
					nameof(totalItems),
					totalItems,
					"The number of items in the list must not be negative!");
			}

			this.CurrentPage = currentPage;
			this.TotalItems = totalItems;
		}

		#endregion

		/// <summary>
		/// Gets a value indicating whether
		/// the <see cref="CurrentPage" />
		/// and <see cref="TotalItems" />
		/// values are valid.
		/// </summary>
		public bool HasValue => this.CurrentPage.HasValue && this.TotalItems >= 0;

/*
		/// <summary>
		/// Gets an internal reference to all of the values
		/// calculated based on initial <see cref="CurrentPage" />
		/// and <see cref="TotalItems" /> values.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This property does some clever sleight-of-hand
		/// for the sake of optimizing serialization and deserialization.
		/// When deserialized, only the <see cref="CurrentPage" />
		/// and <see cref="TotalItems" /> are required, then other
		/// values are calculated once into a "state" object.
		/// </para>
		/// <para>
		/// Internally, the first access of this property
		/// also has the effect of replacing the parent
		/// <see cref="PagingState" /> value.  It's a "lazy"
		/// initialization optimized for <see cref="ValueType" />
		/// requiring this serialization feature.
		/// </para>
		/// <para>
		/// This property backs all of the serialized public
		/// read-only properties of <see cref="PagingState" />.
		/// </para>
		/// </remarks>
		private PagingInfoCalculator Calculator
		{
			get
			{
				// Is initialized already?
				if (this.calculator.CurrentPage.HasValue)
				{
					return this.calculator;
				}

				// If this is an empty value,
				// return a corresponding empty
				// and skip calculations.
				if (!this.CurrentPage.HasValue)
				{
					return PagingInfoCalculator.Empty;
				}

				// Some clever sleight-of-hand for the sake
				// of optimizing serialization and deserialization.
				// When deserialized, only the CurrentPage and
				// TotalItems are required, then other values
				// are calculated once into a "state" object.
				PagingInfoCalculator newCalculator = new PagingInfoCalculator(
					this.CurrentPage, this.TotalItems, this.calculateAllPagesAndItemNumbers);

				// Having initialized the values,
				// replace the original PagingInfo
				// with a fully-initialized value.
				this = new PagingState(newCalculator);

				return newCalculator;
			}
		}
*/

		#region [ Public Static Overrides of Equality Operators ]

		/// <summary>
		/// Indicates whether the values of two
		/// specified <see cref="PagingState" />
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
		public static bool operator ==(PagingState left, PagingState right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Indicates whether the values of two
		/// specified <see cref="PagingState" />
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
		public static bool operator !=(PagingState left, PagingState right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region [ Public TurnToPage and CalculateAllPagesAndItemNumbers Methods ]

		/// <summary>
		/// Calculates a <see cref="PageNumberAndSize" /> for an
		/// arbitrary page <see cref="PageNumberAndSize.Number" />
		/// using the same page <see cref="PageNumberAndSize.Size" />.
		/// </summary>
		/// <param name="pageNumber">
		/// The one-based ordinal
		/// <see cref="PageNumberAndSize.Number" />
		/// of the page to fetch.
		/// </param>
		/// <returns>
		/// A <see cref="PageNumberAndSize" /> value calculated by
		/// using the same <see cref="PageNumberAndSize.Size" />
		/// as the <see cref="CurrentPage" /> and the page
		/// <see cref="PageNumberAndSize.Number" /> given
		/// as the <paramref name="pageNumber" /> parameter.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Should this operation be closed under the set of
		/// <see cref="PagingState" /> operations? In other words,
		/// should this method return a new <see cref="PagingState" />
		/// value instead of a <see cref="PageNumberAndSize" /> value?
		/// </para>
		/// <para>
		/// The reason it should not is that the number of
		/// <see cref="TotalItems" /> cannot be assumed to
		/// remain constant between requests. That number
		/// may have changed between the time that this
		/// page was retrieved and the retrieval of a different
		/// page using the return from this function.
		/// </para>
		/// <para>
		/// That is the same reason for checking the maximum
		/// allowed page number in this function.
		/// The <see cref="PagingState" /> constructor gracefully
		/// handles the situation in which a page number
		/// is higher than the total number of pages.
		/// </para>
		/// </remarks>
		public PageNumberAndSize TurnToPage(int pageNumber)
		{
			if (pageNumber < PageNumberAndSize.FirstPageNumber)
			{
				throw new ArgumentOutOfRangeException(
					nameof(pageNumber),
					pageNumber,
					"An ordinal page number is not a zero-based index. The number must be at least one.");
			}

			// Prevent a runtime exception from possible division by zero.
			if (this.CurrentPage.HasValue)
			{
				// Always return the unbounded page if the current page is unbounded.
				return this.CurrentPage.IsUnbounded ? PageNumberAndSize.Unbounded
					: new PageNumberAndSize(pageNumber, this.CurrentPage.Size);
			}

			// Return empty if uninitialized.
			return PageNumberAndSize.Empty;
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

			return this.Equals((PagingState)obj);
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

		#region [ Implementation of IEquatable<PagingInfo> ]

		/// <summary>
		/// Indicates whether this value and another
		/// specified <see cref="PagingState" />
		/// value are equal.
		/// </summary>
		/// <param name="other">
		/// The <see cref="PagingState" /> value
		/// to compare with the current value.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="other" /> and this
		/// value have the same <see cref="CurrentPage" /> and
		/// <see cref="TotalItems" /> values; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(PagingState other)
		{
			return this.CurrentPage.Equals(other.CurrentPage)
				&& this.TotalItems == other.TotalItems;
		}

		bool IEquatable<PagingState>.Equals(PagingState other)
		{
			// Return the public method.
			// Using an explicit implementation is a way
			// to avoid accidental boxing or unboxing.
			return this.Equals(other);
		}

		#endregion

		#region [ Explicit Implementation of IComparable<PagingInfo> ]

		int IComparable<PagingState>.CompareTo(PagingState other)
		{
			// TotalPages doesn't matter,
			// for this rarely used
			// explicit implementation.
			return this.CurrentPage.CompareTo(other.CurrentPage);
		}

		#endregion

		#region [ Explicit Implementation of IComparable<PageNumberAndSize> ]

		int IComparable<PageNumberAndSize>.CompareTo(PageNumberAndSize other)
		{
			// TotalPages doesn't matter,
			// for this rarely used
			// explicit implementation.
			return this.CurrentPage.CompareTo(other);
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
			return $"PagingState[{this.CurrentPage},TotalItems={this.TotalItems}]";
		}
	}
}