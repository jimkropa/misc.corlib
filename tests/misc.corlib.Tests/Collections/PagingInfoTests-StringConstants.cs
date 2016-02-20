// ReSharper disable InconsistentNaming
namespace MiscCorLib.Collections
{
	// This is a partial class. Refer to PagingInfoTests.cs for the main part.
	public sealed partial class PagingInfoTests
	{
		private const string Page7_Size20_Total1138 = @"{
  ""CurrentPage"": {
    ""Number"": 7,
    ""Size"": 20,
    ""Index"": 6,
    ""IsUnbounded"": false
  },
  ""TotalItems"": 1138,
  ""TotalPages"": 57,
  ""IsFirstPage"": false,
  ""IsLastPage"": false,
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
  }
}";

		private const string Page2_Size10_Total27_WithAllPages = @"{
  ""CurrentPage"": {
    ""Number"": 2,
    ""Size"": 10,
    ""Index"": 1,
    ""IsUnbounded"": false
  },
  ""TotalItems"": 27,
  ""TotalPages"": 3,
  ""IsFirstPage"": false,
  ""IsLastPage"": false,
  ""FirstItemNumber"": 11,
  ""LastItemNumber"": 20,
  ""FirstItemIndex"": 10,
  ""LastItemIndex"": 19,
  ""ItemCount"": 10,
  ""NextPage"": {
    ""Number"": 3,
    ""Size"": 10,
    ""Index"": 2,
    ""IsUnbounded"": false
  },
  ""PreviousPage"": {
    ""Number"": 1,
    ""Size"": 10,
    ""Index"": 0,
    ""IsUnbounded"": false
  },
  ""FirstPage"": {
    ""Number"": 1,
    ""Size"": 10,
    ""Index"": 0,
    ""IsUnbounded"": false
  },
  ""LastPage"": {
    ""Number"": 3,
    ""Size"": 10,
    ""Index"": 2,
    ""IsUnbounded"": false
  },
  ""AllPages"": [
    {
      ""PageNumber"": 1,
      ""FirstItemNumber"": 1,
      ""LastItemNumber"": 10
    },
    {
      ""PageNumber"": 2,
      ""FirstItemNumber"": 11,
      ""LastItemNumber"": 20
    },
    {
      ""PageNumber"": 3,
      ""FirstItemNumber"": 21,
      ""LastItemNumber"": 27
    }
  ]
}";

		private const string Deserialize_Page7_Size20_Total1138
			= @"{""CurrentPage"":{""Number"":7,""Size"":20},""TotalItems"":1138}";

		private const string Unbounded_Total_WithAllPages = @"";

		private const string Unbounded_Total1138_WithAllPages = @"";
	}
}