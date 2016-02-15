namespace MiscCorLib.Collections
{
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
		}

		[TestFixture]
		public sealed class EmptyValue
		{
			private readonly PagingInfo emptyPagingInfo = new PagingInfo();

			[Test]
			public void HasInvalidPages()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.emptyPagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.emptyPagingInfo.FirstPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.emptyPagingInfo.LastPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.emptyPagingInfo.NextPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.emptyPagingInfo.PreviousPage);

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
		}

		[TestFixture]
		public sealed class UnboundedValueWithManyItems
		{
			private const int TestTotalItems = int.MaxValue;
			private readonly PagingInfo unboundedPagingInfo = new PagingInfo(
				PageNumberAndSize.Unbounded, TestTotalItems);

			[Test]
			public void HasValidFirstAndLastPages()
			{
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.FirstPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.unboundedPagingInfo.LastPage);

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
				Assert.AreEqual(1, this.unboundedPagingInfo.TotalPages);
			}
		}
	}
}