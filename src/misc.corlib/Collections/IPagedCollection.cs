namespace MiscCorLib.Collections
{
	using System;
	using System.Collections;

	/// <summary>
	/// Contract for one "page" in a longer list of items
	/// which is broken into multiple pages, and which
	/// contains metadata about the position of the
	/// page within the longer list.
	/// </summary>
	[CLSCompliant(true)]
	public interface IPagedCollection : IEnumerable
	{
		/// <summary>
		/// Gets the metadata about this "page" of
		/// a longer list which spans multiple pages.
		/// </summary>
		PagingInfo PagingInfo { get; }
	}
}