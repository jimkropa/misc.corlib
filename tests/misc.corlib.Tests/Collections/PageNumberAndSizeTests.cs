namespace MiscCorLib.Collections
{
	using System;

	using Newtonsoft.Json;

	using NUnit.Framework;

	[TestFixture]
	public sealed class PageNumberAndSizeTests
	{
		[TestFixture]
		public sealed class JsonNetSerialization
		{
			[Test]
			public void Serializes_All_Properties()
			{
				PageNumberAndSize page = new PageNumberAndSize(7, 20);
				string serializedPage = JsonConvert.SerializeObject(page);

				Assert.AreEqual(
					"{\"Number\":7,\"Size\":20,\"Index\":6,\"IsUnbounded\":false,\"IsValid\":true}",
					serializedPage);
			}

			[Test]
			public void Serializes_Unbounded_Value()
			{
				string serializedPage = JsonConvert.SerializeObject(PageNumberAndSize.Unbounded);

				Assert.AreEqual(
					"{\"Number\":1,\"Size\":0,\"Index\":0,\"IsUnbounded\":true,\"IsValid\":true}",
					serializedPage);
			}

			[Test]
			public void Serializes_Empty_Value()
			{
				string serializedPage = JsonConvert.SerializeObject(PageNumberAndSize.Empty);

				Assert.AreEqual(
					"{\"Number\":0,\"Size\":0,\"Index\":-1,\"IsUnbounded\":false,\"IsValid\":false}",
					serializedPage);
			}

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
						"{\"Number\":7,\"Size\":20,\"Index\":1111111,\"IsUnbounded\":true,\"IsValid\":false}");

				AssertEquality(page, deserializedPage);
			}

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

		#region [ Internal Static Test Assertion Methods ]

		internal static void AssertEquality(
			PageNumberAndSize expected, PageNumberAndSize actual)
		{
			Assert.IsTrue(expected.Equals(actual));
			Assert.AreEqual(expected, actual);
			Assert.AreEqual(expected.Number, actual.Number);
			Assert.AreEqual(expected.Size, actual.Size);
			Assert.AreEqual(expected.Index, actual.Index);
			Assert.AreEqual(expected.IsUnbounded, actual.IsUnbounded);
			Assert.AreEqual(expected.IsValid, actual.IsValid);
		}

		internal static void AssertIsEmpty(PageNumberAndSize page)
		{
			Assert.IsFalse(page.IsValid);
			Assert.IsFalse(page.IsUnbounded);
			Assert.AreEqual(byte.MinValue, page.Size);
			Assert.AreEqual(0, page.Number);
			Assert.AreEqual(-1, page.Index);
		}

		internal static void AssertIsFirstPage(PageNumberAndSize page)
		{
			Assert.IsTrue(page.IsValid);
			Assert.GreaterOrEqual(page.Size, byte.MinValue);
			Assert.AreEqual(PageNumberAndSize.FirstPageNumber, page.Number);
			Assert.AreEqual(0, page.Index);
		}

		internal static void AssertIsUnbounded(PageNumberAndSize page)
		{
			Assert.IsTrue(page.IsUnbounded);
			Assert.AreEqual(byte.MinValue, page.Size);

			AssertIsFirstPage(page);
		}

		#endregion
	}
}