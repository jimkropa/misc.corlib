using System;

namespace MiscCorLib
{
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