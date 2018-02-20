using System;
using System.Collections.Generic;
using System.Linq;

namespace MiscCorLib.Collections.Generic
{
	/// <summary>
	/// A set of static extension methods for converting generic
	/// collections of <see cref="ValueType" /> (a.k.a. "struct" values)
	/// into string arrays and delimited strings,
	/// optionally removing duplicate entries.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For reverse effect, use static extension
	/// methods in <see cref="ConvertStrings" />
	/// and <see cref="ConvertDelimitedString" />.
	/// </para>
	/// </remarks>
	public static class ConvertStructCollection
	{
		/// <summary>
		/// A value to use when translating between collections of strings
		/// and generic collections of <see cref="ValueType" />,
		/// indicating to preserve duplicate values
		/// in the resulting collection by default.
		/// </summary>
		public const bool DefaultRemoveDuplicates = false;

		#region [ Overloads of ToStringArray Extension Method ]

		/// <summary>
		/// Converts a generic collection of <see cref="ValueType" />
		/// to a one-dimensional array of <see cref="string" />,
		/// by calling the <see cref="ValueType.ToString" />
		/// method on each item in the source collection,
		/// and optionally removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of <see cref="ValueType" /> items.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of <paramref name="collection" />,
		/// derived from <see cref="ValueType" />.
		/// </typeparam>
		/// <returns>
		/// An array of strings with null or empty values omitted,
		/// and duplicates omitted unless the
		/// <paramref name="removeDuplicates" />
		/// value is <c>true</c>.
		/// </returns>
		public static string[] ToStringArray<T>(
			this IEnumerable<T> collection,
			bool removeDuplicates = DefaultRemoveDuplicates)
			where T : struct, IEquatable<T>
		{
			return collection.ToStringArray(null, removeDuplicates);
		}

		/// <summary>
		/// Converts a generic collection of <see cref="ValueType" />
		/// to a one-dimensional array of <see cref="string" />,
		/// by calling the <paramref name="toStringMethod" />
		/// delegate on each item in the source collection,
		/// and optionally removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of <see cref="ValueType" /> items.
		/// </param>
		/// <param name="toStringMethod">
		/// A delegate function which accepts a <typeparamref name="T" />
		/// value and returns a string. Pass this parameter if a custom
		/// format string is needed. If not specified, uses the default
		/// <see cref="ValueType.ToString" /> method.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of <paramref name="collection" />,
		/// derived from <see cref="ValueType" />.
		/// </typeparam>
		/// <returns>
		/// An array of strings created by the <paramref name="toStringMethod" />,
		/// with null or empty values omitted, and duplicates omitted unless
		/// the <paramref name="removeDuplicates" /> value is <c>true</c>.
		/// </returns>
		public static string[] ToStringArray<T>(
			this IEnumerable<T> collection,
			Func<T, string> toStringMethod,
			bool removeDuplicates = DefaultRemoveDuplicates)
			where T : struct, IEquatable<T>
		{
			return collection.ToStrings(
				toStringMethod == null
					? (item => item.ToString())
					: toStringMethod, removeDuplicates).ToArray();
		}

		#endregion

		#region [ Overloads of ToDelimitedString Extension Method ]

		/// <summary>
		/// Converts a generic collection of <see cref="ValueType" />
		/// to a comma-delimited <see cref="string" />,
		/// by calling the <see cref="ValueType.ToString()" />
		/// method on each item in the source collection,
		/// and optionally removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of <see cref="ValueType" /> items.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of <paramref name="collection" />,
		/// derived from <see cref="ValueType" />.
		/// </typeparam>
		/// <returns>
		/// A comma-delimited string.
		/// </returns>
		public static string ToDelimitedString<T>(
			this IEnumerable<T> collection,
			bool removeDuplicates = DefaultRemoveDuplicates)
			where T : struct, IEquatable<T>
		{
			return collection.ToDelimitedString(
				ConvertDelimitedString.DefaultSeparator, removeDuplicates);
		}

		/// <summary>
		/// Converts a generic collection of <see cref="ValueType" />
		/// to a comma-delimited <see cref="string" />,
		/// by calling the <paramref name="toStringMethod" />
		/// delegate on each item in the source collection,
		/// and optionally removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of <see cref="ValueType" /> items.
		/// </param>
		/// <param name="toStringMethod">
		/// A delegate function which accepts a <typeparamref name="T" />
		/// value and returns a string. Pass this parameter if a custom
		/// format string is needed. If not specified, uses the default
		/// <see cref="ValueType.ToString" /> method.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of <paramref name="collection" />,
		/// derived from <see cref="ValueType" />.
		/// </typeparam>
		/// <returns>
		/// A comma-delimited string.
		/// </returns>
		public static string ToDelimitedString<T>(
			this IEnumerable<T> collection,
			Func<T, string> toStringMethod,
			bool removeDuplicates = DefaultRemoveDuplicates)
			where T : struct, IEquatable<T>
		{
			return collection.ToDelimitedString(
				toStringMethod, ConvertDelimitedString.DefaultSeparator, removeDuplicates);
		}

