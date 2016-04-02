#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Collections.Generic
	ConvertStructCollectionTests.cs

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
	using System.Collections.Generic;

	using NUnit.Framework;

	/// <summary>
	/// Automated NUnit tests of the
	/// <see cref="ConvertStructCollection"/>
	/// static extension methods.
	/// </summary>
	[TestFixture]
	public sealed class ConvertStructCollectionTests
	{
		private static readonly IEnumerable<int> SampleIntegerCollection = new[] { 7, 3, 9, 3, 5 };
		private static readonly IEnumerable<char> SampleCharCollection = new[] { 'A', 'a', ' ' };

		[TestFixture]
		public sealed class ToStringArray
		{
			[Test]
			public void Preserves_Duplicates_By_Default()
			{
				string[] result = SampleIntegerCollection.ToStringArray();

				Assert.AreEqual(5, result.Length);
				Assert.AreEqual("7", result[0]);
				Assert.AreEqual("3", result[1]);
				Assert.AreEqual("9", result[2]);
				Assert.AreEqual("3", result[3]);
				Assert.AreEqual("5", result[4]);
			}

			[Test]
			public void Removes_Duplicates_When_Specified()
			{
				string[] result = SampleIntegerCollection.ToStringArray(true);

				Assert.AreEqual(4, result.Length);
				Assert.AreEqual("7", result[0]);
				Assert.AreEqual("3", result[1]);
				Assert.AreEqual("9", result[2]);
				Assert.AreEqual("5", result[3]);
			}

			[Test]
			public void Allows_Custom_Formatter()
			{
				string[] result = SampleIntegerCollection.ToStringArray(item => item.ToString("00"), true);

				Assert.AreEqual(4, result.Length);
				Assert.AreEqual("07", result[0]);
				Assert.AreEqual("03", result[1]);
				Assert.AreEqual("09", result[2]);
				Assert.AreEqual("05", result[3]);
			}

			[Test]
			public void Uses_Ordinal_String_Comparison()
			{
				string[] result = SampleCharCollection.ToStringArray();

				Assert.AreEqual("A", result[0]);
				Assert.AreEqual("a", result[1]);
			}

			[Test]
			public void Omits_Empty_Strings()
			{
				string[] result = SampleCharCollection.ToStringArray();

				Assert.AreEqual(2, result.Length);
			}

			[Test]
			public void Returns_Empty_From_Null_Input()
			{
				IEnumerable<char> nullArray = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				string[] result = nullArray.ToStringArray();

				Assert.AreEqual(0, result.Length);
			}
		}

		[TestFixture]
		public sealed class ToDelimitedString
		{
			[Test]
			public void Preserves_Duplicates_By_Default()
			{
				string result = SampleIntegerCollection.ToDelimitedString();

				Assert.AreEqual("7,3,9,3,5", result);
			}

			[Test]
			public void Removes_Duplicates_When_Specified()
			{
				string result = SampleIntegerCollection.ToDelimitedString(true);

				Assert.AreEqual("7,3,9,5", result);
			}

			[Test]
			public void Allows_Custom_Formatter()
			{
				string result = SampleIntegerCollection.ToDelimitedString(item => item.ToString("00"));

				Assert.AreEqual("07,03,09,03,05", result);
			}

			[Test]
			public void Allows_Custom_Separator()
			{
				string result = SampleIntegerCollection.ToDelimitedString(":", true);

				Assert.AreEqual("7:3:9:5", result);
			}

			[Test]
			public void Uses_Ordinal_String_Comparison()
			{
				string result = SampleCharCollection.ToDelimitedString();

				Assert.AreEqual("A,a", result);
			}

			[Test]
			public void Omits_Empty_Strings()
			{
				string result = SampleCharCollection.ToDelimitedString();

				Assert.AreEqual("A,a", result);
			}

			[Test]
			public void Returns_Empty_From_Null_Input()
			{
				IEnumerable<char> nullArray = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				string result = nullArray.ToDelimitedString();

				Assert.AreEqual(string.Empty, result);
			}
		}

	}
}