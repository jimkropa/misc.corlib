using Newtonsoft.Json;
using Xunit;

namespace MiscCorLib.Collections.Paged
{
	// This is a partial class. Refer to PagingStateTests.cs for the main part.
	public sealed partial class PagingStateTests
	{
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

			/*
			[Fact]
			public void Scratchpad()
			{
				PagingInfo pagingInfo1 = new PagingInfo(PageNumberAndSize.Unbounded, 1701);
				PagingInfo pagingInfo2 = new PagingInfo(PageNumberAndSize.Unbounded, 1138, true);

				string serializedPagingInfo1 = JsonConvert.SerializeObject(PagingInfo.Empty, DefaultFormatting, this.settings);
				//string serializedPagingInfo2 = JsonConvert.SerializeObject(pagingInfo2, DefaultFormatting, this.settings);

				Console.WriteLine(serializedPagingInfo1);
				//Console.WriteLine(serializedPagingInfo2);
			}
			*/

			[Fact]
			public void Serializes_All_Properties()
			{
				PagingState pagingInfo = new PagingState(7, 20, 1138);
				string serializedPagingInfo = JsonConvert.SerializeObject(
					pagingInfo, DefaultFormatting, this.settings);

				Assert.Equal(Page7_Size20_Total1138, serializedPagingInfo);
			}

			[Fact]
			public void Serializes_With_All_Pages_When_Specified()
			{
				PagingState pagingInfo = new PagingState(2, 10, 27);
				string serializedPagingInfo = JsonConvert.SerializeObject(
					pagingInfo, DefaultFormatting, this.settings);

				Assert.Equal(Page2_Size10_Total27_WithAllPages, serializedPagingInfo);
			}

			[Fact]
			public void Serializes_Unbounded_Value()
			{
				PagingState pagingInfo = new PagingState(PageNumberAndSize.Unbounded, 1701);
				string serializedPagingInfo = JsonConvert.SerializeObject(pagingInfo, DefaultFormatting, this.settings);

				Assert.Equal(Unbounded_Total1701, serializedPagingInfo);
			}

			[Fact]
			public void Serializes_Unbounded_Value_With_All_Pages()
			{
				PagingState pagingInfo = new PagingState(PageNumberAndSize.Unbounded, 1138);
				string serializedPagingInfo = JsonConvert.SerializeObject(pagingInfo, DefaultFormatting, this.settings);

				Assert.Equal(Unbounded_Total1138_WithAllPages, serializedPagingInfo);
			}

			[Fact]
			public void Serializes_Empty_Value()
			{
				string serializedPagingInfo = JsonConvert.SerializeObject(PagingState.Empty, settings);

				Assert.Equal(EmptySerialized, serializedPagingInfo);
			}

			[Fact]
			public void Deserializes_From_Minimal_Specification()
			{
				PagingState expectedPagingInfo = new PagingState(7, 20, 1138);
				PagingState deserializedPagingInfo
					= JsonConvert.DeserializeObject<PagingState>(
						DeserializeMinimal_Page7_Size20_Total1138, this.settings);

				Assert.Equal(expectedPagingInfo.CurrentPage.Number, deserializedPagingInfo.CurrentPage.Number);
				Assert.Equal(expectedPagingInfo.CurrentPage.Size, deserializedPagingInfo.CurrentPage.Size);
				Assert.Equal(expectedPagingInfo.TotalItems, deserializedPagingInfo.TotalItems);

				AssertEquality(expectedPagingInfo, deserializedPagingInfo);
			}

			[Fact]
			public void Deserializes_And_Ignores_Inconsistency_From_Excessive_Specification()
			{
				PagingState expectedPagingInfo = new PagingState(8, 17, 666);
				PagingState deserializedPagingInfo
					= JsonConvert.DeserializeObject<PagingState>(
						DeserializeExcessive_Page8_Size17_Total666, this.settings);

				Assert.Equal(expectedPagingInfo.CurrentPage.Number, deserializedPagingInfo.CurrentPage.Number);
				Assert.Equal(expectedPagingInfo.CurrentPage.Size, deserializedPagingInfo.CurrentPage.Size);
				Assert.Equal(expectedPagingInfo.TotalItems, deserializedPagingInfo.TotalItems);

				AssertEquality(expectedPagingInfo, deserializedPagingInfo);
			}

			[Fact]
			public void Deserializes_As_Invalid_From_Negative_Page_Number()
			{
				PagingState deserializedPagingInfo
					= JsonConvert.DeserializeObject<PagingState>(
						 @"{""CurrentPage"":{""Number"":-7,""Size"":20},""TotalItems"":1138}");

				Assert.False(deserializedPagingInfo.HasValue);
				Assert.Equal(-7, deserializedPagingInfo.CurrentPage.Number);
				Assert.False(deserializedPagingInfo.CurrentPage.HasValue);
			}

			[Fact]
			public void Deserializes_As_Invalid_From_Negative_TotalItems()
			{
				PagingState deserializedPagingInfo
					= JsonConvert.DeserializeObject<PagingState>(
						 @"{""CurrentPage"":{""Number"":7,""Size"":20},""TotalItems"":-1138}");

				Assert.False(deserializedPagingInfo.HasValue);
				Assert.Equal(-1138, deserializedPagingInfo.TotalItems);
				Assert.True(deserializedPagingInfo.CurrentPage.HasValue);
			}

			[Fact]
			public void Does_Not_Deserialize_From_Negative_Page_Size()
			{
				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PagingState>(
						@"{""CurrentPage"":{""Number"":7,""Size"":-20},""TotalItems"":1138}"));
			}

			[Fact]
			public void Does_Not_Deserialize_From_Omitted_Page_Number()
			{
				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PagingState>(
						@"{""CurrentPage"":{""Size"":20},""TotalItems"":1138}"));
			}

			[Fact]
			public void Does_Not_Deserialize_From_Omitted_Page_Size()
			{
				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PagingState>(
						@"{""CurrentPage"":{""Number"":7},""TotalItems"":1138}"));
			}

			[Fact]
			public void Does_Not_Deserialize_From_Omitted_TotalItems()
			{
				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PagingState>(
						@"{""CurrentPage"":{""Number"":7,""Size"":20}}"));
			}
		}

		#region [ Internal Static Test Assertion Methods ]

		internal static void AssertEquality(PagingState expected, PagingState actual)
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

		internal static void AssertInequality(PagingState expected, PagingState actual)
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

		#endregion
	}
}