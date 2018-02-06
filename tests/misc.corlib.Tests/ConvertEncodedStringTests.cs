using System;
using Xunit;

namespace MiscCorLib
{
	/// <summary>
	/// Automated unit tests of the <see cref="ConvertEncodedString"/> extention methods.
	/// </summary>
	public sealed class ConvertEncodedStringTests
	{
		internal const string KnownConstantBase64String = @"AAEDBw8fP3//";
		internal const string KnownConstantHexadecimalString = @"000103070f1f3f7fff";

		public sealed class FromBase64String : ConvertEncodedStringTestsBase
		{
			protected override ConvertEncodedString.ConvertNonNullString NonNullConverter => ConvertEncodedString.FromBase64;

			protected override ConvertEncodedString.ConvertString Converter => ConvertEncodedString.FromBase64;

			protected override string KnownConstantString => KnownConstantBase64String;
		}

		public sealed class ToHexadecimalString : ConvertEncodedStringTestsBase
		{
			protected override ConvertEncodedString.ConvertNonNullString NonNullConverter => ConvertEncodedString.FromHexadecimal;

			protected override ConvertEncodedString.ConvertString Converter => ConvertEncodedString.FromHexadecimal;

			protected override string KnownConstantString => KnownConstantHexadecimalString;
		}

		public sealed class ToByteArray
		{
			[Fact]
			public void Disallows_Null_Array_By_Default()
			{
				// ReSharper disable once AssignNullToNotNullAttribute
				Assert.Throws<ArgumentNullException>(
					() => ConvertEncodedString.ToByteArray(null));

				// ReSharper disable once AssignNullToNotNullAttribute
				Assert.Throws<ArgumentNullException>(
					() => ConvertEncodedString.ToByteArray(null, ByteArrayStringEncoding.Hexadecimal));
			}

			[Fact]
			public void Optionally_Returns_Null_From_Null_Array()
			{
				Assert.Throws<ArgumentNullException>(
					() => ConvertEncodedString.ToByteArray(null, false));

				Assert.Null(ConvertEncodedString.ToByteArray(null, true));

				Assert.Throws<ArgumentNullException>(
					() => ConvertEncodedString.ToByteArray(null, false, ByteArrayStringEncoding.Hexadecimal));

				Assert.Null(ConvertEncodedString.ToByteArray(null, true, ByteArrayStringEncoding.Hexadecimal));
			}

			[Fact]
			public void Ignores_Invalid_Encoding_With_Null_Input()
			{
				const ByteArrayStringEncoding invalidEncoding = (ByteArrayStringEncoding)17;

				// ReSharper disable once AssignNullToNotNullAttribute
				Assert.Throws<ArgumentNullException>(
					() => ConvertEncodedString.ToByteArray(null, invalidEncoding));

				Assert.Throws<ArgumentNullException>(
					() => ConvertEncodedString.ToByteArray(null, false, invalidEncoding));

				Assert.Null(ConvertEncodedString.ToByteArray(null, true, invalidEncoding));
			}

			[Fact]
			public void Rejects_Invalid_Encoding_With_Valid_Input()
			{
				const ByteArrayStringEncoding invalidEncoding = (ByteArrayStringEncoding)17;
				const string validInputString = KnownConstantHexadecimalString;

				Assert.Throws<ArgumentOutOfRangeException>(
					() => validInputString.ToByteArray(invalidEncoding));

				Assert.Throws<ArgumentOutOfRangeException>(
					() => validInputString.ToByteArray(true, invalidEncoding));

				Assert.Throws<ArgumentOutOfRangeException>(
					() => validInputString.ToByteArray(false, invalidEncoding));
			}

			[Fact]
			public void Uses_Correct_Encoding()
			{
				Assert.Equal(
					ConvertByteArrayTests.KnownConstantByteArray,
					KnownConstantBase64String.ToByteArray());

				Assert.Equal(
					ConvertByteArrayTests.KnownConstantByteArray,
					KnownConstantBase64String.ToByteArray(true));

				Assert.Equal(
					ConvertByteArrayTests.KnownConstantByteArray,
					KnownConstantBase64String.ToByteArray(false));

				Assert.Equal(
					ConvertByteArrayTests.KnownConstantByteArray,
					KnownConstantHexadecimalString.ToByteArray(ByteArrayStringEncoding.Hexadecimal));

				Assert.Equal(
					ConvertByteArrayTests.KnownConstantByteArray,
					KnownConstantHexadecimalString.ToByteArray(true, ByteArrayStringEncoding.Hexadecimal));

				Assert.Equal(
					ConvertByteArrayTests.KnownConstantByteArray,
					KnownConstantHexadecimalString.ToByteArray(false, ByteArrayStringEncoding.Hexadecimal));
			}
		}

		/// <summary>
		/// This base class contains the implementations
		/// of common tests and assertions. Implementers
		/// override the delegate methods under test.
		/// </summary>
		public abstract class ConvertEncodedStringTestsBase
		{
			protected abstract ConvertEncodedString.ConvertNonNullString NonNullConverter { get; }
			protected abstract ConvertEncodedString.ConvertString Converter { get; }
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
			public void Converts_Known_String_Input_To_Known_Bytes()
			{
				Assert.Equal(ConvertByteArrayTests.KnownConstantByteArray, this.NonNullConverter(this.KnownConstantString));
				Assert.Equal(ConvertByteArrayTests.KnownConstantByteArray, this.Converter(this.KnownConstantString, true));
				Assert.Equal(ConvertByteArrayTests.KnownConstantByteArray, this.Converter(this.KnownConstantString, false));
			}
		}
	}
}