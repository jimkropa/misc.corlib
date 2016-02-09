namespace MiscCorLib.Collections.Generic
{
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
		private static readonly IEnumerable<string> SampleCharStringCollection = new[] { "A", "a", " " };

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
			public void Allows_Custom_Converter()
			{
				IList<int> result = SampleIntegerStringCollection.ToList<int>(true);

				Assert.AreEqual(4, result.Count);
				Assert.AreEqual("07", result[0]);
				Assert.AreEqual("03", result[1]);
				Assert.AreEqual("09", result[2]);
				Assert.AreEqual("05", result[3]);
			}

			[Test]
			public void Allows_Custom_Parser()
			{
				IList<int> result = SampleIntegerStringCollection.ToList<int>(int.TryParse);

				Assert.AreEqual(4, result.Count);
				Assert.AreEqual("07", result[0]);
				Assert.AreEqual("03", result[1]);
				Assert.AreEqual("09", result[2]);
				Assert.AreEqual("05", result[3]);
			}

			[Test]
			public void Uses_Ordinal_String_Comparison()
			{
			}

			[Test]
			public void Omits_Empty_Strings()
			{
			}
		}
	}
}