		/// <summary>
		/// Converts a generic collection of <see cref="ValueType" />
		/// to a <see cref="string" /> delimited by the value
		/// of the <paramref name="separator" /> parameter,
		/// by calling the <see cref="ValueType.ToString()" />
		/// method on each item in the source collection,
		/// and removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of <see cref="ValueType" /> items.
		/// </param>
		/// <param name="separator">
		/// A string delimiter to use instead of a comma.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of <paramref name="collection" />,
		/// derived from <see cref="ValueType" />.
		/// </typeparam>
		/// <returns>
		/// A <paramref name="separator" />-delimited string.
		/// </returns>
		public static string ToDelimitedString<T>(
			this IEnumerable<T> collection,
			string separator,
			bool removeDuplicates = DefaultRemoveDuplicates)
			where T : struct, IEquatable<T>
		{
			if (string.IsNullOrWhiteSpace(separator))
			{
				throw new ArgumentException("A non-empty string is required.", nameof(separator));
			}

			return collection.ToDelimitedString(
				null, separator, removeDuplicates);
		}

		/// <summary>
		/// Converts a generic collection of <see cref="ValueType" />
		/// to a <see cref="string" /> delimited by the value
		/// of the <paramref name="separator" /> parameter,
		/// by calling the <paramref name="toStringMethod" />
		/// delegate on each item in the source collection,
		/// and optionally removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of <see cref="ValueType" /> items.
		/// </param>
		/// <param name="toStringMethod">
		/// A delegate function which accepts a <typeparamref name="T" />
		/// value and returns a string. Pass this parameter if a custom
		/// format string is needed. If not specified, uses the default
		/// <see cref="ValueType.ToString" /> method.
		/// </param>
		/// <param name="separator">
		/// A string delimiter to use instead of a comma.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of <paramref name="collection" />,
		/// derived from <see cref="ValueType" />.
		/// </typeparam>
		/// <returns>
		/// A <paramref name="separator" />-delimited string.
		/// </returns>
		public static string ToDelimitedString<T>(
			this IEnumerable<T> collection,
			Func<T, string> toStringMethod,
			string separator,
			bool removeDuplicates = DefaultRemoveDuplicates)
			where T : struct, IEquatable<T>
		{
			if (string.IsNullOrWhiteSpace(separator))
			{
				throw new ArgumentException("A non-empty string is required.", nameof(separator));
			}

			return string.Join(
				separator, collection.ToStrings(
					toStringMethod == null
						? (item => item.ToString())
						: toStringMethod, removeDuplicates));
		}

		#endregion

		/// <summary>
		/// Converts a generic collection of <see cref="ValueType" />
		/// to a one-dimensional array of <see cref="string" />,
		/// by calling the <paramref name="toStringMethod" />
		/// delegate on each item in the source collection,
		/// and optionally removing any duplicates.
		/// </summary>
		/// <param name="collection">
		/// A generic collection of <see cref="ValueType" /> items.
		/// </param>
		/// <param name="toStringMethod">
		/// A delegate function which accepts a <typeparamref name="T" />
		/// value and returns a string. Pass this parameter if a custom
		/// format string is needed. If not specified, uses the default
		/// <see cref="ValueType.ToString" /> method.
		/// </param>
		/// <param name="removeDuplicates">
		/// Whether to remove duplicate items in the array returned.
		/// Parameter is optional, default value is <c>false</c>.
		/// </param>
		/// <typeparam name="T">
		/// The generic type of <paramref name="collection" />,
		/// derived from <see cref="ValueType" />.
		/// </typeparam>
		/// <returns>
		/// An array of strings created by the <paramref name="toStringMethod" />,
		/// with null or empty values omitted, and duplicates omitted unless
		/// the <paramref name="removeDuplicates" /> value is <c>true</c>.
		/// </returns>
		private static IEnumerable<string> ToStrings<T>(
			this IEnumerable<T> collection,
			Func<T, string> toStringMethod,
			bool removeDuplicates)
			where T : struct, IEquatable<T>
		{
			if (toStringMethod == null)
			{
				throw new ArgumentNullException(nameof(toStringMethod));
			}

			if (collection == null)
			{
				return new string[0];
			}

			return (removeDuplicates
				// To remove duplicates, it's more
				// accurate and efficient to prefer
				// ValueType EqualityComparer over
				// comparing converted strings.
				? collection.Distinct()
				: collection).Select(
					item => toStringMethod(item)
				).Where(
					// Don't bother to return empty,
					// meaningless strings...
					s => !string.IsNullOrWhiteSpace(s));
		}
	}
}