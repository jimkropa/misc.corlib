#region [ license and copyright boilerplate ]
/*
	MiscCorLib
	ConvertEnum.cs

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
	using System.Collections.Generic;

	/// <summary>
	/// A set of static method for converting an
	/// enumeration type into other constructs.
	/// </summary>
	[CLSCompliant(true)]
	public static class ConvertEnum
	{
		#region [ Main Overloads of ToDictionary ]

		/// <summary>
		/// Returns a dictionary of enumeration
		/// values and names to use for each value.
		/// </summary>
		/// <typeparam name="T">
		/// An <see cref="Enum"/> type.
		/// </typeparam>
		/// <returns>
		/// A dictionary with the enumeration values as the keys,
		/// and the name of each value as the values.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// The generic type parameter <typeparamref name="T"/>
		/// does not derive from <see cref="Enum"/>.
		/// </exception>
		public static IDictionary<T, string> ToDictionary<T>()
			where T : struct, IComparable, IFormattable
		{
			Type underlyingType;

			return ToDictionary<T>(out underlyingType);
		}

		/// <summary>
		/// Returns a dictionary of enumeration
		/// values and names to use for each value.
		/// </summary>
		/// <typeparam name="T">
		/// An <see cref="Enum"/> type.
		/// </typeparam>
		/// <param name="underlyingType">
		/// Returns the integral type underlying the enumeration,
		/// from the <see cref="Enum.GetUnderlyingType"/> method.
		/// </param>
		/// <returns>
		/// A dictionary with the enumeration values as the keys,
		/// and the name of each value as the values.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// The generic type parameter <typeparamref name="T"/>
		/// does not derive from <see cref="Enum"/>.
		/// </exception>
		public static IDictionary<T, string> ToDictionary<T>(out Type underlyingType)
			where T : struct, IComparable, IFormattable
		{
			Type enumType = typeof(T);

			if (!enumType.IsEnum)
			{
				throw new ArgumentException("The generic type parameter must be a System.Enum type.");
			}

			underlyingType = Enum.GetUnderlyingType(enumType);

			Array values = Enum.GetValues(enumType);
			IDictionary<T, string> dict = new Dictionary<T, string>();

			foreach (object value in values)
			{
				string name;
				if (TryParseName<T>(enumType, value, out name))
				{
					dict.Add((T)value, name);
				}
			}

			return dict;
		}

		/// <summary>
		/// Returns a dictionary of values of an enumeration's
		/// underlying type, and names to use for each value.
		/// </summary>
		/// <typeparam name="T">
		/// An <see cref="Enum"/> type.
		/// </typeparam>
		/// <typeparam name="TU">
		/// The underlying type of the <see cref="Enum"/>
		/// value of <typeparamref name="T"/>, from the
		/// <see cref="Enum.GetUnderlyingType"/> method.
		/// </typeparam>
		/// <returns>
		/// The generic type parameter <typeparamref name="T"/>
		/// does not derive from <see cref="Enum"/>.
		/// </returns>
		public static IDictionary<TU, string> ToDictionary<T, TU>()
			where T : struct, IComparable, IFormattable
			where TU : struct
		{
			Type enumType = typeof(T);

			if (!enumType.IsEnum)
			{
				throw new ArgumentException("The generic type parameter must be a System.Enum type.");
			}

			Type underlyingType = Enum.GetUnderlyingType(enumType);

			if (underlyingType != typeof(TU))
			{
				throw new InvalidCastException(
					"Cannot convert the System.Enum type to the specified type becaues the underlying type does not match.");
			}

			Array values = Enum.GetValues(enumType);
			IDictionary<TU, string> dict = new Dictionary<TU, string>();

			foreach (object value in values)
			{
				string name;
				if (TryParseName<T>(enumType, value, out name))
				{
					dict.Add((TU)value, name);
				}
			}

			return dict;
		}

		#endregion

		#region [ Overloads of ToDictionary for Signed Integral Types ]

		/// <summary>
		/// Returns a dictionary of byte keys based on
		/// an enumeration with byte as its underlying type,
		/// and names to use for each value.
		/// </summary>
		/// <typeparam name="T">
		/// An <see cref="Enum"/> type.
		/// </typeparam>
		/// <returns>
		/// The generic type parameter <typeparamref name="T"/>
		/// does not derive from <see cref="Enum"/>.
		/// </returns>
		public static IDictionary<byte, string> ToDictionaryByte<T>()
			where T : struct, IComparable, IFormattable
		{
			return ToDictionary<T, byte>();
		}

		/// <summary>
		/// Returns a dictionary of short integer keys based on
		/// an enumeration with byte as its underlying type,
		/// and names to use for each value.
		/// </summary>
		/// <typeparam name="T">
		/// An <see cref="Enum"/> type.
		/// </typeparam>
		/// <returns>
		/// The generic type parameter <typeparamref name="T"/>
		/// does not derive from <see cref="Enum"/>.
		/// </returns>
		public static IDictionary<short, string> ToDictionaryInt16<T>()
			where T : struct, IComparable, IFormattable
		{
			return ToDictionary<T, short>();
		}

		/// <summary>
		/// Returns a dictionary of integer keys based on
		/// an enumeration with byte as its underlying type,
		/// and names to use for each value.
		/// </summary>
		/// <typeparam name="T">
		/// An <see cref="Enum"/> type.
		/// </typeparam>
		/// <returns>
		/// The generic type parameter <typeparamref name="T"/>
		/// does not derive from <see cref="Enum"/>.
		/// </returns>
		public static IDictionary<int, string> ToDictionaryInt32<T>()
			where T : struct, IComparable, IFormattable
		{
			return ToDictionary<T, int>();
		}

		/// <summary>
		/// Returns a dictionary of long integer keys based on
		/// an enumeration with byte as its underlying type,
		/// and names to use for each value.
		/// </summary>
		/// <typeparam name="T">
		/// An <see cref="Enum"/> type.
		/// </typeparam>
		/// <returns>
		/// The generic type parameter <typeparamref name="T"/>
		/// does not derive from <see cref="Enum"/>.
		/// </returns>
		public static IDictionary<long, string> ToDictionaryInt64<T>()
			where T : struct, IComparable, IFormattable
		{
			return ToDictionary<T, long>();
		}

		#endregion

		#region [ Overloads of ToDictionary for Unsigned Integral Types (commented, not CLS compliant) ]
		/*

			/// <summary>
			/// Returns a dictionary of signed byte keys based on
			/// an enumeration with byte as its underlying type,
			/// and names to use for each value.
			/// </summary>
			/// <typeparam name="T">
			/// An <see cref="Enum" /> type.
			/// </typeparam>
			/// <returns>
			/// The generic type parameter <typeparamref name="T" />
			/// does not derive from <see cref="Enum" />.
			/// </returns>
			public static IDictionary<sbyte, string> ToDictionarySByte<T>()
				where T : struct, IComparable, IFormattable
			{
				return ToDictionary<T, sbyte>();
			}

			/// <summary>
			/// Returns a dictionary of unsigned short integer keys based on
			/// an enumeration with byte as its underlying type,
			/// and names to use for each value.
			/// </summary>
			/// <typeparam name="T">
			/// An <see cref="Enum" /> type.
			/// </typeparam>
			/// <returns>
			/// The generic type parameter <typeparamref name="T" />
			/// does not derive from <see cref="Enum" />.
			/// </returns>
			public static IDictionary<ushort, string> ToDictionaryUInt16<T>()
				where T : struct, IComparable, IFormattable
			{
				return ToDictionary<T, ushort>();
			}

			/// <summary>
			/// Returns a dictionary of unsigned integer keys based on
			/// an enumeration with byte as its underlying type,
			/// and names to use for each value.
			/// </summary>
			/// <typeparam name="T">
			/// An <see cref="Enum" /> type.
			/// </typeparam>
			/// <returns>
			/// The generic type parameter <typeparamref name="T" />
			/// does not derive from <see cref="Enum" />.
			/// </returns>
			public static IDictionary<uint, string> ToDictionaryUInt32<T>()
				where T : struct, IComparable, IFormattable
			{
				return ToDictionary<T, uint>();
			}

			/// <summary>
			/// Returns a dictionary of unsigned long integer keys based on
			/// an enumeration with byte as its underlying type,
			/// and names to use for each value.
			/// </summary>
			/// <typeparam name="T">
			/// An <see cref="Enum" /> type.
			/// </typeparam>
			/// <returns>
			/// The generic type parameter <typeparamref name="T" />
			/// does not derive from <see cref="Enum" />.
			/// </returns>
			public static IDictionary<ulong, string> ToDictionaryUInt64<T>()
				where T : struct, IComparable, IFormattable
			{
				return ToDictionary<T, ulong>();
			}

		*/
		#endregion

		#region [ TryParse Methods For Enums ]

		/// <summary>
		/// This method takes a string and attempts to return the enumeration value.
		/// If the conversion is not possible the function returns false,
		/// and the default enumeration is returned via the output parameter.
		/// otherwise true is returned from the function and the converted value is returned.
		/// </summary>
		/// <typeparam name="TEnum">
		/// The type of enumeration to parse.
		/// </typeparam>
		/// <param name="enumValue">
		/// A byte representing a value of the
		/// <typeparamref name="TEnum"/>.
		/// </param>
		/// <param name="enumOut">
		/// If this method returns true, returns
		/// the enumeration value matching the
		/// given <paramref name="enumValue"/>.
		/// Otherwise, returns zero.
		/// </param>
		/// <returns>
		/// True if the <paramref name="enumValue"/>
		/// was parsed into exactly one value of the
		/// <typeparamref name="TEnum"/>. Otherwise, false.
		/// </returns>
		public static bool TryParse<TEnum>(this byte enumValue, out TEnum enumOut)
			where TEnum : struct, IComparable, IFormattable
		{
			return TryParseInternal(enumValue, out enumOut);
		}

		/// <summary>
		/// This method takes a string and attempts to return the enumeration value.
		/// If the conversion is not possible the function returns false,
		/// and the default enumeration is returned via the output parameter.
		/// otherwise true is returned from the function and the converted value is returned.
		/// </summary>
		/// <typeparam name="TEnum">
		/// The type of enumeration to parse.
		/// </typeparam>
		/// <param name="enumValue">
		/// A byte representing a value of the
		/// <typeparamref name="TEnum"/>.
		/// </param>
		/// <param name="enumOut">
		/// If this method returns true, returns
		/// the enumeration value matching the
		/// given <paramref name="enumValue"/>.
		/// Otherwise, returns zero.
		/// </param>
		/// <returns>
		/// True if the <paramref name="enumValue"/>
		/// was parsed into exactly one value of the
		/// <typeparamref name="TEnum"/>. Otherwise, false.
		/// </returns>
		public static bool TryParse<TEnum>(this short enumValue, out TEnum enumOut)
			where TEnum : struct, IComparable, IFormattable
		{
			return TryParseInternal(enumValue, out enumOut);
		}

		/// <summary>
		/// This method takes a string and attempts to return the enumeration value.
		/// if the conversion is not possible the function returns false, and the default enumeration is returned via the output parameter.
		/// otherwise true is returned from the function and the converted value is returned.
		/// </summary>
		/// <typeparam name="TEnum">
		/// The type of enumeration to parse.
		/// </typeparam>
		/// <param name="enumValue">
		/// A byte representing a value of the
		/// <typeparamref name="TEnum"/>.
		/// </param>
		/// <param name="enumOut">
		/// If this method returns true, returns
		/// the enumeration value matching the
		/// given <paramref name="enumValue"/>.
		/// Otherwise, returns zero.
		/// </param>
		/// <returns>
		/// True if the <paramref name="enumValue"/>
		/// was parsed into exactly one value of the
		/// <typeparamref name="TEnum"/>. Otherwise, false.
		/// </returns>
		public static bool TryParse<TEnum>(this int enumValue, out TEnum enumOut)
			where TEnum : struct, IComparable, IFormattable
		{
			return TryParseInternal(enumValue, out enumOut);
		}

		/// <summary>
		/// This method takes a string and attempts to return the enumeration value.
		/// if the conversion is not possible the function returns false, and the default enumeration is returned via the output parameter.
		/// otherwise true is returned from the function and the converted value is returned.
		/// </summary>
		/// <typeparam name="TEnum">
		/// The type of enumeration to parse.
		/// </typeparam>
		/// <param name="enumValue">
		/// A byte representing a value of the
		/// <typeparamref name="TEnum"/>.
		/// </param>
		/// <param name="enumOut">
		/// If this method returns true, returns
		/// the enumeration value matching the
		/// given <paramref name="enumValue"/>.
		/// Otherwise, returns zero.
		/// </param>
		/// <returns>
		/// True if the <paramref name="enumValue"/>
		/// was parsed into exactly one value of the
		/// <typeparamref name="TEnum"/>. Otherwise, false.
		/// </returns>
		public static bool TryParse<TEnum>(this long enumValue, out TEnum enumOut)
			where TEnum : struct, IComparable, IFormattable
		{
			return TryParseInternal(enumValue, out enumOut);
		}

		/// <summary>
		/// This method takes an Enum and returns the same enumeration value.
		/// </summary>
		/// <typeparam name="TEnum">
		/// The type of enumeration to parse.
		/// </typeparam>
		/// <param name="enumValue">
		/// A byte representing a value of the
		/// <typeparamref name="TEnum"/>.
		/// </param>
		/// <param name="enumOut">
		/// If this method returns true, returns
		/// the enumeration value matching the
		/// given <paramref name="enumValue"/>.
		/// Otherwise, returns zero.
		/// </param>
		/// <returns>
		/// True if the <paramref name="enumValue"/>
		/// was parsed into exactly one value of the
		/// <typeparamref name="TEnum"/>. Otherwise, false.
		/// </returns>
		public static bool TryParse<TEnum>(this TEnum enumValue, out TEnum enumOut)
			where TEnum : struct, IComparable, IFormattable
		{
			VerifyIsEnumType<TEnum>();

			enumOut = enumValue;

			return true;
		}

		/// <summary>
		/// This method takes a string and attempts to return the enumeration value.
		/// If the conversion is not possible the function returns false,
		/// and the default enumeration is returned via the output parameter.
		/// otherwise true is returned from the function and the converted value is returned.
		/// </summary>
		/// <typeparam name="TEnum">
		/// The type of enumeration to parse.
		/// </typeparam>
		/// <param name="enumValue">
		/// A byte representing a value of the
		/// <typeparamref name="TEnum"/>.
		/// </param>
		/// <param name="enumOut">
		/// If this method returns true, returns
		/// the enumeration value matching the
		/// given <paramref name="enumValue"/>.
		/// Otherwise, returns zero.
		/// </param>
		/// <returns>
		/// True if the <paramref name="enumValue"/>
		/// was parsed into exactly one value of the
		/// <typeparamref name="TEnum"/>. Otherwise, false.
		/// </returns>
		public static bool TryParse<TEnum>(this string enumValue, out TEnum enumOut)
			where TEnum : struct, IComparable, IFormattable
		{
			VerifyIsEnumType<TEnum>();

			Type enumType = typeof(TEnum);

			if ((enumValue == null) || (!Enum.IsDefined(enumType, enumValue)))
			{
				enumOut = default(TEnum);
				return false;
			}

			enumOut = (TEnum)Enum.Parse(enumType, enumValue, false);
			return true;
		}

		#endregion

		#region [ TryParse Methods For Enums with the Flags Attribute ]
		/*
			/// <summary>
			/// Returns the values of a flags
			/// enumeration from a given byte value.
			/// </summary>
			/// <typeparam name="TEnum">
			/// The type of enumeration to parse.
			/// </typeparam>
			/// <param name="enumValue">
			/// A byte representing a set of bits of
			/// the <typeparamref name="TEnum"/>.
			/// </param>
			/// <param name="enumOut">
			/// If this method returns true, returns
			/// an array with the bits of a flags
			/// enumeration contained in the given
			/// <paramref name="enumValue"/>.
			/// Otherwise, returns an empty array.
			/// </param>
			/// <returns>
			/// True if the <paramref name="enumValue"/>
			/// was parsed into at least one value of the
			/// <typeparamref name="TEnum"/>. Otherwise, false.
			/// </returns>
			public static bool TryParse<TEnum>(this byte enumValue, out TEnum[] enumOut)
				where TEnum : struct, IComparable, IFormattable
			{
				return TryParseInternal(enumValue, out enumOut);
			}

			/// <summary>
			/// This method takes a short and returns the Prime Parts that Compose the Flags Enumeration.
			/// </summary>
			/// <typeparam name="TEnum">
			/// The type of enumeration to parse.
			/// </typeparam>
			/// <param name="enumValue">
			/// A short integer representing a set of bits
			/// of the <typeparamref name="TEnum"/>.
			/// </param>
			/// <param name="enumOut">
			/// If this method returns true, returns
			/// an array with the bits of a flags
			/// enumeration contained in the given
			/// <paramref name="enumValue"/>.
			/// Otherwise, returns an empty array.
			/// </param>
			/// <returns>
			/// True if the <paramref name="enumValue"/>
			/// was parsed into at least one value of the
			/// <typeparamref name="TEnum"/>. Otherwise, false.
			/// </returns>
			public static bool TryParse<TEnum>(this short enumValue, out TEnum[] enumOut)
				where TEnum : struct, IComparable, IFormattable
			{
				return TryParseInternal(enumValue, out enumOut);
			}

			/// <summary>
			/// This method takes an int and returns the Prime Parts that Compose the Flags Enumeration.
			/// </summary>
			/// <typeparam name="TEnum">
			/// The type of enumeration to parse.
			/// </typeparam>
			/// <param name="enumValue">
			/// An integer representing a set of bits
			/// of the <typeparamref name="TEnum"/>.
			/// </param>
			/// <param name="enumOut">
			/// If this method returns true, returns
			/// an array with the bits of a flags
			/// enumeration contained in the given
			/// <paramref name="enumValue"/>.
			/// Otherwise, returns an empty array.
			/// </param>
			/// <returns>
			/// True if the <paramref name="enumValue"/>
			/// was parsed into at least one value of the
			/// <typeparamref name="TEnum"/>. Otherwise, false.
			/// </returns>
			public static bool TryParse<TEnum>(this int enumValue, out TEnum[] enumOut)
				where TEnum : struct, IComparable, IFormattable
			{
				return TryParseInternal(enumValue, out enumOut);
			}

			/// <summary>
			/// This method takes a long and returns the Prime Parts that Compose the Flags Enumeration.
			/// </summary>
			/// <typeparam name="TEnum">
			/// The type of enumeration to parse.
			/// </typeparam>
			/// <param name="enumValue">
			/// A long integer representing a set of bits
			/// of the <typeparamref name="TEnum"/>.
			/// </param>
			/// <param name="enumOut">
			/// If this method returns true, returns
			/// an array with the bits of a flags
			/// enumeration contained in the given
			/// <paramref name="enumValue"/>.
			/// Otherwise, returns an empty array.
			/// </param>
			/// <returns>
			/// True if the <paramref name="enumValue"/>
			/// was parsed into at least one value of the
			/// <typeparamref name="TEnum"/>. Otherwise, false.
			/// </returns>
			public static bool TryParse<TEnum>(this long enumValue, out TEnum[] enumOut)
				where TEnum : struct, IComparable, IFormattable
			{
				return TryParseInternal(enumValue, out enumOut);
			}

			/// <summary>
			/// This method is used to split an enumeration that is flags
			/// into its defined values, which are typically powers of two.
			/// This implementation always returns true.
			/// </summary>
			/// <typeparam name="TEnum">
			/// The type of enumeration to parse.
			/// </typeparam>
			/// <param name="enumValue">
			/// A <typeparamref name="TEnum"/> value to
			/// be parsed into its individually defined values.
			/// </param>
			/// <param name="enumOut">
			/// If this method returns true, returns
			/// an array with the bits of a flags
			/// enumeration contained in the given
			/// <paramref name="enumValue"/>.
			/// Otherwise, returns an empty array.
			/// </param>
			/// <returns>
			/// Always returns true.
			/// </returns>
			public static bool TryParse<TEnum>(this TEnum enumValue, out TEnum[] enumOut)
				where TEnum : struct, IComparable, IFormattable
			{
				VerifyIsEnumType<TEnum>();

				ICollection<TEnum> enumsOut = new List<TEnum>();
				foreach (TEnum actualEnumValue in Enum.GetValues(typeof(TEnum)))
				{
					if ((enumValue & actualEnumValue) == actualEnumValue))
					{
						enumsOut.Add(actualEnumValue);
					}
				}

				RemoveNoneIfFlagsAndHasOtherValues(enumsOut);

				enumOut = enumsOut.ToArray<TEnum>();

				return true;
			}
		*/

		/// <summary>
		/// This method is used to determine if a string value is the text of an enumeration.
		/// returns true if any values in the string were in the enumeration.
		/// This method is to be used if flags are involved.
		/// </summary>
		/// <typeparam name="TEnum">
		/// The type of enumeration to parse.
		/// </typeparam>
		/// <param name="enumValue">
		/// A string to be converted <typeparamref name="TEnum"/>.
		/// </param>
		/// <param name="enumOut">
		/// If this method returns true, returns
		/// an array with the bits of a flags
		/// enumeration contained in the given
		/// <paramref name="enumValue"/>.
		/// Otherwise, returns an empty array.
		/// </param>
		/// <returns>
		/// True if the <paramref name="enumValue"/>
		/// was parsed into at least one value of the
		/// <typeparamref name="TEnum"/>. Otherwise, false.
		/// </returns>
		public static bool TryParse<TEnum>(this string enumValue, out TEnum[] enumOut)
			where TEnum : struct, IComparable, IFormattable
		{
			VerifyIsEnumType<TEnum>();

			if (enumValue == null)
			{
				enumOut = new TEnum[0];
				return false;
			}

			string[] enumValues = enumValue.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < enumValues.Length; i++)
			{
				enumValues[i] = enumValues[i].Trim();
			}

			Type enumType = typeof(TEnum);
			List<TEnum> enumsOut = new List<TEnum>();
			foreach (string item in enumValues)
			{
				if (Enum.IsDefined(enumType, item))
				{
					enumsOut.Add((TEnum)Enum.Parse(enumType, item));
				}
			}

			enumOut = enumsOut.ToArray();

			if (enumsOut.Count > 0)
			{
				return true;
			}

			return false;
		}

		#endregion

		#region [ Private Static Methods used by TryParse Overloads ]

		/// <summary>
		/// Verifies that the generic type parameter is an enumeration,
		/// throws an exception if not.
		/// </summary>
		/// <typeparam name="TEnum">
		/// Generic type parameter which should be an enumeration.
		/// </typeparam>
		private static void VerifyIsEnumType<TEnum>()
			where TEnum : struct, IComparable, IFormattable
		{
			if (!typeof(TEnum).IsEnum)
			{
				throw new ArgumentException("The generic type parameter must be a System.Enum type.");
			}
		}

		/// <summary>
		/// Gets the the value of an enumeration
		/// as its underlying type.
		/// </summary>
		/// <typeparam name="TUnderlyingType">
		/// The underlying type of <typeparamref name="TEnum"/>,
		/// or an integer type which may be converted to that type.
		/// </typeparam>
		/// <typeparam name="TEnum">
		/// Generic type parameter which should be an enumeration.
		/// </typeparam>
		/// <param name="enumValue">
		/// A value of type <typeparamref name="TEnum"/> to be
		/// be converted to <typeparamref name="TUnderlyingType"/>.
		/// </param>
		/// <returns>
		/// The <paramref name="enumValue"/> converted
		/// to <typeparamref name="TUnderlyingType"/>.
		/// </returns>
		private static TUnderlyingType GetUnderlyingValueOfEnum<TUnderlyingType, TEnum>(TEnum enumValue)
			where TEnum : struct, IComparable, IFormattable
			where TUnderlyingType : struct
		{
			object convertedEnumValue = enumValue;
			Type underlyingType = Enum.GetUnderlyingType(typeof(TUnderlyingType));

			if (underlyingType != typeof(TUnderlyingType))
			{
				if (underlyingType == typeof(byte))
				{
					convertedEnumValue = Convert.ToByte(enumValue);
				}
				else if (underlyingType == typeof(short))
				{
					convertedEnumValue = Convert.ToInt16(enumValue);
				}
				else if (underlyingType == typeof(int))
				{
					convertedEnumValue = Convert.ToInt32(enumValue);
				}
				else if (underlyingType == typeof(long))
				{
					convertedEnumValue = Convert.ToInt64(enumValue);
				}
			}

			return (TUnderlyingType)convertedEnumValue;
		}

		/// <summary>
		/// Parses a single integer value to its
		/// corresponding value in an enumeration.
		/// </summary>
		/// <typeparam name="TEnum">
		/// Generic type parameter which should be an enumeration.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// The underlying type of <typeparamref name="TEnum"/>,
		/// or an integer type which may be converted to that type.
		/// </typeparam>
		/// <param name="enumValue">
		/// An integer value which may be converted
		/// to <typeparamref name="TEnum"/>.
		/// </param>
		/// <param name="enumOut">
		/// If this method returns true, returns an
		/// enumeration value corresponding to the
		/// given <paramref name="enumValue"/>.
		/// Otherwise, returns zero.
		/// </param>
		/// <returns>
		/// True if the <paramref name="enumValue"/>
		/// was parsed into at least one value of the
		/// <typeparamref name="TEnum"/>. Otherwise, false.
		/// </returns>
		private static bool TryParseInternal<TEnum, TValue>(TValue enumValue, out TEnum enumOut)
			where TEnum : struct, IComparable, IFormattable
			where TValue : struct, IComparable, IFormattable
		{
			VerifyIsEnumType<TEnum>();

			Type enumType = typeof(TEnum);

			TEnum convertedEnumValue = GetUnderlyingValueOfEnum<TEnum, TValue>(enumValue);

			if (Enum.IsDefined(enumType, convertedEnumValue))
			{
				enumOut = (TEnum)Enum.ToObject(enumType, convertedEnumValue);
				return true;
			}

			enumOut = default(TEnum);

			return false;
		}

		/*
			/// <summary>
			/// Returns the values of a flags
			/// enumeration from a given byte value.
			/// </summary>
			/// <typeparam name="TEnum">
			/// The type of enumeration to parse.
			/// </typeparam>
			/// <typeparam name="TValue">
			/// The underlying type of <typeparamref name="TEnum"/>,
			/// or an integer type which may be converted to that type.
			/// </typeparam>
			/// <param name="enumValue">
			/// A byte representing a set of bits of
			/// the <typeparamref name="TEnum"/>.
			/// </param>
			/// <param name="enumOut">
			/// If this method returns true, returns
			/// an array with the bits of a flags
			/// enumeration contained in the given
			/// <paramref name="enumValue"/>.
			/// Otherwise, returns an empty array.
			/// </param>
			/// <returns>
			/// True if the <paramref name="enumValue"/>
			/// was parsed into at least one value of the
			/// <typeparamref name="TEnum"/>. Otherwise, false.
			/// </returns>
			private static bool TryParseInternal<TEnum, TValue>(TValue enumValue, out TEnum[] enumOut)
				where TEnum : struct, IComparable, IFormattable
				where TValue : struct, IComparable, IFormattable
			{
				VerifyIsEnumType<TEnum>();

				TEnum convertedEnumValue = GetUnderlyingValueOfEnum<TEnum, TValue>(enumValue);

				ICollection<TEnum> enumsOut = new List<TEnum>();

				foreach (TEnum actualEnumValue in Enum.GetValues(typeof(TEnum)))
				{
					if ((convertedEnumValue & actualEnumValue) == actualEnumValue))
					{
						enumsOut.Add(actualEnumValue);
					}
				}

				RemoveNoneIfFlagsAndHasOtherValues(enumsOut);

				enumOut = enumsOut.ToArray<TEnum>();

				if (enumsOut.Count > 0)
				{
					return true;
				}

				return false;
			}

			/// <summary>
			/// Removes the "zero" value from the given
			/// collection if it contains other values.
			/// </summary>
			/// <typeparam name="TEnum">
			/// The type of enumeration to parse.
			/// </typeparam>
			/// <param name="enumsOut">
			/// A reference to an <see cref="ICollection{TEnum}"/>
			/// which may contain the "zero" value and others.
			/// </param>
			private static void RemoveNoneIfFlagsAndHasOtherValues<TEnum>(ICollection<TEnum> enumsOut)
				where TEnum : struct, IComparable, IFormattable
			{
				Type enumType = typeof(TEnum);
				if ((enumsOut.Count > 1)
					&& Enum.IsDefined(enumType, GetUnderlyingValueOfEnum<TEnum, int>(0))
					&& enumsOut.Contains(default(TEnum)))
				{
					enumsOut.Remove(default(TEnum));
				}
			}
		*/

		/// <summary>
		/// Private method used by the ToDictionary overloads.
		/// </summary>
		/// <typeparam name="T">
		/// An <see cref="Enum"/> type which is the same
		/// as the <paramref name="enumType"/> parameter.
		/// </typeparam>
		/// <param name="enumType">
		/// The type of <typeparamref name="T"/>.
		/// </param>
		/// <param name="value">
		/// A value to convert to the <paramref name="enumType"/>.
		/// </param>
		/// <param name="name">
		/// The name resolved for the <paramref name="value"/>.
		/// </param>
		/// <returns>
		/// True if the <paramref name="name"/> is resolved
		/// for the <paramref name="value"/>, otherwise false.
		/// </returns>
		private static bool TryParseName<T>(Type enumType, object value, out string name)
			where T : struct, IComparable, IFormattable
		{
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("The generic type parameter must be a System.Enum type.", "enumType");
			}

			if (enumType != typeof(T))
			{
				throw new ArgumentException("The generic type parameter must be the same as the enumType parameter.", "enumType");
			}

			if (!Enum.IsDefined(enumType, value))
			{
				name = string.Empty;

				return false;
			}

			name = Enum.GetName(enumType, value);
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}

			// Remove underscores, replace with spaces.
			name = name.Replace("_", " ");

			return true;
		}

		#endregion
	}
}