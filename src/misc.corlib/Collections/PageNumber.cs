namespace MiscCorLib.Collections
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Runtime.Serialization;

	/// <summary>
	/// A simple struct representing the one-based ordinal
	/// number of a page within a "paged" collection of items.
	/// </summary>
	/// <remarks>
	/// A zero-based index value is accessible
	/// via the <see cref="Index"/> property.
	/// </remarks>
	[CLSCompliant(true), Serializable, DataContract]
	public struct PageNumber : IEquatable<PageNumber>, IComparable<PageNumber>
	{
		#region [ Constants and Static ReadOnly Fields ]

		/// <summary>
		/// The lowest allowed value of a one-based ordinal number: one.
		/// </summary>
		public const int FirstPageNumber = 1;

		/// <summary>
		/// A value of <see cref="PageNumber"/>
		/// with its <see cref="Number"/> set to the value
		/// of <see cref="FirstPageNumber"/> (one).
		/// </summary>
		public static readonly PageNumber Default
			= new PageNumber(FirstPageNumber);

		/// <summary>
		/// A value of <see cref="PageNumber"/>
		/// which is not valid, indicating an unspecified value.
		/// </summary>
		public static readonly PageNumber Empty
			= new PageNumber();

		#endregion

		#region [ Immutable Public Field and Public Constructor ]

		/// <summary>
		/// The one-based ordinal position of a page
		/// within a "paged" collection of items.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is not a zero-based index!
		/// This is a one-based ordinal like
		/// usage of natural numbers in
		/// human speech, so the first page
		/// is "page one."  If the zero-based
		/// index is needed, use the
		/// <see cref="Index"/> property
		/// or subtract one from this value.
		/// </para>
		/// </remarks>
		[DataMember(IsRequired = true, Order = 0)]
		public readonly int Number;

		/// <summary>
		/// Initializes a new instance of the <see cref="PageNumber"/> struct.
		/// </summary>
		/// <param name="number">
		/// A one-based ordinal position of a page
		/// within a "paged" collection of items,
		/// initial value of the immutable
		/// <see cref="Number"/> field.
		/// </param>
		public PageNumber(int number)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				number >= FirstPageNumber,
				"An ordinal page number is not a zero-based index. The number must be at least one.");

			this.Number = number;
		}

		#endregion

		#region [ Public ReadOnly Index, IsUnbounded, and HasValue Properties ]

		/// <summary>
		/// Gets the zero-based index of a page within
		/// a "paged" collection of items, equal to the
		/// value of <see cref="Number"/> minus one.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This index represents the position of a page within
		/// a list of pages, such as the list of pages returned by
		/// <see cref="PagingInfo.AllPagesAndItemNumbers"/>.
		/// </para>
		/// </remarks>
		[DataMember(IsRequired = false, Order = 1)]
		public int Index { get { return this.Number - 1; } }

		/// <summary>
		/// Gets a value indicating whether the
		/// <see cref="Number"/> value is valid.
		/// </summary>
		////	[DataMember(IsRequired = false, Order = 2)]
		////	[NonSerialized] // (this is applicable only to fields, not properties)
		public bool HasValue { get { return this.Number >= FirstPageNumber; } }

		#endregion

		#region [ Public Static Overrides of Comparison and Equality Operators ]

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageNumber"/> value is less than
		/// another specified <see cref="PageNumber"/> value.
		/// </summary>
		/// <param name="left">
		/// The first value to compare.
		/// </param>
		/// <param name="right">
		/// The second value to compare.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> is less than
		/// <paramref name="right"/>; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator <(PageNumber left, PageNumber right)
		{
			return left.Number < right.Number;
		}

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageNumber"/> value is greater than
		/// another specified <see cref="PageNumber"/> value.
		/// </summary>
		/// <param name="left">
		/// The first value to compare.
		/// </param>
		/// <param name="right">
		/// The second value to compare.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> is greater than
		/// <paramref name="right"/>; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator >(PageNumber left, PageNumber right)
		{
			return left.Number > right.Number;
		}

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageNumber"/> value
		/// is less than or equal to another specified
		/// <see cref="PageNumber"/> value.
		/// </summary>
		/// <param name="left">
		/// The first value to compare.
		/// </param>
		/// <param name="right">
		/// The second value to compare.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> is
		/// less than or equal to <paramref name="right"/>;
		/// otherwise, <c>false</c>.
		/// </returns>
		public static bool operator <=(PageNumber left, PageNumber right)
		{
			return left.Number <= right.Number;
		}

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageNumber"/> value
		/// is greater than or equal to another specified
		/// <see cref="PageNumber"/> value.
		/// </summary>
		/// <param name="left">
		/// The first value to compare.
		/// </param>
		/// <param name="right">
		/// The second value to compare.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> is
		/// greater than or equal to <paramref name="right"/>;
		/// otherwise, <c>false</c>.
		/// </returns>
		public static bool operator >=(PageNumber left, PageNumber right)
		{
			return left.Number >= right.Number;
		}

		/// <summary>
		/// Indicates whether the values of two
		/// specified <see cref="PageNumber"/>
		/// objects are equal.
		/// </summary>
		/// <param name="left">
		/// The first object to compare.
		/// </param>
		/// <param name="right">
		/// The second object to compare.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/>
		/// and <paramref name="right"/> are equal;
		/// otherwise <c>false</c>.
		/// </returns>
		public static bool operator ==(PageNumber left, PageNumber right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Indicates whether the values of two
		/// specified <see cref="PageNumber"/>
		/// objects are not equal.
		/// </summary>
		/// <param name="left">
		/// The first object to compare.
		/// </param>
		/// <param name="right">
		/// The second object to compare.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/>
		/// and <paramref name="right"/> are not equal;
		/// otherwise <c>false</c>.
		/// </returns>
		public static bool operator !=(PageNumber left, PageNumber right)
		{
			return !left.Equals(right);
		}

		#endregion

		// TODO: Test ToString
		public override string ToString()
		{
			return string.Format("Page[Number={0}]", this.Number);
		}

		// TODO: Override implicit operators for comparing PageNumber to Int32.
		/*
			public static implicit operator PageNumber(int value)
			{
				return new PageNumber(value);
			}

			public static implicit operator int(PageNumber value)
			{
				return value.Number;
			}

			bool IEquatable<int>.Equals(int other)
			{
				return this.Number.Equals(other);
			}
		*/

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

			return this.Equals((PageNumber)obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return this.Number.GetHashCode();
		}

		#endregion

		#region [ Implementation of IComparable<PageNumber> and IEquatable<ITenantIdentifier> ]

		/// <summary>
		/// Compares the current value with another
		/// <see cref="PageNumber"/> value
		/// and returns an integer that indicates whether
		/// the current instance precedes, follows, or occurs
		/// in the same position in the sort order
		/// as the <paramref name="other"/> value.
		/// </summary>
		/// <param name="other">
		/// An object to compare with this instance.
		/// </param>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared,
		/// to implement <see cref="IComparable{T}.CompareTo"/>.
		/// </returns>
		/// <remarks>
		/// In this case, the comparison is based on the
		/// <see cref="Number"/> value.
		/// </remarks>
		public int CompareTo(PageNumber other)
		{
			int thisComposite = this.Number;
			int otherComposite = other.Number;

			return thisComposite.CompareTo(otherComposite);
		}

		/// <summary>
		/// Indicates whether this value and another
		/// specified <see cref="PageNumber"/>
		/// value are equal.
		/// </summary>
		/// <param name="other">
		/// The <see cref="PageNumber"/> value
		/// to compare with the current value.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="other"/> and this
		/// value have the same <see cref="Number"/> values;
		/// otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(PageNumber other)
		{
			return this.Number == other.Number;
		}

		int IComparable<PageNumber>.CompareTo(PageNumber other)
		{
			// Return the public method.
			// Using an explicit implementation is a way
			// to avoid accidental boxing or unboxing.
			return this.CompareTo(other);
		}

		bool IEquatable<PageNumber>.Equals(PageNumber other)
		{
			// Return the public method.
			// Using an explicit implementation is a way
			// to avoid accidental boxing or unboxing.
			return this.Equals(other);
		}

		#endregion
	}
}