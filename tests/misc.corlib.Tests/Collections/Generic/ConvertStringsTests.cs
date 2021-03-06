﻿#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Collections.Generic
	ConvertStringsTests.cs

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
	/// <see cref="ConvertStrings"/>
	/// static extension methods.
	/// </summary>
	[TestFixture]
	public sealed class ConvertStringsTests
	{
		private static readonly IEnumerable<string> SampleIntegerStringCollection = new[] { "7", "3", "9", "3", "5" };
		private static readonly IEnumerable<string> PartlyInvalidIntStringCollection = new[] { "8", "whatever", "2", string.Empty, "6", null, "2", "4" };

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
				const IEnumerable<string> nullArray = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				int[] result1 = nullArray.ToArray<int>();
				int[] result2 = nullArray.ToArray<int>(int.TryParse);

				Assert.AreEqual(0, result1.Length);
				Assert.AreEqual(0, result2.Length);
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
				const IEnumerable<string> nullArray = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				IReadOnlyList<int> result1 = nullArray.ToEnumerable<int>();
				IReadOnlyList<int> result2 = nullArray.ToEnumerable<int>(int.TryParse);

				Assert.AreEqual(0, result1.Count);
				Assert.AreEqual(0, result2.Count);
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
				const IEnumerable<string> nullArray = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				IList<int> result1 = nullArray.ToList<int>();
				IList<int> result2 = nullArray.ToList<int>(int.TryParse);

				Assert.AreEqual(0, result1.Count);
				Assert.AreEqual(0, result2.Count);
			}
		}
	}
}