namespace MiscCorLib.Collections
{
	using Newtonsoft.Json;

	using NUnit.Framework;

	// TODO: Test ToString
	// TODO: Test AllPagesAndItemNumbers
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