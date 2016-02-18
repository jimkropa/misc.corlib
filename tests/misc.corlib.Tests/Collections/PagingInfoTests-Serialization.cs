namespace MiscCorLib.Collections
{
	using System;

	using Newtonsoft.Json;

	using NUnit.Framework;

	// This is a partial class. Refer to PagingInfoTests.cs for the main part.
	public sealed partial class PagingInfoTests
	{
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
			public void Scratchpad()
			{
				PagingInfo pagingInfo1 = new PagingInfo(PageNumberAndSize.Unbounded, 1701);
				PagingInfo pagingInfo2 = new PagingInfo(PageNumberAndSize.Unbounded, 1138, true);

				string serializedPagingInfo1 = JsonConvert.SerializeObject(pagingInfo1, DefaultFormatting, this.settings);
				string serializedPagingInfo2 = JsonConvert.SerializeObject(pagingInfo2, DefaultFormatting, this.settings);

				Console.WriteLine(serializedPagingInfo1);
				Console.WriteLine(serializedPagingInfo2);
			}

			[Test]
			public void Serializes_All_Properties()
			{
				PagingInfo pagingInfo = new PagingInfo(7, 20, 1138);
				string serializedPagingInfo = JsonConvert.SerializeObject(
					pagingInfo, DefaultFormatting, this.settings);

				Assert.AreEqual(Page7_Size20_Total1138, serializedPagingInfo);
			}

			[Test]
			public void Serializes_With_All_Pages_When_Specified()
			{
				PagingInfo pagingInfo = new PagingInfo(2, 10, 27, true);
				string serializedPagingInfo = JsonConvert.SerializeObject(
					pagingInfo, DefaultFormatting, this.settings);

				Assert.AreEqual(Page2_Size10_Total27_WithAllPages, serializedPagingInfo);
			}

			[Test]
			public void Serializes_Unbounded_Value()
			{
				PagingInfo pagingInfo = new PagingInfo(PageNumberAndSize.Unbounded, 1701);
				string serializedPagingInfo = JsonConvert.SerializeObject(pagingInfo, DefaultFormatting, this.settings);

				Assert.AreEqual(
					"{\"Number\":1,\"Size\":0,\"Index\":0,\"IsUnbounded\":true}", // ,\"IsValid\":true
					serializedPagingInfo);
			}

			[Test]
			public void Serializes_Unbounded_Value_With_All_Pages()
			{
				PagingInfo pagingInfo = new PagingInfo(PageNumberAndSize.Unbounded, 1138, true);
				string serializedPagingInfo = JsonConvert.SerializeObject(pagingInfo, DefaultFormatting, this.settings);

				Assert.AreEqual(
					"{\"Number\":1,\"Size\":0,\"Index\":0,\"IsUnbounded\":true}", // ,\"IsValid\":true
					serializedPagingInfo);
			}

			[Test]
			public void Serializes_Empty_Value()
			{
				string serializedPage = JsonConvert.SerializeObject(PagingInfo.Empty);

				Assert.AreEqual(
					"{\"Number\":0,\"Size\":0,\"Index\":-1,\"IsUnbounded\":false}", // ,\"IsValid\":false
					serializedPage);
			}

			[Test]
			public void Deserializes_From_Minimal_Specification()
			{
				PagingInfo expectedPagingInfo = new PagingInfo(7, PageNumberAndSize.DefaultPageSize, 1138);
				PagingInfo deserializedPagingInfo
					= JsonConvert.DeserializeObject<PagingInfo>(
						"{\"CurrentPage\":{\"Number\":7,\"Size\":10},\"TotalItems\":1138}", this.settings);

				//Assert.AreEqual(expectedPagingInfo, deserializedPagingInfo);

				Assert.AreEqual(expectedPagingInfo.CurrentPage.Number, deserializedPagingInfo.CurrentPage.Number);
				Assert.AreEqual(expectedPagingInfo.CurrentPage.Size, deserializedPagingInfo.CurrentPage.Size);
				Assert.AreEqual(expectedPagingInfo.TotalItems, deserializedPagingInfo.TotalItems);
				Assert.AreEqual(expectedPagingInfo.TotalPages, deserializedPagingInfo.TotalPages);
			}

			/*
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

				Assert.IsFalse(deserializedPage.HasValue);
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