namespace MiscCorLib
{
	using System;
	using Globalization;
	using TimeZoneInfo = Globalization.TimeZoneInfo;

	/// <summary>
	/// A set of static methods for converting a UTC
	/// date and time into a time-zone-sensitive string.
	/// Also contains extension methods for formatting
	/// any DateTime (regardless of the timezone)
	/// and readonly fields (effectively constants)
	/// for maximum and minimum allowed values for
	/// <see cref="System.Data.SqlDbType.SmallDateTime"/>
	/// values in SQL server, for parameters of
	/// <see cref="System.Data.SqlDbType"/>.
	/// </summary>
	[CLSCompliant(true)]
	public static class FormatDateTime
	{
		/// <summary>
		/// Static readonly field (effectively a constant)
		/// with the minimum allowed value of a
		/// <see cref="System.Data.SqlDbType.SmallDateTime"/>
		/// value in SQL server, for parameters of
		/// <see cref="System.Data.SqlDbType"/>.
		/// </summary>
		public static readonly DateTime SqlSmallDateTimeMaxValue = new DateTime(2079, 6, 6, 23, 59, 59, DateTimeKind.Utc);

		/// <summary>
		/// Static readonly field (effectively a constant)
		/// with the minimum allowed value of a
		/// <see cref="System.Data.SqlDbType.SmallDateTime"/>
		/// value in SQL server, for parameters of
		/// <see cref="System.Data.SqlDbType"/>.
		/// </summary>
		public static readonly DateTime SqlSmallDateTimeMinValue = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// Throws an <see cref="ArgumentOutOfRangeException"/>
		/// if the <paramref name="date"/> value is less than
		/// <see cref="SqlSmallDateTimeMinValue"/> or greater
		/// than <see cref="SqlSmallDateTimeMaxValue"/>.
		/// </summary>
		/// <param name="date">
		/// A <see cref="DateTime"/> value to check.
		/// </param>
		public static void ValidateSqlSmallDateTime(this DateTime date)
		{
			if ((date < SqlSmallDateTimeMinValue) || (date > SqlSmallDateTimeMaxValue))
			{
				throw new ArgumentOutOfRangeException(
					"date",
					date,
					"The value of a date parameter is outside the range allowed for storage in a SQL Server database.");
			}
		}

		/// <summary>
		/// Extends <see cref="System.DateTime"/> to output
		/// a valid RFC-822 string.  Always converts to GMT.
		/// This does NOT address timezone offsetting.
		/// </summary>
		/// <remarks>
		/// This method actually returns RFC-1123, which is
		/// itself valid RFC-822.
		/// see "(http://www.hackcraft.net/web/datetime/#rfc822)"
		/// This method would be useful where a valid RFC-822
		/// DateTime is required, such as RSS
		/// </remarks>
		/// <param name="dateTime">
		/// This is an extension method so when calling it,
		/// this becomes a parameterless method.
		/// </param>
		/// <returns>
		/// An RFC-822-formatted string for a DateTime.
		/// </returns>
		public static string ToRFC822String(this DateTime dateTime)
		{
			return ToRFC1123String(dateTime);
		}

		/// <summary>
		/// Extends <see cref="System.DateTime"/> to output
		/// a valid RFC-1123 string.  Always converts to GMT.
		/// This does NOT address timezone offsetting.
		/// </summary>
		/// <remarks>
		/// This method returns a RFC-1123 formatted datetime.
		/// see "(http://www.hackcraft.net/web/datetime/#rfc822)"
		/// This method would be useful where a valid RFC-822
		/// DateTime is required, such as RSS
		/// </remarks>
		/// <param name="dateTime">
		/// This is an extension method so when calling it,
		/// this becomes a parameterless method.
		/// </param>
		/// <returns>
		/// An RFC-1123-formatted string for a DateTime.
		/// </returns>
		public static string ToRFC1123String(this DateTime dateTime)
		{
			return dateTime.ToString("r");
		}

		/// <summary>
		/// Formats a date to a string, using the
		/// <see cref="Globalization.TimeZoneInfo.DefaultTimeZoneId"/>.
		/// </summary>
		/// <param name="date">
		/// A date and time to format.
		/// </param>
		/// <returns>
		/// The <see cref="string"/> representing the date.
		/// </returns>
		public static string ToString(DateTime date)
		{
			return ToString(date, false);
		}

