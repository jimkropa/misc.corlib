#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Collections
	PageNumberAndSizeTests.cs

	Copyright (c) 2016 Jim Kropa (https://github.com/jimkropa)

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

		http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/
#endregion

namespace MiscCorLib.Collections
{
	using Newtonsoft.Json;

	using NUnit.Framework;

	// TODO: Test ToString
	// TODO: Test IComparable
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
					"{\"Number\":7,\"Size\":20,\"Index\":6,\"IsUnbounded\":false}", // ,\"HasValue\":true
					serializedPage);
			}

			[Test]
			public void Serializes_Unbounded_Value()
			{
				string serializedPage = JsonConvert.SerializeObject(PageNumberAndSize.Unbounded);

				Assert.AreEqual(
					"{\"Number\":1,\"Size\":0,\"Index\":0,\"IsUnbounded\":true}", // ,\"HasValue\":true
					serializedPage);
			}

			[Test]
			public void Serializes_Empty_Value()
			{
				string serializedPage = JsonConvert.SerializeObject(PageNumberAndSize.Empty);

				Assert.AreEqual(
					"{\"Number\":0,\"Size\":0,\"Index\":-1,\"IsUnbounded\":false}", // ,\"HasValue\":false
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
						"{\"Number\":7,\"Size\":20,\"Index\":1111111,\"IsUnbounded\":true}"); // ,\"HasValue\":false

				AssertEquality(page, deserializedPage);
			}

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

		#region [ Internal Static Test Assertion Methods ]

		internal static void AssertEquality(
			PageNumberAndSize expected, PageNumberAndSize actual)
		{
			Assert.IsTrue(expected == actual);
			Assert.IsFalse(expected != actual);
			Assert.IsTrue(expected.Equals(actual));
			Assert.AreEqual(expected, actual);

			Assert.IsTrue(actual == expected);
			Assert.IsFalse(actual != expected);
			Assert.IsTrue(actual.Equals(expected));
			Assert.AreEqual(actual, expected);

			Assert.AreEqual(expected.Number, actual.Number);
			Assert.AreEqual(expected.Size, actual.Size);
			Assert.AreEqual(expected.Index, actual.Index);
			Assert.AreEqual(expected.IsUnbounded, actual.IsUnbounded);
			Assert.AreEqual(expected.HasValue, actual.HasValue);
		}

		internal static void AssertIsEmpty(PageNumberAndSize emptyPage)
		{
			Assert.IsFalse(emptyPage.HasValue);
			Assert.IsFalse(emptyPage.IsUnbounded);
			Assert.AreEqual(byte.MinValue, emptyPage.Size);
			Assert.AreEqual(0, emptyPage.Number);
			Assert.AreEqual(-1, emptyPage.Index);
		}

		internal static void AssertIsFirstPage(PageNumberAndSize firstPage)
		{
			Assert.IsTrue(firstPage.HasValue);
			Assert.GreaterOrEqual(firstPage.Size, byte.MinValue);
			Assert.AreEqual(PageNumberAndSize.FirstPageNumber, firstPage.Number);
			Assert.AreEqual(0, firstPage.Index);
		}

		internal static void AssertIsUnbounded(PageNumberAndSize unboundedPage)
		{
			Assert.IsTrue(unboundedPage.IsUnbounded);
			Assert.AreEqual(byte.MinValue, unboundedPage.Size);

			AssertIsFirstPage(unboundedPage);
		}

		#endregion
	}
}