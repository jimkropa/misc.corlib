using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xunit;

namespace MiscCorLib.Collections
{
	// TODO: Test ToString
	// TODO: Test IComparable
	public sealed class PageNumberAndItemNumbersTests
	{
		public sealed class JsonNetSerialization
		{
			[Fact]
			public void Serializes_All_Properties()
			{
				PageNumberAndItemNumbers page = new PageNumberAndItemNumbers(4, 10, 36);
				string serializedPage = JsonConvert.SerializeObject(page);

				Assert.Equal(
					"{\"PageNumber\":4,\"FirstItemNumber\":31,\"LastItemNumber\":36}",
					serializedPage);
			}

			[Fact]
			public void Serializes_Empty_Value()
			{
				string serializedPage = JsonConvert.SerializeObject(PageNumberAndItemNumbers.Empty);

				Assert.Equal(
					"{\"PageNumber\":0,\"FirstItemNumber\":0,\"LastItemNumber\":0}",
					serializedPage);
			}

			[Fact]
			public void Deserializes_From_Minimal_Specification()
			{
				PageNumberAndItemNumbers page = new PageNumberAndItemNumbers(8, 20, 148);
				PageNumberAndItemNumbers deserializedPage
					= JsonConvert.DeserializeObject<PageNumberAndItemNumbers>(
						"{\"PageNumber\":8,\"FirstItemNumber\":141,\"LastItemNumber\":148}");

				AssertEquality(page, deserializedPage);
			}

			[Fact]
			public void Deserializes_As_Invalid_From_Negative_Numbers()
			{
				PageNumberAndItemNumbers deserializedPage
					= JsonConvert.DeserializeObject<PageNumberAndItemNumbers>(
						"{\"PageNumber\":-7,\"FirstItemNumber\":20,\"LastItemNumber\":148}");

				Assert.False(deserializedPage.HasValue);

				deserializedPage
					= JsonConvert.DeserializeObject<PageNumberAndItemNumbers>(
						"{\"PageNumber\":7,\"FirstItemNumber\":-20,\"LastItemNumber\":148}");

				Assert.False(deserializedPage.HasValue);

				deserializedPage
					= JsonConvert.DeserializeObject<PageNumberAndItemNumbers>(
						"{\"PageNumber\":7,\"FirstItemNumber\":20,\"LastItemNumber\":-148}");

				Assert.False(deserializedPage.HasValue);
			}

			[Fact]
			public void Does_Not_Deserialize_From_Omitted_Numbers()
			{
				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PageNumberAndItemNumbers>(
						"{\"FirstItemNumber\":20,\"LastItemNumber\":148}"));

				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PageNumberAndItemNumbers>(
						"{\"PageNumber\":7,\"LastItemNumber\":148}"));

				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PageNumberAndItemNumbers>(
						"{\"PageNumber\":7,\"FirstItemNumber\":20}"));
			}
		}

		public sealed class AllPagesAndItemNumbers
		{
			[Fact]
			public void CaclucatesCorrectlyFromValidSizes()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pages
					= PageNumberAndItemNumbers.Calculate(20, 119);

				Assert.NotNull(pages);
				Assert.Equal(6, pages.Count);
				Assert.Equal(119, pages[pages.Count - 1].LastItemNumber);

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

				Assert.Equal(119, lastItemNumber);
				Assert.True(pages[0].HasValue);
			}

			[Fact]
			public void ReturnsOnePageForZeroItems()
			{
				IReadOnlyList<PageNumberAndItemNumbers> zeroPagesWithSize
					= PageNumberAndItemNumbers.Calculate(20, 0);

				Assert.NotNull(zeroPagesWithSize);
				Assert.Equal(1, zeroPagesWithSize.Count);
				Assert.Equal(1, zeroPagesWithSize[0].PageNumber);
				Assert.Equal(0, zeroPagesWithSize[0].FirstItemNumber);
				Assert.Equal(0, zeroPagesWithSize[0].LastItemNumber);

				IReadOnlyList<PageNumberAndItemNumbers> zeroPagesUnbounded
					= PageNumberAndItemNumbers.Calculate(0, 0);

				Assert.NotNull(zeroPagesUnbounded);
				Assert.Equal(1, zeroPagesUnbounded.Count);
				Assert.Equal(1, zeroPagesUnbounded[0].PageNumber);
				Assert.Equal(0, zeroPagesUnbounded[0].FirstItemNumber);
				Assert.Equal(0, zeroPagesUnbounded[0].LastItemNumber);
			}

			[Fact]
			public void ReturnsOnePageForUnbounded()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pagesUnbounded
					= PageNumberAndItemNumbers.Calculate(0, 57);

				Assert.NotNull(pagesUnbounded);
				Assert.Equal(1, pagesUnbounded.Count);
				Assert.Equal(1, pagesUnbounded[0].PageNumber);
				Assert.Equal(1, pagesUnbounded[0].FirstItemNumber);
				Assert.Equal(57, pagesUnbounded[0].LastItemNumber);
			}

			[Fact]
			public void DoesNotAllowInvalidSizes()
			{
				Assert.Throws<ArgumentOutOfRangeException>(
					() => PageNumberAndItemNumbers.Calculate(0, -1));
			}
		}

		#region [ Internal Static Test Assertion Methods ]

		internal static void AssertEquality(
			PageNumberAndItemNumbers expected, PageNumberAndItemNumbers actual)
		{
			Assert.True(expected == actual);
			Assert.False(expected != actual);
			Assert.True(expected.Equals(actual));
			Assert.Equal(expected, actual);

			Assert.True(actual == expected);
			Assert.False(actual != expected);
			Assert.True(actual.Equals(expected));
			Assert.Equal(actual, expected);

			Assert.Equal(expected.PageNumber, actual.PageNumber);
			Assert.Equal(expected.FirstItemNumber, actual.FirstItemNumber);
			Assert.Equal(expected.LastItemNumber, actual.LastItemNumber);
			Assert.Equal(expected.HasValue, actual.HasValue);
		}

		internal static void AssertIsEmpty(PageNumberAndItemNumbers page)
		{
			Assert.False(page.HasValue);
			Assert.Equal(0, page.PageNumber);
			Assert.Equal(0, page.FirstItemNumber);
			Assert.Equal(0, page.LastItemNumber);
		}

		#endregion
	}
}