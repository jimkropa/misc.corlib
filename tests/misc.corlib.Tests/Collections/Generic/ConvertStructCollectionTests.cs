using System.Collections.Generic;
using Xunit;

namespace MiscCorLib.Collections.Generic
{
	/// <summary>
	/// Automated NUnit tests of the
	/// <see cref="ConvertStructCollection"/>
	/// static extension methods.
	/// </summary>
	public sealed class ConvertStructCollectionTests
	{
		private static readonly IEnumerable<int> SampleIntegerCollection = new[] { 7, 3, 9, 3, 5 };
		private static readonly IEnumerable<char> SampleCharCollection = new[] { 'A', 'a', ' ' };

		public sealed class ToStringArray
		{
			[Fact]
			public void Preserves_Duplicates_By_Default()
			{
				string[] result = SampleIntegerCollection.ToStringArray();

				Assert.Equal(5, result.Length);
				Assert.Equal("7", result[0]);
				Assert.Equal("3", result[1]);
				Assert.Equal("9", result[2]);
				Assert.Equal("3", result[3]);
				Assert.Equal("5", result[4]);
			}

			[Fact]
			public void Removes_Duplicates_When_Specified()
			{
				string[] result = SampleIntegerCollection.ToStringArray(true);

				Assert.Equal(4, result.Length);
				Assert.Equal("7", result[0]);
				Assert.Equal("3", result[1]);
				Assert.Equal("9", result[2]);
				Assert.Equal("5", result[3]);
			}

			[Fact]
			public void Allows_Custom_Formatter()
			{
				string[] result = SampleIntegerCollection.ToStringArray(item => item.ToString("00"), true);

				Assert.Equal(4, result.Length);
				Assert.Equal("07", result[0]);
				Assert.Equal("03", result[1]);
				Assert.Equal("09", result[2]);
				Assert.Equal("05", result[3]);
			}

			[Fact]
			public void Uses_Ordinal_String_Comparison()
			{
				string[] result = SampleCharCollection.ToStringArray();

				Assert.Equal("A", result[0]);
				Assert.Equal("a", result[1]);
			}

			[Fact]
			public void Omits_Empty_Strings()
			{
				string[] result = SampleCharCollection.ToStringArray();

				Assert.Equal(2, result.Length);
			}

			[Fact]
			public void Returns_Empty_From_Null_Input()
			{
				IEnumerable<char> nullArray = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				string[] result = nullArray.ToStringArray();

				Assert.Equal(0, result.Length);
			}
		}

		public sealed class ToDelimitedString
		{
			[Fact]
			public void Preserves_Duplicates_By_Default()
			{
				string result = SampleIntegerCollection.ToDelimitedString();

				Assert.Equal("7,3,9,3,5", result);
			}

			[Fact]
			public void Removes_Duplicates_When_Specified()
			{
				string result = SampleIntegerCollection.ToDelimitedString(true);

				Assert.Equal("7,3,9,5", result);
			}

			[Fact]
			public void Allows_Custom_Formatter()
			{
				string result = SampleIntegerCollection.ToDelimitedString(item => item.ToString("00"));

				Assert.Equal("07,03,09,03,05", result);
			}

			[Fact]
			public void Allows_Custom_Separator()
			{
				string result = SampleIntegerCollection.ToDelimitedString(":", true);

				Assert.Equal("7:3:9:5", result);
			}

			[Fact]
			public void Uses_Ordinal_String_Comparison()
			{
				string result = SampleCharCollection.ToDelimitedString();

				Assert.Equal("A,a", result);
			}

			[Fact]
			public void Omits_Empty_Strings()
			{
				string result = SampleCharCollection.ToDelimitedString();

				Assert.Equal("A,a", result);
			}

			[Fact]
			public void Returns_Empty_From_Null_Input()
			{
				IEnumerable<char> nullArray = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				string result = nullArray.ToDelimitedString();

				Assert.Equal(string.Empty, result);
			}
		}

	}
}