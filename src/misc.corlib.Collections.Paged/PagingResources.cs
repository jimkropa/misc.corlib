using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MiscCorLib.Collections.Paged
{
	[Serializable, DataContract]
	public struct PagingResources : IHasValue
	{
		public static PagingResources Empty = new PagingResources();

		public PagingResources(PagingInfo pagingInfo)
		{
			if (!pagingInfo.HasValue)
			{
				throw new ArgumentException(
					"The paging metadata must have a value.",
					nameof(pagingInfo));
			}

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
						PageNumberAndSize.PageOne, this.CurrentPage.Size);

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

		/// <summary>
		/// Gets a value indicating whether the
		/// <see cref="PageItemNumbers" /> value is valid.
		/// </summary>
		public bool HasValue => this.CurrentPage.HasValue;

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

		/// <summary>
		/// Converts this value to its equivalent string representation.
		/// </summary>
		/// <returns>
		/// The string representation of this value.
		/// </returns>
		public override string ToString()
		{
			return $"PagingResources[CurrentPage={this.CurrentPage}]";
		}
	}
}