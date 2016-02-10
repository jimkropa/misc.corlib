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
				int[] result = SampleIntegerStringCollection.ToArray<int>();

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
				int[] result = SampleIntegerStringCollection.ToArray<int>(true);

				Assert.AreEqual(4, result.Length);
				Assert.AreEqual(7, result[0]);
				Assert.AreEqual(3, result[1]);
				Assert.AreEqual(9, result[2]);
				Assert.AreEqual(5, result[3]);
			}

			[Test]
			public void Converter_Suppresses_Exceptions_By_Default()
			{
				int[] result = PartlyInvalidIntStringCollection.ToArray<int>(true);

				Assert.AreEqual(4, result.Length);
				Assert.AreEqual(8, result[0]);
				Assert.AreEqual(2, result[1]);
				Assert.AreEqual(6, result[2]);
				Assert.AreEqual(4, result[3]);
			}

			[Test]
			public void Converter_Throws_Exceptions_When_Specified()
			{
				Assert.Throws<FormatException>(() => PartlyInvalidIntStringCollection.ToArray<int>(false, true));
			}

			[Test]
			public void Allows_Custom_Parser()
			{
				int[] result = SampleIntegerStringCollection.ToArray<int>(int.TryParse, true);

				Assert.AreEqual(4, result.Length);
				Assert.AreEqual(7, result[0]);
				Assert.AreEqual(3, result[1]);
				Assert.AreEqual(9, result[2]);
				Assert.AreEqual(5, result[3]);
			}

			[Test]
			public void Custom_Parser_Ignores_NonParsing_Values()
			{
				int[] result = PartlyInvalidIntStringCollection.ToArray<int>(int.TryParse);

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
		}

		[TestFixture]
		public sealed class ToEnumerable
		{
			[Test]
			public void Preserves_Duplicates_By_Default()
			{
				IReadOnlyList<int> result = SampleIntegerStringCollection.ToEnumerable<int>();

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
				IReadOnlyList<int> result = SampleIntegerStringCollection.ToEnumerable<int>(true);

				Assert.AreEqual(4, result.Count);
				Assert.AreEqual(7, result[0]);
				Assert.AreEqual(3, result[1]);
				Assert.AreEqual(9, result[2]);
				Assert.AreEqual(5, result[3]);
			}

			[Test]
			public void Converter_Suppresses_Exceptions_By_Default()
			{
				IReadOnlyList<int> result = PartlyInvalidIntStringCollection.ToEnumerable<int>(true);

				Assert.AreEqual(4, result.Count);
				Assert.AreEqual(8, result[0]);
				Assert.AreEqual(2, result[1]);
				Assert.AreEqual(6, result[2]);
				Assert.AreEqual(4, result[3]);
			}

			[Test]
			public void Converter_Throws_Exceptions_When_Specified()
			{
				Assert.Throws<FormatException>(() => PartlyInvalidIntStringCollection.ToEnumerable<int>(false, true));
			}

			[Test]
			public void Allows_Custom_Parser()
			{
				IReadOnlyList<int> result = SampleIntegerStringCollection.ToEnumerable<int>(int.TryParse, true);

				Assert.AreEqual(4, result.Count);
				Assert.AreEqual(7, result[0]);
				Assert.AreEqual(3, result[1]);
				Assert.AreEqual(9, result[2]);
				Assert.AreEqual(5, result[3]);
			}

			[Test]
			public void Custom_Parser_Ignores_NonParsing_Values()
			{
				IReadOnlyList<int> result = PartlyInvalidIntStringCollection.ToEnumerable<int>(int.TryParse);

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
		}

		[TestFixture]
		public sealed class ToList
		{
			[Test]
			public void Preserves_Duplicates_By_Default()
			{
				IList<int> result = SampleIntegerStringCollection.ToList<int>();

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
				IList<int> result = SampleIntegerStringCollection.ToList<int>(true);

				Assert.AreEqual(4, result.Count);
				Assert.AreEqual(7, result[0]);
				Assert.AreEqual(3, result[1]);
				Assert.AreEqual(9, result[2]);
				Assert.AreEqual(5, result[3]);
			}

			[Test]
			public void Converter_Suppresses_Exceptions_By_Default()
			{
				IList<int> result = PartlyInvalidIntStringCollection.ToList<int>(true);

				Assert.AreEqual(4, result.Count);
				Assert.AreEqual(8, result[0]);
				Assert.AreEqual(2, result[1]);
				Assert.AreEqual(6, result[2]);
				Assert.AreEqual(4, result[3]);
			}

			[Test]
			public void Converter_Throws_Exceptions_When_Specified()
			{
				Assert.Throws<FormatException>(() => PartlyInvalidIntStringCollection.ToList<int>(false, true));
			}

			[Test]
			public void Allows_Custom_Parser()
			{
				IList<int> result = SampleIntegerStringCollection.ToList<int>(int.TryParse, true);

				Assert.AreEqual(4, result.Count);
				Assert.AreEqual(7, result[0]);
				Assert.AreEqual(3, result[1]);
				Assert.AreEqual(9, result[2]);
				Assert.AreEqual(5, result[3]);
			}

			[Test]
			public void Custom_Parser_Ignores_NonParsing_Values()
			{
				IList<int> result = PartlyInvalidIntStringCollection.ToList<int>(int.TryParse);

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
		}
	}
}