		/// <summary>
		/// Formats a date to a string, using the
		/// <see cref="Globalization.TimeZoneInfo.DefaultTimeZoneId"/>.
		/// </summary>
		/// <param name="date">
		/// A date and time to format.
		/// </param>
		/// <param name="formatDateTimeToShortString">
		/// A boolean value indicating if the time format string should exclude
		/// GMT timezone info.
		/// </param>
		/// <returns>
		/// The <see cref="string"/> representing the date.
		/// </returns>
		public static string ToString(DateTime date, bool formatDateTimeToShortString)
		{
			if (date.Kind == DateTimeKind.Utc)
			{
				return ToString(TimeZoneInfo.DefaultTimeZoneId, date, formatDateTimeToShortString);
			}

			return ToString(
				TimeZoneInfo.DefaultTimeZoneId,
				new DateTime(date.Ticks, DateTimeKind.Utc),
				formatDateTimeToShortString);
		}

		/// <summary>
		/// Formats a date to a string, using the
		/// <see cref="Globalization.TimeZoneInfo.DefaultTimeZoneId"/>.
		/// </summary>
		/// <param name="date">
		/// A date and time to format.
		/// </param>
		/// <param name="otherMonthFormat">
		/// A format string to use for dates in a different month from today.
		/// </param>
		/// <returns>
		/// The <see cref="string"/> representing the date.
		/// </returns>
		public static string ToString(DateTime date, string otherMonthFormat)
		{
			return ToString(date, otherMonthFormat, false);
		}

		/// <summary>
		/// Formats a date to a string, using the
		/// <see cref="Globalization.TimeZoneInfo.DefaultTimeZoneId"/>.
		/// </summary>
		/// <param name="date">
		/// A date and time to format.
		/// </param>
		/// <param name="otherMonthFormat">
		/// A format string to use for dates in a different month from today.
		/// </param>
		/// <param name="formatDateTimeToShortString">
		/// A boolean value indicating if the time format string should exclude
		/// GMT timezone info.
		/// </param>
		/// <returns>
		/// The <see cref="string"/> representing the date.
		/// </returns>
		public static string ToString(
			DateTime date,
			string otherMonthFormat,
			bool formatDateTimeToShortString)
		{
			if (date.Kind == DateTimeKind.Utc)
			{
				return ToString(TimeZoneInfo.DefaultTimeZoneId, date, otherMonthFormat, formatDateTimeToShortString);
			}

			return ToString(
				TimeZoneInfo.DefaultTimeZoneId,
				new DateTime(date.Ticks, DateTimeKind.Utc),
				otherMonthFormat,
				formatDateTimeToShortString);
		}

		/// <summary>
		/// Formats a date to a string, using the
		/// <see cref="Globalization.TimeZoneInfo.DefaultTimeZoneId"/>.
		/// </summary>
		/// <param name="date">
		/// A date and time to format.
		/// </param>
		/// <param name="otherYearFormat">
		/// A format string to use for dates in a different year from today.
		/// </param>
		/// <param name="otherMonthFormat">
		/// A format string to use for dates in a different month and year from today.
		/// </param>
		/// <param name="sameMonthFormat">
		/// A format string to use for dates in the same month and year as today.
		/// </param>
		/// <returns>
		/// The <see cref="string"/> representing the date.
		/// </returns>
		public static string ToString(
			DateTime date,
			string otherYearFormat,
			string otherMonthFormat,
			string sameMonthFormat)
		{
			return ToString(date, otherYearFormat, otherMonthFormat, sameMonthFormat, false);
		}

		/// <summary>
		/// Formats a date to a string, using the
		/// <see cref="Globalization.TimeZoneInfo.DefaultTimeZoneId"/>.
		/// </summary>
		/// <param name="date">
		/// A date and time to format.
		/// </param>
		/// <param name="otherYearFormat">
		/// A format string to use for dates in a different year from today.
		/// </param>
		/// <param name="otherMonthFormat">
		/// A format string to use for dates in a different month and year from today.
		/// </param>
		/// <param name="sameMonthFormat">
		/// A format string to use for dates in the same month and year as today.
		/// </param>
		/// <param name="formatDateTimeToShortString">
		/// A boolean value indicating if the time format string should exclude
		/// GMT timezone info.
		/// </param>
		/// <returns>
		/// The <see cref="string"/> representing the date.
		/// </returns>
		public static string ToString(
			DateTime date,
			string otherYearFormat,
			string otherMonthFormat,
			string sameMonthFormat,
			bool formatDateTimeToShortString)
		{
			if (date.Kind == DateTimeKind.Utc)
			{
				return ToString(
					TimeZoneInfo.DefaultTimeZoneId,
					date,
					otherYearFormat,
					otherMonthFormat,
					sameMonthFormat,
					formatDateTimeToShortString);
			}

			return ToString(
				TimeZoneInfo.DefaultTimeZoneId,
				new DateTime(date.Ticks, DateTimeKind.Utc),
				otherYearFormat,
				otherMonthFormat,
				sameMonthFormat,
				formatDateTimeToShortString);
		}

