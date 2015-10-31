namespace MiscCorLib.Collections.Generic
{
	using System.Collections.Generic;

	public interface IPagedList<out T> : IReadOnlyList<T>, IPagedCollection
	{
	}
}