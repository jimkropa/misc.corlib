using System;
using System.Collections.Generic;

namespace MiscCorLib.Collections.Paged
{
	/// <summary>
	/// A set of functions to create and transform
	/// values describing paged lists, and the main
	/// API to interact with this library.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Defines a "Fluent" interface, implemented
	/// via static extension methods of
	/// <see cref="PageNumberAndSize" />
	/// and <see cref="PagingInfo" /> values.
	/// </para>
	/// <para>
	/// Also includes static methods
	/// </para>
	/// </remarks>
	public static partial class Paging
	{
		// This files has static methods
		// to begin a paging expression:
		// Paging.OnPage(3).ItemsPerPage(20).

		public static PageNumberAndSize UnboundedSinglePage => PageNumberAndSize.Unbounded;

		public static PageNumberAndSize OnPage(int pageNumber, byte pageSize)
		{
			return new PageNumberAndSize(pageNumber, pageSize);
		}

		public static PageNumberAndSize OnPage(int pageNumber)
		{
			return new PageNumberAndSize(pageNumber);
		}

		public static PageNumberAndSize ItemsPerPage(byte pageSize)
		{
			return new PageNumberAndSize(PageNumberAndSize.PageOne, pageSize);
		}
	}
}