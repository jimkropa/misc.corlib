#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Collections
	PagingInfoTests.cs

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
	using System.Collections.Generic;

	using NUnit.Framework;

	[TestFixture]
	public sealed partial class PagingInfoTests
	{
		[TestFixture]
		public sealed class DefaultValue
		{
			private const int TestTotalItems = 1138;
			private readonly PagingInfo defaultPagingInfo = new PagingInfo(PageNumberAndSize.Default, TestTotalItems);

			[Fact]
			public void HasValidItemNumbers()
			{
				Assert.AreEqual(1, this.defaultPagingInfo.FirstItemNumber);
				Assert.AreEqual(this.defaultPagingInfo.CurrentPage.Size, this.defaultPagingInfo.LastItemNumber);

				Assert.AreEqual(this.defaultPagingInfo.FirstItemNumber - 1, this.defaultPagingInfo.FirstItemIndex);
				Assert.AreEqual(this.defaultPagingInfo.LastItemNumber - 1, this.defaultPagingInfo.LastItemIndex);
			}

			[Fact]
			public void HasValidFirstAndLastPages()
			{
				AssertIsFirstPage(this.defaultPagingInfo);

				PageNumberAndSizeTests.AssertIsFirstPage(this.defaultPagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsFirstPage(this.defaultPagingInfo.FirstPage);

				Assert.Greater(this.defaultPagingInfo.CurrentPage.Size, byte.MinValue);
				Assert.Greater(this.defaultPagingInfo.FirstPage.Size, byte.MinValue);
				Assert.Greater(this.defaultPagingInfo.LastPage.Size, byte.MinValue);

				Assert.IsNull(this.defaultPagingInfo.AllPages);
				Assert.AreEqual(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfo.ItemCount);
				Assert.AreEqual(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfo.CurrentPage.Size);
				Assert.AreEqual(this.defaultPagingInfo.CurrentPage.Size, this.defaultPagingInfo.FirstPage.Size);
				Assert.AreEqual(this.defaultPagingInfo.CurrentPage.Size, this.defaultPagingInfo.LastPage.Size);
				Assert.AreEqual(this.defaultPagingInfo.CurrentPage.Size, this.defaultPagingInfo.ItemCount);
			}

			[Fact]
			public void HasValidNextPageAndEmptyPreviousPage()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.defaultPagingInfo.PreviousPage);

				Assert.AreEqual(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfo.CurrentPage.Size);
				Assert.AreEqual(this.defaultPagingInfo.CurrentPage.Size, this.defaultPagingInfo.NextPage.Size);
			}

			[Fact]
			public void HasFullSetOfAllPages()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.defaultPagingInfo.AllPagesAndItemNumbers();

				Assert.AreEqual(114, pages.Count);
				Assert.AreEqual(
					this.defaultPagingInfo.TotalItems,
					pages[pages.Count - 1].LastItemNumber);

				int lastItemNumber = 0;
				for (int i = PageNumberAndSize.FirstPageNumber; i <= pages.Count; i++)
				{
					int firstItemNumber = lastItemNumber + 1;
					int pageIndex = i - 1;
					PageNumberAndItemNumbers page = pages[pageIndex];

					Assert.AreEqual(i, page.PageNumber);
					Assert.AreEqual(firstItemNumber, page.FirstItemNumber);

					lastItemNumber = page.LastItemNumber;
				}

				Assert.AreEqual(this.defaultPagingInfo.TotalItems, lastItemNumber);
				Assert.IsTrue(pages[0].HasValue);
			}

			[Fact]
			public void ConvertsToString()
			{
				Assert.AreEqual("PagingInfo[Page[Number=1,Size=10],TotalItems=1138]", this.defaultPagingInfo.ToString());
			}
		}

		[TestFixture]
		public sealed class DefaultValueOnLastPage
		{
			private const int TestTotalItems = 1138;
			private readonly PagingInfo defaultPagingInfoLastPage = new PagingInfo(250, 10, TestTotalItems);

			[Fact]
			public void HasValidItemNumbers()
			{
				Assert.AreEqual(1131, this.defaultPagingInfoLastPage.FirstItemNumber);
				Assert.AreEqual(1138, this.defaultPagingInfoLastPage.LastItemNumber);

				Assert.AreEqual(this.defaultPagingInfoLastPage.FirstItemNumber - 1, this.defaultPagingInfoLastPage.FirstItemIndex);
				Assert.AreEqual(this.defaultPagingInfoLastPage.LastItemNumber - 1, this.defaultPagingInfoLastPage.LastItemIndex);
			}

			[Fact]
			public void HasValidFirstAndLastPages()
			{
				AssertIsLastPage(this.defaultPagingInfoLastPage);

				Assert.Greater(this.defaultPagingInfoLastPage.CurrentPage.Size, byte.MinValue);
				Assert.Greater(this.defaultPagingInfoLastPage.FirstPage.Size, byte.MinValue);
				Assert.Greater(this.defaultPagingInfoLastPage.LastPage.Size, byte.MinValue);

				Assert.IsNull(this.defaultPagingInfoLastPage.AllPages);
				Assert.AreEqual(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfoLastPage.ItemCount);
				Assert.AreEqual(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfoLastPage.CurrentPage.Size);
				Assert.AreEqual(this.defaultPagingInfoLastPage.CurrentPage.Size, this.defaultPagingInfoLastPage.FirstPage.Size);
				Assert.AreEqual(this.defaultPagingInfoLastPage.CurrentPage.Size, this.defaultPagingInfoLastPage.LastPage.Size);
				Assert.AreEqual(this.defaultPagingInfoLastPage.CurrentPage.Size, this.defaultPagingInfoLastPage.ItemCount);
			}

			[Fact]
			public void HasValidNextPageAndEmptyPreviousPage()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.defaultPagingInfoLastPage.NextPage);

				Assert.AreEqual(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfoLastPage.CurrentPage.Size);
				Assert.AreEqual(this.defaultPagingInfoLastPage.CurrentPage.Size, this.defaultPagingInfoLastPage.PreviousPage.Size);
			}

			[Fact]
			public void HasFullSetOfAllPages()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.defaultPagingInfoLastPage.AllPagesAndItemNumbers();

				Assert.AreEqual(114, pages.Count);
				Assert.AreEqual(
					this.defaultPagingInfoLastPage.TotalItems,
					pages[pages.Count - 1].LastItemNumber);

				int lastItemNumber = 0;
				for (int i = PageNumberAndSize.FirstPageNumber; i <= pages.Count; i++)
				{
					int firstItemNumber = lastItemNumber + 1;
					int pageIndex = i - 1;
					PageNumberAndItemNumbers page = pages[pageIndex];

					Assert.AreEqual(i, page.PageNumber);
					Assert.AreEqual(firstItemNumber, page.FirstItemNumber);

					lastItemNumber = page.LastItemNumber;
				}

				Assert.AreEqual(this.defaultPagingInfoLastPage.TotalItems, lastItemNumber);
				Assert.IsTrue(pages[0].HasValue);
			}

			[Fact]
			public void ConvertsToString()
			{
				Assert.AreEqual("PagingInfo[Page[Number=114,Size=10],TotalItems=1138]", this.defaultPagingInfoLastPage.ToString());
			}
		}

		[TestFixture]
		public sealed class DefaultValueWithNoItems
		{
			private const int ZeroTotalItems = 0;
			private readonly PagingInfo defaultPagingInfo = new PagingInfo(
				new PageNumberAndSize(250, 20), ZeroTotalItems);

			[Fact]
			public void HasValidItemNumbers()
			{
				Assert.AreEqual(0, this.defaultPagingInfo.FirstItemNumber);
				Assert.AreEqual(0, this.defaultPagingInfo.LastItemNumber);
				Assert.AreEqual(-1, this.defaultPagingInfo.FirstItemIndex);
				Assert.AreEqual(-1, this.defaultPagingInfo.LastItemIndex);
			}

			[Fact]
			public void HasValidFirstAndLastPages()
			{
				PageNumberAndSizeTests.AssertIsFirstPage(this.defaultPagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsFirstPage(this.defaultPagingInfo.FirstPage);

				AssertIsFirstPage(this.defaultPagingInfo);
				AssertIsLastPage(this.defaultPagingInfo);

				Assert.AreEqual(20, this.defaultPagingInfo.CurrentPage.Size);
				Assert.AreEqual(20, this.defaultPagingInfo.FirstPage.Size);
				Assert.AreEqual(this.defaultPagingInfo.TotalItems, this.defaultPagingInfo.ItemCount);
				Assert.IsNull(this.defaultPagingInfo.AllPages);
				Assert.IsTrue(this.defaultPagingInfo.IsFirstPage);
				Assert.IsTrue(this.defaultPagingInfo.IsLastPage);
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
				Assert.AreEqual(-1, this.defaultPagingInfo.FirstItemIndex, "FirstItemIndex");
				Assert.AreEqual(-1, this.defaultPagingInfo.LastItemIndex, "LastItemIndex");
				Assert.AreEqual(ZeroTotalItems, this.defaultPagingInfo.TotalItems, "TotalItems");
				Assert.AreEqual(ZeroTotalItems, this.defaultPagingInfo.ItemCount, "ItemCount");
				Assert.AreEqual(1, this.defaultPagingInfo.TotalPages, "TotalPages");
			}

			[Fact]
			public void HasOneEmptyPage()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.defaultPagingInfo.AllPagesAndItemNumbers();

				Assert.AreEqual(1, pages.Count);
				Assert.AreEqual(0, pages[0].FirstItemNumber);
				Assert.AreEqual(0, pages[0].LastItemNumber);
				Assert.AreEqual(PageNumberAndSize.FirstPageNumber, pages[0].PageNumber);
				Assert.IsTrue(pages[0].HasValue);
			}

			[Fact]
			public void ConvertsToString()
			{
				Assert.AreEqual("PagingInfo[Page[Number=1,Size=20],TotalItems=0]", this.defaultPagingInfo.ToString());
			}
		}

		[TestFixture]
		public sealed class EmptyValue
		{
			private readonly PagingInfo emptyPagingInfo = new PagingInfo();

			[Fact]
			public void HasValidItemNumbers()
			{
				Assert.AreEqual(0, this.emptyPagingInfo.FirstItemNumber);
				Assert.AreEqual(0, this.emptyPagingInfo.LastItemNumber);
				Assert.AreEqual(-1, this.emptyPagingInfo.FirstItemIndex);
				Assert.AreEqual(-1, this.emptyPagingInfo.LastItemIndex);
			}

			[Fact]
			public void HasEmptyPages()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.emptyPagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.emptyPagingInfo.FirstPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.emptyPagingInfo.LastPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.emptyPagingInfo.NextPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.emptyPagingInfo.PreviousPage);

				Assert.AreEqual(0, this.emptyPagingInfo.ItemCount);
				Assert.IsNull(this.emptyPagingInfo.AllPages);
				Assert.IsFalse(this.emptyPagingInfo.IsFirstPage);
				Assert.IsFalse(this.emptyPagingInfo.IsLastPage);
			}

			[Fact]
			public void HasUninitializedItemIndices()
			{
				Assert.AreEqual(-1, this.emptyPagingInfo.FirstItemIndex);
				Assert.AreEqual(-1, this.emptyPagingInfo.LastItemIndex);
			}

			[Fact]
			public void HasZeroTotalItems()
			{
				Assert.AreEqual(0, this.emptyPagingInfo.TotalItems);
				Assert.AreEqual(0, this.emptyPagingInfo.TotalPages);
				Assert.AreEqual(0, this.emptyPagingInfo.ItemCount);
			}

			[Fact]
			public void HasNoPages()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.emptyPagingInfo.AllPagesAndItemNumbers();

				Assert.AreEqual(0, pages.Count);
			}

			[Fact]
			public void ConvertsToString()
			{
				Assert.AreEqual("PagingInfo[Page[Number=0,Size=0],TotalItems=0]", this.emptyPagingInfo.ToString());
			}
		}

		[TestFixture]
		public sealed class UnboundedValueWithManyItems
		{
			private const int TestTotalItems = int.MaxValue / 33;
			private readonly PagingInfo unboundedPagingInfo = new PagingInfo(
				PageNumberAndSize.Unbounded, TestTotalItems);

			[Fact]
			public void HasValidItemNumbers()
			{
				Assert.AreEqual(1, this.unboundedPagingInfo.FirstItemNumber);
				Assert.AreEqual(this.unboundedPagingInfo.TotalItems, this.unboundedPagingInfo.LastItemNumber);

				Assert.AreEqual(this.unboundedPagingInfo.FirstItemNumber - 1, this.unboundedPagingInfo.FirstItemIndex);
				Assert.AreEqual(this.unboundedPagingInfo.LastItemNumber - 1, this.unboundedPagingInfo.LastItemIndex);
			}

			[Fact]
			public void HasValidFirstAndLastPages()
			{
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.FirstPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.LastPage);

				AssertIsFirstPage(this.unboundedPagingInfo);
				AssertIsLastPage(this.unboundedPagingInfo);

				Assert.AreEqual(this.unboundedPagingInfo.TotalItems, this.unboundedPagingInfo.ItemCount);
				Assert.IsNull(this.unboundedPagingInfo.AllPages);
				Assert.IsTrue(this.unboundedPagingInfo.IsFirstPage);
				Assert.IsTrue(this.unboundedPagingInfo.IsLastPage);
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
				Assert.AreEqual(0, this.unboundedPagingInfo.FirstItemIndex);
				Assert.AreEqual(TestTotalItems - 1, this.unboundedPagingInfo.LastItemIndex);
				Assert.AreEqual(TestTotalItems, this.unboundedPagingInfo.TotalItems);
				Assert.AreEqual(TestTotalItems, this.unboundedPagingInfo.ItemCount);
				Assert.AreEqual(1, this.unboundedPagingInfo.TotalPages);
			}

			[Fact]
			public void HasOneHugePage()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.unboundedPagingInfo.AllPagesAndItemNumbers();

				Assert.AreEqual(1, pages.Count);
				Assert.AreEqual(1, pages[0].FirstItemNumber);
				Assert.AreEqual(this.unboundedPagingInfo.TotalItems, pages[0].LastItemNumber);
				Assert.AreEqual(PageNumberAndSize.FirstPageNumber, pages[0].PageNumber);
				Assert.IsTrue(pages[0].HasValue);
			}

			[Fact]
			public void ConvertsToString()
			{
				Assert.AreEqual("PagingInfo[Page[Number=1,Size=0],TotalItems=65075262]", this.unboundedPagingInfo.ToString());
			}
		}

		[TestFixture]
		public sealed class UnboundedValueWithNoItems
		{
			private const int ZeroTotalItems = 0;
			private readonly PagingInfo unboundedPagingInfo = new PagingInfo(
				PageNumberAndSize.Unbounded, ZeroTotalItems);

			[Fact]
			public void HasValidItemNumbers()
			{
				Assert.AreEqual(0, this.unboundedPagingInfo.FirstItemNumber);
				Assert.AreEqual(0, this.unboundedPagingInfo.LastItemNumber);
				Assert.AreEqual(-1, this.unboundedPagingInfo.FirstItemIndex);
				Assert.AreEqual(-1, this.unboundedPagingInfo.LastItemIndex);
			}

			[Fact]
			public void HasValidFirstAndLastPages()
			{
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.FirstPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.LastPage);

				AssertIsFirstPage(this.unboundedPagingInfo);
				AssertIsLastPage(this.unboundedPagingInfo);

				Assert.AreEqual(this.unboundedPagingInfo.TotalItems, this.unboundedPagingInfo.ItemCount);
				Assert.IsNull(this.unboundedPagingInfo.AllPages);
				Assert.IsTrue(this.unboundedPagingInfo.IsFirstPage);
				Assert.IsTrue(this.unboundedPagingInfo.IsLastPage);
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
				Assert.AreEqual(-1, this.unboundedPagingInfo.FirstItemIndex, "FirstItemIndex");
				Assert.AreEqual(-1, this.unboundedPagingInfo.LastItemIndex, "LastItemIndex");
				Assert.AreEqual(ZeroTotalItems, this.unboundedPagingInfo.TotalItems, "TotalItems");
				Assert.AreEqual(ZeroTotalItems, this.unboundedPagingInfo.ItemCount, "ItemCount");
				Assert.AreEqual(1, this.unboundedPagingInfo.TotalPages, "TotalPages");
			}

			[Fact]
			public void HasOneEmptyPage()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.unboundedPagingInfo.AllPagesAndItemNumbers();

				Assert.AreEqual(1, pages.Count);
				Assert.AreEqual(0, pages[0].FirstItemNumber);
				Assert.AreEqual(0, pages[0].LastItemNumber);
				Assert.AreEqual(PageNumberAndSize.FirstPageNumber, pages[0].PageNumber);
				Assert.IsTrue(pages[0].HasValue);
			}

			[Fact]
			public void ConvertsToString()
			{
				Assert.AreEqual("PagingInfo[Page[Number=1,Size=0],TotalItems=0]", this.unboundedPagingInfo.ToString());
			}
		}

		[TestFixture]
		public sealed class Calculator
		{
			private readonly PagingInfo readonlyPagingInfo = new PagingInfo();

			[Fact]
			public void DoesNotDisruptReadOnlyReference()
			{
				Assert.IsNull(this.readonlyPagingInfo.AllPages);

				System.Reflection.PropertyInfo privateProperty
					= this.readonlyPagingInfo.GetType().GetProperty("Calculator",
						System.Reflection.BindingFlags.NonPublic
						| System.Reflection.BindingFlags.Instance);

				privateProperty.GetMethod.Invoke(this.readonlyPagingInfo, null);

				Assert.IsTrue(PagingInfo.Empty.Equals(this.readonlyPagingInfo));
			}

			[Fact]
			public void DoesNotDisruptDeserializedReference()
			{
				PagingInfo deserializedPagingInfo
					= Newtonsoft.Json.JsonConvert.DeserializeObject<PagingInfo>(
						DeserializeMinimal_Page7_Size20_Total1138);

				Assert.IsNull(deserializedPagingInfo.AllPages);

				Assert.AreEqual(7, deserializedPagingInfo.CurrentPage.Number);
				Assert.AreEqual(20, deserializedPagingInfo.CurrentPage.Size);
				Assert.AreEqual(1138, deserializedPagingInfo.TotalItems);

				Assert.AreEqual(57, deserializedPagingInfo.TotalPages);

				Assert.IsNull(deserializedPagingInfo.AllPages);

				Assert.AreEqual(7, deserializedPagingInfo.CurrentPage.Number);
				Assert.AreEqual(20, deserializedPagingInfo.CurrentPage.Size);
				Assert.AreEqual(1138, deserializedPagingInfo.TotalItems);

				Assert.IsFalse(deserializedPagingInfo.IsFirstPage);
				Assert.IsFalse(deserializedPagingInfo.IsLastPage);
			}

			[Fact]
			public void MovesToLastPageWhenTurningPast()
			{
				PagingInfo pagingInfo = new PagingInfo(27, 20, 39);

				Assert.AreEqual(2, pagingInfo.CurrentPage.Number);

				AssertIsLastPage(pagingInfo);
			}

			[Fact]
			public void CalculatesCorrectTotalPages()
			{
				Assert.Throws<ArgumentOutOfRangeException>(
					() => PagingInfoCalculator.CalculateTotalPages(10, 0));

				Assert.Throws<ArgumentOutOfRangeException>(
					() => PagingInfoCalculator.CalculateTotalPages(0, 1138));

				Assert.AreEqual(1, PagingInfoCalculator.CalculateTotalPages(10, 1));
				Assert.AreEqual(1, PagingInfoCalculator.CalculateTotalPages(10, 10));
				Assert.AreEqual(2, PagingInfoCalculator.CalculateTotalPages(10, 11));
				Assert.AreEqual(2, PagingInfoCalculator.CalculateTotalPages(10, 20));
			}
		}

		[TestFixture]
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

		[TestFixture]
		public sealed class TurnToPage
		{
			[Fact]
			public void DefaultCaseReusesSize()
			{
				PagingInfo validPagingInfo = new PagingInfo(3, 10, 57);
				PageNumberAndSize newPage = validPagingInfo.TurnToPage(2);

				Assert.AreEqual(2, newPage.Number);
				Assert.AreEqual(10, newPage.Size);
				Assert.IsFalse(newPage.IsUnbounded);
			}

			[Fact]
			public void AllowsTurningPastLastPage()
			{
				PagingInfo validPagingInfo = new PagingInfo(3, 10, 57);
				PageNumberAndSize newPage = validPagingInfo.TurnToPage(8);

				Assert.AreEqual(8, newPage.Number);
				Assert.AreEqual(10, newPage.Size);
				Assert.IsFalse(newPage.IsUnbounded);
			}

			[Fact]
			public void ReturnsUnboundedFromUnbounded()
			{
				PagingInfo unboundedPagingInfo = new PagingInfo(PageNumberAndSize.Unbounded, 57);
				PageNumberAndSize newPage = unboundedPagingInfo.TurnToPage(8);

				Assert.AreEqual(1, newPage.Number);
				Assert.AreEqual(0, newPage.Size);
				Assert.IsTrue(newPage.IsUnbounded);
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
			Assert.IsTrue(expected == actual);
			Assert.IsFalse(expected != actual);
			Assert.IsTrue(expected.Equals(actual));
			Assert.AreEqual(expected, actual);

			Assert.IsTrue(actual == expected);
			Assert.IsFalse(actual != expected);
			Assert.IsTrue(actual.Equals(expected));
			Assert.AreEqual(actual, expected);

			Assert.IsTrue(expected.CurrentPage.Equals(actual.CurrentPage));
			Assert.IsTrue(actual.CurrentPage.Equals(expected.CurrentPage));
			Assert.IsTrue(expected.CurrentPage == actual.CurrentPage);
			Assert.IsTrue(actual.CurrentPage == expected.CurrentPage);
			Assert.IsFalse(expected.CurrentPage != actual.CurrentPage);
			Assert.IsFalse(actual.CurrentPage != expected.CurrentPage);

			Assert.AreEqual(expected.CurrentPage, actual.CurrentPage);
			Assert.AreEqual(expected.CurrentPage.Number, actual.CurrentPage.Number);
			Assert.AreEqual(expected.CurrentPage.Size, actual.CurrentPage.Size);
			Assert.AreEqual(expected.TotalItems, actual.TotalItems);
		}

		internal static void AssertInequality(PagingInfo expected, PagingInfo actual)
		{
			Assert.IsFalse(expected == actual);
			Assert.IsTrue(expected != actual);
			Assert.IsFalse(expected.Equals(actual));
			Assert.AreNotEqual(expected, actual);

			Assert.IsFalse(actual == expected);
			Assert.IsTrue(actual != expected);
			Assert.IsFalse(actual.Equals(expected));
			Assert.AreNotEqual(actual, expected);
		}

		internal static void AssertIsFirstPage(PagingInfo firstPageInfo)
		{
			PageNumberAndSizeTests.AssertIsFirstPage(firstPageInfo.CurrentPage);

			Assert.AreEqual(0, firstPageInfo.CurrentPage.Index);
			Assert.AreEqual(
				PageNumberAndSize.FirstPageNumber,
				firstPageInfo.CurrentPage.Number);

			Assert.IsTrue(firstPageInfo.HasValue);
			Assert.IsFalse(firstPageInfo.PreviousPage.HasValue);
			Assert.IsTrue(firstPageInfo.IsFirstPage);

			PageNumberAndSizeTests.AssertEquality(
				firstPageInfo.FirstPage, firstPageInfo.CurrentPage);
		}

		internal static void AssertIsLastPage(PagingInfo lastPageInfo)
		{
			Assert.AreEqual(
				lastPageInfo.LastPage.Number,
				lastPageInfo.CurrentPage.Number);

			Assert.IsTrue(lastPageInfo.HasValue);
			Assert.IsFalse(lastPageInfo.NextPage.HasValue);
			Assert.IsTrue(lastPageInfo.IsLastPage);

			PageNumberAndSizeTests.AssertEquality(
				lastPageInfo.LastPage, lastPageInfo.CurrentPage);
		}

		#endregion
	}
}