#region [ license and copyright boilerplate ]
/*
	MiscCorLib
	ConvertEncodedStringTests.cs

	Copyright (c) 2015 Jim Kropa (http://www.kropa.net/)

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

namespace MiscCorLib
{
	using System;

	using NUnit.Framework;

	/// <summary>
	/// Automated unit tests of the <see cref="ConvertEncodedString"/> extention methods.
	/// </summary>
	[TestFixture]
	public sealed class ConvertEncodedStringTests
	{
		internal const string KnownConstantBase64String = @"AAEDBw8fP3//";
		internal const string KnownConstantHexadecimalString = @"000103070f1f3f7fff";

		[TestFixture]
		public sealed class FromBase64String : ConvertEncodedStringTestsBase
		{
			protected override ConvertEncodedString.ConvertNonNullString NonNullConverter
			{
				get { return ConvertEncodedString.FromBase64; }
			}

			protected override ConvertEncodedString.ConvertString Converter
			{
				get { return ConvertEncodedString.FromBase64; }
			}

			protected override string KnownConstantString
			{
				get { return KnownConstantBase64String; }
			}
		}

		[TestFixture]
		public sealed class ToHexadecimalString : ConvertEncodedStringTestsBase
		{
			protected override ConvertEncodedString.ConvertNonNullString NonNullConverter
			{
				get { return ConvertEncodedString.FromHexadecimal; }
			}

			protected override ConvertEncodedString.ConvertString Converter
			{
				get { return ConvertEncodedString.FromHexadecimal; }
			}

			protected override string KnownConstantString
			{
				get { return KnownConstantHexadecimalString; }
			}
		}


		[TestFixture]
		public sealed class ToByteArray
		{
			[Test]
			public void Disallows_Null_Array_By_Default()
			{
				// ReSharper disable once AssignNullToNotNullAttribute
				Assert.Throws<ArgumentNullException>(
					() => ConvertEncodedString.ToByteArray(null));

				// ReSharper disable once AssignNullToNotNullAttribute
				Assert.Throws<ArgumentNullException>(
					() => ConvertEncodedString.ToByteArray(null, ByteArrayStringEncoding.Hexadecimal));
			}

			[Test]
			public void Optionally_Returns_Null_From_Null_Array()
			{
				Assert.Throws<ArgumentNullException>(
					() => ConvertEncodedString.ToByteArray(null, false));

				Assert.IsNull(ConvertEncodedString.ToByteArray(null, true));

				Assert.Throws<ArgumentNullException>(
					() => ConvertEncodedString.ToByteArray(null, false, ByteArrayStringEncoding.Hexadecimal));

				Assert.IsNull(ConvertEncodedString.ToByteArray(null, true, ByteArrayStringEncoding.Hexadecimal));
			}

			[Test]
			public void Ignores_Invalid_Encoding_With_Null_Input()
			{
				const ByteArrayStringEncoding invalidEncoding = (ByteArrayStringEncoding)17;

				// ReSharper disable once AssignNullToNotNullAttribute
				Assert.Throws<ArgumentNullException>(
					() => ConvertEncodedString.ToByteArray(null, invalidEncoding));

				Assert.Throws<ArgumentNullException>(
					() => ConvertEncodedString.ToByteArray(null, false, invalidEncoding));

				Assert.IsNull(ConvertEncodedString.ToByteArray(null, true, invalidEncoding));
			}

			[Test]
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

			[Test]
			public void Uses_Correct_Encoding()
			{
				Assert.AreEqual(
					ConvertByteArrayTests.KnownConstantByteArray,
					KnownConstantBase64String.ToByteArray());

				Assert.AreEqual(
					ConvertByteArrayTests.KnownConstantByteArray,
					KnownConstantBase64String.ToByteArray(true));

				Assert.AreEqual(
					ConvertByteArrayTests.KnownConstantByteArray,
					KnownConstantBase64String.ToByteArray(false));

				Assert.AreEqual(
					ConvertByteArrayTests.KnownConstantByteArray,
					KnownConstantHexadecimalString.ToByteArray(ByteArrayStringEncoding.Hexadecimal));

				Assert.AreEqual(
					ConvertByteArrayTests.KnownConstantByteArray,
					KnownConstantHexadecimalString.ToByteArray(true, ByteArrayStringEncoding.Hexadecimal));

				Assert.AreEqual(
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

			[Test]
			public void Disallows_Null_Array_By_Default()
			{
				// ReSharper disable once AssignNullToNotNullAttribute
				Assert.Throws<ArgumentNullException>(
					() => this.NonNullConverter(null));
			}

			[Test]
			public void Optionally_Returns_Null_From_Null_Array()
			{
				Assert.Throws<ArgumentNullException>(
					() => this.Converter(null, false));

				Assert.IsNull(this.Converter(null, true));
			}

			[Test]
			public void Converts_Known_String_Input_To_Known_Bytes()
			{
				Assert.AreEqual(ConvertByteArrayTests.KnownConstantByteArray, this.NonNullConverter(this.KnownConstantString));
				Assert.AreEqual(ConvertByteArrayTests.KnownConstantByteArray, this.Converter(this.KnownConstantString, true));
				Assert.AreEqual(ConvertByteArrayTests.KnownConstantByteArray, this.Converter(this.KnownConstantString, false));
			}
		}
	}
}