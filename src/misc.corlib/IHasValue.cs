#region [ license and copyright boilerplate ]
/*
	MiscCorLib
	IHasValue.cs

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

namespace MiscCorLib
{
	using System;

	/// <summary>
	/// Similar to the <see cref="Nullable{T}.HasValue"/>
	/// property of <see cref="Nullable{T}"/>, adds a
	/// <see cref="HasValue"/> property to an object or struct.
	/// </summary>
	public interface IHasValue
	{
		/// <summary>
		/// Gets a value indicating whether this object
		/// or struct is considered to have a value.
		/// If <c>false</c>, then it is considered to be empty.
		/// </summary>
		bool HasValue { get; }
	}
}