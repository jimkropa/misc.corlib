using System;
using System.Runtime.Serialization;

namespace MiscCorLib.Collections
{
	/// <summary>
	/// A simple struct representing the one-based ordinal
	/// number of a page within a "paged" collection of items,
	/// and the number of items per page.
	/// </summary>
	/// <remarks>
	/// A zero-based index value is accessible
	/// via the <see cref="Index" /> property.
	/// </remarks>
	[Serializable, DataContract]
	public struct PageNumberAndSize
		: IEquatable<PageNumberAndSize>, IComparable<PageNumberAndSize>, IHasValue
	{
		#region [ Constants and Static ReadOnly Fields ]

		/// <summary>
		/// The lowest allowed value of a one-based ordinal number: one.
		/// </summary>
		public const int FirstPageNumber = 1;

		/// <summary>
		/// The default value for the <see cref="Size" />
		/// of a page within a "paged" collection of items: ten.
		/// </summary>
		public const byte DefaultPageSize = 10;

		/// <summary>
		/// The lowest allowed value for the <see cref="Size" />
		/// of a page within a "paged" collection of items: one.
		/// Zero may be used to indicate an <see cref="Unbounded" />
		/// value, with all items in the collection on a single page.
		/// </summary>
		public const byte MinimumPageSize = 1;

		/// <summary>
		/// A value of <see cref="PageNumberAndSize" />
		/// with its <see cref="Number" /> set to the value
		/// of <see cref="FirstPageNumber" /> (one) and its
		/// <see cref="Size" /> set to a default value of ten.
		/// </summary>
		public static readonly PageNumberAndSize Default
			= new PageNumberAndSize(FirstPageNumber);

		/// <summary>
		/// A value of <see cref="PageNumberAndSize" />
		/// with its <see cref="Number" /> equal to
		/// <see cref="FirstPageNumber" />
		/// and <see cref="Size" /> of zero,
		/// to indicate no paging. This value
		/// represents listing all items on a single
		/// page whose size is the total number
		/// of items in the collection.
		/// </summary>
		/// <remarks>
		/// There is a risk of division by zero when using this value,
		/// because the <see cref="Size" /> will be zero.
		/// </remarks>
		public static readonly PageNumberAndSize Unbounded
			= new PageNumberAndSize(true);

		/// <summary>
		/// A value of <see cref="PageNumberAndSize" />
		/// which is not valid, indicating an unspecified value.
		/// </summary>
		public static readonly PageNumberAndSize Empty
			= new PageNumberAndSize();

		#endregion

		#region [ Immutable Public Fields: Number and Size ]

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
		/// <see cref="Index" /> property
		/// or subtract one from this value.
		/// </para>
		/// <para>
		/// If <see cref="IsUnbounded" /> is
		/// <c>true</c>, this value will be
		/// <see cref="FirstPageNumber" />.
		/// </para>
		/// <para>
		/// If <see cref="HasValue" /> is
		/// <c>false</c>, this value will be
		/// zero and <see cref="Index" />
		/// will equal negative one.
		/// </para>
		/// </remarks>
		[DataMember(IsRequired = true, Order = 1, EmitDefaultValue = true)]
		public readonly int Number;

		/// <summary>
		/// The number of items on each page
		/// within a "paged" collection of items,
		/// or zero to indicate an <see cref="Unbounded" />
		/// single page with all items.
		/// </summary>
		/// <remarks>
		/// <para>
		/// If <see cref="IsUnbounded" /> is
		/// <c>true</c>, this value will be zero.
		/// </para>
		/// </remarks>
		[DataMember(IsRequired = true, Order = 2, EmitDefaultValue = true)]
		public readonly byte Size;

		#endregion

		#region [ Constructor Overloads ]

		/// <summary>
		/// Initializes a new instance of the <see cref="PageNumberAndSize" /> struct
		/// for a page size between 1 and 255.
		/// </summary>
		/// <param name="pageNumber">
		/// A one-based ordinal position of a page
		/// within a "paged" collection of items,
		/// initial value of the immutable
		/// <see cref="Number" /> field.
		/// </param>
		/// <param name="pageSize">
		/// The number of items on each page
		/// within a "paged" collection of items,
		/// initial value of the immutable
		/// <see cref="Size" /> field.
		/// </param>
		public PageNumberAndSize(int pageNumber, byte pageSize = DefaultPageSize)
		{
			if (pageNumber < FirstPageNumber)
			{
				throw new ArgumentOutOfRangeException(
					nameof(pageNumber),
					pageNumber,
					"An ordinal page number is not a zero-based index. The number must be at least one.");
			}

			// Prevent possibility of division by zero.
			if (pageSize < MinimumPageSize)
			{
				throw new ArgumentOutOfRangeException(
					nameof(pageSize),
					pageSize,
					"There must be at least one item per page or there could be division by zero!");
			}

			this.Number = pageNumber;
			this.Size = pageSize;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PageNumberAndSize" /> struct
		/// for the static using the <see cref="Unbounded" /> value, having a
		/// <see cref="Number" /> equal to <see cref="FirstPageNumber" />
		/// and <see cref="Size" /> of zero.
		/// </summary>
		/// <param name="unbounded">
		/// Always sent as <c>true</c> by the static
		/// <see cref="Unbounded" /> initializer. Would
		/// other wise create a value equal to <see cref="Empty" />.
		/// </param>
		private PageNumberAndSize(bool unbounded)
		{
			// This private constructor exists only for the
			// initialization of the static Unbounded value.
			this.Size = default(byte);

			// If the "unbounded" parameter is false,
			// then this constructor creates the same
			// as the default constructor.
			this.Number = unbounded ? FirstPageNumber : default(int);
		}

		#endregion

		#region [ Public ReadOnly HasValue and Index Properties ]

		/// <summary>
		/// Gets a value indicating whether the
		/// <see cref="Number" /> and <see cref="Size" />
		/// values are valid.
		/// </summary>
		/// <remarks>
		/// The <see cref="byte" /> type ensures that the
		/// <see cref="Size" /> value will never be invalid.
		/// Zero indicates <see cref="IsUnbounded" />.
		/// </remarks>
		public bool HasValue => this.Number >= FirstPageNumber;

		/// <summary>
		/// Gets the zero-based index of a page within
		/// a "paged" collection of items, equal to the
		/// value of <see cref="Number" /> minus one.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is also the index of a
		/// <see cref="PageNumberAndItemNumbers" />
		/// value within a list such as the one returned by
		/// <see cref="PagingState.AllPagesAndItemNumbers" />.
		/// </para>
		/// <para>
		/// If <see cref="IsUnbounded" /> is <c>true</c>,
		/// then this property returns a value of zero.
		/// The "default" (and invalid) value of this property,
		/// matching that of the <see cref="Empty" /> value,
		/// is one less than zero.
		/// </para>
		/// </remarks>
		public int Index => this.Number - 1;

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
		[DataMember(Order = 3, EmitDefaultValue = false)]
		public bool IsUnbounded => this.Equals(Unbounded);

		#endregion

		#region [ Public Static Overrides of Comparison and Equality Operators ]

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageNumberAndSize" /> value is less than
		/// another specified <see cref="PageNumberAndSize" /> value.
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
		public static bool operator <(PageNumberAndSize left, PageNumberAndSize right)
		{
			return left.CreateComposite() < right.CreateComposite();
		}

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageNumberAndSize" /> value is greater than
		/// another specified <see cref="PageNumberAndSize" /> value.
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
		public static bool operator >(PageNumberAndSize left, PageNumberAndSize right)
		{
			return left.CreateComposite() > right.CreateComposite();
		}

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageNumberAndSize" /> value
		/// is less than or equal to another specified
		/// <see cref="PageNumberAndSize" /> value.
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
		public static bool operator <=(PageNumberAndSize left, PageNumberAndSize right)
		{
			return left.CreateComposite() <= right.CreateComposite();
		}

		/// <summary>
		/// Returns a value that indicates whether a specified
		/// <see cref="PageNumberAndSize" /> value
		/// is greater than or equal to another specified
		/// <see cref="PageNumberAndSize" /> value.
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
		public static bool operator >=(PageNumberAndSize left, PageNumberAndSize right)
		{
			return left.CreateComposite() >= right.CreateComposite();
		}

		/// <summary>
		/// Indicates whether the values of two
		/// specified <see cref="PageNumberAndSize" />
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
		public static bool operator ==(PageNumberAndSize left, PageNumberAndSize right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Indicates whether the values of two
		/// specified <see cref="PageNumberAndSize" />
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
		public static bool operator !=(PageNumberAndSize left, PageNumberAndSize right)
		{
			return !left.Equals(right);
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
			return $"Page[Number={this.Number},Size={this.Size}]";
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

			if (obj.GetType() != this.GetType())
			{
				return false;
			}

			return this.Equals((PageNumberAndSize)obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return this.Number.GetHashCode() + this.Size.GetHashCode();
		}

		#endregion

		#region [ Implementation of IComparable<PageNumberAndSize> and IEquatable<PageNumberAndSize> ]

		/// <summary>
		/// Compares the current value with another
		/// <see cref="PageNumberAndSize" /> value
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
		/// <see cref="Size" /> value multiplied by the
		/// <see cref="Number" /> value.
		/// </remarks>
		public int CompareTo(PageNumberAndSize other)
		{
			return this.CreateComposite().CompareTo(
				other.CreateComposite());
		}

		/// <summary>
		/// Indicates whether this value and another
		/// specified <see cref="PageNumberAndSize" />
		/// value are equal.
		/// </summary>
		/// <param name="other">
		/// The <see cref="PageNumberAndSize" /> value
		/// to compare with the current value.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="other" /> and this
		/// value have the same <see cref="Number" /> and
		/// <see cref="Size" /> values; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(PageNumberAndSize other)
		{
			return (this.Number == other.Number) && (this.Size == other.Size);
		}

		int IComparable<PageNumberAndSize>.CompareTo(PageNumberAndSize other)
		{
			// Return the public method.
			// Using an explicit implementation is a way
			// to avoid accidental boxing or unboxing.
			return this.CompareTo(other);
		}

		bool IEquatable<PageNumberAndSize>.Equals(PageNumberAndSize other)
		{
			// Return the public method.
			// Using an explicit implementation is a way
			// to avoid accidental boxing or unboxing.
			return this.Equals(other);
		}

		/// <summary>
		/// Returns a value to use for comparing
		/// <see cref="PageNumberAndSize" /> values.
		/// </summary>
		/// <returns>
		/// The value of <see cref="Size" /> multiplied
		/// by the value of <see cref="Number" />.
		/// </returns>
		private int CreateComposite()
		{
			return this.Size * this.Number;
		}

		#endregion
	}
}