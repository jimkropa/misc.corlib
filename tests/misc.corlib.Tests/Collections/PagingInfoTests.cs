namespace MiscCorLib.Collections
{
	using System;
	using Newtonsoft.Json;

	using NUnit.Framework;

	[TestFixture]
	public sealed class PagingInfoTests
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

		[TestFixture]
		public sealed class JsonNetSerialization
		{
			private const Formatting DefaultFormatting = Formatting.Indented;

			private readonly JsonSerializerSettings settings;

			public JsonNetSerialization()
			{
				JsonSerializerSettings newSettings = new JsonSerializerSettings();

				newSettings.Formatting = DefaultFormatting;

				this.settings = newSettings;
			}

			[Test]
			public void Serializes_All_Properties()
			{
				PagingInfo pagingInfo = new PagingInfo(7, 20, 1138);
				string serializedPagingInfo = JsonConvert.SerializeObject(pagingInfo, DefaultFormatting, this.settings);
				const string expectedSerialization = @"{
  ""CurrentPage"": {
    ""Number"": 7,
    ""Size"": 20,
    ""Index"": 6,
    ""IsUnbounded"": false
  },
  ""TotalItems"": 1138,
  ""TotalPages"": 57,
  ""FirstItemNumber"": 121,
  ""LastItemNumber"": 140,
  ""FirstItemIndex"": 120,
  ""LastItemIndex"": 139,
  ""ItemCount"": 20,
  ""NextPage"": {
    ""Number"": 8,
    ""Size"": 20,
    ""Index"": 7,
    ""IsUnbounded"": false
  },
  ""PreviousPage"": {
    ""Number"": 6,
    ""Size"": 20,
    ""Index"": 5,
    ""IsUnbounded"": false
  },
  ""FirstPage"": {
    ""Number"": 1,
    ""Size"": 20,
    ""Index"": 0,
    ""IsUnbounded"": false
  },
  ""LastPage"": {
    ""Number"": 57,
    ""Size"": 20,
    ""Index"": 56,
    ""IsUnbounded"": false
  },
  ""IsFirstPage"": false,
  ""IsLastPage"": false
}";

				Assert.AreEqual(expectedSerialization,serializedPagingInfo);
			}

			[Test]
			public void Serializes_Unbounded_Value()
			{
				PagingInfo pagingInfo = new PagingInfo(PageNumberAndSize.Unbounded, 1138);
				string serializedPagingInfo = JsonConvert.SerializeObject(pagingInfo, DefaultFormatting, this.settings);

				Assert.AreEqual(
					"{\"Number\":1,\"Size\":0,\"Index\":0,\"IsUnbounded\":true}", // ,\"IsValid\":true
					serializedPagingInfo);
			}

			[Test]
			public void Serializes_Empty_Value()
			{
				string serializedPage = JsonConvert.SerializeObject(PageNumberAndSize.Empty);

				Assert.AreEqual(
					"{\"Number\":0,\"Size\":0,\"Index\":-1,\"IsUnbounded\":false}", // ,\"IsValid\":false
					serializedPage);
			}

			/*
			[Test]
			public void Deserializes_From_Minimal_Specification()
			{
				PageNumberAndSize page = new PageNumberAndSize(7);
				PageNumberAndSize deserializedPage
					= JsonConvert.DeserializeObject<PageNumberAndSize>(
						"{\"Number\":7,\"Size\":10}");

				AssertEquality(page, deserializedPage);
			}

			[Test]
			public void Deserializes_And_Ignores_Inconsistency_From_Excessive_Specification()
			{
				PageNumberAndSize page = new PageNumberAndSize(7, 20);
				PageNumberAndSize deserializedPage
					= JsonConvert.DeserializeObject<PageNumberAndSize>(
						"{\"Number\":7,\"Size\":20,\"Index\":1111111,\"IsUnbounded\":true}"); // ,\"IsValid\":false

				AssertEquality(page, deserializedPage);
			}
			*/

			[Test]
			public void Deserializes_As_Invalid_From_Negative_Page_Number()
			{
				PageNumberAndSize deserializedPage
					= JsonConvert.DeserializeObject<PageNumberAndSize>(
						"{\"Number\":-7,\"Size\":10}");

				Assert.IsFalse(deserializedPage.IsValid);
				Assert.IsFalse(deserializedPage.IsUnbounded);
			}

			[Test]
			public void Does_Not_Deserialize_From_Negative_Page_Size()
			{
				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PageNumberAndSize>(
						"{\"Number\":0,\"Size\":-1}"));
			}

			[Test]
			public void Does_Not_Deserialize_From_Omitted_Page_Number()
			{
				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PageNumberAndSize>(
						"{\"Size\":20}"));
			}

			[Test]
			public void Does_Not_Deserialize_From_Omitted_Page_Size()
			{
				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PageNumberAndSize>(
						"{\"Number\":0}"));
			}
		}
	}
}