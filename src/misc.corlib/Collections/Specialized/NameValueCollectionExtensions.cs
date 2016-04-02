#region [ license and copyright boilerplate ]
/*
	MiscCorLib.Collections.Specialized
	NameValueCollectionExtensions.cs

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

namespace MiscCorLib.Collections.Specialized
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Diagnostics.Contracts;

	using JetBrains.Annotations;

	using MiscCorLib.Collections.Generic;

	/// <summary>
	/// A set of static methods for extracting values from
	/// <see cref="NameValueCollection"/> objects.
	/// </summary>
	[CLSCompliant(true)]
	public static class NameValueCollectionExtensions
	{
		/// <summary>
		/// A value to use when converting from a delimited string.
		/// </summary>
		private const string DefaultSeparator = ConvertDelimitedString.DefaultSeparator;

		/// <summary>
		/// Checks the existence of a named item in
		/// a <see cref="NameValueCollection"/>.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <returns>
		/// True if the <paramref name="name"/> value is present in the
		/// keys of the <see cref="NameValueCollection"/>, otherwise false.
		/// </returns>
		public static bool ValueExists(this NameValueCollection collection, string name)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			return collection[name] != null;
		}

		#region [ Generic Retrieval of Values based on Keys ]

		/// <summary>
		/// Converts an item in a <see cref="NameValueCollection"/>
		/// to the generic type <typeparamref name="T"/>.
		/// A return value indicates whether the conversion succeeded or failed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of value to return.
		/// </typeparam>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="tryParseDelegate">
		/// 
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="result">
		/// Returns the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter
		/// converted to the generic type <typeparamref name="T"/>,
		/// or the default value of the generic type if the conversion failed.
		/// </param>
		/// <returns>
		/// True if the named value was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParseValue<T>(
			[NotNull] this NameValueCollection collection,
			[NotNull] ConvertStrings.TryParseFromString<T> tryParseDelegate,
			string name,
			out T result)
			where T : struct
		{
			Contract.Requires<ArgumentNullException>(collection != null);
			Contract.Requires<ArgumentNullException>(tryParseDelegate != null);

			return tryParseDelegate(collection[name], out result);
		}

		/// <summary>
		/// Gets the value from the <see cref="NameValueCollection"/>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method seeks to make value retrieval from
		/// the <see cref="NameValueCollection"/> as
		/// simple as possible. Implemented using generics,
		/// you simply pass both the <see cref="Type"/>
		/// to return and the name of the item in the collection.
		/// </para>
		/// <para>
		/// In the event that the collection does not contain the
		/// named item or the proper conversion cannot be made,
		/// the value of the <paramref name="defaultReturn"/>
		/// parameter is returned.
		/// </para>
		/// </remarks>
		/// <typeparam name="T">
		/// The type of value to return.
		/// </typeparam>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/>
		/// from which to retrieve the value.
		/// </param>
		/// <param name="tryParseDelegate">
		/// 
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="defaultReturn">
		/// A value to return if the item could not be found or if the conversion fails.
		/// </param>
		/// <returns>
		/// The value from the <see cref="NameValueCollection"/>
		/// matching the <paramref name="name"/> parameter,
		/// coverted to the <see cref="Type"/> indicated by
		/// the generic type parameter <typeparamref name="T"/>,
		/// or the value of the <paramref name="defaultReturn"/> parameter.
		/// </returns>
		public static T GetValue<T>(
			[NotNull] this NameValueCollection collection,
			[NotNull] ConvertStrings.TryParseFromString<T> tryParseDelegate, 
			string name,
			T defaultReturn)
			where T : struct
		{
			Contract.Requires<ArgumentNullException>(collection != null);
			Contract.Requires<ArgumentNullException>(tryParseDelegate != null);

			T result;
			if (!collection.TryParseValue(tryParseDelegate, name, out result))
			{
				result = defaultReturn;
			}

			return result;
		}

		/// <summary>
		/// Gets the value from the <see cref="NameValueCollection"/>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method seeks to make value retrieval from
		/// the <see cref="NameValueCollection"/> as
		/// simple as possible. Implemented using generics,
		/// you simply pass both the <see cref="Type"/>
		/// to return and the name of the item in the collection.
		/// </para>
		/// <para>
		/// In the event that the collection does not contain the
		/// named item or the proper conversion cannot be made,
		/// the default value of type <typeparamref name="T"/> is returned.
		/// </para>
		/// </remarks>
		/// <typeparam name="T">
		/// The type of value to return.
		/// </typeparam>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/>
		/// from which to retrieve the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="tryParseDelegate">
		/// 
		/// </param>
		/// <returns>
		/// The value from the <see cref="NameValueCollection"/>
		/// matching the <paramref name="name"/> parameter,
		/// coverted to the <see cref="Type"/> indicated by
		/// the generic type parameter <typeparamref name="T"/>,
		/// or the type's default value.
		/// </returns>
		public static T GetValue<T>(
			[NotNull] this NameValueCollection collection,
			[NotNull] ConvertStrings.TryParseFromString<T> tryParseDelegate, 
			string name)
			where T : struct
		{
			Contract.Requires<ArgumentNullException>(collection != null);
			Contract.Requires<ArgumentNullException>(tryParseDelegate != null);

			return GetValue(collection, tryParseDelegate, name, default(T));
		}

		#endregion

		#region [ Public Static TryGetString and GetString Overloads ]

		/// <summary>
		/// Retrieves a string value from a <see cref="NameValueCollection"/>,
		/// or an empty string if the value does not exist.
		/// A return value indicates whether the value exists.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="result">
		/// Returns the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/>, or an empty
		/// string if there is no matching value in the collection.
		/// </param>
		/// <returns>
		/// true if the named value exists in the <paramref name="collection"/>, otherwise false.
		/// </returns>
		public static bool TryGetString(this NameValueCollection collection, string name, out string result)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			if (ValueExists(collection, name))
			{
				result = collection[name];
				return true;
			}

			result = string.Empty;
			return false;
		}

		/// <summary>
		/// Retrieves a string value from a <see cref="NameValueCollection"/>,
		/// or an empty string if the value does not exist.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/>
		/// from which to retrieve the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <returns>
		/// The value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter,
		/// or an empty string if none could be found.
		/// </returns>
		public static string GetString(this NameValueCollection collection, string name)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			return GetString(collection, name, string.Empty);
		}

		/// <summary>
		/// Retrieves a string value from a <see cref="NameValueCollection"/>,
		/// or a specified default value if the value does not exist.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/>
		/// from which to retrieve the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="defaultReturn">
		/// A value to return if the item could not be found or if the conversion fails.
		/// </param>
		/// <returns>
		/// The value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter,
		/// or the value of the <paramref name="defaultReturn"/>
		/// parameter if none could be found.
		/// </returns>
		public static string GetString(this NameValueCollection collection, string name, string defaultReturn)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			string result;
			if (!TryGetString(collection, name, out result))
			{
				return defaultReturn;
			}

			return result;
		}

		#endregion

		#region [ Public Static TryParseEnum and GetEnum Overloads ]

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/>
		/// to an <see cref="Enum"/> value. A return value indicates
		/// whether the conversion succeeded or failed.
		/// </summary>
		/// <typeparam name="T">
		/// A type derived from <see cref="Enum"/>
		/// for the value to return.
		/// </typeparam>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="result">
		/// Returns the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter
		/// converted to <see cref="Enum"/>, or
		/// zero if the conversion failed.
		/// </param>
		/// <returns>
		/// true if the named value was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParseEnum<T>(this NameValueCollection collection, string name, out T result)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			return TryParseEnum(collection, name, false, out result);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/>
		/// to an <see cref="Enum"/> value. A return value indicates
		/// whether the conversion succeeded or failed.
		/// </summary>
		/// <typeparam name="T">
		/// A type derived from <see cref="Enum"/>
		/// for the value to return.
		/// </typeparam>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="allowMultipleFlagsBits">
		/// Whether to allow any combination of bitwise values matching
		/// the underlying type of <typeparamref name="T"/> where
		/// the <see cref="FlagsAttribute"/> is set, or to return zero
		/// if the value does not match one of the explicitly enumerated
		/// values for <typeparamref name="T"/>.
		/// </param>
		/// <param name="result">
		/// Returns the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter
		/// converted to <see cref="Enum"/>, or
		/// zero if the conversion failed.
		/// </param>
		/// <returns>
		/// true if the named value was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParseEnum<T>(
			[NotNull] this NameValueCollection collection, string name, bool allowMultipleFlagsBits, out T result)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			Type enumType = typeof(T);

			if (!enumType.IsEnum)
			{
				throw new ArgumentException("The generic type parameter must be a System.Enum type.");
			}

			Type underlyingType = Enum.GetUnderlyingType(enumType);

			T zero = default(T);
			string value = collection[name];

			if (string.IsNullOrEmpty(value))
			{
				result = zero;
				return false;
			}

			object obj;
			try
			{
				obj = Convert.ChangeType(value, underlyingType);
			}
			catch (FormatException)
			{
				result = zero;
				return false;
			}
			catch (InvalidCastException)
			{
				result = zero;
				return false;
			}
			catch (OverflowException)
			{
				result = zero;
				return false;
			}

			if (obj == null)
			{
				result = zero;
				return false;
			}

			if ((!allowMultipleFlagsBits) && (!Enum.IsDefined(enumType, obj)))
			{
				result = zero;
				return false;
			}

			Type type = obj.GetType();

			if (!(type == underlyingType))
			{
				result = zero;
				return false;
			}

			result = (T)obj;
			return true;
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to an
		/// <see cref="Enum"/> value, returning the enumeration's default
		/// value if the item could not be found or if the conversion fails.
		/// </summary>
		/// <typeparam name="T">
		/// A type derived from <see cref="Enum"/>
		/// for the value to return.
		/// </typeparam>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <returns>
		/// The value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter,
		/// or zero if none could be found.
		/// </returns>
		public static T GetEnum<T>(this NameValueCollection collection, string name) where T : struct
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			return GetEnum<T>(collection, name, false);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to an
		/// <see cref="Enum"/> value, returning the enumeration's default
		/// value if the item could not be found or if the conversion fails.
		/// </summary>
		/// <typeparam name="T">
		/// A type derived from <see cref="Enum"/>
		/// for the value to return.
		/// </typeparam>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="allowMultipleFlagsBits">
		/// Whether to allow any combination of bitwise values matching
		/// the underlying type of <typeparamref name="T"/> where
		/// the <see cref="FlagsAttribute"/> is set, or to return zero
		/// if the value does not match one of the explicitly enumerated
		/// values for <typeparamref name="T"/>.
		/// </param>
		/// <returns>
		/// The value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter,
		/// or zero if none could be found.
		/// </returns>
		public static T GetEnum<T>(this NameValueCollection collection, string name, bool allowMultipleFlagsBits)
			where T : struct
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			T result;
			TryParseEnum(collection, name, allowMultipleFlagsBits, out result);
			return result;
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to
		/// an <see cref="Enum"/> value, returning a specified default
		/// value if the item could not be found or if the conversion fails.
		/// </summary>
		/// <typeparam name="T">
		/// A type derived from <see cref="Enum"/>
		/// for the value to return.
		/// </typeparam>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="allowMultipleFlagsBits">
		/// Whether to allow any combination of bitwise values matching
		/// the underlying type of <typeparamref name="T"/> where
		/// the <see cref="FlagsAttribute"/> is set, or to return zero
		/// if the value does not match one of the explicitly enumerated
		/// values for <typeparamref name="T"/>.
		/// </param>
		/// <param name="defaultReturn">
		/// A value to return if the item could not be found or if the conversion fails.
		/// </param>
		/// <returns>
		/// The value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter,
		/// or the value of <paramref name="defaultReturn"/> if none could be found.
		/// </returns>
		public static T GetEnum<T>(
			this NameValueCollection collection,
			string name,
			bool allowMultipleFlagsBits,
			T defaultReturn) where T : struct
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			T result;
			if (!TryParseEnum(collection, name, allowMultipleFlagsBits, out result))
			{
				return defaultReturn;
			}

			return result;
		}

		#endregion
	}
}