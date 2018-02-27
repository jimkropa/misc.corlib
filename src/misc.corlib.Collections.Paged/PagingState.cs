using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MiscCorLib.Collections.Paged
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

		#region [ Internal Constructor ]

		/// <summary>
		/// Initializes a new instance of the <see cref="PagingState" /> struct
		/// upon deserialization of another <see cref="PagingState" /> which
		/// this one replaces, used for lazy initialization by the internal
		/// <see cref="Calculator" /> property.
		/// </summary>
		/// <remarks>
		/// Constructor is internal to avoid possibly creating an invalid state
		/// where the current page is impossible for a given number of items.
		/// Internally, this type should be used only to create a
		/// <see cref="PagingInfo" /> value, 
		/// </remarks>
		internal PagingState(PageNumberAndSize currentPage, int totalItems)
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