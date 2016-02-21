namespace MiscCorLib.Collections
{
	using System;
	using System.Collections.Generic;

	using Newtonsoft.Json;

	using NUnit.Framework;

	// TODO: Test ToString
	// TODO: Test IComparable
	[TestFixture]
	public sealed class PageNumberAndItemNumbersTests
	{
		[TestFixture]
		public sealed class JsonNetSerialization
		{
			[Test]
			public void Serializes_All_Properties()
			{
				PageNumberAndItemNumbers page = new PageNumberAndItemNumbers(4, 10, 36);
				string serializedPage = JsonConvert.SerializeObject(page);

				Assert.AreEqual(
					"{\"PageNumber\":4,\"FirstItemNumber\":31,\"LastItemNumber\":36}",
					serializedPage);
			}

			[Test]
			public void Serializes_Empty_Value()
			{
				string serializedPage = JsonConvert.SerializeObject(PageNumberAndItemNumbers.Empty);

				Assert.AreEqual(
					"{\"PageNumber\":0,\"FirstItemNumber\":0,\"LastItemNumber\":0}",
					serializedPage);
			}

			[Test]
			public void Deserializes_From_Minimal_Specification()
			{
				PageNumberAndItemNumbers page = new PageNumberAndItemNumbers(8, 20, 148);
				PageNumberAndItemNumbers deserializedPage
					= JsonConvert.DeserializeObject<PageNumberAndItemNumbers>(
						"{\"PageNumber\":8,\"FirstItemNumber\":141,\"LastItemNumber\":148}");

				AssertEquality(page, deserializedPage);
			}

			[Test]
			public void Deserializes_As_Invalid_From_Negative_Numbers()
			{
				PageNumberAndItemNumbers deserializedPage
					= JsonConvert.DeserializeObject<PageNumberAndItemNumbers>(
						"{\"PageNumber\":-7,\"FirstItemNumber\":20,\"LastItemNumber\":148}");

				Assert.IsFalse(deserializedPage.HasValue);

				deserializedPage
					= JsonConvert.DeserializeObject<PageNumberAndItemNumbers>(
						"{\"PageNumber\":7,\"FirstItemNumber\":-20,\"LastItemNumber\":148}");

				Assert.IsFalse(deserializedPage.HasValue);

				deserializedPage
					= JsonConvert.DeserializeObject<PageNumberAndItemNumbers>(
						"{\"PageNumber\":7,\"FirstItemNumber\":20,\"LastItemNumber\":-148}");

				Assert.IsFalse(deserializedPage.HasValue);
			}

			[Test]
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

		[TestFixture]
		public sealed class AllPagesAndItemNumbers
		{
			[Test]
			public void CaclucatesCorrectlyFromValidSizes()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pages
					= PageNumberAndItemNumbers.Calculate(20, 119);

				Assert.IsNotNull(pages);
				Assert.AreEqual(6, pages.Count);
				Assert.AreEqual(119, pages[pages.Count - 1].LastItemNumber);

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

				Assert.AreEqual(119, lastItemNumber);
				Assert.IsTrue(pages[0].HasValue);
			}

			[Test]
			public void ReturnsOnePageForZeroItems()
			{
				IReadOnlyList<PageNumberAndItemNumbers> zeroPagesWithSize
					= PageNumberAndItemNumbers.Calculate(20, 0);

				Assert.IsNotNull(zeroPagesWithSize);
				Assert.AreEqual(1, zeroPagesWithSize.Count);
				Assert.AreEqual(1, zeroPagesWithSize[0].PageNumber);
				Assert.AreEqual(0, zeroPagesWithSize[0].FirstItemNumber);
				Assert.AreEqual(0, zeroPagesWithSize[0].LastItemNumber);

				IReadOnlyList<PageNumberAndItemNumbers> zeroPagesUnbounded
					= PageNumberAndItemNumbers.Calculate(0, 0);

				Assert.IsNotNull(zeroPagesUnbounded);
				Assert.AreEqual(1, zeroPagesUnbounded.Count);
				Assert.AreEqual(1, zeroPagesUnbounded[0].PageNumber);
				Assert.AreEqual(0, zeroPagesUnbounded[0].FirstItemNumber);
				Assert.AreEqual(0, zeroPagesUnbounded[0].LastItemNumber);
			}

			[Test]
			public void ReturnsOnePageForUnbounded()
			{
				IReadOnlyList<PageNumberAndItemNumbers> pagesUnbounded
					= PageNumberAndItemNumbers.Calculate(0, 57);

				Assert.IsNotNull(pagesUnbounded);
				Assert.AreEqual(1, pagesUnbounded.Count);
				Assert.AreEqual(1, pagesUnbounded[0].PageNumber);
				Assert.AreEqual(1, pagesUnbounded[0].FirstItemNumber);
				Assert.AreEqual(57, pagesUnbounded[0].LastItemNumber);
			}

			[Test]
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
			Assert.IsTrue(expected == actual);
			Assert.IsFalse(expected != actual);
			Assert.IsTrue(expected.Equals(actual));
			Assert.AreEqual(expected, actual);

			Assert.IsTrue(actual == expected);
			Assert.IsFalse(actual != expected);
			Assert.IsTrue(actual.Equals(expected));
			Assert.AreEqual(actual, expected);

			Assert.AreEqual(expected.PageNumber, actual.PageNumber, "PageNumber");
			Assert.AreEqual(expected.FirstItemNumber, actual.FirstItemNumber, "FirstItemNumber");
			Assert.AreEqual(expected.LastItemNumber, actual.LastItemNumber, "LastItemNumber");
			Assert.AreEqual(expected.HasValue, actual.HasValue, "HasValue");
		}

		internal static void AssertIsEmpty(PageNumberAndItemNumbers page)
		{
			Assert.IsFalse(page.HasValue);
			Assert.AreEqual(0, page.PageNumber);
			Assert.AreEqual(0, page.FirstItemNumber);
			Assert.AreEqual(0, page.LastItemNumber);
		}

		#endregion
	}
}