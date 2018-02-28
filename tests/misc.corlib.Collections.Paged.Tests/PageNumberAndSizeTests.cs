using Newtonsoft.Json;
using Xunit;

namespace MiscCorLib.Collections.Paged
{
	// TODO: Test ToString
	// TODO: Test IComparable
	public sealed class PageNumberAndSizeTests
	{
		public sealed class JsonNetSerialization
		{
			[Fact]
			public void Serializes_All_Properties()
			{
				PageNumberAndSize page = new PageNumberAndSize(7, 20);
				string serializedPage = JsonConvert.SerializeObject(page);

				Assert.Equal(
					"{\"Number\":7,\"Size\":20,\"Index\":6,\"IsUnbounded\":false}", // ,\"HasValue\":true
					serializedPage);
			}

			[Fact]
			public void Serializes_Unbounded_Value()
			{
				string serializedPage = JsonConvert.SerializeObject(PageNumberAndSize.Unbounded);

				Assert.Equal(
					"{\"Number\":1,\"Size\":0,\"Index\":0,\"IsUnbounded\":true}", // ,\"HasValue\":true
					serializedPage);
			}

			[Fact]
			public void Serializes_Empty_Value()
			{
				string serializedPage = JsonConvert.SerializeObject(PageNumberAndSize.Empty);

				Assert.Equal(
					"{\"Number\":0,\"Size\":0,\"Index\":-1,\"IsUnbounded\":false}", // ,\"HasValue\":false
					serializedPage);
			}

			[Fact]
			public void Deserializes_From_Minimal_Specification()
			{
				PageNumberAndSize page = new PageNumberAndSize(7);
				PageNumberAndSize deserializedPage
					= JsonConvert.DeserializeObject<PageNumberAndSize>(
						"{\"Number\":7,\"Size\":10}");

				AssertEquality(page, deserializedPage);
			}

			[Fact]
			public void Deserializes_And_Ignores_Inconsistency_From_Excessive_Specification()
			{
				PageNumberAndSize page = new PageNumberAndSize(7, 20);
				PageNumberAndSize deserializedPage
					= JsonConvert.DeserializeObject<PageNumberAndSize>(
						"{\"Number\":7,\"Size\":20,\"Index\":1111111,\"IsUnbounded\":true}"); // ,\"HasValue\":false

				AssertEquality(page, deserializedPage);
			}

			[Fact]
			public void Deserializes_As_Invalid_From_Negative_Page_Number()
			{
				PageNumberAndSize deserializedPage
					= JsonConvert.DeserializeObject<PageNumberAndSize>(
						"{\"Number\":-7,\"Size\":10}");

				Assert.False(deserializedPage.HasValue);
				Assert.False(deserializedPage.IsUnbounded);
			}

			[Fact]
			public void Does_Not_Deserialize_From_Negative_Page_Size()
			{
				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PageNumberAndSize>(
						"{\"Number\":0,\"Size\":-1}"));
			}

			[Fact]
			public void Does_Not_Deserialize_From_Omitted_Page_Number()
			{
				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PageNumberAndSize>(
						"{\"Size\":20}"));
			}

			[Fact]
			public void Does_Not_Deserialize_From_Omitted_Page_Size()
			{
				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PageNumberAndSize>(
						"{\"Number\":0}"));
			}
		}

		#region [ Internal Static Test Assertion Methods ]

		internal static void AssertEquality(
			PageNumberAndSize expected, PageNumberAndSize actual)
		{
			Assert.True(expected == actual);
			Assert.False(expected != actual);
			Assert.True(expected.Equals(actual));
			Assert.Equal(expected, actual);

			Assert.True(actual == expected);
			Assert.False(actual != expected);
			Assert.True(actual.Equals(expected));
			Assert.Equal(actual, expected);

			Assert.Equal(expected.Number, actual.Number);
			Assert.Equal(expected.Size, actual.Size);
			Assert.Equal(expected.Index, actual.Index);
			Assert.Equal(expected.IsUnbounded, actual.IsUnbounded);
			Assert.Equal(expected.HasValue, actual.HasValue);
		}

		internal static void AssertIsEmpty(PageNumberAndSize emptyPage)
		{
			Assert.False(emptyPage.HasValue);
			Assert.False(emptyPage.IsUnbounded);
			Assert.Equal(byte.MinValue, emptyPage.Size);
			Assert.Equal(0, emptyPage.Number);
			Assert.Equal(-1, emptyPage.Index);
		}

		internal static void AssertIsFirstPage(PageNumberAndSize firstPage)
		{
			Assert.True(firstPage.HasValue);
			Assert.InRange(firstPage.Size, byte.MinValue, byte.MaxValue);
			Assert.Equal(PageNumberAndSize.PageOne, firstPage.Number);
			Assert.Equal(0, firstPage.Index);
		}

		internal static void AssertIsUnbounded(PageNumberAndSize unboundedPage)
		{
			Assert.True(unboundedPage.IsUnbounded);
			Assert.Equal(byte.MinValue, unboundedPage.Size);

			AssertIsFirstPage(unboundedPage);
		}

		#endregion
	}
}