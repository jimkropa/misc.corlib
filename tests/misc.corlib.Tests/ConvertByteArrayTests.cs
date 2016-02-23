namespace MiscCorLib
{
	using System;

	using NUnit.Framework;

	/// <summary>
	/// Automated unit tests of the <see cref="ConvertByteArray"/> extention methods.
	/// </summary>
	[TestFixture]
	public sealed class ConvertByteArrayTests
	{
		[TestFixture]
		public sealed class ToBase64String : ConvertByteArrayTestsBase
		{
			protected override ConvertByteArray.ConvertNonNullArray NonNullConverter
			{
				get { return ConvertByteArray.ToBase64String; }
			}

			protected override ConvertByteArray.ConvertArray Converter
			{
				get { return ConvertByteArray.ToBase64String; }
			}


		}

		[TestFixture]
		public sealed class ToHexadecimalString : ConvertByteArrayTestsBase
		{
			protected override ConvertByteArray.ConvertNonNullArray NonNullConverter
			{
				get { return ConvertByteArray.ToHexadecimalString; }
			}

			protected override ConvertByteArray.ConvertArray Converter
			{
				get { return ConvertByteArray.ToHexadecimalString; }
			}
		}

		public abstract class ConvertByteArrayTestsBase
		{
			protected abstract ConvertByteArray.ConvertNonNullArray NonNullConverter { get; }
			protected abstract ConvertByteArray.ConvertArray Converter { get; }

			[Test]
			public void Disallows_Null_Array_By_Default()
			{
				// ReSharper disable once AssignNullToNotNullAttribute
				Assert.Throws<ArgumentNullException>(
					() => ConvertByteArray.ToBase64String(null));
			}

			[Test]
			public void Optionally_Returns_Null_From_Null_Array()
			{
				// ReSharper disable once AssignNullToNotNullAttribute
				Assert.Throws<ArgumentNullException>(
					() => ConvertByteArray.ToBase64String(null, false));

				Assert.IsNull(ConvertByteArray.ToBase64String(null, true));
			}
		}

		//internal static Disallows_Null_Array_By_Default()
	}
}