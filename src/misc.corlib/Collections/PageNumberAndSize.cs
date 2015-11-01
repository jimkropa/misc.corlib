namespace MiscCorLib.Collections
{
	using System;
	using System.Diagnostics.Contracts;

	/// <summary>
	/// A simple struct representing the one-based ordinal
	/// number of a page within a "paged" collection of items,
	/// and the number of items per page.
	/// </summary>
	/// <remarks>
	/// A zero-based index value is accessible
	/// via the <see cref="Index"/> proiperty.
	/// </remarks>
	[Serializable]
	public struct PageNumberAndSize : IEquatable<PageNumberAndSize>
	{
		#region [ Constants and Static ReadOnly Fields ]

		/// <summary>
		/// The lowest allowed value of a one-based ordinal number.
		/// </summary>
		public const int FirstPageNumber = 1;

		internal const byte DefaultPageSize = 10;
		internal const int MinimumPageSize = 1;

		/// <summary>
		/// A value of <see cref="PageNumberAndSize"/>
		/// with its <see cref="Number"/> set to the value
		/// of <see cref="FirstPageNumber"/> and its
		/// <see cref="Size"/> set to a default value of ten.
		/// </summary>
		public static readonly PageNumberAndSize Default
			= new PageNumberAndSize(FirstPageNumber, DefaultPageSize);

		/// <summary>
		/// A value of <see cref="PageNumberAndSize"/>
		/// with its <see cref="Number"/> and <see cref="Size"/>
		/// values set to indicate no paging.
		/// </summary>
		/// <remarks>
		/// There is a risk of division by zero when using this value,
		/// because the <see cref="Size"/> will be zero.
		/// </remarks>
		public static readonly PageNumberAndSize Unbounded
			= new PageNumberAndSize(true);

		/// <summary>
		/// A value of <see cref="PageNumberAndSize"/>
		/// which is not valid, indicating an unspecifed value.
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
		/// This is not a zero-based index,
		/// this is a one-based ordinal like
		/// usage of natural numbers in
		/// human speech, so the first page
		/// is "page one."  If the zero-based
		/// index is needed, use the
		/// <see cref="Index"/> property
		/// or subtract one from this value.
		/// </remarks>
		public readonly int Number;

		/// <summary>
		/// The number of items on each page
		/// within a "paged" collection of items.
		/// </summary>
		/// <remarks>
		/// If <see cref="IsUnbounded"/> is <c>true</c>, this value will be zero.
		/// </remarks>
		public readonly byte Size;

		#endregion

		#region [ Constructor Overloads ]

		public PageNumberAndSize(int number)
			: this(number, DefaultPageSize)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				(number >= FirstPageNumber),
				"An ordinal page number is not a zero-based index. The number must be at least one.");
		}

		public PageNumberAndSize(int number, byte size)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				(number >= FirstPageNumber),
				"An ordinal page number is not a zero-based index. The number must be at least one.");

			// Beware of possible division by zero.
			Contract.Requires<ArgumentOutOfRangeException>(
				(size >= MinimumPageSize),
				"There must be at least one item per page or there could be division by zero!");

			// With code contracts in place, the following generates compiler warnings:
			////	if (number < FirstPageNumber)
			////	{
			////		throw new ArgumentOutOfRangeException(
			////			"number", number, "An ordinal page number is not a zero-based index. The number must be at least one.");
			////	}
			////
			////	if (size < MinimumPageSize)
			////	{
			////		// Beware of possible division by zero.
			////		throw new ArgumentOutOfRangeException(
			////			"size", size, "There must be at least one item per page or there could be division by zero!");
			////	}

			this.Number = number;
			this.Size = size;
		}

		private PageNumberAndSize(bool unbounded)
		{
			// This private constructor exists only for the
			// initialization of the static Unbounded value.
			this.Size = byte.MinValue;

			// If the "unbounded" parameter is false,
			// then this constructor creates the same
			// as the default constructor.
			this.Number = unbounded ? FirstPageNumber : 0;
		}

		#endregion

		#region [ Public ReadOnly HasValue and Index Properties ]

		/// <summary>
		/// Gets the zero-based index of a page within a "paged" collection of items,
		/// equal to the value of <see cref="Number"/> minus one.
		/// </summary>
		/// <remarks>
		/// If <see cref="IsUnbounded"/> is <c>true</c>,
		/// then this property returns a value of zero.
		/// </remarks>
		public int Index
		{
			get { return (this.Number - 1); }
		}

		/// <summary>
		/// Gets a value indicating whether the
		/// <see cref="Number"/> and <see cref="Size"/>
		/// values are set to indicate that all items
		/// should be shown on a single page of
		/// unbounded size.
		/// </summary>
		/// <remarks>
		/// When this value is <c>true</c>, there is a risk
		/// of division by zero because the <see cref="Size"/>
		/// value is zero.
		/// </remarks>
		public bool IsUnbounded
		{
			get { return ((this.Size == byte.MinValue) && (this.Number == FirstPageNumber)); }
		}

		/// <summary>
		/// Gets a value indicating whether the
		/// <see cref="Number"/> and <see cref="Size"/>
		/// values are valid.
		/// </summary>
		public bool IsValid
		{
			// There is no need to check this.Size >= byte.MinValue
			// unless the type of this.Size changes to a signed type.
			get { return (this.Number >= FirstPageNumber); }
		}

		#endregion

		// TODO: Override implicit operators for comparing PageNumber to Int32.
		// TODO: Test JSON Serializability, test ToString();
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
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(PageNumberAndSize other)
		{
			return ((this.Number == other.Number) && (this.Size == other.Size));
		}

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
			if (obj == null) { return false; }
			if (obj.GetType() != this.GetType()) { return false; }

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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static bool operator ==(PageNumberAndSize x, PageNumberAndSize y)
		{
			return x.Equals(y);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static bool operator !=(PageNumberAndSize x, PageNumberAndSize y)
		{
			return (!x.Equals(y));
		}

		#endregion

		#region [ Explicit Implementation of IEquatable<ITenantIdentifier> ]

		bool IEquatable<PageNumberAndSize>.Equals(PageNumberAndSize other)
		{
			// Return the public method.
			// Using an explicit implementation is a way
			// to avoid accidental boxing or unboxing.
			return this.Equals(other);
		}

		#endregion
	}
}