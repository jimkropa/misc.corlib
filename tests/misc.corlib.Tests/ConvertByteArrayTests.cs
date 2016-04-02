#region [ license and copyright boilerplate ]
/*
	MiscCorLib
	ConvertByteArrayTests.cs

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
		internal static readonly byte[] KnownConstantByteArray = { 0, 1, 3, 7, 15, 31, 63, 127, 255 };

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

			protected override string KnownConstantString
			{
				get { return ConvertEncodedStringTests.KnownConstantBase64String; }
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

			protected override string KnownConstantString
			{
				get { return ConvertEncodedStringTests.KnownConstantHexadecimalString; }
			}
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
			public void Converts_Known_Input_To_Known_String()
			{
				Assert.AreEqual(this.KnownConstantString, this.NonNullConverter(KnownConstantByteArray));
				Assert.AreEqual(this.KnownConstantString, this.Converter(KnownConstantByteArray, true));
				Assert.AreEqual(this.KnownConstantString, this.Converter(KnownConstantByteArray, false));

				Assert.IsTrue(string.Equals(
					this.KnownConstantString,
					this.NonNullConverter(KnownConstantByteArray),
					StringComparison.Ordinal));
				Assert.IsTrue(string.Equals(
					this.KnownConstantString,
					this.Converter(KnownConstantByteArray, true),
					StringComparison.Ordinal));
				Assert.IsTrue(string.Equals(
					this.KnownConstantString,
					this.Converter(KnownConstantByteArray, false),
					StringComparison.Ordinal));
			}
		}
	}
}