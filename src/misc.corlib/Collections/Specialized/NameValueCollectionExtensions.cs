namespace MiscCorLib.Collections.Specialized
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
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
		public static bool TryParseValue<T>(this NameValueCollection collection, string name, out T result)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection", "The instance of NameValueCollection is a null reference!");
			}

			string s = collection[name];

			if (string.IsNullOrEmpty(s))
			{
				result = default(T);
				return false;
			}

			TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

			try
			{
				object converted = converter.ConvertFrom(s);
				result = (T)converted;
				return true;
			}
			catch (Exception)
			{
				result = default(T);
				return false;
			}
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
		public static T GetValue<T>(this NameValueCollection collection, string name, T defaultReturn)
		{
			T result;
			if (!TryParseValue(collection, name, out result))
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
		/// <returns>
		/// The value from the <see cref="NameValueCollection"/>
		/// matching the <paramref name="name"/> parameter,
		/// coverted to the <see cref="Type"/> indicated by
		/// the generic type parameter <typeparamref name="T"/>,
		/// or the type's default value.
		/// </returns>
		public static T GetValue<T>(this NameValueCollection collection, string name)
		{
			return GetValue(collection, name, default(T));
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

		#region [ Public Static TryParseBoolean and GetBoolean Overloads ]

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="bool"/>.
		/// A return value indicates whether the conversion succeeded or failed.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="result">
		/// Returns the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter
		/// converted to <see cref="bool"/>, or false if the conversion failed.
		/// </param>
		/// <returns>
		/// true if the named value was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParseBoolean(this NameValueCollection collection, string name, out bool result)
		{
			return bool.TryParse(collection[name], out result);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="bool"/>,
		/// returning false if the item could not be found or if the conversion fails.
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
		/// or false if none could be found.
		/// </returns>
		public static bool GetBoolean([NotNull] this NameValueCollection collection, string name)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			return GetBoolean(collection, name, false);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="bool"/>,
		/// returning a specified default value if the item could
		/// not be found or if the conversion fails.
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
		public static bool GetBoolean(this NameValueCollection collection, string name, bool defaultReturn)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			bool result;
			if (!TryParseBoolean(collection, name, out result))
			{
				return defaultReturn;
			}

			return result;
		}

		#endregion

		#region [ Public Static TryParseByte, GetByte Overloads, and GetBytes ]

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="byte"/>.
		/// A return value indicates whether the conversion succeeded or failed.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="result">
		/// Returns the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter
		/// converted to <see cref="byte"/>, or zero if the conversion failed.
		/// </param>
		/// <returns>
		/// true if the named value was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParseByte(this NameValueCollection collection, string name, out byte result)
		{
			return byte.TryParse(collection[name], out result);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="byte"/>,
		/// returning zero if the item could not be found or if the conversion fails.
		/// </summary>
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
		public static byte GetByte(this NameValueCollection collection, string name)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			return GetByte(collection, name, 0);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="byte"/>,
		/// returning a specified default value if the item could
		/// not be found or if the conversion fails.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
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
		public static byte GetByte(this NameValueCollection collection, string name, byte defaultReturn)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			byte result;
			if (!TryParseByte(collection, name, out result))
			{
				return defaultReturn;
			}

			return result;
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to a
		/// one-dimensional array of <see cref="byte"/>, assuming that a value
		/// defined more than once is represented as a comma-delimited string.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the values.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <returns>
		/// An array of the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter.
		/// </returns>
		public static IEnumerable<byte> GetBytes(this NameValueCollection collection, string name)
		{
			if (string.IsNullOrEmpty(collection[name]))
			{
				return new byte[0];
			}

			return ConvertDelimitedString.ToEnumerable<byte>(collection[name], DefaultSeparator);
		}

		#endregion

		#region [ Public Static TryParseInt16, GetInt16 Overloads, and GetInt16s ]

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="short"/>.
		/// A return value indicates whether the conversion succeeded or failed.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="result">
		/// Returns the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter
		/// converted to <see cref="short"/>, or zero if the conversion failed.
		/// </param>
		/// <returns>
		/// true if the named value was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParseInt16(this NameValueCollection collection, string name, out short result)
		{
			return short.TryParse(collection[name], out result);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="short"/>,
		/// returning zero if the item could not be found or if the conversion fails.
		/// </summary>
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
		public static short GetInt16(this NameValueCollection collection, string name)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			return GetInt16(collection, name, 0);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="short"/>,
		/// returning a specified default value if the item could
		/// not be found or if the conversion fails.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
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
		public static short GetInt16(this NameValueCollection collection, string name, short defaultReturn)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			short result;
			if (!TryParseInt16(collection, name, out result))
			{
				return defaultReturn;
			}

			return result;
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to a
		/// one-dimensional array of <see cref="short"/>, assuming that a value
		/// defined more than once is represented as a comma-delimited string.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the values.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <returns>
		/// An array of the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter.
		/// </returns>
		public static IEnumerable<short> GetInt16s(this NameValueCollection collection, string name)
		{
			if (string.IsNullOrEmpty(collection[name]))
			{
				return new short[0];
			}

			return ConvertDelimitedString.ToEnumerable<short>(collection[name], DefaultSeparator);
		}

		#endregion

		#region [ Public Static TryParseInt32, GetInt32 Overloads, and GetInt32s ]

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="int"/>.
		/// A return value indicates whether the conversion succeeded or failed.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="result">
		/// Returns the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter
		/// converted to <see cref="int"/>, or zero if the conversion failed.
		/// </param>
		/// <returns>
		/// true if the named value was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParseInt32(this NameValueCollection collection, string name, out int result)
		{
			return int.TryParse(collection[name], out result);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="int"/>,
		/// returning zero if the item could not be found or if the conversion fails.
		/// </summary>
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
		public static int GetInt32(this NameValueCollection collection, string name)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			return GetInt32(collection, name, 0);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="int"/>,
		/// returning a specified default value if the item could
		/// not be found or if the conversion fails.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
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
		public static int GetInt32(this NameValueCollection collection, string name, int defaultReturn)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			int result;
			if (!TryParseInt32(collection, name, out result))
			{
				return defaultReturn;
			}

			return result;
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to a
		/// one-dimensional array of <see cref="int"/>, assuming that a value
		/// defined more than once is represented as a comma-delimited string.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the values.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <returns>
		/// An array of the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter.
		/// </returns>
		public static IEnumerable<int> GetInt32s(this NameValueCollection collection, string name)
		{
			if (string.IsNullOrEmpty(collection[name]))
			{
				return new int[0];
			}

			return ConvertDelimitedString.ToEnumerable<int>(collection[name], DefaultSeparator);
		}

		#endregion

		#region [ Public Static TryParseInt64, GetInt64 Overloads, and GetInt64s ]

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="long"/>.
		/// A return value indicates whether the conversion succeeded or failed.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="result">
		/// Returns the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter
		/// converted to <see cref="long"/>, or zero if the conversion failed.
		/// </param>
		/// <returns>
		/// true if the named value was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParseInt64(this NameValueCollection collection, string name, out long result)
		{
			return long.TryParse(collection[name], out result);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="long"/>,
		/// returning zero if the item could not be found or if the conversion fails.
		/// </summary>
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
		public static long GetInt64(this NameValueCollection collection, string name)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			return GetInt64(collection, name, 0L);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="long"/>,
		/// returning a specified default value if the item could
		/// not be found or if the conversion fails.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
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
		public static long GetInt64(this NameValueCollection collection, string name, long defaultReturn)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			long result;
			if (!TryParseInt64(collection, name, out result))
			{
				return defaultReturn;
			}

			return result;
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to a
		/// one-dimensional array of <see cref="long"/>, assuming that a value
		/// defined more than once is represented as a comma-delimited string.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the values.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <returns>
		/// An array of the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter.
		/// </returns>
		public static IEnumerable<long> GetInt64s(this NameValueCollection collection, string name)
		{
			if (string.IsNullOrEmpty(collection[name]))
			{
				return new long[0];
			}

			return ConvertDelimitedString.ToEnumerable<long>(collection[name], DefaultSeparator);
		}

		#endregion

		#region [ Public Static TryParseGuid, GetGuid Overloads, and GetGuids ]

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="Guid"/>.
		/// A return value indicates whether the conversion succeeded or failed.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="result">
		/// Returns the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter
		/// converted to <see cref="Guid"/>, or <see cref="Guid.Empty"/>
		/// if the conversion failed.
		/// </param>
		/// <returns>
		/// true if the named value was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParseGuid(this NameValueCollection collection, string name, out Guid result)
		{
			return Guid.TryParse(collection[name], out result);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="Guid"/>,
		/// returning <see cref="Guid.Empty"/> if the item could
		/// not be found or if the conversion fails.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <returns>
		/// The value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter,
		/// or <see cref="Guid.Empty"/> if none could be found.
		/// </returns>
		public static Guid GetGuid(this NameValueCollection collection, string name)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			return GetGuid(collection, name, Guid.Empty);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="Guid"/>,
		/// returning a specified default value if the item could
		/// not be found or if the conversion fails.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
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
		public static Guid GetGuid(this NameValueCollection collection, string name, Guid defaultReturn)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			Guid result;
			if (!TryParseGuid(collection, name, out result))
			{
				return defaultReturn;
			}

			return result;
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to a
		/// one-dimensional array of <see cref="Guid"/>, assuming that a value
		/// defined more than once is represented as a comma-delimited string.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <returns>
		/// An array of the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter.
		/// </returns>
		public static IEnumerable<Guid> GetGuids(this NameValueCollection collection, string name)
		{
			if (string.IsNullOrEmpty(collection[name]))
			{
				return new Guid[0];
			}

			return ConvertDelimitedString.ToEnumerable<Guid>(collection[name], DefaultSeparator);
		}

		#endregion

		#region [ Public Static TryParseDate and GetDate Overloads ]

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="DateTime"/>.
		/// A return value indicates whether the conversion succeeded or failed.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <param name="result">
		/// Returns the value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter
		/// converted to <see cref="DateTime"/>, or
		/// <see cref="DateTime.MinValue"/> if the conversion failed.
		/// </param>
		/// <returns>
		/// true if the named value was converted successfully, otherwise false.
		/// </returns>
		public static bool TryParseDate(this NameValueCollection collection, string name, out DateTime result)
		{
			return DateTime.TryParse(collection[name], out result);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="DateTime"/>,
		/// returning <see cref="DateTime.MinValue"/> if the item could
		/// not be found or if the conversion fails.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
		/// </param>
		/// <param name="name">
		/// The indexer for the <see cref="NameValueCollection"/>.
		/// </param>
		/// <returns>
		/// The value from the <see cref="NameValueCollection"/>
		/// corresponding to the <paramref name="name"/> parameter,
		/// or <see cref="DateTime.MinValue"/> if none could be found.
		/// </returns>
		public static DateTime GetDate(this NameValueCollection collection, string name)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			return GetDate(collection, name, DateTime.MinValue);
		}

		/// <summary>
		/// Converts a value in a <see cref="NameValueCollection"/> to <see cref="DateTime"/>,
		/// returning a specified default value if the item could
		/// not be found or if the conversion fails.
		/// </summary>
		/// <param name="collection">
		/// An instance of <see cref="NameValueCollection"/> to check for the value.
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
		public static DateTime GetDate(this NameValueCollection collection, string name, DateTime defaultReturn)
		{
			Contract.Requires<ArgumentNullException>(collection != null);

			DateTime result;
			return TryParseDate(collection, name, out result) ? result : defaultReturn;
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
			this NameValueCollection collection, string name, bool allowMultipleFlagsBits, out T result)
		{
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