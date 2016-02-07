namespace MiscCorLib.Collections
{
	using NUnit.Framework;

	[TestFixture]
	public sealed class PagingInfoTests
	{
		[TestFixture]
		public sealed class DefaultValue
		{
			private const int TestTotalItems = 1138;
			private readonly PagingInfo pagingInfo = new PagingInfo(PageNumberAndSize.Default, TestTotalItems);

			[Test]
			public void HasValidFirstAndLastPages()
			{
				PageNumberAndSizeTests.AssertIsFirstPage(this.pagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsFirstPage(this.pagingInfo.FirstPage);

				Assert.Greater(this.pagingInfo.CurrentPage.Size, byte.MinValue);
				Assert.Greater(this.pagingInfo.FirstPage.Size, byte.MinValue);
				Assert.Greater(this.pagingInfo.LastPage.Size, byte.MinValue);

				Assert.AreEqual(this.pagingInfo.CurrentPage.Size, this.pagingInfo.FirstPage.Size);
				Assert.AreEqual(this.pagingInfo.CurrentPage.Size, this.pagingInfo.LastPage.Size);
			}

			[Test]
			public void HasValidNextPageAndEmptyPreviousPage()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.pagingInfo.PreviousPage);

				Assert.AreEqual(this.pagingInfo.CurrentPage.Size, this.pagingInfo.NextPage.Size);
			}
		}

		[TestFixture]
		public sealed class EmptyValue
		{
			private readonly PagingInfo pagingInfo = new PagingInfo();

			[Test]
			public void HasInvalidPages()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.pagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.pagingInfo.FirstPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.pagingInfo.LastPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.pagingInfo.NextPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.pagingInfo.PreviousPage);

				Assert.IsFalse(this.pagingInfo.IsFirstPage);
				Assert.IsFalse(this.pagingInfo.IsLastPage);
			}

			[Test]
			public void HasUninitializedItemIndices()
			{
				Assert.AreEqual(-1, this.pagingInfo.FirstItemIndex);
				Assert.AreEqual(-1, this.pagingInfo.LastItemIndex);
			}

			[Test]
			public void HasZeroTotalItems()
			{
				Assert.AreEqual(0, this.pagingInfo.TotalItems);
				Assert.AreEqual(0, this.pagingInfo.TotalPages);
			}
		}

		[TestFixture]
		public sealed class UnboundedValueWithManyItems
		{
			private const int TestTotalItems = int.MaxValue;
			private readonly PagingInfo pagingInfo = new PagingInfo(
				PageNumberAndSize.Unbounded, TestTotalItems);

			[Test]
			public void HasValidFirstAndLastPages()
			{
				PageNumberAndSizeTests.AssertIsUnbounded(this.pagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.pagingInfo.FirstPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.pagingInfo.LastPage);

				Assert.IsTrue(this.pagingInfo.IsFirstPage);
				Assert.IsTrue(this.pagingInfo.IsLastPage);
			}

			[Test]
			public void HasEmptyNextAndPreviousPages()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.pagingInfo.NextPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.pagingInfo.PreviousPage);
			}

			[Test]
			public void HasAllItems()
			{
				Assert.AreEqual(0, this.pagingInfo.FirstItemIndex);
				Assert.AreEqual(TestTotalItems - 1, this.pagingInfo.LastItemIndex);
				Assert.AreEqual(TestTotalItems, this.pagingInfo.TotalItems);
				Assert.AreEqual(1, this.pagingInfo.TotalPages);
			}
		}

		[TestFixture]
		public sealed class UnboundedValueWithNoItems
		{
			private const int TestTotalItems = 0;
			private readonly PagingInfo pagingInfo = new PagingInfo(
				PageNumberAndSize.Unbounded, TestTotalItems);

			[Test]
			public void HasValidFirstAndLastPages()
			{
				PageNumberAndSizeTests.AssertIsUnbounded(this.pagingInfo.CurrentPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.pagingInfo.FirstPage);
				PageNumberAndSizeTests.AssertIsUnbounded(this.pagingInfo.LastPage);

				Assert.IsTrue(this.pagingInfo.IsFirstPage);
				Assert.IsTrue(this.pagingInfo.IsLastPage);
			}

			[Test]
			public void HasEmptyNextAndPreviousPages()
			{
				PageNumberAndSizeTests.AssertIsEmpty(this.pagingInfo.NextPage);
				PageNumberAndSizeTests.AssertIsEmpty(this.pagingInfo.PreviousPage);
			}

			[Test]
			public void HasAllItems()
			{
				Assert.AreEqual(0, this.pagingInfo.FirstItemIndex);
				Assert.AreEqual(0, this.pagingInfo.LastItemIndex);
				Assert.AreEqual(TestTotalItems, this.pagingInfo.TotalItems);
				Assert.AreEqual(1, this.pagingInfo.TotalPages);
			}
		}
	}
}