		/// <summary>
		/// Returns a formatted string for a local time which corresponds
		/// to a specified coordinated universal time (UTC).
		/// </summary>
		/// <param name="timeZoneId">
		/// An identifier for the <see cref="Globalization.TimeZoneInfo"/> instance
		/// to use for translating the UTC <paramref name="utcDateTime"/>
		/// into a local date and time.
		/// </param>
		/// <param name="utcDateTime">
		/// A date and time expressed in UTC.
		/// </param>
		/// <returns>
		/// A formatted string representing the local date and time
		/// which corresponds to the UTC <paramref name="utcDateTime"/>.
		/// </returns>
		public static string ToString(TimeZoneEnum timeZoneId, DateTime utcDateTime)
		{
			return ToString(timeZoneId, utcDateTime, false);
		}

		/// <summary>
		/// Returns a formatted string for a local time which corresponds
		/// to a specified coordinated universal time (UTC), with an option to
		/// include timezone info.
		/// </summary>
		/// <param name="timeZoneId">
		/// An identifier for the <see cref="Globalization.TimeZoneInfo"/> instance
		/// to use for translating the UTC <paramref name="utcDateTime"/>
		/// into a local date and time.
		/// </param>
		/// <param name="utcDateTime">
		/// A date and time expressed in UTC.
		/// </param>
		/// <param name="formatDateTimeToShortString">
		/// A boolean value indicating if the time format string should exclude
		/// GMT timezone info.
		/// </param>
		/// <returns>
		/// A formatted string representing the local date and time
		/// which corresponds to the UTC <paramref name="utcDateTime"/>.
		/// </returns>
		public static string ToString(
			TimeZoneEnum timeZoneId,
			DateTime utcDateTime,
			bool formatDateTimeToShortString)
		{
			// Fetch a TimeZoneInfo instance.
			TimeZoneInfo zone = TimeZoneInfo.GetTimeZoneInfo(timeZoneId);

			// Use the TimeZoneInfo to get the formatted date.
			if (formatDateTimeToShortString)
			{
				return zone.FormatToShortLocalTime(utcDateTime);
			}

			return zone.FormatToLocalTime(utcDateTime);
		}

		/// <summary>
		/// Returns a formatted string for a local time which corresponds
		/// to a specified coordinated universal time (UTC).
		/// </summary>
		/// <param name="timeZoneId">
		/// An identifier for the <see cref="Globalization.TimeZoneInfo"/> instance
		/// to use for translating the UTC <paramref name="utcDateTime"/>
		/// into a local date and time.
		/// </param>
		/// <param name="utcDateTime">
		/// A date and time expressed in UTC.
		/// </param>
		/// <param name="otherMonthFormat">
		/// A format string to use for dates in a different month from today.
		/// </param>
		/// <returns>
		/// A formatted string representing the local date and time
		/// which corresponds to the UTC <paramref name="utcDateTime"/>.
		/// </returns>
		public static string ToString(
			TimeZoneEnum timeZoneId,
			DateTime utcDateTime,
			string otherMonthFormat)
		{
			return ToString(timeZoneId, utcDateTime, otherMonthFormat, false);
		}

		/// <summary>
		/// Returns a formatted string for a local time which corresponds
		/// to a specified coordinated universal time (UTC).
		/// </summary>
		/// <param name="timeZoneId">
		/// An identifier for the <see cref="Globalization.TimeZoneInfo"/> instance
		/// to use for translating the UTC <paramref name="utcDateTime"/>
		/// into a local date and time.
		/// </param>
		/// <param name="utcDateTime">
		/// A date and time expressed in UTC.
		/// </param>
		/// <param name="otherMonthFormat">
		/// A format string to use for dates in a different month from today.
		/// </param>
		/// <param name="formatDateTimeToShortString">
		/// A boolean value indicating if the time format string should exclude
		/// GMT timezone info.
		/// </param>
		/// <returns>
		/// A formatted string representing the local date and time
		/// which corresponds to the UTC <paramref name="utcDateTime"/>.
		/// </returns>
		public static string ToString(
			TimeZoneEnum timeZoneId,
			DateTime utcDateTime,
			string otherMonthFormat,
			bool formatDateTimeToShortString)
		{
			// Fetch a TimeZoneInfo instance.
			TimeZoneInfo zone = TimeZoneInfo.GetTimeZoneInfo(timeZoneId);

			// Use the TimeZoneInfo to get the formatted date.
			if (formatDateTimeToShortString)
			{
				return zone.FormatToShortLocalTime(utcDateTime, otherMonthFormat);
			}

			return zone.FormatToLocalTime(utcDateTime, otherMonthFormat);
		}

