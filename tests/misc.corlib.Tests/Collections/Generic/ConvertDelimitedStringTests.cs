﻿#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Collections.Generic
	ConvertDelimitedStringTests.cs

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
				const string nullString = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				int[] result1 = nullString.ToArray<int>();
				int[] result2 = nullString.ToArray<int>(int.TryParse);
				int[] result3 = nullString.ToArray<int>(":");
				int[] result4 = nullString.ToArray<int>(":", int.TryParse);

				Assert.AreEqual(0, result1.Length);
				Assert.AreEqual(0, result2.Length);
				Assert.AreEqual(0, result3.Length);
				Assert.AreEqual(0, result4.Length);
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
				const string nullString = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				IReadOnlyList<int> result1 = nullString.ToEnumerable<int>();
				IReadOnlyList<int> result2 = nullString.ToEnumerable<int>(int.TryParse);
				IReadOnlyList<int> result3 = nullString.ToEnumerable<int>(":");
				IReadOnlyList<int> result4 = nullString.ToEnumerable<int>(":", int.TryParse);

				Assert.AreEqual(0, result1.Count);
				Assert.AreEqual(0, result2.Count);
				Assert.AreEqual(0, result3.Count);
				Assert.AreEqual(0, result4.Count);
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
				const string nullString = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				IList<int> result1 = nullString.ToList<int>();
				IList<int> result2 = nullString.ToList<int>(int.TryParse);
				IList<int> result3 = nullString.ToList<int>(":");
				IList<int> result4 = nullString.ToList<int>(":", int.TryParse);

				Assert.AreEqual(0, result1.Count);
				Assert.AreEqual(0, result2.Count);
				Assert.AreEqual(0, result3.Count);
				Assert.AreEqual(0, result4.Count);
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