using System;
using System.Collections.Generic;
using Xunit;

namespace MiscCorLib.Collections
{
	public sealed partial class PagingInfoTests
	{
		public sealed class DefaultValue
		{
			private const int TestTotalItems = 1138;
			private readonly PagingInfo defaultPagingInfo = new PagingInfo(PageNumberAndSize.Default, TestTotalItems);

			[Fact]
			public void HasValidItemNumbers()
			{
				Assert.Equal(1, this.defaultPagingInfo.FirstItemNumber);
				Assert.Equal(this.defaultPagingInfo.CurrentPage.Size, this.defaultPagingInfo.LastItemNumber);

				Assert.Equal(this.defaultPagingInfo.FirstItemNumber - 1, this.defaultPagingInfo.FirstItemIndex);
				Assert.Equal(this.defaultPagingInfo.LastItemNumber - 1, this.defaultPagingInfo.LastItemIndex);
			}

			[Fact]
			public void HasValidFirstAndLastPages()
			{
				AssertIsFirstPage(this.defaultPagingInfo);

				PageNumberAndSizeTests.AssertIsFirstPage(this.defaultPagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsFirstPage(this.defaultPagingInfo.FirstPage);

				Assert.InRange(this.defaultPagingInfo.CurrentPage.Size, 1, byte.MaxValue);
				Assert.InRange(this.defaultPagingInfo.FirstPage.Size, 1, byte.MaxValue);
				Assert.InRange(this.defaultPagingInfo.LastPage.Size, 1, byte.MaxValue);

				Assert.Null(this.defaultPagingInfo.AllPages);
				Assert.Equal(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfo.ItemCount);
				Assert.Equal(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfo.CurrentPage.Size);
				Assert.Equal(this.defaultPagingInfo.CurrentPage.Size, this.defaultPagingInfo.FirstPage.Size);
				Assert.Equal(this.defaultPagingInfo.CurrentPage.Size, this.defaultPagingInfo.LastPage.Size);
				Assert.Equal(this.defaultPagingInfo.CurrentPage.Size, this.defaultPagingInfo.ItemCount);
			}

			[Fact]
			public void HasValidNextPageAndEmptyPreviousPage()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.defaultPagingInfo.PreviousPage);

				Assert.Equal(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfo.CurrentPage.Size);
				Assert.Equal(this.defaultPagingInfo.CurrentPage.Size, this.defaultPagingInfo.NextPage.Size);
			}

			[Fact]
			public void HasFullSetOfAllPages()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.defaultPagingInfo.AllPagesAndItemNumbers();

				Assert.Equal(114, pages.Count);
				Assert.Equal(
					this.defaultPagingInfo.TotalItems,
					pages[pages.Count - 1].LastItemNumber);

				int lastItemNumber = 0;
				for (int i = PageNumberAndSize.FirstPageNumber; i <= pages.Count; i++)
				{
					int firstItemNumber = lastItemNumber + 1;
					int pageIndex = i - 1;
					PageNumberAndItemNumbers page = pages[pageIndex];

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
			private readonly PagingInfo defaultPagingInfoLastPage = new PagingInfo(250, 10, TestTotalItems);

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

				Assert.InRange(this.defaultPagingInfoLastPage.CurrentPage.Size, 1, byte.MaxValue);
				Assert.InRange(this.defaultPagingInfoLastPage.FirstPage.Size, 1, byte.MaxValue);
				Assert.InRange(this.defaultPagingInfoLastPage.LastPage.Size, 1, byte.MaxValue);

				Assert.Null(this.defaultPagingInfoLastPage.AllPages);
				Assert.Equal(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfoLastPage.ItemCount);
				Assert.Equal(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfoLastPage.CurrentPage.Size);
				Assert.Equal(this.defaultPagingInfoLastPage.CurrentPage.Size, this.defaultPagingInfoLastPage.FirstPage.Size);
				Assert.Equal(this.defaultPagingInfoLastPage.CurrentPage.Size, this.defaultPagingInfoLastPage.LastPage.Size);
				Assert.Equal(this.defaultPagingInfoLastPage.CurrentPage.Size, this.defaultPagingInfoLastPage.ItemCount);
			}

