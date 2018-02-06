using System;
using System.Collections.Generic;
using Xunit;

namespace MiscCorLib.Collections.Generic
{
	/// <summary>
	/// Automated NUnit tests of the
	/// <see cref="ConvertStrings"/>
	/// static extension methods.
	/// </summary>
	public sealed class ConvertStringsTests
	{
		private static readonly IEnumerable<string> SampleIntegerStringCollection = new[] { "7", "3", "9", "3", "5" };
		private static readonly IEnumerable<string> PartlyInvalidIntStringCollection = new[] { "8", "whatever", "2", string.Empty, "6", null, "2", "4" };

		public sealed class ToArray
		{
			[Fact]
			public void Preserves_Duplicates_By_Default()
			{
				int[] result = SampleIntegerStringCollection.ToArray<int>();

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
				int[] result = SampleIntegerStringCollection.ToArray<int>(true);

				Assert.Equal(4, result.Length);
				Assert.Equal(7, result[0]);
				Assert.Equal(3, result[1]);
				Assert.Equal(9, result[2]);
				Assert.Equal(5, result[3]);
			}

			[Fact]
			public void Converter_Suppresses_Exceptions_By_Default()
			{
				int[] result = PartlyInvalidIntStringCollection.ToArray<int>(true);

				Assert.Equal(4, result.Length);
				Assert.Equal(8, result[0]);
				Assert.Equal(2, result[1]);
				Assert.Equal(6, result[2]);
				Assert.Equal(4, result[3]);
			}

			[Fact]
			public void Converter_Throws_Exceptions_When_Specified()
			{
				Assert.Throws<FormatException>(() => PartlyInvalidIntStringCollection.ToArray<int>(false, true));
			}

			[Fact]
			public void Allows_Custom_Parser()
			{
				int[] result = SampleIntegerStringCollection.ToArray<int>(int.TryParse, true);

				Assert.Equal(4, result.Length);
				Assert.Equal(7, result[0]);
				Assert.Equal(3, result[1]);
				Assert.Equal(9, result[2]);
				Assert.Equal(5, result[3]);
			}

			[Fact]
			public void Custom_Parser_Ignores_NonParsing_Values()
			{
				int[] result = PartlyInvalidIntStringCollection.ToArray<int>(int.TryParse);

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
				const IEnumerable<string> nullArray = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				int[] result1 = nullArray.ToArray<int>();
				int[] result2 = nullArray.ToArray<int>(int.TryParse);

				Assert.Equal(0, result1.Length);
				Assert.Equal(0, result2.Length);
			}
		}

		public sealed class ToEnumerable
		{
			[Fact]
			public void Preserves_Duplicates_By_Default()
			{
				IReadOnlyList<int> result = SampleIntegerStringCollection.ToEnumerable<int>();

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
				IReadOnlyList<int> result = SampleIntegerStringCollection.ToEnumerable<int>(true);

				Assert.Equal(4, result.Count);
				Assert.Equal(7, result[0]);
				Assert.Equal(3, result[1]);
				Assert.Equal(9, result[2]);
				Assert.Equal(5, result[3]);
			}

			[Fact]
			public void Converter_Suppresses_Exceptions_By_Default()
			{
				IReadOnlyList<int> result = PartlyInvalidIntStringCollection.ToEnumerable<int>(true);

				Assert.Equal(4, result.Count);
				Assert.Equal(8, result[0]);
				Assert.Equal(2, result[1]);
				Assert.Equal(6, result[2]);
				Assert.Equal(4, result[3]);
			}

			[Fact]
			public void Converter_Throws_Exceptions_When_Specified()
			{
				Assert.Throws<FormatException>(() => PartlyInvalidIntStringCollection.ToEnumerable<int>(false, true));
			}

			[Fact]
			public void Allows_Custom_Parser()
			{
				IReadOnlyList<int> result = SampleIntegerStringCollection.ToEnumerable<int>(int.TryParse, true);

				Assert.Equal(4, result.Count);
				Assert.Equal(7, result[0]);
				Assert.Equal(3, result[1]);
				Assert.Equal(9, result[2]);
				Assert.Equal(5, result[3]);
			}

			[Fact]
			public void Custom_Parser_Ignores_NonParsing_Values()
			{
				IReadOnlyList<int> result = PartlyInvalidIntStringCollection.ToEnumerable<int>(int.TryParse);

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
				const IEnumerable<string> nullArray = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				IReadOnlyList<int> result1 = nullArray.ToEnumerable<int>();
				IReadOnlyList<int> result2 = nullArray.ToEnumerable<int>(int.TryParse);

				Assert.Equal(0, result1.Count);
				Assert.Equal(0, result2.Count);
			}
		}

		public sealed class ToList
		{
			[Fact]
			public void Preserves_Duplicates_By_Default()
			{
				IList<int> result = SampleIntegerStringCollection.ToList<int>();

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
				IList<int> result = SampleIntegerStringCollection.ToList<int>(true);

				Assert.Equal(4, result.Count);
				Assert.Equal(7, result[0]);
				Assert.Equal(3, result[1]);
				Assert.Equal(9, result[2]);
				Assert.Equal(5, result[3]);
			}

			[Fact]
			public void Converter_Suppresses_Exceptions_By_Default()
			{
				IList<int> result = PartlyInvalidIntStringCollection.ToList<int>(true);

				Assert.Equal(4, result.Count);
				Assert.Equal(8, result[0]);
				Assert.Equal(2, result[1]);
				Assert.Equal(6, result[2]);
				Assert.Equal(4, result[3]);
			}

			[Fact]
			public void Converter_Throws_Exceptions_When_Specified()
			{
				Assert.Throws<FormatException>(() => PartlyInvalidIntStringCollection.ToList<int>(false, true));
			}

			[Fact]
			public void Allows_Custom_Parser()
			{
				IList<int> result = SampleIntegerStringCollection.ToList<int>(int.TryParse, true);

				Assert.Equal(4, result.Count);
				Assert.Equal(7, result[0]);
				Assert.Equal(3, result[1]);
				Assert.Equal(9, result[2]);
				Assert.Equal(5, result[3]);
			}

			[Fact]
			public void Custom_Parser_Ignores_NonParsing_Values()
			{
				IList<int> result = PartlyInvalidIntStringCollection.ToList<int>(int.TryParse);

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
				const IEnumerable<string> nullArray = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				IList<int> result1 = nullArray.ToList<int>();
				IList<int> result2 = nullArray.ToList<int>(int.TryParse);

				Assert.Equal(0, result1.Count);
				Assert.Equal(0, result2.Count);
			}
		}
	}
}