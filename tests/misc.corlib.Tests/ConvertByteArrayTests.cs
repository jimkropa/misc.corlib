using System;
using Xunit;

namespace MiscCorLib
{
	/// <summary>
	/// Automated unit tests of the <see cref="ConvertByteArray" /> extention methods.
	/// </summary>
	public sealed class ConvertByteArrayTests
	{
		internal static readonly byte[] KnownConstantByteArray = { 0, 1, 3, 7, 15, 31, 63, 127, 255 };

		public sealed class ToBase64String : ConvertByteArrayTestsBase
		{
			protected override ConvertByteArray.ConvertNonNullArray NonNullConverter => ConvertByteArray.ToBase64String;

			protected override ConvertByteArray.ConvertArray Converter => ConvertByteArray.ToBase64String;

			protected override string KnownConstantString => ConvertEncodedStringTests.KnownConstantBase64String;
		}

		public sealed class ToHexadecimalString : ConvertByteArrayTestsBase
		{
			protected override ConvertByteArray.ConvertNonNullArray NonNullConverter => ConvertByteArray.ToHexadecimalString;

			protected override ConvertByteArray.ConvertArray Converter => ConvertByteArray.ToHexadecimalString;

			protected override string KnownConstantString => ConvertEncodedStringTests.KnownConstantHexadecimalString;
		}

		/// <summary>
		/// This base class contains the implementations
		/// of common tests and assertions. Implementers
		/// override the delegate methods under test.
		/// </summary>
		public abstract class ConvertByteArrayTestsBase
		{
			protected abstract ConvertByteArray.ConvertNonNullArray NonNullConverter { get; }
			protected abstract ConvertByteArray.ConvertArray Converter { get; }
			protected abstract string KnownConstantString { get; }

			[Fact]
			public void Disallows_Null_Array_By_Default()
			{
				// ReSharper disable once AssignNullToNotNullAttribute
				Assert.Throws<ArgumentNullException>(
					() => this.NonNullConverter(null));
			}

			[Fact]
			public void Optionally_Returns_Null_From_Null_Array()
			{
				Assert.Throws<ArgumentNullException>(
					() => this.Converter(null, false));

				Assert.Null(this.Converter(null, true));
			}

			[Fact]
			public void Converts_Known_Input_To_Known_String()
			{
				Assert.Equal(this.KnownConstantString, this.NonNullConverter(KnownConstantByteArray));
				Assert.Equal(this.KnownConstantString, this.Converter(KnownConstantByteArray, true));
				Assert.Equal(this.KnownConstantString, this.Converter(KnownConstantByteArray, false));

				Assert.True(string.Equals(
					this.KnownConstantString,
					this.NonNullConverter(KnownConstantByteArray),
					StringComparison.Ordinal));
				Assert.True(string.Equals(
					this.KnownConstantString,
					this.Converter(KnownConstantByteArray, true),
					StringComparison.Ordinal));
				Assert.True(string.Equals(
					this.KnownConstantString,
					this.Converter(KnownConstantByteArray, false),
					StringComparison.Ordinal));
			}
		}
	}
}