		/// <summary>
		/// Returns a formatted string for a local time which corresponds
		/// to a specified coordinated universal time (UTC).
		/// </summary>
		/// <param name="timeZoneId">
		/// An identifier for the <see cref="Globalization.TimeZoneInfo"/> instance
		/// to use for translating the UTC <paramref name="utcDateTime"/>
		/// into a local date and time.
		/// </param>
		/// <param name="utcDateTime">
		/// A date and time expressed in UTC.
		/// </param>
		/// <param name="otherYearFormat">
		/// A format string to use for dates in a different year from today.
		/// </param>
		/// <param name="otherMonthFormat">
		/// A format string to use for dates in a different month and year from today.
		/// </param>
		/// <param name="sameMonthFormat">
		/// A format string to use for dates in the same month and year as today.
		/// </param>
		/// <returns>
		/// A formatted string representing the local date and time
		/// which corresponds to the UTC <paramref name="utcDateTime"/>.
		/// </returns>
		public static string ToString(
			TimeZoneEnum timeZoneId,
			DateTime utcDateTime,
			string otherYearFormat,
			string otherMonthFormat,
			string sameMonthFormat)
		{
			return ToString(
				timeZoneId,
				utcDateTime,
				otherYearFormat,
				otherMonthFormat,
				sameMonthFormat,
				false);
		}

		/// <summary>
		/// Returns a formatted string for a local time which corresponds
		/// to a specified coordinated universal time (UTC).
		/// </summary>
		/// <param name="timeZoneId">
		/// An identifier for the <see cref="Globalization.TimeZoneInfo"/> instance
		/// to use for translating the UTC <paramref name="utcDateTime"/>
		/// into a local date and time.
		/// </param>
		/// <param name="utcDateTime">
		/// A date and time expressed in UTC.
		/// </param>
		/// <param name="otherYearFormat">
		/// A format string to use for dates in a different year from today.
		/// </param>
		/// <param name="otherMonthFormat">
		/// A format string to use for dates in a different month and year from today.
		/// </param>
		/// <param name="sameMonthFormat">
		/// A format string to use for dates in the same month and year as today.
		/// </param>
		/// <param name="formatDateTimeToShortString">
		/// A boolean value indicating whether the formatted string should contain
		/// timezone and user Info.
		/// </param>
		/// <returns>
		/// A formatted string representing the local date and time
		/// which corresponds to the UTC <paramref name="utcDateTime"/>.
		/// </returns>
		public static string ToString(
			TimeZoneEnum timeZoneId,
			DateTime utcDateTime,
			string otherYearFormat,
			string otherMonthFormat,
			string sameMonthFormat,
			bool formatDateTimeToShortString)
		{
			// Fetch a TimeZoneInfo instance.
			TimeZoneInfo zone = TimeZoneInfo.GetTimeZoneInfo(timeZoneId);

			if (formatDateTimeToShortString)
			{
				return zone.FormatToShortLocalTime(utcDateTime, otherYearFormat, otherMonthFormat, sameMonthFormat);
			}

			// Use the TimeZoneInfo to get the formatted date.
			return zone.FormatToLocalTime(utcDateTime, otherYearFormat, otherMonthFormat, sameMonthFormat);
		}

		/// <summary>
		/// Converts a <see cref="DateTime"/> instance to universal time
		/// using the <see cref="DateTime.ToUniversalTime"/> method,
		/// or returns <see cref="DateTime.MinValue"/> if the original
		/// <paramref name="dateTime"/> value is the minimum value.
		/// </summary>
		/// <param name="dateTime">
		/// An instance of <see cref="DateTime"/>.
		/// </param>
		/// <returns>
		/// An instance of <see cref="DateTime"/> which is the
		/// universal time from the original <paramref name="dateTime"/>
		/// value or <see cref="DateTime.MinValue"/>.
		/// </returns>
		public static DateTime ToUniversalTimeOrMinValue(this DateTime dateTime)
		{
			if (dateTime.Equals(DateTime.MinValue))
			{
				return DateTime.MinValue;
			}

			return dateTime.ToUniversalTime();
		}
	}
}