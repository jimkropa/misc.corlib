using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MiscCorLib.Collections
{
	[Serializable, DataContract]
	public struct PagingResources
	{
		public PagingResources(PagingInfo pagingInfo)
		{
			this.CurrentPage = pagingInfo.State.CurrentPage;

			if ((pagingInfo.TotalItems > 0) && (pagingInfo.PageSize > 0))
			{
				if (pagingInfo.IsFirstPage)
				{
					this.FirstPage = this.CurrentPage;
					this.PreviousPage = PageNumberAndSize.Empty;
				}
				else
				{
					this.FirstPage = new PageNumberAndSize(
						PageNumberAndSize.FirstPageNumber, this.CurrentPage.Size);

					this.PreviousPage = new PageNumberAndSize(
						this.CurrentPage.Number - 1, this.CurrentPage.Size);
				}

				if (pagingInfo.IsLastPage)
				{
					this.LastPage = this.CurrentPage;
					this.NextPage = PageNumberAndSize.Empty;
				}
				else
				{
					this.LastPage = new PageNumberAndSize(
						pagingInfo.TotalPages, this.CurrentPage.Size);

					this.NextPage = new PageNumberAndSize(
						this.CurrentPage.Number + 1, this.CurrentPage.Size);
				}
			}
			else
			{
				this.PreviousPage = PageNumberAndSize.Empty;
				this.NextPage = PageNumberAndSize.Empty;
				this.FirstPage = this.CurrentPage;
				this.LastPage = this.CurrentPage;
			}
		}

		[DataMember(Order = 1)]
		public readonly PageNumberAndSize FirstPage;

		[DataMember(Order = 2)]
		public readonly PageNumberAndSize PreviousPage;

		[DataMember(Order = 3)]
		public readonly PageNumberAndSize CurrentPage;

		[DataMember(Order = 4)]
		public readonly PageNumberAndSize NextPage;

		[DataMember(Order = 5)]
		public readonly PageNumberAndSize LastPage;
	}
}