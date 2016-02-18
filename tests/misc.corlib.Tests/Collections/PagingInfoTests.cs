namespace MiscCorLib.Collections
{
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

			[Test]
			public void HasValidFirstAndLastPages()
			{
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

			[Test]
			public void HasValidNextPageAndEmptyPreviousPage()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.defaultPagingInfo.PreviousPage);

				Assert.AreEqual(PageNumberAndSize.DefaultPageSize, this.defaultPagingInfo.CurrentPage.Size);
				Assert.AreEqual(this.defaultPagingInfo.CurrentPage.Size, this.defaultPagingInfo.NextPage.Size);
			}

			[Test]
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

			[Test]
			public void ConvertsToString()
			{
				Assert.AreEqual("PagingInfo[Page[Number=1,Size=10],TotalItems=1138]", this.defaultPagingInfo.ToString());
			}
		}

		[TestFixture]
		public sealed class DefaultValueWithNoItems
		{
			private const int ZeroTotalItems = 0;
			private readonly PagingInfo defaultPagingInfo = new PagingInfo(
				new PageNumberAndSize(250, 20), ZeroTotalItems);

			[Test]
			public void HasValidFirstAndLastPages()
			{
				PageNumberAndSizeTests.AssertIsFirstPage(this.defaultPagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsFirstPage(this.defaultPagingInfo.FirstPage);

				Assert.AreEqual(20, this.defaultPagingInfo.CurrentPage.Size);
				Assert.AreEqual(20, this.defaultPagingInfo.FirstPage.Size);
				Assert.AreEqual(this.defaultPagingInfo.TotalItems, this.defaultPagingInfo.ItemCount);
				Assert.IsNull(this.defaultPagingInfo.AllPages);
				Assert.IsTrue(this.defaultPagingInfo.IsFirstPage);
				Assert.IsTrue(this.defaultPagingInfo.IsLastPage);
			}

			[Test]
			public void HasEmptyNextAndPreviousPages()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.defaultPagingInfo.NextPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.defaultPagingInfo.PreviousPage);
			}

			[Test]
			public void HasZeroItems()
			{
				Assert.AreEqual(-1, this.defaultPagingInfo.FirstItemIndex, "FirstItemIndex");
				Assert.AreEqual(-1, this.defaultPagingInfo.LastItemIndex, "LastItemIndex");
				Assert.AreEqual(ZeroTotalItems, this.defaultPagingInfo.TotalItems, "TotalItems");
				Assert.AreEqual(ZeroTotalItems, this.defaultPagingInfo.ItemCount, "ItemCount");
				Assert.AreEqual(1, this.defaultPagingInfo.TotalPages, "TotalPages");
			}

			[Test]
			public void HasOneEmptyPage()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.defaultPagingInfo.AllPagesAndItemNumbers();

				Assert.AreEqual(1, pages.Count);
				Assert.AreEqual(0, pages[0].FirstItemNumber);
				Assert.AreEqual(0, pages[0].LastItemNumber);
				Assert.AreEqual(PageNumberAndSize.FirstPageNumber, pages[0].PageNumber);
				Assert.IsTrue(pages[0].HasValue);
			}

			[Test]
			public void ConvertsToString()
			{
				Assert.AreEqual("PagingInfo[Page[Number=1,Size=20],TotalItems=0]", this.defaultPagingInfo.ToString());
			}
		}

		[TestFixture]
		public sealed class EmptyValue
		{
			private readonly PagingInfo emptyPagingInfo = new PagingInfo();

			[Test]
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

			[Test]
			public void HasUninitializedItemIndices()
			{
				Assert.AreEqual(-1, this.emptyPagingInfo.FirstItemIndex);
				Assert.AreEqual(-1, this.emptyPagingInfo.LastItemIndex);
			}

			[Test]
			public void HasZeroTotalItems()
			{
				Assert.AreEqual(0, this.emptyPagingInfo.TotalItems);
				Assert.AreEqual(0, this.emptyPagingInfo.TotalPages);
				Assert.AreEqual(0, this.emptyPagingInfo.ItemCount);
			}

			[Test]
			public void HasNoPages()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.emptyPagingInfo.AllPagesAndItemNumbers();

				Assert.AreEqual(0, pages.Count);
			}

			[Test]
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

			[Test]
			public void HasValidFirstAndLastPages()
			{
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.FirstPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.LastPage);

				Assert.AreEqual(this.unboundedPagingInfo.TotalItems, this.unboundedPagingInfo.ItemCount);
				Assert.IsNull(this.unboundedPagingInfo.AllPages);
				Assert.IsTrue(this.unboundedPagingInfo.IsFirstPage);
				Assert.IsTrue(this.unboundedPagingInfo.IsLastPage);
			}

			[Test]
			public void HasEmptyNextAndPreviousPages()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.unboundedPagingInfo.NextPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.unboundedPagingInfo.PreviousPage);
			}

			[Test]
			public void HasAllItems()
			{
				Assert.AreEqual(0, this.unboundedPagingInfo.FirstItemIndex);
				Assert.AreEqual(TestTotalItems - 1, this.unboundedPagingInfo.LastItemIndex);
				Assert.AreEqual(TestTotalItems, this.unboundedPagingInfo.TotalItems);
				Assert.AreEqual(TestTotalItems, this.unboundedPagingInfo.ItemCount);
				Assert.AreEqual(1, this.unboundedPagingInfo.TotalPages);
			}

			[Test]
			public void HasOneHugePage()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.unboundedPagingInfo.AllPagesAndItemNumbers();

				Assert.AreEqual(1, pages.Count);
				Assert.AreEqual(1, pages[0].FirstItemNumber);
				Assert.AreEqual(this.unboundedPagingInfo.TotalItems, pages[0].LastItemNumber);
				Assert.AreEqual(PageNumberAndSize.FirstPageNumber, pages[0].PageNumber);
				Assert.IsTrue(pages[0].HasValue);
			}

			[Test]
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

			[Test]
			public void HasValidFirstAndLastPages()
			{
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.FirstPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.LastPage);

				Assert.AreEqual(this.unboundedPagingInfo.TotalItems, this.unboundedPagingInfo.ItemCount);
				Assert.IsNull(this.unboundedPagingInfo.AllPages);
				Assert.IsTrue(this.unboundedPagingInfo.IsFirstPage);
				Assert.IsTrue(this.unboundedPagingInfo.IsLastPage);
			}

			[Test]
			public void HasEmptyNextAndPreviousPages()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.unboundedPagingInfo.NextPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.unboundedPagingInfo.PreviousPage);
			}

			[Test]
			public void HasZeroItems()
			{
				Assert.AreEqual(-1, this.unboundedPagingInfo.FirstItemIndex, "FirstItemIndex");
				Assert.AreEqual(-1, this.unboundedPagingInfo.LastItemIndex, "LastItemIndex");
				Assert.AreEqual(ZeroTotalItems, this.unboundedPagingInfo.TotalItems, "TotalItems");
				Assert.AreEqual(ZeroTotalItems, this.unboundedPagingInfo.ItemCount, "ItemCount");
				Assert.AreEqual(1, this.unboundedPagingInfo.TotalPages, "TotalPages");
			}

			[Test]
			public void HasOneEmptyPage()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pages = this.unboundedPagingInfo.AllPagesAndItemNumbers();

				Assert.AreEqual(1, pages.Count);
				Assert.AreEqual(0, pages[0].FirstItemNumber);
				Assert.AreEqual(0, pages[0].LastItemNumber);
				Assert.AreEqual(PageNumberAndSize.FirstPageNumber, pages[0].PageNumber);
				Assert.IsTrue(pages[0].HasValue);
			}

			[Test]
			public void ConvertsToString()
			{
				Assert.AreEqual("PagingInfo[Page[Number=1,Size=0],TotalItems=0]", this.unboundedPagingInfo.ToString());
			}
		}

		[TestFixture]
		public sealed class Calculator
		{
			private readonly PagingInfo readonlyPagingInfo = new PagingInfo();

			[Test]
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

			[Test]
			public void DoesNotDisruptDeserializedReference()
			{
				PagingInfo deserializedPagingInfo
					= Newtonsoft.Json.JsonConvert.DeserializeObject<PagingInfo>(
						Page7_Size20_Total1138);

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
		}

		[TestFixture]
		public sealed class Equality
		{
			public static void IsTrueWhenSame()
			{
				PagingInfo empty1 = new PagingInfo();
				PagingInfo empty2 = new PagingInfo();
			}

			public static void IsTrueWhenDeserialized()
			{
				PagingInfo empty1 = new PagingInfo();
				PagingInfo empty2 = new PagingInfo();
			}

			public static void IsTrueWhenHasAllPages()
			{
				PagingInfo empty1 = new PagingInfo();
				PagingInfo empty2 = new PagingInfo();
			}

			public static void IsFalseWhenDifferentPageNumber()
			{
				
			}

			public static void IsFalseWhenDifferentPageSize()
			{

			}

			public static void IsFalseWhenDifferentTotalItems()
			{
				
			}
		}

		internal void AssertEquality(PagingInfo x, PagingInfo y)
		{
			Assert.IsTrue(x == y);
			Assert.IsTrue(y == x);
			Assert.IsFalse(x != y);
			Assert.IsFalse(y != x);

			Assert.IsTrue(x.CurrentPage.Equals(y.CurrentPage));
			Assert.IsTrue(y.CurrentPage.Equals(x.CurrentPage));
			Assert.IsTrue(x.CurrentPage == y.CurrentPage);
			Assert.IsTrue(y.CurrentPage == x.CurrentPage);
			Assert.IsFalse(x.CurrentPage != y.CurrentPage);
			Assert.IsFalse(y.CurrentPage != x.CurrentPage);

			Assert.AreEqual(x, y);
			Assert.AreEqual(y, x);

			Assert.IsTrue(x.Equals(y));
			Assert.IsTrue(y.Equals(x));

			Assert.AreEqual(x.CurrentPage, y.CurrentPage);
			Assert.AreEqual(x.CurrentPage.Number, y.CurrentPage.Number);
			Assert.AreEqual(x.CurrentPage.Size, y.CurrentPage.Size);
			Assert.AreEqual(x.TotalItems, y.TotalItems);
		}

		internal void AssertInequality(PagingInfo x, PagingInfo y)
		{
			Assert.IsFalse(x == y);
			Assert.IsFalse(y == x);
			Assert.IsTrue(x != y);
			Assert.IsTrue(y != x);

			Assert.AreNotEqual(x, y);
			Assert.AreNotEqual(y, x);

			Assert.IsTrue(x.Equals(y));
			Assert.IsTrue(y.Equals(x));
		}
	}
}