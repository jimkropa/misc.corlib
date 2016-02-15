namespace MiscCorLib.Collections.Generic
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics.Contracts;
	using System.Linq;

	using JetBrains.Annotations;

	/// <summary>
	/// A set of static methods for converting collections of strings
	/// into generic collections of <see cref="ValueType"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// These methods operate midway between delimited strings
	/// created by <see cref="ConvertStructCollection"/> methods
	/// and the <see cref="ConvertDelimitedString"/> methods for
	/// reversing the process.
	/// </para>
	/// </remarks>
	[CLSCompliant(true)]
	public static class ConvertStrings
	{
		#region [ TryConvertFromString Method and TryParseFromString<T> Delegate Contract ]

		/// <summary>
		/// A value to use when translating from a collection
		/// of strings to a generic collection of <see cref="ValueType"/>,
		/// indicating a default preference to suppress any
		/// <see cref="FormatException"/> or <see cref="NotSupportedException"/>
		/// encountered during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using a <see cref="TypeConverter"/> to translate
		/// each string to a generic <see cref="ValueType"/>.
		/// </summary>
		public const bool DefaultThrowTypeConversionExceptions = false;

		/// <summary>
		/// This is a workaround for a problem in CodeContracts.
		/// </summary>
		/// <remarks>
		/// Refer to <a href="https://github.com/Microsoft/CodeContracts/issues/339">the issue report</a>
		/// and <a href="http://stackoverflow.com/questions/34612382/">the source of this workaround</a>.
		/// </remarks>
		public static readonly Predicate<string> IsNullOrWhiteSpace = string.IsNullOrWhiteSpace;

		/// <summary>
		/// Delegate contract for an optimization when
		/// converting generic <see cref="ValueType"/>
		/// values to string. The delegate signature matches
		/// that of a "TryParse" method common to many
		/// primitive <see cref="ValueType"/> classes, such as
		/// <see cref="int.TryParse(string,out int)"/> from integers,
		/// <see cref="DateTime.TryParse(string,out DateTime)"/>
		/// from <see cref="DateTime"/>, or
		/// <see cref="Guid.TryParse(string,out Guid)"/>
		/// from <see cref="Guid"/>.
		/// </summary>
		/// <typeparam name="T">
		/// A type of <see cref="ValueType"/> to parse from a string.
		/// </typeparam>
		/// <param name="s">
		/// A string containing a <typeparamref name="T"/> value to convert.
		/// </param>
		/// <param name="result">
		/// When this method returns, contains the <typeparamref name="T"/>
		/// value equivalent of the string contained in <paramref name="s"/>
		/// if the conversion succeeded, or the default value of
		/// <typeparamref name="T"/> if the conversion failed.
		/// This parameter is passed uninitialized; any value originally
		/// supplied in <paramref name="result"/> will be overwritten.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="s"/> was
		/// converted successfully; otherwise, <c>false</c>.
		/// </returns>
		public delegate bool TryParseFromString<T>(string s, out T result) where T : struct;

		/// <summary>
		/// Converts a string to a <see cref="ValueType"/>
		/// of <typeparamref name="T"/>, optionally
		/// suppressing exceptions in the conversion.
		/// </summary>
		/// <typeparam name="T">
		/// A type of <see cref="ValueType"/> with
		/// values to derive from strings.
		/// </typeparam>
		/// <param name="converter">
		/// A <see cref="TypeConverter"/> to convert
		/// strings to values of <typeparamref name="T"/>.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the given <paramref name="converter"/>.
		/// </param>
		/// <param name="s">
		/// A string value to convert to <typeparamref name="T"/>.
		/// </param>
		/// <param name="result">
		/// When this method returns, contains the <typeparamref name="T"/>
		/// value equivalent of the string contained in <paramref name="s"/>,
		/// if the conversion succeeded, or the default value of
		/// <typeparamref name="T"/> if the conversion failed.
		/// This parameter is passed uninitialized; any value originally
		/// supplied in <paramref name="result"/> will be overwritten.
		/// </param>
		/// <returns>
		/// <c>true</c> if <paramref name="s"/> was
		/// converted successfully; otherwise, <c>false</c>.
		/// </returns>
		[System.Diagnostics.Contracts.Pure, JetBrains.Annotations.Pure]
		public static bool TryConvertFromString<T>(
			[NotNull] TypeConverter converter,
			bool throwTypeConversionExceptions,
			[NotNull] string s,
			out T result)
			where T : struct
		{
			Contract.Requires<ArgumentNullException>(converter != null);
			Contract.Requires<ArgumentException>(!IsNullOrWhiteSpace(s));

			// This produces a compiler warning due to CodeContracts issue.
			// See workaround above for https://github.com/Microsoft/CodeContracts/issues/339
			////	Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(s));

			result = default(T);

			try
			{
				object converted = converter.ConvertFrom(s);
				if (converted == null)
				{
					return false;
				}

				result = (T)converted;

				return true;
			}
			catch (FormatException)
			{
				if (throwTypeConversionExceptions)
				{
					throw;
				}

				return false;
			}
			catch (NotSupportedException)
			{
				if (throwTypeConversionExceptions)
				{
					throw;
				}

				return false;
			}
			catch (Exception e)
			{
				if (!throwTypeConversionExceptions)
				{
					return false;
				}

				FormatException formatException = e.InnerException as FormatException;
				if (formatException != null)
				{
					throw formatException;
				}

				NotSupportedException notSupportedException = e.InnerException as NotSupportedException;
				if (notSupportedException != null)
				{
					throw notSupportedException;
				}

				throw;
			}
		}

		#endregion

		#region [ Overloads of Public Static ToArray<T> Method ]

		/// <summary>
		/// Returns a typed array of <see cref="ValueType"/>
		/// values parsed from a collection of strings
		/// using a default <see cref="TypeConverter"/>,
		/// optionally having duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, must
		/// be a <see cref="ValueType"/>.
		/// </typeparam>
		/// <param name="strings">
		/// A collection of strings to be converted
		/// to <typeparamref name="T"/>.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the default <see cref="TypeConverter"/>.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(
			this IEnumerable<string> strings,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates,
			bool throwTypeConversionExceptions = DefaultThrowTypeConversionExceptions)
			where T : struct
		{
			return strings.ToList<T>(
				removeDuplicates, throwTypeConversionExceptions).ToArray();
		}

		/// <summary>
		/// Returns a typed array of <see cref="ValueType"/>
		/// values parsed from a collection of strings
		/// using a given <see cref="TypeConverter"/>,
		/// optionally having duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, must
		/// be a <see cref="ValueType"/>.
		/// </typeparam>
		/// <param name="strings">
		/// A collection of strings to be converted
		/// to <typeparamref name="T"/>.
		/// </param>
		/// <param name="converter">
		/// A <see cref="TypeConverter"/> to convert
		/// strings to values of <typeparamref name="T"/>.
		/// If <c>null</c>, the default converter will be used.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the given <paramref name="converter"/>.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(
			this IEnumerable<string> strings,
			TypeConverter converter,
			bool throwTypeConversionExceptions,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return strings.ToList<T>(
				converter, throwTypeConversionExceptions, removeDuplicates).ToArray();
		}

		/// <summary>
		/// Returns a typed array of <see cref="ValueType"/> values
		/// parsed from a collection of strings using a specified
		/// <see cref="TryParseFromString{T}"/> delegate,
		/// ignoring any values which cannot be parsed
		/// and optionally having duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, must
		/// be a <see cref="ValueType"/>.
		/// </typeparam>
		/// <param name="strings">
		/// A collection of strings to be converted
		/// to <typeparamref name="T"/>.
		/// </param>
		/// <param name="tryParseDelegate">
		/// A delegate method conforming to the
		/// <see cref="TryParseFromString{T}"/>
		/// signature, for optimized type conversion
		/// via a "TryParse" method common to many
		/// primitive <see cref="ValueType"/> classes.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(
			this IEnumerable<string> strings,
			TryParseFromString<T> tryParseDelegate,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return strings.ToList(
				tryParseDelegate, removeDuplicates).ToArray();
		}

		#endregion

		#region [ Overloads of Public Static ToEnumerable<T> Method ]

		/// <summary>
		/// Returns a generic <see cref="IReadOnlyList{T}"/>
		/// of <see cref="ValueType"/> values parsed
		/// from a collection of strings using a default
		/// <see cref="TypeConverter"/>, optionally
		/// having duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, must
		/// be a <see cref="ValueType"/>.
		/// </typeparam>
		/// <param name="strings">
		/// A collection of strings to be converted
		/// to <typeparamref name="T"/>.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the default <see cref="TypeConverter"/>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static IReadOnlyList<T> ToEnumerable<T>(
			this IEnumerable<string> strings,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates,
			bool throwTypeConversionExceptions = DefaultThrowTypeConversionExceptions)
			where T : struct
		{
			return strings.ToList<T>(
				removeDuplicates, throwTypeConversionExceptions);
		}

		/// <summary>
		/// Returns a generic <see cref="IReadOnlyList{T}"/>
		/// of <see cref="ValueType"/> values parsed
		/// from a collection of strings using a given
		/// <see cref="TypeConverter"/>, optionally
		/// having duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, must
		/// be a <see cref="ValueType"/>.
		/// </typeparam>
		/// <param name="strings">
		/// A collection of strings to be converted
		/// to <typeparamref name="T"/>.
		/// </param>
		/// <param name="converter">
		/// A <see cref="TypeConverter"/> to convert
		/// strings to values of <typeparamref name="T"/>.
		/// If <c>null</c>, the default converter will be used.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the given <paramref name="converter"/>.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static IReadOnlyList<T> ToEnumerable<T>(
			this IEnumerable<string> strings,
			TypeConverter converter,
			bool throwTypeConversionExceptions,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return strings.ToList<T>(
				converter, throwTypeConversionExceptions, removeDuplicates);
		}

		/// <summary>
		/// Returns a generic <see cref="IReadOnlyList{T}"/>
		/// of <see cref="ValueType"/> values parsed
		/// from a collection of strings using a specified
		/// <see cref="TryParseFromString{T}"/> delegate,
		/// ignoring any values which cannot be parsed
		/// and optionally having duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, must
		/// be a <see cref="ValueType"/>.
		/// </typeparam>
		/// <param name="strings">
		/// A collection of strings to be converted
		/// to <typeparamref name="T"/>.
		/// </param>
		/// <param name="tryParseDelegate">
		/// A delegate method conforming to the
		/// <see cref="TryParseFromString{T}"/>
		/// signature, for optimized type conversion
		/// via a "TryParse" method common to many
		/// primitive <see cref="ValueType"/> classes.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static IReadOnlyList<T> ToEnumerable<T>(
			this IEnumerable<string> strings,
			TryParseFromString<T> tryParseDelegate,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			return strings.ToList(
				tryParseDelegate, removeDuplicates);
		}

		#endregion

		#region [ Public Static ToList<T> Method Overloads, invoked by other Static Methods ]

		/// <summary>
		/// Returns a generic <see cref="List{T}"/>
		/// of <see cref="ValueType"/> values parsed
		/// from a collection of strings using a default
		/// <see cref="TypeConverter"/>, optionally
		/// having duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, must
		/// be a <see cref="ValueType"/>.
		/// </typeparam>
		/// <param name="strings">
		/// A collection of strings to be converted
		/// to <typeparamref name="T"/>.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the default <see cref="TypeConverter"/>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static List<T> ToList<T>(
			this IEnumerable<string> strings,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates,
			bool throwTypeConversionExceptions = DefaultThrowTypeConversionExceptions)
			where T : struct
		{
			return strings.ToList<T>(null, throwTypeConversionExceptions, removeDuplicates);
		}

		/// <summary>
		/// Returns a generic <see cref="List{T}"/>
		/// of <see cref="ValueType"/> values parsed
		/// from a collection of strings using a given
		/// <see cref="TypeConverter"/>, optionally
		/// having duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, must
		/// be a <see cref="ValueType"/>.
		/// </typeparam>
		/// <param name="strings">
		/// A collection of strings to be converted
		/// to <typeparamref name="T"/>.
		/// </param>
		/// <param name="converter">
		/// A <see cref="TypeConverter"/> to convert
		/// strings to values of <typeparamref name="T"/>.
		/// If <c>null</c>, the default converter will be used.
		/// </param>
		/// <param name="throwTypeConversionExceptions">
		/// Whether to throw any <see cref="FormatException"/>
		/// or <see cref="NotSupportedException"/> encountered
		/// during <see cref="TypeConverter.ConvertFrom(object)"/>
		/// using the given <paramref name="converter"/>.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static List<T> ToList<T>(
			this IEnumerable<string> strings,
			TypeConverter converter,
			bool throwTypeConversionExceptions,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			List<T> list = new List<T>();
			if (strings == null)
			{
				return list;
			}

			if (converter == null)
			{
				Type t = typeof(T);
				converter = TypeDescriptor.GetConverter(t);

				// Underlying code contracts for TypeDescriptor.GetConverter()
				// ensure that this value will never be null.
				////	if (converter == null)
				////	{
				////		throw new NullReferenceException(string.Format(
				////			"No TypeConverter could be resolved for the \"{0}\" type.", t.Name));
				////	}
			}

			// Do not try to parse strings that are null or white space.
			foreach (string s in strings.Where(value => !string.IsNullOrWhiteSpace(value)))
			{
				T result;
				if (!TryConvertFromString(converter, throwTypeConversionExceptions, s, out result))
				{
					continue;
				}

				if (removeDuplicates && list.Contains(result))
				{
					continue;
				}

				list.Add(result);
			}

			return list;
		}

		/// <summary>
		/// Returns a generic <see cref="List{T}"/>
		/// of <see cref="ValueType"/> values parsed
		/// from a collection of strings using a specified
		/// <see cref="TryParseFromString{T}"/> delegate,
		/// ignoring any values which cannot be parsed
		/// and optionally having duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, must
		/// be a <see cref="ValueType"/>.
		/// </typeparam>
		/// <param name="strings">
		/// A collection of strings to be converted
		/// to <typeparamref name="T"/>.
		/// </param>
		/// <param name="tryParseDelegate">
		/// A delegate method conforming to the
		/// <see cref="TryParseFromString{T}"/>
		/// signature, for optimized type conversion
		/// via a "TryParse" method common to many
		/// primitive <see cref="ValueType"/> classes.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <returns>
		/// A list of type <typeparamref name="T"/>.
		/// </returns>
		public static List<T> ToList<T>(
			this IEnumerable<string> strings,
			TryParseFromString<T> tryParseDelegate,
			bool removeDuplicates = ConvertStructCollection.DefaultRemoveDuplicates)
			where T : struct
		{
			if (tryParseDelegate == null)
			{
				return strings.ToList<T>(null, false, removeDuplicates);
			}

			List<T> list = new List<T>();
			if (strings == null)
			{
				return list;
			}

			// Do not try to parse strings that are null or white space.
			foreach (string s in strings.Where(value => !string.IsNullOrWhiteSpace(value)))
			{
				T result;
				if (!tryParseDelegate(s, out result))
				{
					continue;
				}

				if (removeDuplicates && list.Contains(result))
				{
					continue;
				}

				list.Add(result);
			}

			return list;
		}

		#endregion
	}
}