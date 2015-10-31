namespace MiscCorLib.Collections
{
	using NUnit.Framework;

	[TestFixture]
	public sealed class PageNumberAndSizeTests
	{

		#region [ Internal Static Test Assertion Methods ]

		internal static void AssertIsEmpty(PageNumberAndSize page)
		{
			Assert.IsFalse(page.IsValid);
			Assert.IsFalse(page.IsUnbounded);
			Assert.AreEqual(byte.MinValue, page.Size);
			Assert.AreEqual(0, page.Number);
			Assert.AreEqual(-1, page.Index);
		}

		internal static void AssertIsFirstPage(PageNumberAndSize page)
		{
			Assert.IsTrue(page.IsValid);
			Assert.GreaterOrEqual(page.Size, byte.MinValue);
			Assert.AreEqual(PageNumberAndSize.FirstPageNumber, page.Number);
			Assert.AreEqual(0, page.Index);
		}

		internal static void AssertIsUnbounded(PageNumberAndSize page)
		{
			Assert.IsTrue(page.IsUnbounded);
			Assert.AreEqual(byte.MinValue, page.Size);

			AssertIsFirstPage(page);
		}

		#endregion
	}
}