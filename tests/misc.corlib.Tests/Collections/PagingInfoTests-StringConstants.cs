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
    ""Index"": 7
  },
  ""PreviousPage"": {
    ""Number"": 6,
    ""Index"": 5
  },
  ""FirstPage"": {
    ""Number"": 1,
    ""Index"": 0
  },
  ""LastPage"": {
    ""Number"": 57,
    ""Index"": 56
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
    ""Index"": 2
  },
  ""PreviousPage"": {
    ""Number"": 1,
    ""Index"": 0
  },
  ""FirstPage"": {
    ""Number"": 1,
    ""Index"": 0
  },
  ""LastPage"": {
    ""Number"": 3,
    ""Index"": 2
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
	}
}