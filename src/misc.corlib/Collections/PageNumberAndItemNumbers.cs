namespace MiscCorLib.Collections
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Runtime.Serialization;

	[CLSCompliant(true), Serializable, DataContract]
	public struct PageNumberAndItemNumbers
	{
		public static PageNumberAndItemNumbers Empty = new PageNumberAndItemNumbers();

		[DataMember(IsRequired = true, Order = 0)]
		public readonly int PageNumber;

		[DataMember(IsRequired = true, Order = 1)]
		public int FirstItemNumber;

		[DataMember(IsRequired = true, Order = 2)]
		public int LastItemNumber;

		internal PageNumberAndItemNumbers(
			int pageNumber, byte pageSize, int totalItems)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				pageNumber >= PageNumberAndSize.FirstPageNumber,
				"An ordinal page number is not a zero-based index. The number must be at least one.");

			// Beware of possible division by zero.
			////	Contract.Requires<ArgumentOutOfRangeException>(
			////		pageSize >= PageNumberAndSize.MinimumPageSize,
			////		"There must be at least one item per page or there could be division by zero!");
			Contract.Requires<ArgumentOutOfRangeException>(
				totalItems >= 0, "The number of items in the list must not be negative!");

			if (pageSize > 0)
			{
				this.PageNumber = pageNumber;
				this.LastItemNumber = pageNumber * pageSize;
				this.FirstItemNumber = this.LastItemNumber - pageSize + 1;

				// Determine whether this is the last page.
				if ((totalItems + pageSize - 1) / pageSize == this.PageNumber)
				{
					this.LastItemNumber = totalItems;
				}
			}
			else
			{
				this.PageNumber = PageNumberAndSize.FirstPageNumber;
				this.FirstItemNumber = totalItems > 0 ? 1 : 0;
				this.LastItemNumber = totalItems;
			}
		}
	}
}