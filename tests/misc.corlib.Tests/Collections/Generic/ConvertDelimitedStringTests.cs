namespace MiscCorLib.Collections.Generic
{
	using System;
	using System.Collections.Generic;

	using NUnit.Framework;

	/// <summary>
	/// Automated NUnit tests of the
	/// <see cref="ConvertDelimitedString"/>
	/// static extension methods.
	/// </summary>
	[TestFixture]
	public sealed class ConvertDelimitedStringTests
	{
		private const string SampleIntegersString = "7,3, 9, 3,  5";
		private const string PartlyInvalidIntsString = "8, whatever,2,,6,   , 2,4";

		[TestFixture]
		public sealed class ToArray
		{
			[Test]
			public void Preserves_Duplicates_By_Default()
			{
				int[] result = SampleIntegersString.ToArray<int>();

				Assert.AreEqual(5, result.Length);
				Assert.AreEqual(7, result[0]);
				Assert.AreEqual(3, result[1]);
				Assert.AreEqual(9, result[2]);
				Assert.AreEqual(3, result[3]);
				Assert.AreEqual(5, result[4]);
			}

			[Test]
			public void Removes_Duplicates_When_Specified()
			{
				int[] result = SampleIntegersString.ToArray<int>(true);

				Assert.AreEqual(4, result.Length);
				Assert.AreEqual(7, result[0]);
				Assert.AreEqual(3, result[1]);
				Assert.AreEqual(9, result[2]);
				Assert.AreEqual(5, result[3]);
			}

			[Test]
			public void Converter_Suppresses_Exceptions_By_Default()
			{
				int[] result = PartlyInvalidIntsString.ToArray<int>(true);

				Assert.AreEqual(4, result.Length);
				Assert.AreEqual(8, result[0]);
				Assert.AreEqual(2, result[1]);
				Assert.AreEqual(6, result[2]);
				Assert.AreEqual(4, result[3]);
			}

			[Test]
			public void Converter_Throws_Exceptions_When_Specified()
			{
				Assert.Throws<FormatException>(() => PartlyInvalidIntsString.ToArray<int>(false, true));
				Assert.Throws<FormatException>(() => "1:7:0:1:A".ToArray<int>(":", false, true));
			}

			[Test]
			public void Allows_Custom_Parser()
			{
				int[] result = SampleIntegersString.ToArray<int>(int.TryParse, true);

				Assert.AreEqual(4, result.Length);
				Assert.AreEqual(7, result[0]);
				Assert.AreEqual(3, result[1]);
				Assert.AreEqual(9, result[2]);
				Assert.AreEqual(5, result[3]);
			}

			[Test]
			public void Custom_Parser_Ignores_NonParsing_Values()
			{
				int[] result = PartlyInvalidIntsString.ToArray<int>(int.TryParse);

				Assert.AreEqual(5, result.Length);
				Assert.AreEqual(8, result[0]);
				Assert.AreEqual(2, result[1]);
				Assert.AreEqual(6, result[2]);
				Assert.AreEqual(2, result[3]);
				Assert.AreEqual(4, result[4]);
			}

			[Test]
			public void Returns_Empty_From_Null_Input()
			{
				IEnumerable<string> nullArray = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				int[] result = nullArray.ToArray<int>();

				Assert.AreEqual(0, result.Length);
			}

			[Test]
			public void TryParse_Allows_Custom_Separator()
			{
				int[] result = "1:7:0:1:A".ToArray<int>(":", int.TryParse, true);

				Assert.AreEqual(3, result.Length);
				Assert.AreEqual(1, result[0]);
				Assert.AreEqual(7, result[1]);
				Assert.AreEqual(0, result[2]);
			}

			[Test]
			public void Convert_Allows_Custom_Separator()
			{
				int[] result = "1:7:0:1:A".ToArray<int>(":", true);

				Assert.AreEqual(3, result.Length);
				Assert.AreEqual(1, result[0]);
				Assert.AreEqual(7, result[1]);
				Assert.AreEqual(0, result[2]);
			}
		}

		[TestFixture]
		public sealed class ToEnumerable
		{
			[Test]
			public void Preserves_Duplicates_By_Default()
			{
				IReadOnlyList<int> result = SampleIntegersString.ToEnumerable<int>();

				Assert.AreEqual(5, result.Count);
				Assert.AreEqual(7, result[0]);
				Assert.AreEqual(3, result[1]);
				Assert.AreEqual(9, result[2]);
				Assert.AreEqual(3, result[3]);
				Assert.AreEqual(5, result[4]);
			}

			[Test]
			public void Removes_Duplicates_When_Specified()
			{
				IReadOnlyList<int> result = SampleIntegersString.ToEnumerable<int>(true);

				Assert.AreEqual(4, result.Count);
				Assert.AreEqual(7, result[0]);
				Assert.AreEqual(3, result[1]);
				Assert.AreEqual(9, result[2]);
				Assert.AreEqual(5, result[3]);
			}

			[Test]
			public void Converter_Suppresses_Exceptions_By_Default()
			{
				IReadOnlyList<int> result = PartlyInvalidIntsString.ToEnumerable<int>(true);

				Assert.AreEqual(4, result.Count);
				Assert.AreEqual(8, result[0]);
				Assert.AreEqual(2, result[1]);
				Assert.AreEqual(6, result[2]);
				Assert.AreEqual(4, result[3]);
			}

			[Test]
			public void Converter_Throws_Exceptions_When_Specified()
			{
				Assert.Throws<FormatException>(() => PartlyInvalidIntsString.ToEnumerable<int>(false, true));
				Assert.Throws<FormatException>(() => "1:7:0:1:A".ToEnumerable<int>(":", false, true));
			}

			[Test]
			public void Allows_Custom_Parser()
			{
				IReadOnlyList<int> result = SampleIntegersString.ToEnumerable<int>(int.TryParse, true);

				Assert.AreEqual(4, result.Count);
				Assert.AreEqual(7, result[0]);
				Assert.AreEqual(3, result[1]);
				Assert.AreEqual(9, result[2]);
				Assert.AreEqual(5, result[3]);
			}

			[Test]
			public void Custom_Parser_Ignores_NonParsing_Values()
			{
				IReadOnlyList<int> result = PartlyInvalidIntsString.ToEnumerable<int>(int.TryParse);

				Assert.AreEqual(5, result.Count);
				Assert.AreEqual(8, result[0]);
				Assert.AreEqual(2, result[1]);
				Assert.AreEqual(6, result[2]);
				Assert.AreEqual(2, result[3]);
				Assert.AreEqual(4, result[4]);
			}

			[Test]
			public void Returns_Empty_From_Null_Input()
			{
				IEnumerable<string> nullArray = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				IReadOnlyList<int> result = nullArray.ToEnumerable<int>();

				Assert.AreEqual(0, result.Count);
			}

			[Test]
			public void TryParse_Allows_Custom_Separator()
			{
				IReadOnlyList<int> result = "1:7:0:1:A".ToEnumerable<int>(":", int.TryParse, true);

				Assert.AreEqual(3, result.Count);
				Assert.AreEqual(1, result[0]);
				Assert.AreEqual(7, result[1]);
				Assert.AreEqual(0, result[2]);
			}

			[Test]
			public void Convert_Allows_Custom_Separator()
			{
				IReadOnlyList<int> result = "1:7:0:1:A".ToEnumerable<int>(":", true);

				Assert.AreEqual(3, result.Count);
				Assert.AreEqual(1, result[0]);
				Assert.AreEqual(7, result[1]);
				Assert.AreEqual(0, result[2]);
			}
		}

		[TestFixture]
		public sealed class ToList
		{
			[Test]
			public void Preserves_Duplicates_By_Default()
			{
				IList<int> result = SampleIntegersString.ToList<int>();

				Assert.AreEqual(5, result.Count);
				Assert.AreEqual(7, result[0]);
				Assert.AreEqual(3, result[1]);
				Assert.AreEqual(9, result[2]);
				Assert.AreEqual(3, result[3]);
				Assert.AreEqual(5, result[4]);
			}

			[Test]
			public void Removes_Duplicates_When_Specified()
			{
				IList<int> result = SampleIntegersString.ToList<int>(true);

				Assert.AreEqual(4, result.Count);
				Assert.AreEqual(7, result[0]);
				Assert.AreEqual(3, result[1]);
				Assert.AreEqual(9, result[2]);
				Assert.AreEqual(5, result[3]);
			}

			[Test]
			public void Converter_Suppresses_Exceptions_By_Default()
			{
				IList<int> result = PartlyInvalidIntsString.ToList<int>(true);

				Assert.AreEqual(4, result.Count);
				Assert.AreEqual(8, result[0]);
				Assert.AreEqual(2, result[1]);
				Assert.AreEqual(6, result[2]);
				Assert.AreEqual(4, result[3]);
			}

			[Test]
			public void Converter_Throws_Exceptions_When_Specified()
			{
				Assert.Throws<FormatException>(() => PartlyInvalidIntsString.ToList<int>(false, true));
				Assert.Throws<FormatException>(() => "1:7:0:1:A".ToList<int>(":", false, true));
			}

			[Test]
			public void Allows_Custom_Parser()
			{
				IList<int> result = SampleIntegersString.ToList<int>(int.TryParse, true);

				Assert.AreEqual(4, result.Count);
				Assert.AreEqual(7, result[0]);
				Assert.AreEqual(3, result[1]);
				Assert.AreEqual(9, result[2]);
				Assert.AreEqual(5, result[3]);
			}

			[Test]
			public void Custom_Parser_Ignores_NonParsing_Values()
			{
				IList<int> result = PartlyInvalidIntsString.ToList<int>(int.TryParse);

				Assert.AreEqual(5, result.Count);
				Assert.AreEqual(8, result[0]);
				Assert.AreEqual(2, result[1]);
				Assert.AreEqual(6, result[2]);
				Assert.AreEqual(2, result[3]);
				Assert.AreEqual(4, result[4]);
			}

			[Test]
			public void Returns_Empty_From_Null_Input()
			{
				IEnumerable<string> nullArray = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				IList<int> result = nullArray.ToList<int>();

				Assert.AreEqual(0, result.Count);
			}

			[Test]
			public void TryParse_Allows_Custom_Separator()
			{
				IList<int> result = "1:7:0:1:A".ToList<int>(":", int.TryParse, true);

				Assert.AreEqual(3, result.Count);
				Assert.AreEqual(1, result[0]);
				Assert.AreEqual(7, result[1]);
				Assert.AreEqual(0, result[2]);
			}

			[Test]
			public void Convert_Allows_Custom_Separator()
			{
				IList<int> result = "1:7:0:1:A".ToList<int>(":", true);

				Assert.AreEqual(3, result.Count);
				Assert.AreEqual(1, result[0]);
				Assert.AreEqual(7, result[1]);
				Assert.AreEqual(0, result[2]);
			}
		}
	}
}