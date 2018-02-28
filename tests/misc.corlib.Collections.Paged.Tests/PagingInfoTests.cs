using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace MiscCorLib.Collections.Paged
{
	public sealed partial class PagingInfoTests
	{
		public sealed class DefaultValue
		{
			private const int TestTotalItems = 1138;
			private readonly PagingState defaultPagingState = new PagingState(PageNumberAndSize.Default, TestTotalItems);
			private readonly PagingInfo defaultPagingInfo;

			public DefaultValue()
			{
				defaultPagingInfo = new PagingInfo(defaultPagingState);
			}

			[Fact]
			public void HasValidItemNumbers()
			{
				Assert.Equal(1, this.defaultPagingInfo.FirstItemNumber);
				Assert.Equal(this.defaultPagingInfo.State.CurrentPage.Size, this.defaultPagingInfo.LastItemNumber);

				Assert.Equal(this.defaultPagingInfo.FirstItemNumber - 1, this.defaultPagingInfo.FirstItemIndex);
				Assert.Equal(this.defaultPagingInfo.LastItemNumber - 1, this.defaultPagingInfo.LastItemIndex);
			}

			[Fact]
			public void HasValidFirstAndLastPages()
			{
				AssertIsFirstPage(this.defaultPagingInfo);

				PageNumberAndSizeTests.AssertIsFirstPage(this.defaultPagingInfo.State.CurrentPage);
				PageNumberAndSizeTests.AssertIsFirstPage(this.defaultPagingInfo.State.FirstPage);

				Assert.InRange(this.defaultPagingInfo.State.CurrentPage.Size, 1, byte.MaxValue);
				Assert.InRange(this.defaultPagingInfo.State.FirstPage.Size, 1, byte.MaxValue);
				Assert.InRange(this.defaultPagingInfo.State.LastPage.Size, 1, byte.MaxValue);

				Assert.Equal(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfo.ItemCount);
				Assert.Equal(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfo.State.CurrentPage.Size);
				Assert.Equal(this.defaultPagingInfo.State.CurrentPage.Size, this.defaultPagingInfo.State.FirstPage.Size);
				Assert.Equal(this.defaultPagingInfo.State.CurrentPage.Size, this.defaultPagingInfo.State.LastPage.Size);
				Assert.Equal(this.defaultPagingInfo.State.CurrentPage.Size, this.defaultPagingInfo.ItemCount);
			}

			[Fact]
			public void HasValidNextPageAndEmptyPreviousPage()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.defaultPagingInfo.State.PreviousPage);

				Assert.Equal(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfo.State.CurrentPage.Size);
				Assert.Equal(this.defaultPagingInfo.State.CurrentPage.Size, this.defaultPagingInfo.State.NextPage.Size);
			}

			[Fact]
			public void HasFullSetOfAllPages()
			{
				IReadOnlyList<PageItemNumbers> pages = new PagingInfo(this.defaultPagingState).CalculateAllPagesAndItemNumbers().ToList();

				Assert.Equal(114, pages.Count);
				Assert.Equal(
					this.defaultPagingInfo.TotalItems,
					pages[pages.Count - 1].LastItemNumber);

				int lastItemNumber = 0;
				for (int i = PageNumberAndSize.PageOne; i <= pages.Count; i++)
				{
					int firstItemNumber = lastItemNumber + 1;
					int pageIndex = i - 1;
					PageItemNumbers page = pages[pageIndex];

					Assert.Equal(i, page.PageNumber);
					Assert.Equal(firstItemNumber, page.FirstItemNumber);

					lastItemNumber = page.LastItemNumber;
				}

				Assert.Equal(this.defaultPagingInfo.TotalItems, lastItemNumber);
				Assert.True(pages[0].HasValue);
			}

			[Fact]
			public void ConvertsToString()
			{
				Assert.Equal("PagingInfo[Page[Number=1,Size=10],TotalItems=1138]", this.defaultPagingInfo.ToString());
			}
		}

		public sealed class DefaultValueOnLastPage
		{
			private const int TestTotalItems = 1138;
			private readonly PagingInfo defaultPagingInfoLastPage
				= new PageNumberAndSize(250, 10).WithTotalItems(TestTotalItems);

			[Fact]
			public void HasValidItemNumbers()
			{
				Assert.Equal(1131, this.defaultPagingInfoLastPage.FirstItemNumber);
				Assert.Equal(1138, this.defaultPagingInfoLastPage.LastItemNumber);

				Assert.Equal(this.defaultPagingInfoLastPage.FirstItemNumber - 1, this.defaultPagingInfoLastPage.FirstItemIndex);
				Assert.Equal(this.defaultPagingInfoLastPage.LastItemNumber - 1, this.defaultPagingInfoLastPage.LastItemIndex);
			}

			[Fact]
			public void HasValidFirstAndLastPages()
			{
				AssertIsLastPage(this.defaultPagingInfoLastPage);

				Assert.InRange(this.defaultPagingInfoLastPage.State.CurrentPage.Size, 1, byte.MaxValue);
				Assert.InRange(this.defaultPagingInfoLastPage.State.FirstPage.Size, 1, byte.MaxValue);
				Assert.InRange(this.defaultPagingInfoLastPage.State.LastPage.Size, 1, byte.MaxValue);

				Assert.Equal(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfoLastPage.ItemCount);
				Assert.Equal(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfoLastPage.State.CurrentPage.Size);
				Assert.Equal(this.defaultPagingInfoLastPage.State.CurrentPage.Size, this.defaultPagingInfoLastPage.State.FirstPage.Size);
				Assert.Equal(this.defaultPagingInfoLastPage.State.CurrentPage.Size, this.defaultPagingInfoLastPage.State.LastPage.Size);
				Assert.Equal(this.defaultPagingInfoLastPage.State.CurrentPage.Size, this.defaultPagingInfoLastPage.ItemCount);
			}

			[Fact]
			public void HasValidNextPageAndEmptyPreviousPage()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.defaultPagingInfoLastPage.NextPage);

				Assert.Equal(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfoLastPage.State.CurrentPage.Size);
				Assert.Equal(this.defaultPagingInfoLastPage.State.CurrentPage.Size, this.defaultPagingInfoLastPage.PreviousPage.Size);
			}

			[Fact]
			public void HasFullSetOfAllPages()
			{
				IReadOnlyList<PageItemNumbers> pages = this.defaultPagingInfoLastPage.AllPagesAndItemNumbers();

				Assert.Equal(114, pages.Count);
				Assert.Equal(
					this.defaultPagingInfoLastPage.TotalItems,
					pages[pages.Count - 1].LastItemNumber);

				int lastItemNumber = 0;
				for (int i = PageNumberAndSize.PageOne; i <= pages.Count; i++)
				{
					int firstItemNumber = lastItemNumber + 1;
					int pageIndex = i - 1;
					PageItemNumbers page = pages[pageIndex];

					Assert.Equal(i, page.PageNumber);
					Assert.Equal(firstItemNumber, page.FirstItemNumber);

					lastItemNumber = page.LastItemNumber;
				}

				Assert.Equal(this.defaultPagingInfoLastPage.TotalItems, lastItemNumber);
				Assert.True(pages[0].HasValue);
			}

			[Fact]
			public void ConvertsToString()
			{
				Assert.Equal("PagingInfo[Page[Number=114,Size=10],TotalItems=1138]", this.defaultPagingInfoLastPage.ToString());
			}
		}

		public sealed class DefaultValueWithNoItems
		{
			private const int ZeroTotalItems = 0;
			private readonly PagingInfo defaultPagingInfo = new PagingInfo(
				new PageNumberAndSize(250, 20), ZeroTotalItems);

			[Fact]
			public void HasValidItemNumbers()
			{
				Assert.Equal(0, this.defaultPagingInfo.FirstItemNumber);
				Assert.Equal(0, this.defaultPagingInfo.LastItemNumber);
				Assert.Equal(-1, this.defaultPagingInfo.FirstItemIndex);
				Assert.Equal(-1, this.defaultPagingInfo.LastItemIndex);
			}

			[Fact]
			public void HasValidFirstAndLastPages()
			{
				PageNumberAndSizeTests.AssertIsFirstPage(this.defaultPagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsFirstPage(this.defaultPagingInfo.FirstPage);

				AssertIsFirstPage(this.defaultPagingInfo);
				AssertIsLastPage(this.defaultPagingInfo);

				Assert.Equal(20, this.defaultPagingInfo.State.CurrentPage.Size);
				Assert.Equal(20, this.defaultPagingInfo.State.FirstPage.Size);
				Assert.Equal(this.defaultPagingInfo.TotalItems, this.defaultPagingInfo.ItemCount);
				Assert.Null(this.defaultPagingInfo.AllPages);
				Assert.True(this.defaultPagingInfo.IsFirstPage);
				Assert.True(this.defaultPagingInfo.IsLastPage);
			}

			[Fact]
			public void HasEmptyNextAndPreviousPages()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.defaultPagingInfo.NextPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.defaultPagingInfo.PreviousPage);
			}

			[Fact]
			public void HasZeroItems()
			{
				Assert.Equal(-1, this.defaultPagingInfo.FirstItemIndex);
				Assert.Equal(-1, this.defaultPagingInfo.LastItemIndex);
				Assert.Equal(ZeroTotalItems, this.defaultPagingInfo.TotalItems);
				Assert.Equal(ZeroTotalItems, this.defaultPagingInfo.ItemCount);
				Assert.Equal(1, this.defaultPagingInfo.TotalPages);
			}

			[Fact]
			public void HasOneEmptyPage()
			{
				IReadOnlyList<PageItemNumbers> pages = this.defaultPagingInfo.AllPagesAndItemNumbers();

				Assert.Equal(1, pages.Count);
				Assert.Equal(0, pages[0].FirstItemNumber);
				Assert.Equal(0, pages[0].LastItemNumber);
				Assert.Equal(PageNumberAndSize.PageOne, pages[0].PageNumber);
				Assert.True(pages[0].HasValue);
			}

			[Fact]
			public void ConvertsToString()
			{
				Assert.Equal("PagingInfo[Page[Number=1,Size=20],TotalItems=0]", this.defaultPagingInfo.ToString());
			}
		}

		public sealed class EmptyValue
		{
			private readonly PagingInfo emptyPagingInfo = new PagingInfo();

			[Fact]
			public void HasValidItemNumbers()
			{
				Assert.Equal(0, this.emptyPagingInfo.FirstItemNumber);
				Assert.Equal(0, this.emptyPagingInfo.LastItemNumber);
				Assert.Equal(-1, this.emptyPagingInfo.FirstItemIndex);
				Assert.Equal(-1, this.emptyPagingInfo.LastItemIndex);
			}

			[Fact]
			public void HasEmptyPages()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.emptyPagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.emptyPagingInfo.FirstPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.emptyPagingInfo.LastPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.emptyPagingInfo.NextPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.emptyPagingInfo.PreviousPage);

				Assert.Equal(0, this.emptyPagingInfo.ItemCount);
				Assert.Null(this.emptyPagingInfo.AllPages);
				Assert.False(this.emptyPagingInfo.IsFirstPage);
				Assert.False(this.emptyPagingInfo.IsLastPage);
			}

			[Fact]
			public void HasUninitializedItemIndices()
			{
				Assert.Equal(-1, this.emptyPagingInfo.FirstItemIndex);
				Assert.Equal(-1, this.emptyPagingInfo.LastItemIndex);
			}

			[Fact]
			public void HasZeroTotalItems()
			{
				Assert.Equal(0, this.emptyPagingInfo.TotalItems);
				Assert.Equal(0, this.emptyPagingInfo.TotalPages);
				Assert.Equal(0, this.emptyPagingInfo.ItemCount);
			}

			[Fact]
			public void HasNoPages()
			{
				IReadOnlyList<PageItemNumbers> pages = this.emptyPagingInfo.AllPagesAndItemNumbers();

				Assert.Equal(0, pages.Count);
			}

			[Fact]
			public void ConvertsToString()
			{
				Assert.Equal("PagingInfo[Page[Number=0,Size=0],TotalItems=0]", this.emptyPagingInfo.ToString());
			}
		}

		public sealed class UnboundedValueWithManyItems
		{
			private const int TestTotalItems = int.MaxValue / 33;
			private readonly PagingInfo unboundedPagingInfo = new PagingInfo(
				PageNumberAndSize.Unbounded, TestTotalItems);

			[Fact]
			public void HasValidItemNumbers()
			{
				Assert.Equal(1, this.unboundedPagingInfo.FirstItemNumber);
				Assert.Equal(this.unboundedPagingInfo.TotalItems, this.unboundedPagingInfo.LastItemNumber);

				Assert.Equal(this.unboundedPagingInfo.FirstItemNumber - 1, this.unboundedPagingInfo.FirstItemIndex);
				Assert.Equal(this.unboundedPagingInfo.LastItemNumber - 1, this.unboundedPagingInfo.LastItemIndex);
			}

			[Fact]
			public void HasValidFirstAndLastPages()
			{
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.FirstPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.LastPage);

				AssertIsFirstPage(this.unboundedPagingInfo);
				AssertIsLastPage(this.unboundedPagingInfo);

				Assert.Equal(this.unboundedPagingInfo.TotalItems, this.unboundedPagingInfo.ItemCount);
				Assert.Null(this.unboundedPagingInfo.AllPages);
				Assert.True(this.unboundedPagingInfo.IsFirstPage);
				Assert.True(this.unboundedPagingInfo.IsLastPage);
			}

			[Fact]
			public void HasEmptyNextAndPreviousPages()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.unboundedPagingInfo.NextPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.unboundedPagingInfo.PreviousPage);
			}

			[Fact]
			public void HasAllItems()
			{
				Assert.Equal(0, this.unboundedPagingInfo.FirstItemIndex);
				Assert.Equal(TestTotalItems - 1, this.unboundedPagingInfo.LastItemIndex);
				Assert.Equal(TestTotalItems, this.unboundedPagingInfo.TotalItems);
				Assert.Equal(TestTotalItems, this.unboundedPagingInfo.ItemCount);
				Assert.Equal(1, this.unboundedPagingInfo.TotalPages);
			}

			[Fact]
			public void HasOneHugePage()
			{
				IReadOnlyList<PageItemNumbers> pages = this.unboundedPagingInfo.AllPagesAndItemNumbers();

				Assert.Equal(1, pages.Count);
				Assert.Equal(1, pages[0].FirstItemNumber);
				Assert.Equal(this.unboundedPagingInfo.TotalItems, pages[0].LastItemNumber);
				Assert.Equal(PageNumberAndSize.PageOne, pages[0].PageNumber);
				Assert.True(pages[0].HasValue);
			}

			[Fact]
			public void ConvertsToString()
			{
				Assert.Equal("PagingInfo[Page[Number=1,Size=0],TotalItems=65075262]", this.unboundedPagingInfo.ToString());
			}
		}

		public sealed class UnboundedValueWithNoItems
		{
			private const int ZeroTotalItems = 0;
			private readonly PagingInfo unboundedPagingInfo = new PagingInfo(
				PageNumberAndSize.Unbounded, ZeroTotalItems);

			[Fact]
			public void HasValidItemNumbers()
			{
				Assert.Equal(0, this.unboundedPagingInfo.FirstItemNumber);
				Assert.Equal(0, this.unboundedPagingInfo.LastItemNumber);
				Assert.Equal(-1, this.unboundedPagingInfo.FirstItemIndex);
				Assert.Equal(-1, this.unboundedPagingInfo.LastItemIndex);
			}

			[Fact]
			public void HasValidFirstAndLastPages()
			{
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.FirstPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.LastPage);

				AssertIsFirstPage(this.unboundedPagingInfo);
				AssertIsLastPage(this.unboundedPagingInfo);

				Assert.Equal(this.unboundedPagingInfo.TotalItems, this.unboundedPagingInfo.ItemCount);
				Assert.Null(this.unboundedPagingInfo.AllPages);
				Assert.True(this.unboundedPagingInfo.IsFirstPage);
				Assert.True(this.unboundedPagingInfo.IsLastPage);
			}

			[Fact]
			public void HasEmptyNextAndPreviousPages()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.unboundedPagingInfo.NextPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.unboundedPagingInfo.PreviousPage);
			}

			[Fact]
			public void HasZeroItems()
			{
				Assert.Equal(-1, this.unboundedPagingInfo.FirstItemIndex);
				Assert.Equal(-1, this.unboundedPagingInfo.LastItemIndex);
				Assert.Equal(ZeroTotalItems, this.unboundedPagingInfo.TotalItems);
				Assert.Equal(ZeroTotalItems, this.unboundedPagingInfo.ItemCount);
				Assert.Equal(1, this.unboundedPagingInfo.TotalPages);
			}

			[Fact]
			public void HasOneEmptyPage()
			{
				IReadOnlyList<PageItemNumbers> pages = this.unboundedPagingInfo.AllPagesAndItemNumbers();

				Assert.Equal(1, pages.Count);
				Assert.Equal(0, pages[0].FirstItemNumber);
				Assert.Equal(0, pages[0].LastItemNumber);
				Assert.Equal(PageNumberAndSize.PageOne, pages[0].PageNumber);
				Assert.True(pages[0].HasValue);
			}

			[Fact]
			public void ConvertsToString()
			{
				Assert.Equal("PagingInfo[Page[Number=1,Size=0],TotalItems=0]", this.unboundedPagingInfo.ToString());
			}
		}

		public sealed class Calculator
		{
			[Fact]
			public void MovesToLastPageWhenTurningPast()
			{
				// Try to get to the 27th page
				// when there are only 39 items
				// in pages of 20...
				PagingInfo pagingInfo = new PageNumberAndSize(27, 20).WithTotalItems(39);

				pagingInfo.State.CurrentPage.Number.Should().Be(2, "turning past the last page of a paged list should return to the last page of the list");

				AssertIsLastPage(pagingInfo);
			}

			[Fact]
			public void CalculatesCorrectTotalPages()
			{
				Assert.Throws<ArgumentOutOfRangeException>(
					() => Paging.CalculateTotalPages(10, -255));

				Assert.Throws<ArgumentOutOfRangeException>(
					() => Paging.CalculateTotalPages(0, 1138));

				Assert.Equal(0, Paging.CalculateTotalPages(10, 0));
				Assert.Equal(1, Paging.CalculateTotalPages(10, 1));
				Assert.Equal(1, Paging.CalculateTotalPages(10, 10));
				Assert.Equal(2, Paging.CalculateTotalPages(10, 11));
				Assert.Equal(2, Paging.CalculateTotalPages(10, 20));
			}
		}

		public sealed class Equality
		{
			private readonly PagingInfo samplePagingInfo
				= new PageNumberAndSize(7, 20).WithTotalItems(1138);

			[Fact]
			public void IsTrueWhenSame()
			{
				PagingInfo samePagingInfo
					= new PageNumberAndSize(7, 20).WithTotalItems(1138);

				AssertEquality(this.samplePagingInfo, samePagingInfo);
			}

			[Fact]
			public void IsTrueWhenEmpty()
			{
				PagingInfo empty1 = new PagingInfo();
				PagingInfo empty2 = PagingInfo.Empty;

				AssertEquality(empty1, empty2);
				AssertInequality(this.samplePagingInfo, PagingState.Empty);
			}

			[Fact]
			public void IsTrueWhenDeserialized()
			{
				PagingState deserializedPagingInfo
					= Newtonsoft.Json.JsonConvert.DeserializeObject<PagingState>(
						DeserializeMinimal_Page7_Size20_Total1138);

				AssertEquality(this.samplePagingInfo, deserializedPagingInfo);
			}

			[Fact]
			public void IsTrueWhenHasAllPages()
			{
				PagingState samePagingInfo = new PagingState(7, 20, 1138, true);

				AssertEquality(this.samplePagingInfo, samePagingInfo);
			}

			[Fact]
			public void IsFalseWhenDifferentPageNumber()
			{
				PagingState differentPageNumber = new PagingState(8, 20, 1138);

				AssertInequality(samplePagingInfo, differentPageNumber);
			}

			[Fact]
			public void IsFalseWhenDifferentPageSize()
			{
				PagingState differentPageSize = new PagingState(7, 19, 1138);

				AssertInequality(samplePagingInfo, differentPageSize);
			}

			public void IsFalseWhenDifferentTotalItems()
			{
				PagingState differentTotalItems = new PagingState(7, 20, 1137);

				AssertInequality(samplePagingInfo, differentTotalItems);
			}
		}

		public sealed class TurnToPage
		{
			[Fact]
			public void DefaultCaseReusesSize()
			{
				PagingState validPagingInfo = new PagingState(3, 10, 57);
				PageNumberAndSize newPage = validPagingInfo.TurnToPage(2);

				Assert.Equal(2, newPage.Number);
				Assert.Equal(10, newPage.Size);
				Assert.False(newPage.IsUnbounded);
			}

			[Fact]
			public void AllowsTurningPastLastPage()
			{
				PagingState validPagingInfo = new PagingState(3, 10, 57);
				PageNumberAndSize newPage = validPagingInfo.TurnToPage(8);

				Assert.Equal(8, newPage.Number);
				Assert.Equal(10, newPage.Size);
				Assert.False(newPage.IsUnbounded);
			}

			[Fact]
			public void ReturnsUnboundedFromUnbounded()
			{
				PagingInfo unboundedPagingInfo = PageNumberAndSize.Unbounded.WithTotalItems(57);
				PagingInfo newPage = unboundedPagingInfo.TurnToPage(8);

				Assert.Equal(1, newPage.PageNumber);
				Assert.Equal(0, newPage.PageSize);
				Assert.True(newPage.IsUnbounded);
			}

			[Fact]
			public void ReturnsEmptyFromEmpty()
			{
				PageNumberAndSize newPage = PagingState.Empty.TurnToPage(6);

				PageNumberAndSizeTests.AssertIsEmpty(newPage);
			}
		}

		internal static void AssertIsFirstPage(PagingInfo firstPageInfo)
		{
			PageNumberAndSizeTests.AssertIsFirstPage(firstPageInfo.CurrentPage);

			Assert.Equal(0, firstPageInfo.State.CurrentPage.Index);
			Assert.Equal(
				PageNumberAndSize.PageOne,
				firstPageInfo.State.CurrentPage.Number);

			Assert.True(firstPageInfo.HasValue);
			Assert.False(firstPageInfo.PreviousPage.HasValue);
			Assert.True(firstPageInfo.IsFirstPage);

			PageNumberAndSizeTests.AssertEquality(
				firstPageInfo.FirstPage, firstPageInfo.CurrentPage);
		}

		internal static void AssertIsLastPage(PagingInfo currentPageInfo)
		{
			currentPageInfo.IsLastPage.Should().BeTrue("the last page of a paged list should indicate so");

			PagingInfo lastPageInfo = currentPageInfo.TurnToPage(currentPageInfo.TotalPages);
			currentPageInfo.PageNumber.Should().Be(lastPageInfo.PageNumber, "the last page's PageNumber should be the same as the TotalPages");

			PageNumberAndSizeTests.AssertEquality(
				lastPageInfo.State.CurrentPage, lastPageInfo.State.CurrentPage);
		}
	}
}