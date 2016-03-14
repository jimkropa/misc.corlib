#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Collections.Generic
	IPagedList.cs

	Copyright (c) 2015 Jim Kropa (http://www.kropa.net/)

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

namespace MiscCorLib.Collections.Generic
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Represents a read-only collection of elements
	/// that can be accessed by index, and which are
	/// grouped onto one "page" in a longer list of items
	/// which is broken into multiple pages. Adds the
	/// <see cref="IPagedCollection"/> contract to
	/// <see cref="IReadOnlyList{T}"/> to provide
	/// metadata about this "page" of a longer list.
	/// </summary>
	/// <typeparam name="T">
	/// The type of elements in the paged read-only list.
	/// </typeparam>
	[CLSCompliant(true)]
	public interface IPagedList<out T> : IReadOnlyList<T>, IPagedCollection
	{
	}
}