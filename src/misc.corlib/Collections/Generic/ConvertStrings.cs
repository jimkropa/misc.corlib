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
	/// into typed collections of <see cref="IComparable"/> or delimited strings,
	/// with options for sorting and removing duplicate entries.
	/// </summary>
	[CLSCompliant(true)]
	public static class ConvertStrings
	{
		/// <summary>
		/// 
		/// </summary>
		public const bool DefaultThrowTypeConversionExceptions = false;

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="s">
		/// A string containing a <typeparamref name="T"/> value to convert.
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
		public delegate bool TryParseFromString<T>(string s, out T result) where T : struct;

		#region [ Overloads of Public Static ToArray<T> Method ]

		/// <summary>
		/// Returns a typed array from a collection of strings,
		/// sorted and with duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="strings">
		/// A collection of strings which may be converted to the
		/// <see cref="IComparable"/> type <typeparamref name="T"/>.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>,
		/// sorted and with duplicates removed.
		/// </returns>
		public static T[] ToArray<T>(
			this IEnumerable<string> strings)
			where T : struct
		{
			return ToArray<T>(strings, ConvertValueTypeCollection.DefaultRemoveDuplicates);
		}

		/// <summary>
		/// Returns a typed array from a collection of strings,
		/// optionally sorting the array and removing duplicates.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="strings">
		/// A collection of strings which may be converted to the
		/// <see cref="IComparable"/> type <typeparamref name="T"/>.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to keep duplicate entries, or to
		/// remove duplicates from the array returned.
		/// </param>
		/// <returns>
		/// An array of type <typeparamref name="T"/>.
		/// </returns>
		public static T[] ToArray<T>(
			this IEnumerable<string> strings, bool removeDuplicates)
			where T : struct
		{
			return ToList<T>(strings, removeDuplicates).ToArray();
		}

		#endregion

		#region [ Overloads of Public Static ToEnumerable<T> Method ]

		/// <summary>
		/// Returns a typed collection from a collection of strings,
		/// sorted and with duplicates removed.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="values">
		/// A collection of strings which may be converted to the
		/// <see cref="IComparable"/> type <typeparamref name="T"/>.
		/// </param>
		/// <returns>
		/// A collection of type <typeparamref name="T"/>,
		/// sorted and with duplicates removed.
		/// </returns>
		public static IReadOnlyList<T> ToEnumerable<T>(
			this IEnumerable<string> values)
			where T : struct
		{
			return ToEnumerable<T>(values, ConvertValueTypeCollection.DefaultRemoveDuplicates);
		}

		/// <summary>
		/// Returns a typed collection from a collection of strings,
		/// optionally sorting the array and removing duplicates.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="values">
		/// A collection of strings which may be converted to the
		/// <see cref="IComparable"/> type <typeparamref name="T"/>.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to keep duplicate entries, or to
		/// remove duplicates from the collection returned.
		/// </param>
		/// <returns>
		/// An collection of type <typeparamref name="T"/>.
		/// </returns>
		public static IReadOnlyList<T> ToEnumerable<T>(
			this IEnumerable<string> values,
			bool removeDuplicates)
			where T : struct
		{
			List<T> list = ToList<T>(values, removeDuplicates);

			return list;
		}

		#endregion

		#region [ Public Static ToList<T> Method, invoked by other Static Methods ]

		/// <summary>
		/// Returns a typed list from a collection of strings,
		/// optionally sorting the list and removing duplicates.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="strings">
		/// A collection of strings which may be converted to the
		/// <see cref="ValueType"/> <typeparamref name="T"/>.
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
			bool removeDuplicates = ConvertValueTypeCollection.DefaultRemoveDuplicates,
			bool throwTypeConversionExceptions = DefaultThrowTypeConversionExceptions)
			where T : struct
		{
			return strings.ToList<T>(null, throwTypeConversionExceptions, removeDuplicates);
		}

		/// <summary>
		/// Returns a typed list from a collection of strings,
		/// optionally sorting the list and removing duplicates.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="strings">
		/// A collection of strings which may be converted to the
		/// <see cref="ValueType"/> <typeparamref name="T"/>.
		/// </param>
		/// <param name="tryParseDelegate">
		/// A delegate method conforming to the
		/// <see cref="TryParseFromString{T}"/>
		/// signature, for optimized type coversion
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
			bool removeDuplicates = ConvertValueTypeCollection.DefaultRemoveDuplicates)
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

		/// <summary>
		/// Returns a typed list from a collection of strings,
		/// optionally sorting the list and removing duplicates.
		/// </summary>
		/// <typeparam name="T">
		/// The type of array to return, which must
		/// implement <see cref="IComparable"/>.
		/// </typeparam>
		/// <param name="strings">
		/// A collection of strings which may be converted to the
		/// <see cref="ValueType"/> <typeparamref name="T"/>.
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
			bool removeDuplicates = ConvertValueTypeCollection.DefaultRemoveDuplicates)
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
		public static bool TryConvertFromString<T>(
			[NotNull] TypeConverter converter,
			bool throwTypeConversionExceptions,
			string s,
			out T result)
			where T : struct
		{
			Contract.Requires<ArgumentNullException>(converter != null);
			Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(s));

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
		}

		#endregion
	}
}