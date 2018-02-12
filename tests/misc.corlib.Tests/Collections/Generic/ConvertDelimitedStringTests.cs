using System;
using System.Collections.Generic;
using Xunit;

namespace MiscCorLib.Collections.Generic
{
	/// <summary>
	/// Automated NUnit tests of the
	/// <see cref="ConvertDelimitedString" />
	/// static extension methods.
	/// </summary>
	public sealed class ConvertDelimitedStringTests
	{
		private const string SampleIntegersString = "7,3, 9, 3,  5";
		private const string PartlyInvalidIntsString = "8, whatever,2,,6,   , 2,4";

		public sealed class ToArray
		{
			[Fact]
			public void Preserves_Duplicates_By_Default()
			{
				int[] result = SampleIntegersString.ToArray<int>();

				Assert.Equal(5, result.Length);
				Assert.Equal(7, result[0]);
				Assert.Equal(3, result[1]);
				Assert.Equal(9, result[2]);
				Assert.Equal(3, result[3]);
				Assert.Equal(5, result[4]);
			}

			[Fact]
			public void Removes_Duplicates_When_Specified()
			{
				int[] result = SampleIntegersString.ToArray<int>(true);

				Assert.Equal(4, result.Length);
				Assert.Equal(7, result[0]);
				Assert.Equal(3, result[1]);
				Assert.Equal(9, result[2]);
				Assert.Equal(5, result[3]);
			}

			[Fact]
			public void Converter_Suppresses_Exceptions_By_Default()
			{
				int[] result = PartlyInvalidIntsString.ToArray<int>(true);

				Assert.Equal(4, result.Length);
				Assert.Equal(8, result[0]);
				Assert.Equal(2, result[1]);
				Assert.Equal(6, result[2]);
				Assert.Equal(4, result[3]);
			}

			[Fact]
			public void Converter_Throws_Exceptions_When_Specified()
			{
				Assert.Throws<FormatException>(() => PartlyInvalidIntsString.ToArray<int>(false, true));
				Assert.Throws<FormatException>(() => "1:7:0:1:A".ToArray<int>(":", false, true));
			}

			[Fact]
			public void Allows_Custom_Parser()
			{
				int[] result = SampleIntegersString.ToArray<int>(int.TryParse, true);

				Assert.Equal(4, result.Length);
				Assert.Equal(7, result[0]);
				Assert.Equal(3, result[1]);
				Assert.Equal(9, result[2]);
				Assert.Equal(5, result[3]);
			}

			[Fact]
			public void Custom_Parser_Ignores_NonParsing_Values()
			{
				int[] result = PartlyInvalidIntsString.ToArray<int>(int.TryParse);

				Assert.Equal(5, result.Length);
				Assert.Equal(8, result[0]);
				Assert.Equal(2, result[1]);
				Assert.Equal(6, result[2]);
				Assert.Equal(2, result[3]);
				Assert.Equal(4, result[4]);
			}

			[Fact]
			public void Returns_Empty_From_Null_Input()
			{
				const string nullString = null;

				int[] result1 = nullString.ToArray<int>();
				int[] result2 = nullString.ToArray<int>(int.TryParse);
				int[] result3 = nullString.ToArray<int>(":");
				int[] result4 = nullString.ToArray<int>(":", int.TryParse);

				Assert.Equal(0, result1.Length);
				Assert.Equal(0, result2.Length);
				Assert.Equal(0, result3.Length);
				Assert.Equal(0, result4.Length);
			}

			[Fact]
			public void TryParse_Allows_Custom_Separator()
			{
				int[] result = "1:7:0:1:A".ToArray<int>(":", int.TryParse, true);

				Assert.Equal(3, result.Length);
				Assert.Equal(1, result[0]);
				Assert.Equal(7, result[1]);
				Assert.Equal(0, result[2]);
			}

			[Fact]
			public void Convert_Allows_Custom_Separator()
			{
				int[] result = "1:7:0:1:A".ToArray<int>(":", true);

				Assert.Equal(3, result.Length);
				Assert.Equal(1, result[0]);
				Assert.Equal(7, result[1]);
				Assert.Equal(0, result[2]);
			}
		}

		public sealed class ToEnumerable
		{
			[Fact]
			public void Preserves_Duplicates_By_Default()
			{
				IReadOnlyList<int> result = SampleIntegersString.ToEnumerable<int>();

				Assert.Equal(5, result.Count);
				Assert.Equal(7, result[0]);
				Assert.Equal(3, result[1]);
				Assert.Equal(9, result[2]);
				Assert.Equal(3, result[3]);
				Assert.Equal(5, result[4]);
			}

			[Fact]
			public void Removes_Duplicates_When_Specified()
			{
				IReadOnlyList<int> result = SampleIntegersString.ToEnumerable<int>(true);

				Assert.Equal(4, result.Count);
				Assert.Equal(7, result[0]);
				Assert.Equal(3, result[1]);
				Assert.Equal(9, result[2]);
				Assert.Equal(5, result[3]);
			}

			[Fact]
			public void Converter_Suppresses_Exceptions_By_Default()
			{
				IReadOnlyList<int> result = PartlyInvalidIntsString.ToEnumerable<int>(true);

				Assert.Equal(4, result.Count);
				Assert.Equal(8, result[0]);
				Assert.Equal(2, result[1]);
				Assert.Equal(6, result[2]);
				Assert.Equal(4, result[3]);
			}

			[Fact]
			public void Converter_Throws_Exceptions_When_Specified()
			{
				Assert.Throws<FormatException>(() => PartlyInvalidIntsString.ToEnumerable<int>(false, true));
				Assert.Throws<FormatException>(() => "1:7:0:1:A".ToEnumerable<int>(":", false, true));
			}

