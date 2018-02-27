namespace MiscCorLib.Collections.Paged
{
	// This is a partial class. Refer to PagingStateTests.cs for the main part.
	public sealed partial class PagingStateTests
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

		private const string DeserializeMinimal_Page7_Size20_Total1138
			= @"{""CurrentPage"":{""Number"":7,""Size"":20},""TotalItems"":1138}";

		// Only CurrentPage.Number, CurrentPage.Size, and TotalItems are real.
		// The rest are incorrect!
		private const string DeserializeExcessive_Page8_Size17_Total666 = @"{
  ""CurrentPage"": {
    ""Number"": 8,
    ""Size"": 17,
    ""Index"": 87878,
    ""IsUnbounded"": true
  },
  ""TotalItems"": 666,
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

		private const string Unbounded_Total1701 = @"{
  ""CurrentPage"": {
    ""Number"": 1,
    ""Size"": 0,
    ""Index"": 0,
    ""IsUnbounded"": true
  },
  ""TotalItems"": 1701,
  ""TotalPages"": 1,
  ""IsFirstPage"": true,
  ""IsLastPage"": true,
  ""FirstItemNumber"": 1,
  ""LastItemNumber"": 1701,
  ""FirstItemIndex"": 0,
  ""LastItemIndex"": 1700,
  ""ItemCount"": 1701,
  ""NextPage"": {
    ""Number"": 0,
    ""Size"": 0,
    ""Index"": -1,
    ""IsUnbounded"": false
  },
  ""PreviousPage"": {
    ""Number"": 0,
    ""Size"": 0,
    ""Index"": -1,
    ""IsUnbounded"": false
  },
  ""FirstPage"": {
    ""Number"": 1,
    ""Size"": 0,
    ""Index"": 0,
    ""IsUnbounded"": true
  },
  ""LastPage"": {
    ""Number"": 1,
    ""Size"": 0,
    ""Index"": 0,
    ""IsUnbounded"": true
  }
}";

		private const string Unbounded_Total1138_WithAllPages = @"{
  ""CurrentPage"": {
    ""Number"": 1,
    ""Size"": 0,
    ""Index"": 0,
    ""IsUnbounded"": true
  },
  ""TotalItems"": 1138,
  ""TotalPages"": 1,
  ""IsFirstPage"": true,
  ""IsLastPage"": true,
  ""FirstItemNumber"": 1,
  ""LastItemNumber"": 1138,
  ""FirstItemIndex"": 0,
  ""LastItemIndex"": 1137,
  ""ItemCount"": 1138,
  ""NextPage"": {
    ""Number"": 0,
    ""Size"": 0,
    ""Index"": -1,
    ""IsUnbounded"": false
  },
  ""PreviousPage"": {
    ""Number"": 0,
    ""Size"": 0,
    ""Index"": -1,
    ""IsUnbounded"": false
  },
  ""FirstPage"": {
    ""Number"": 1,
    ""Size"": 0,
    ""Index"": 0,
    ""IsUnbounded"": true
  },
  ""LastPage"": {
    ""Number"": 1,
    ""Size"": 0,
    ""Index"": 0,
    ""IsUnbounded"": true
  },
  ""AllPages"": [
    {
      ""PageNumber"": 1,
      ""FirstItemNumber"": 1,
      ""LastItemNumber"": 1138
    }
  ]
}";

		public const string EmptySerialized = @"{
  ""CurrentPage"": {
    ""Number"": 0,
    ""Size"": 0,
    ""Index"": -1,
    ""IsUnbounded"": false
  },
  ""TotalItems"": 0,
  ""TotalPages"": 0,
  ""IsFirstPage"": false,
  ""IsLastPage"": false,
  ""FirstItemNumber"": 0,
  ""LastItemNumber"": 0,
  ""FirstItemIndex"": -1,
  ""LastItemIndex"": -1,
  ""ItemCount"": 0,
  ""NextPage"": {
    ""Number"": 0,
    ""Size"": 0,
    ""Index"": -1,
    ""IsUnbounded"": false
  },
  ""PreviousPage"": {
    ""Number"": 0,
    ""Size"": 0,
    ""Index"": -1,
    ""IsUnbounded"": false
  },
  ""FirstPage"": {
    ""Number"": 0,
    ""Size"": 0,
    ""Index"": -1,
    ""IsUnbounded"": false
  },
  ""LastPage"": {
    ""Number"": 0,
    ""Size"": 0,
    ""Index"": -1,
    ""IsUnbounded"": false
  }
}";
	}
}