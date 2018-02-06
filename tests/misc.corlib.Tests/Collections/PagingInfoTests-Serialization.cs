#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Collections
	PagingInfoTests-Serialization.cs

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
				PagingInfo pagingInfo = new PagingInfo(7, 20, 1138);
				string serializedPagingInfo = JsonConvert.SerializeObject(
					pagingInfo, DefaultFormatting, this.settings);

				Assert.AreEqual(Page7_Size20_Total1138, serializedPagingInfo);
			}

			[Fact]
			public void Serializes_With_All_Pages_When_Specified()
			{
				PagingInfo pagingInfo = new PagingInfo(2, 10, 27, true);
				string serializedPagingInfo = JsonConvert.SerializeObject(
					pagingInfo, DefaultFormatting, this.settings);

				Assert.AreEqual(Page2_Size10_Total27_WithAllPages, serializedPagingInfo);
			}

			[Fact]
			public void Serializes_Unbounded_Value()
			{
				PagingInfo pagingInfo = new PagingInfo(PageNumberAndSize.Unbounded, 1701);
				string serializedPagingInfo = JsonConvert.SerializeObject(pagingInfo, DefaultFormatting, this.settings);

				Assert.AreEqual(Unbounded_Total1701, serializedPagingInfo);
			}

			[Fact]
			public void Serializes_Unbounded_Value_With_All_Pages()
			{
				PagingInfo pagingInfo = new PagingInfo(PageNumberAndSize.Unbounded, 1138, true);
				string serializedPagingInfo = JsonConvert.SerializeObject(pagingInfo, DefaultFormatting, this.settings);

				Assert.AreEqual(Unbounded_Total1138_WithAllPages, serializedPagingInfo);
			}

			[Fact]
			public void Serializes_Empty_Value()
			{
				string serializedPagingInfo = JsonConvert.SerializeObject(PagingInfo.Empty, settings);

				Assert.AreEqual(EmptySerialized, serializedPagingInfo);
			}

			[Fact]
			public void Deserializes_From_Minimal_Specification()
			{
				PagingInfo expectedPagingInfo = new PagingInfo(7, 20, 1138);
				PagingInfo deserializedPagingInfo
					= JsonConvert.DeserializeObject<PagingInfo>(
						DeserializeMinimal_Page7_Size20_Total1138, this.settings);

				Assert.AreEqual(expectedPagingInfo.CurrentPage.Number, deserializedPagingInfo.CurrentPage.Number);
				Assert.AreEqual(expectedPagingInfo.CurrentPage.Size, deserializedPagingInfo.CurrentPage.Size);
				Assert.AreEqual(expectedPagingInfo.TotalItems, deserializedPagingInfo.TotalItems);
				Assert.AreEqual(expectedPagingInfo.TotalPages, deserializedPagingInfo.TotalPages);

				AssertEquality(expectedPagingInfo, deserializedPagingInfo);
			}

			[Fact]
			public void Deserializes_And_Ignores_Inconsistency_From_Excessive_Specification()
			{
				PagingInfo expectedPagingInfo = new PagingInfo(8, 17, 666);
				PagingInfo deserializedPagingInfo
					= JsonConvert.DeserializeObject<PagingInfo>(
						DeserializeExcessive_Page8_Size17_Total666, this.settings);

				Assert.AreEqual(expectedPagingInfo.CurrentPage.Number, deserializedPagingInfo.CurrentPage.Number);
				Assert.AreEqual(expectedPagingInfo.CurrentPage.Size, deserializedPagingInfo.CurrentPage.Size);
				Assert.AreEqual(expectedPagingInfo.TotalItems, deserializedPagingInfo.TotalItems);
				Assert.AreEqual(expectedPagingInfo.TotalPages, deserializedPagingInfo.TotalPages);

				AssertEquality(expectedPagingInfo, deserializedPagingInfo);
			}

			[Fact]
			public void Deserializes_As_Invalid_From_Negative_Page_Number()
			{
				PagingInfo deserializedPagingInfo
					= JsonConvert.DeserializeObject<PagingInfo>(
						 @"{""CurrentPage"":{""Number"":-7,""Size"":20},""TotalItems"":1138}");

				Assert.IsFalse(deserializedPagingInfo.HasValue);
				Assert.AreEqual(-7, deserializedPagingInfo.CurrentPage.Number);
				Assert.IsFalse(deserializedPagingInfo.CurrentPage.HasValue);
			}

			[Fact]
			public void Deserializes_As_Invalid_From_Negative_TotalItems()
			{
				PagingInfo deserializedPagingInfo
					= JsonConvert.DeserializeObject<PagingInfo>(
						 @"{""CurrentPage"":{""Number"":7,""Size"":20},""TotalItems"":-1138}");

				Assert.IsFalse(deserializedPagingInfo.HasValue);
				Assert.AreEqual(-1138, deserializedPagingInfo.TotalItems);
				Assert.IsTrue(deserializedPagingInfo.CurrentPage.HasValue);
			}

			[Fact]
			public void Does_Not_Deserialize_From_Negative_Page_Size()
			{
				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PagingInfo>(
						@"{""CurrentPage"":{""Number"":7,""Size"":-20},""TotalItems"":1138}"));
			}

			[Fact]
			public void Does_Not_Deserialize_From_Omitted_Page_Number()
			{
				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PagingInfo>(
						@"{""CurrentPage"":{""Size"":20},""TotalItems"":1138}"));
			}

			[Fact]
			public void Does_Not_Deserialize_From_Omitted_Page_Size()
			{
				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PagingInfo>(
						@"{""CurrentPage"":{""Number"":7},""TotalItems"":1138}"));
			}

			[Fact]
			public void Does_Not_Deserialize_From_Omitted_TotalItems()
			{
				Assert.Throws<JsonSerializationException>(
					() => JsonConvert.DeserializeObject<PagingInfo>(
						@"{""CurrentPage"":{""Number"":7,""Size"":20}}"));
			}
		}
	}
}