			[Fact]
			public void Allows_Custom_Parser()
			{
				IReadOnlyList<int> result = SampleIntegersString.ToEnumerable<int>(int.TryParse, true);

				Assert.Equal(4, result.Count);
				Assert.Equal(7, result[0]);
				Assert.Equal(3, result[1]);
				Assert.Equal(9, result[2]);
				Assert.Equal(5, result[3]);
			}

			[Fact]
			public void Custom_Parser_Ignores_NonParsing_Values()
			{
				IReadOnlyList<int> result = PartlyInvalidIntsString.ToEnumerable<int>(int.TryParse);

				Assert.Equal(5, result.Count);
				Assert.Equal(8, result[0]);
				Assert.Equal(2, result[1]);
				Assert.Equal(6, result[2]);
				Assert.Equal(2, result[3]);
				Assert.Equal(4, result[4]);
			}

			[Fact]
			public void Returns_Empty_From_Null_Input()
			{
				const string nullString = null;

				IReadOnlyList<int> result1 = nullString.ToEnumerable<int>();
				IReadOnlyList<int> result2 = nullString.ToEnumerable<int>(int.TryParse);
				IReadOnlyList<int> result3 = nullString.ToEnumerable<int>(":");
				IReadOnlyList<int> result4 = nullString.ToEnumerable<int>(":", int.TryParse);

				Assert.Equal(0, result1.Count);
				Assert.Equal(0, result2.Count);
				Assert.Equal(0, result3.Count);
				Assert.Equal(0, result4.Count);
			}

			[Fact]
			public void TryParse_Allows_Custom_Separator()
			{
				IReadOnlyList<int> result = "1:7:0:1:A".ToEnumerable<int>(":", int.TryParse, true);

				Assert.Equal(3, result.Count);
				Assert.Equal(1, result[0]);
				Assert.Equal(7, result[1]);
				Assert.Equal(0, result[2]);
			}

			[Fact]
			public void Convert_Allows_Custom_Separator()
			{
				IReadOnlyList<int> result = "1:7:0:1:A".ToEnumerable<int>(":", true);

				Assert.Equal(3, result.Count);
				Assert.Equal(1, result[0]);
				Assert.Equal(7, result[1]);
				Assert.Equal(0, result[2]);
			}
		}

		public sealed class ToList
		{
			[Fact]
			public void Preserves_Duplicates_By_Default()
			{
				IList<int> result = SampleIntegersString.ToList<int>();

				Assert.Equal(5, result.Count);
				Assert.Equal(7, result[0]);
				Assert.Equal(3, result[1]);
				Assert.Equal(9, result[2]);
				Assert.Equal(3, result[3]);
				Assert.Equal(5, result[4]);
			}

			[Fact]
			public void Removes_Duplicates_When_Specified()
			{
				IList<int> result = SampleIntegersString.ToList<int>(true);

				Assert.Equal(4, result.Count);
				Assert.Equal(7, result[0]);
				Assert.Equal(3, result[1]);
				Assert.Equal(9, result[2]);
				Assert.Equal(5, result[3]);
			}

			[Fact]
			public void Converter_Suppresses_Exceptions_By_Default()
			{
				IList<int> result = PartlyInvalidIntsString.ToList<int>(true);

				Assert.Equal(4, result.Count);
				Assert.Equal(8, result[0]);
				Assert.Equal(2, result[1]);
				Assert.Equal(6, result[2]);
				Assert.Equal(4, result[3]);
			}

			[Fact]
			public void Converter_Throws_Exceptions_When_Specified()
			{
				Assert.Throws<FormatException>(() => PartlyInvalidIntsString.ToList<int>(false, true));
				Assert.Throws<FormatException>(() => "1:7:0:1:A".ToList<int>(":", false, true));
			}

			[Fact]
			public void Allows_Custom_Parser()
			{
				IList<int> result = SampleIntegersString.ToList<int>(int.TryParse, true);

				Assert.Equal(4, result.Count);
				Assert.Equal(7, result[0]);
				Assert.Equal(3, result[1]);
				Assert.Equal(9, result[2]);
				Assert.Equal(5, result[3]);
			}

			[Fact]
			public void Custom_Parser_Ignores_NonParsing_Values()
			{
				IList<int> result = PartlyInvalidIntsString.ToList<int>(int.TryParse);

				Assert.Equal(5, result.Count);
				Assert.Equal(8, result[0]);
				Assert.Equal(2, result[1]);
				Assert.Equal(6, result[2]);
				Assert.Equal(2, result[3]);
				Assert.Equal(4, result[4]);
			}

			[Fact]
			public void Returns_Empty_From_Null_Input()
			{
				const string nullString = null;

				IList<int> result1 = nullString.ToList<int>();
				IList<int> result2 = nullString.ToList<int>(int.TryParse);
				IList<int> result3 = nullString.ToList<int>(":");
				IList<int> result4 = nullString.ToList<int>(":", int.TryParse);

				Assert.Equal(0, result1.Count);
				Assert.Equal(0, result2.Count);
				Assert.Equal(0, result3.Count);
				Assert.Equal(0, result4.Count);
			}

			[Fact]
			public void TryParse_Allows_Custom_Separator()
			{
				IList<int> result = "1:7:0:1:A".ToList<int>(":", int.TryParse, true);

				Assert.Equal(3, result.Count);
				Assert.Equal(1, result[0]);
				Assert.Equal(7, result[1]);
				Assert.Equal(0, result[2]);
			}

			[Fact]
			public void Convert_Allows_Custom_Separator()
			{
				IList<int> result = "1:7:0:1:A".ToList<int>(":", true);

				Assert.Equal(3, result.Count);
				Assert.Equal(1, result[0]);
				Assert.Equal(7, result[1]);
				Assert.Equal(0, result[2]);
			}
		}
	}
}