			[Fact]
			public void HasValidNextPageAndEmptyPreviousPage()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.defaultPagingInfoLastPage.NextPage);

				Assert.Equal(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfoLastPage.CurrentPage.Size);
				Assert.Equal(this.defaultPagingInfoLastPage.CurrentPage.Size, this.defaultPagingInfoLastPage.PreviousPage.Size);
			}

			[Fact]
			public void HasFullSetOfAllPages()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.defaultPagingInfoLastPage.AllPagesAndItemNumbers();

				Assert.Equal(114, pages.Count);
				Assert.Equal(
					this.defaultPagingInfoLastPage.TotalItems,
					pages[pages.Count - 1].LastItemNumber);

				int lastItemNumber = 0;
				for (int i = PageNumberAndSize.FirstPageNumber; i <= pages.Count; i++)
				{
					int firstItemNumber = lastItemNumber + 1;
					int pageIndex = i - 1;
					PageNumberAndItemNumbers page = pages[pageIndex];

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

				Assert.Equal(20, this.defaultPagingInfo.CurrentPage.Size);
				Assert.Equal(20, this.defaultPagingInfo.FirstPage.Size);
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
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.defaultPagingInfo.AllPagesAndItemNumbers();

				Assert.Equal(1, pages.Count);
				Assert.Equal(0, pages[0].FirstItemNumber);
				Assert.Equal(0, pages[0].LastItemNumber);
				Assert.Equal(PageNumberAndSize.FirstPageNumber, pages[0].PageNumber);
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
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.emptyPagingInfo.AllPagesAndItemNumbers();

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
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.unboundedPagingInfo.AllPagesAndItemNumbers();

				Assert.Equal(1, pages.Count);
				Assert.Equal(1, pages[0].FirstItemNumber);
				Assert.Equal(this.unboundedPagingInfo.TotalItems, pages[0].LastItemNumber);
				Assert.Equal(PageNumberAndSize.FirstPageNumber, pages[0].PageNumber);
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
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.unboundedPagingInfo.AllPagesAndItemNumbers();

				Assert.Equal(1, pages.Count);
				Assert.Equal(0, pages[0].FirstItemNumber);
				Assert.Equal(0, pages[0].LastItemNumber);
				Assert.Equal(PageNumberAndSize.FirstPageNumber, pages[0].PageNumber);
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
			private readonly PagingInfo readonlyPagingInfo = new PagingInfo();

			[Fact]
			public void DoesNotDisruptReadOnlyReference()
			{
				Assert.Null(this.readonlyPagingInfo.AllPages);

				System.Reflection.PropertyInfo privateProperty
					= this.readonlyPagingInfo.GetType().GetProperty("Calculator",
						System.Reflection.BindingFlags.NonPublic
						| System.Reflection.BindingFlags.Instance);

				privateProperty.GetMethod.Invoke(this.readonlyPagingInfo, null);

				Assert.True(PagingInfo.Empty.Equals(this.readonlyPagingInfo));
			}

			[Fact]
			public void DoesNotDisruptDeserializedReference()
			{
				PagingInfo deserializedPagingInfo
					= Newtonsoft.Json.JsonConvert.DeserializeObject<PagingInfo>(
						DeserializeMinimal_Page7_Size20_Total1138);

				Assert.Null(deserializedPagingInfo.AllPages);

				Assert.Equal(7, deserializedPagingInfo.CurrentPage.Number);
				Assert.Equal(20, deserializedPagingInfo.CurrentPage.Size);
				Assert.Equal(1138, deserializedPagingInfo.TotalItems);

				Assert.Equal(57, deserializedPagingInfo.TotalPages);

				Assert.Null(deserializedPagingInfo.AllPages);

				Assert.Equal(7, deserializedPagingInfo.CurrentPage.Number);
				Assert.Equal(20, deserializedPagingInfo.CurrentPage.Size);
				Assert.Equal(1138, deserializedPagingInfo.TotalItems);

				Assert.False(deserializedPagingInfo.IsFirstPage);
				Assert.False(deserializedPagingInfo.IsLastPage);
			}

			[Fact]
			public void MovesToLastPageWhenTurningPast()
			{
				PagingInfo pagingInfo = new PagingInfo(27, 20, 39);

				Assert.Equal(2, pagingInfo.CurrentPage.Number);

				AssertIsLastPage(pagingInfo);
			}

			[Fact]
			public void CalculatesCorrectTotalPages()
			{
				Assert.Throws<ArgumentOutOfRangeException>(
					() => PagingInfoCalculator.CalculateTotalPages(10, 0));

				Assert.Throws<ArgumentOutOfRangeException>(
					() => PagingInfoCalculator.CalculateTotalPages(0, 1138));

				Assert.Equal(1, PagingInfoCalculator.CalculateTotalPages(10, 1));
				Assert.Equal(1, PagingInfoCalculator.CalculateTotalPages(10, 10));
				Assert.Equal(2, PagingInfoCalculator.CalculateTotalPages(10, 11));
				Assert.Equal(2, PagingInfoCalculator.CalculateTotalPages(10, 20));
			}
		}

		public sealed class Equality
		{
			private readonly PagingInfo samplePagingInfo = new PagingInfo(
				new PageNumberAndSize(7, 20), 1138);

			[Fact]
			public void IsTrueWhenSame()
			{
				PagingInfo samePagingInfo = new PagingInfo(7, 20, 1138);

				AssertEquality(this.samplePagingInfo, samePagingInfo);
			}

			[Fact]
			public void IsTrueWhenEmpty()
			{
				PagingInfo empty1 = new PagingInfo();
				PagingInfo empty2 = PagingInfo.Empty;

				AssertEquality(empty1, empty2);
				AssertInequality(this.samplePagingInfo, PagingInfo.Empty);
			}

			[Fact]
			public void IsTrueWhenDeserialized()
			{
				PagingInfo deserializedPagingInfo
					= Newtonsoft.Json.JsonConvert.DeserializeObject<PagingInfo>(
						DeserializeMinimal_Page7_Size20_Total1138);

				AssertEquality(this.samplePagingInfo, deserializedPagingInfo);
			}

			[Fact]
			public void IsTrueWhenHasAllPages()
			{
				PagingInfo samePagingInfo = new PagingInfo(7, 20, 1138, true);

				AssertEquality(this.samplePagingInfo, samePagingInfo);
			}

			[Fact]
			public void IsFalseWhenDifferentPageNumber()
			{
				PagingInfo differentPageNumber = new PagingInfo(8, 20, 1138);

				AssertInequality(samplePagingInfo, differentPageNumber);
			}

			[Fact]
			public void IsFalseWhenDifferentPageSize()
			{
				PagingInfo differentPageSize = new PagingInfo(7, 19, 1138);

				AssertInequality(samplePagingInfo, differentPageSize);
			}

			public void IsFalseWhenDifferentTotalItems()
			{
				PagingInfo differentTotalItems = new PagingInfo(7, 20, 1137);

				AssertInequality(samplePagingInfo, differentTotalItems);
			}
		}

		public sealed class TurnToPage
		{
			[Fact]
			public void DefaultCaseReusesSize()
			{
				PagingInfo validPagingInfo = new PagingInfo(3, 10, 57);
				PageNumberAndSize newPage = validPagingInfo.TurnToPage(2);

				Assert.Equal(2, newPage.Number);
				Assert.Equal(10, newPage.Size);
				Assert.False(newPage.IsUnbounded);
			}

			[Fact]
			public void AllowsTurningPastLastPage()
			{
				PagingInfo validPagingInfo = new PagingInfo(3, 10, 57);
				PageNumberAndSize newPage = validPagingInfo.TurnToPage(8);

				Assert.Equal(8, newPage.Number);
				Assert.Equal(10, newPage.Size);
				Assert.False(newPage.IsUnbounded);
			}

			[Fact]
			public void ReturnsUnboundedFromUnbounded()
			{
				PagingInfo unboundedPagingInfo = new PagingInfo(PageNumberAndSize.Unbounded, 57);
				PageNumberAndSize newPage = unboundedPagingInfo.TurnToPage(8);

				Assert.Equal(1, newPage.Number);
				Assert.Equal(0, newPage.Size);
				Assert.True(newPage.IsUnbounded);
			}

			[Fact]
			public void ReturnsEmptyFromEmpty()
			{
				PageNumberAndSize newPage = PagingInfo.Empty.TurnToPage(6);

				PageNumberAndSizeTests.AssertIsEmpty(newPage);
			}
		}

		#region [ Internal Static Test Assertion Methods ]

		internal static void AssertEquality(PagingInfo expected, PagingInfo actual)
		{
			Assert.True(expected == actual);
			Assert.False(expected != actual);
			Assert.True(expected.Equals(actual));
			Assert.Equal(expected, actual);

			Assert.True(actual == expected);
			Assert.False(actual != expected);
			Assert.True(actual.Equals(expected));
			Assert.Equal(actual, expected);

			Assert.True(expected.CurrentPage.Equals(actual.CurrentPage));
			Assert.True(actual.CurrentPage.Equals(expected.CurrentPage));
			Assert.True(expected.CurrentPage == actual.CurrentPage);
			Assert.True(actual.CurrentPage == expected.CurrentPage);
			Assert.False(expected.CurrentPage != actual.CurrentPage);
			Assert.False(actual.CurrentPage != expected.CurrentPage);

			Assert.Equal(expected.CurrentPage, actual.CurrentPage);
			Assert.Equal(expected.CurrentPage.Number, actual.CurrentPage.Number);
			Assert.Equal(expected.CurrentPage.Size, actual.CurrentPage.Size);
			Assert.Equal(expected.TotalItems, actual.TotalItems);
		}

		internal static void AssertInequality(PagingInfo expected, PagingInfo actual)
		{
			Assert.False(expected == actual);
			Assert.True(expected != actual);
			Assert.False(expected.Equals(actual));
			Assert.NotEqual(expected, actual);

			Assert.False(actual == expected);
			Assert.True(actual != expected);
			Assert.False(actual.Equals(expected));
			Assert.NotEqual(actual, expected);
		}

		internal static void AssertIsFirstPage(PagingInfo firstPageInfo)
		{
			PageNumberAndSizeTests.AssertIsFirstPage(firstPageInfo.CurrentPage);

			Assert.Equal(0, firstPageInfo.CurrentPage.Index);
			Assert.Equal(
				PageNumberAndSize.FirstPageNumber,
				firstPageInfo.CurrentPage.Number);

			Assert.True(firstPageInfo.HasValue);
			Assert.False(firstPageInfo.PreviousPage.HasValue);
			Assert.True(firstPageInfo.IsFirstPage);

			PageNumberAndSizeTests.AssertEquality(
				firstPageInfo.FirstPage, firstPageInfo.CurrentPage);
		}

		internal static void AssertIsLastPage(PagingInfo lastPageInfo)
		{
			Assert.Equal(
				lastPageInfo.LastPage.Number,
				lastPageInfo.CurrentPage.Number);

			Assert.True(lastPageInfo.HasValue);
			Assert.False(lastPageInfo.NextPage.HasValue);
			Assert.True(lastPageInfo.IsLastPage);

			PageNumberAndSizeTests.AssertEquality(
				lastPageInfo.LastPage, lastPageInfo.CurrentPage);
		}

		#endregion
	}
}