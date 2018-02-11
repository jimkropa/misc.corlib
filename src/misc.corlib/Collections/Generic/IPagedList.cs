using System;
using System.Collections.Generic;

namespace MiscCorLib.Collections.Generic
{
	/// <summary>
	/// Represents a read-only collection of elements
	/// that can be accessed by index, and which are
	/// grouped onto one "page" in a longer list of items
	/// which is broken into multiple pages. Adds the
	/// <see cref="IPagedCollection" /> contract to
	/// <see cref="IReadOnlyList{T}" /> to provide
	/// metadata about this "page" of a longer list.
	/// </summary>
	/// <typeparam name="T">
	/// The type of elements in the paged read-only list.
	/// </typeparam>
	public interface IPagedList<out T> : IReadOnlyList<T>, IPagedCollection
	{
	}
}