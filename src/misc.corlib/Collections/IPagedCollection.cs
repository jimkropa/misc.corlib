#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Collections
	IPagedCollection.cs

	Copyright (c) 2016 Jim Kropa (https://github.com/jimkropa)

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

		http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/
#endregion

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