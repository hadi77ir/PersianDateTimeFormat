using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
// Persian DateTime
namespace PersianDateTimeFormat
{
/*
     Customized format patterns:
     P.S. Format in the table below is the internal number format used to display the pattern.
     Patterns   Format      Description                           Example
     =========  ==========  ===================================== ========
        "h"     "0"         hour (12-hour clock)w/o leading zero  3
        "hh"    "00"        hour (12-hour clock)with leading zero 03
        "hh*"   "00"        hour (12-hour clock)with leading zero 03
        "H"     "0"         hour (24-hour clock)w/o leading zero  8
        "HH"    "00"        hour (24-hour clock)with leading zero 08
        "HH*"   "00"        hour (24-hour clock)                  08
        "m"     "0"         minute w/o leading zero
        "mm"    "00"        minute with leading zero
        "mm*"   "00"        minute with leading zero
        "s"     "0"         second w/o leading zero
        "ss"    "00"        second with leading zero
        "ss*"   "00"        second with leading zero
        "f"     "0"         second fraction (1 digit)
        "ff"    "00"        second fraction (2 digit)
        "fff"   "000"       second fraction (3 digit)
        "ffff"  "0000"      second fraction (4 digit)
        "fffff" "00000"         second fraction (5 digit)
        "ffffff"    "000000"    second fraction (6 digit)
        "fffffff"   "0000000"   second fraction (7 digit)
        "F"     "0"         second fraction (up to 1 digit)
        "FF"    "00"        second fraction (up to 2 digit)
        "FFF"   "000"       second fraction (up to 3 digit)
        "FFFF"  "0000"      second fraction (up to 4 digit)
        "FFFFF" "00000"         second fraction (up to 5 digit)
        "FFFFFF"    "000000"    second fraction (up to 6 digit)
        "FFFFFFF"   "0000000"   second fraction (up to 7 digit)
        "t"                 first character of AM/PM designator   A
        "tt"                AM/PM designator                      AM
        "tt*"               AM/PM designator                      PM
        "d"     "0"         day w/o leading zero                  1
        "dd"    "00"        day with leading zero                 01
        "ddd"               short weekday name (abbreviation)     Mon
        "dddd"              full weekday name                     Monday
        "dddd*"             full weekday name                     Monday
        "M"     "0"         month w/o leading zero                2
        "MM"    "00"        month with leading zero               02
        "MMM"               short month name (abbreviation)       Feb
        "MMMM"              full month name                       Febuary
        "MMMM*"             full month name                       Febuary
        "y"     "0"         two digit year (year % 100) w/o leading zero           0
        "yy"    "00"        two digit year (year % 100) with leading zero          00
        "yyy"   "D3"        year                                  2000
        "yyyy"  "D4"        year                                  2000
        "yyyyy" "D5"        year                                  2000
        ...
        "z"     "+0;-0"     timezone offset w/o leading zero      -8
        "zz"    "+00;-00"   timezone offset with leading zero     -08
        "zzz"      "+00;-00" for hour offset, "00" for minute offset  full timezone offset   -07:30
        "zzz*"  "+00;-00" for hour offset, "00" for minute offset   full timezone offset   -08:00
        "K"    -Local       "zzz", e.g. -08:00
               -Utc         "'Z'", representing UTC
               -Unspecified ""
               -DateTimeOffset      "zzzzz" e.g -07:30:15
        "g*"                the current era name                  A.D.
        ":"                 time separator                        : -- DEPRECATED - Insert separator directly into pattern (eg: "H.mm.ss")
        "/"                 date separator                        /-- DEPRECATED - Insert separator directly into pattern (eg: "M-dd-yyyy")
        "'"                 quoted string                         'ABC' will insert ABC into the formatted string.
        '"'                 quoted string                         "ABC" will insert ABC into the formatted string.
        "%"                 used to quote a single pattern characters      E.g.The format character "%y" is to print two digit year.
        "\"                 escaped character                     E.g. '\d' insert the character 'd' into the format string.
        other characters    insert the character into the format string.
    Pre-defined format characters:
        (U) to indicate Universal time is used.
        (G) to indicate Gregorian calendar is used.
        Format              Description                             Real format                             Example
        =========           =================================       ======================                  =======================
        "d"                 short date                              culture-specific                        10/31/1999
        "D"                 long data                               culture-specific                        Sunday, October 31, 1999
        "f"                 full date (long date + short time)      culture-specific                        Sunday, October 31, 1999 2:00 AM
        "F"                 full date (long date + long time)       culture-specific                        Sunday, October 31, 1999 2:00:00 AM
        "g"                 general date (short date + short time)  culture-specific                        10/31/1999 2:00 AM
        "G"                 general date (short date + long time)   culture-specific                        10/31/1999 2:00:00 AM
        "m"/"M"             Month/Day date                          culture-specific                        October 31
(G)     "o"/"O"             Round Trip XML                          "yyyy-MM-ddTHH:mm:ss.fffffffK"          1999-10-31 02:00:00.0000000Z
(G)     "r"/"R"             RFC 1123 date,                          "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'"   Sun, 31 Oct 1999 10:00:00 GMT
(G)     "s"                 Sortable format, based on ISO 8601.     "yyyy-MM-dd'T'HH:mm:ss"                 1999-10-31T02:00:00
                                                                    ('T' for local time)
        "t"                 short time                              culture-specific                        2:00 AM
        "T"                 long time                               culture-specific                        2:00:00 AM
(G)     "u"                 Universal time with sortable format,    "yyyy'-'MM'-'dd HH':'mm':'ss'Z'"        1999-10-31 10:00:00Z
                            based on ISO 8601.
(U)     "U"                 Universal time with full                culture-specific                        Sunday, October 31, 1999 10:00:00 AM
                            (long date + long time) format
                            "y"/"Y"             Year/Month day                          culture-specific                        October, 1999
*/
//TODO: fix roundtrip and sortable to use default .net formatter
    public static class PersianDateTimeFormat
    {
        internal static PersianCalendar cal = new PersianCalendar();
        internal static CultureInfo culture = CultureInfo.GetCultureInfo("fa-IR");
        internal static char farsiZero = '۰';

        internal static IReadOnlyDictionary<int, string> monthNames = new Dictionary<int, string>()
        {
            {1, "فروردین"},
            {2, "اردیبهشت"},
            {3, "خرداد"},
            {4, "تیر"},
            {5, "مرداد"},
            {6, "شهریور"},
            {7, "مهر"},
            {8, "آبان"},
            {9, "آذر"},
            {10, "دی"},
            {11, "بهمن"},
            {12, "اسفند"}
        };

        internal static IReadOnlyDictionary<int, string> abbrMonth = new Dictionary<int, string>()
        {
            {1, "فرو"},
            {2, "ارد"},
            {3, "خرد"},
            {4, "تیر"},
            {5, "مرد"},
            {6, "شهر"},
            {7, "مهر"},
            {8, "آبا"},
            {9, "آذر"},
            {10, "دی"},
            {11, "بهم"},
            {12, "اسف"}
        };

        internal static IReadOnlyDictionary<DayOfWeek, string> dayNames = new Dictionary<DayOfWeek, string>()
        {
            {DayOfWeek.Sunday, "یک‌شنبه"},
            {DayOfWeek.Monday, "دوشنبه"},
            {DayOfWeek.Tuesday, "سه‌شنبه"},
            {DayOfWeek.Wednesday, "چهارشنبه"},
            {DayOfWeek.Thursday, "پنج‌شنبه"},
            {DayOfWeek.Friday, "جمعه"},
            {DayOfWeek.Saturday, "شنبه"}
        };

        internal static IReadOnlyDictionary<DayOfWeek, string> abbrDays = new Dictionary<DayOfWeek, string>()
        {
            {DayOfWeek.Sunday, "بک"},
            {DayOfWeek.Monday, "دو"},
            {DayOfWeek.Tuesday, "سه"},
            {DayOfWeek.Wednesday, "چها"},
            {DayOfWeek.Thursday, "پنج"},
            {DayOfWeek.Friday, "جمع"},
            {DayOfWeek.Saturday, "شنب"}
        };

        internal const string persianEraName = "ه.ش.";
        internal const string AMDesignator = "ق.ظ";
        internal const string PMDesignator = "ب.ظ";
        internal static long TicksPerDay = TimeSpan.TicksPerDay;
        internal static long TicksPerSecond = TimeSpan.TicksPerSecond;

        public static bool HasForceTwoDigitYears = false;

        internal const int MaxSecondsFractionDigits = 7;
        internal static readonly TimeSpan NullOffset = TimeSpan.MinValue;

        internal static char[] allStandardFormats =
        {
            'd', 'D', 'f', 'F', 'g', 'G',
            'm', 'M', 'o', 'O', 'r', 'R',
            's', 't', 'T', 'u', 'U', 'y', 'Y',
        };

        internal const String RoundtripFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK";
        internal const String RoundtripDateTimeUnfixed = "yyyy'-'MM'-'ddTHH':'mm':'ss zzz";

        private const int DEFAULT_ALL_DATETIMES_SIZE = 132;

        internal static String[] fixedNumberFormats = new String[]
        {
            "0",
            "00",
            "000",
            "0000",
            "00000",
            "000000",
            "0000000",
        };

        internal static void FormatDigits(StringBuilder outputBuffer, int value, int len, bool useFarsiDigits)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException();
            int integer = value;
            int remaining = len;
            char zero = useFarsiDigits ? farsiZero : '0';
            StringBuilder tmp = new StringBuilder();
            while (integer != 0)
            {
                tmp.Append((char) (((int) zero) + (integer % 10)));
                integer /= 10;
                remaining--;
            }

            if (remaining != 0)
                tmp.Append(new string(zero, remaining));

            outputBuffer.Append(ReverseStr(tmp.ToString()));
        }

        internal static int ParseRepeatPattern(String format, int pos, char patternChar)
        {
            int len = format.Length;
            int index = pos + 1;
            while ((index < len) && (format[index] == patternChar))
            {
                index++;
            }

            return (index - pos);
        }

        public static string ReverseStr(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private static String FormatDayOfWeek(int dayOfWeek, int repeat)
        {
            if (dayOfWeek < 0 || dayOfWeek > 6)
                throw new ArgumentOutOfRangeException(nameof(dayOfWeek));

            if (repeat == 3)
            {
                return (abbrDays[(DayOfWeek) dayOfWeek]);
            }

            // Call dtfi.GetDayName() here, instead of accessing DayNames property, because we don't
            // want a clone of DayNames, which will hurt perf.
            return dayNames[(DayOfWeek) dayOfWeek];
        }

        private static String FormatMonth(int month, int repeatCount)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month));

            if (repeatCount == 3)
            {
                return (abbrMonth[month]);
            }

            // Call GetMonthName() here, instead of accessing MonthNames property, because we don't
            // want a clone of MonthNames, which will hurt perf.
            return (monthNames[month]);
        }

        //
        // Get the next character at the index of 'pos' in the 'format' string.
        // Return value of -1 means 'pos' is already at the end of the 'format' string.
        // Otherwise, return value is the int value of the next character.
        //
        internal static char? ParseNextChar(String format, int pos)
        {
            if (pos >= format.Length - 1)
            {
                return null;
            }

            return format[pos + 1];
        }

        //
        // The pos should point to a quote character. This method will
        // get the string encloed by the quote character.
        //
        internal static int ParseQuoteString(String format, int pos, StringBuilder result)
        {
            //
            // NOTE : pos will be the index of the quote character in the 'format' string.
            //
            int formatLen = format.Length;
            int beginPos = pos;
            char quoteChar = format[pos++]; // Get the character used to quote the following string.

            bool foundQuote = false;
            while (pos < formatLen)
            {
                char ch = format[pos++];
                if (ch == quoteChar)
                {
                    foundQuote = true;
                    break;
                }
                else if (ch == '\\')
                {
                    // The following are used to support escaped character.
                    // Escaped character is also supported in the quoted string.
                    // Therefore, someone can use a format like "'minute:' mm\"" to display:
                    //  minute: 45"
                    // because the second double quote is escaped.
                    if (pos < formatLen)
                    {
                        result.Append(format[pos++]);
                    }
                    else
                    {
                        //
                        // This means that '\' is at the end of the formatting string.
                        //
                        throw new FormatException();
                    }
                }
                else
                {
                    result.Append(ch);
                }
            }

            if (!foundQuote)
            {
                // Here we can't find the matching quote.
                throw new FormatException();
            }

            //
            // Return the character count including the begin/end quote characters and enclosed string.
            //
            return (pos - beginPos);
        }



        //
        //  FormatCustomized
        //
        //  Actions: Format the DateTime instance using the specified format.
        // 
        private static String FormatCustomized(DateTime dateTime, String format, TimeSpan offset, bool useFarsiDigits)
        {
            StringBuilder result = new StringBuilder();
            // This is a flag to indicate if we are formating hour/minute/second only.
            bool bTimeOnly = true;

            int i = 0;
            int tokenLen, hour12;

            while (i < format.Length)
            {
                char ch = format[i];
                char? nextChar;
                switch (ch)
                {
                    case 'g':
                        tokenLen = ParseRepeatPattern(format, i, ch);
                        result.Append(persianEraName); // persian calendar only has PersianEra
                        break;
                    case 'h':
                        tokenLen = ParseRepeatPattern(format, i, ch);
                        hour12 = dateTime.Hour % 12;
                        if (hour12 == 0)
                        {
                            hour12 = 12;
                        }

                        FormatDigits(result, hour12, tokenLen, useFarsiDigits);
                        break;
                    case 'H':
                        tokenLen = ParseRepeatPattern(format, i, ch);
                        FormatDigits(result, dateTime.Hour, tokenLen, useFarsiDigits);
                        break;
                    case 'm':
                        tokenLen = ParseRepeatPattern(format, i, ch);
                        FormatDigits(result, dateTime.Minute, tokenLen, useFarsiDigits);
                        break;
                    case 's':
                        tokenLen = ParseRepeatPattern(format, i, ch);
                        FormatDigits(result, dateTime.Second, tokenLen, useFarsiDigits);
                        break;
                    case 'f':
                    case 'F':
                        tokenLen = ParseRepeatPattern(format, i, ch);
                        if (tokenLen <= MaxSecondsFractionDigits)
                        {
                            long fraction = (dateTime.Ticks % TicksPerSecond);
                            fraction = fraction / (long) Math.Pow(10, 7 - tokenLen);
                            if (ch == 'f')
                            {

                                string toAppend = ((int) fraction).ToString(fixedNumberFormats[tokenLen - 1],
                                    CultureInfo.InvariantCulture);
                                if (useFarsiDigits)
                                    toAppend = ReplaceWithFarsiDigits(toAppend);

                                result.Append(toAppend);
                            }
                            else
                            {
                                int effectiveDigits = tokenLen;
                                while (effectiveDigits > 0)
                                {
                                    if (fraction % 10 == 0)
                                    {
                                        fraction = fraction / 10;
                                        effectiveDigits--;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                if (effectiveDigits > 0)
                                {
                                    string toAppend = ((int) fraction).ToString(fixedNumberFormats[effectiveDigits - 1],
                                        CultureInfo.InvariantCulture);
                                    if (useFarsiDigits)
                                        toAppend = ReplaceWithFarsiDigits(toAppend);
                                    result.Append(toAppend);
                                }
                                else
                                {
                                    // No fraction to emit, so see if we should remove decimal also.
                                    if (result.Length > 0 && result[result.Length - 1] == '.')
                                    {
                                        result.Remove(result.Length - 1, 1);
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new FormatException();
                        }

                        break;
                    case 't':
                        tokenLen = ParseRepeatPattern(format, i, ch);
                        if (tokenLen == 1)
                        {
                            if (dateTime.Hour < 12)
                            {
                                result.Append(AMDesignator[0]);
                            }
                            else
                            {
                                result.Append(PMDesignator[0]);
                            }

                        }
                        else
                        {
                            result.Append((dateTime.Hour < 12 ? AMDesignator : PMDesignator));
                        }

                        break;
                    case 'd':
                        //
                        // tokenLen == 1 : Day of month as digits with no leading zero.
                        // tokenLen == 2 : Day of month as digits with leading zero for single-digit months.
                        // tokenLen == 3 : Day of week as a three-leter abbreviation.
                        // tokenLen >= 4 : Day of week as its full name.
                        //
                        tokenLen = ParseRepeatPattern(format, i, ch);
                        if (tokenLen <= 2)
                        {
                            int day = cal.GetDayOfMonth(dateTime);
                            FormatDigits(result, day, tokenLen, useFarsiDigits);
                        }
                        else
                        {
                            int dayOfWeek = (int) cal.GetDayOfWeek(dateTime);
                            result.Append(FormatDayOfWeek(dayOfWeek, tokenLen));
                        }

                        bTimeOnly = false;
                        break;
                    case 'M':
                        // 
                        // tokenLen == 1 : Month as digits with no leading zero.
                        // tokenLen == 2 : Month as digits with leading zero for single-digit months.
                        // tokenLen == 3 : Month as a three-letter abbreviation.
                        // tokenLen >= 4 : Month as its full name.
                        //
                        tokenLen = ParseRepeatPattern(format, i, ch);
                        int month = cal.GetMonth(dateTime);
                        if (tokenLen <= 2)
                        {
                            FormatDigits(result, month, tokenLen, useFarsiDigits);
                        }
                        else
                        {
                            if (tokenLen >= 4)
                            {
                                result.Append(monthNames[month]);
                            }
                            else
                            {
                                result.Append(FormatMonth(month, tokenLen));
                            }
                        }

                        bTimeOnly = false;
                        break;
                    case 'y':
                        // Notes about OS behavior:
                        // y: Always print (year % 100). No leading zero.
                        // yy: Always print (year % 100) with leading zero.
                        // yyy/yyyy/yyyyy/... : Print year value.  No leading zero.

                        int year = cal.GetYear(dateTime);
                        tokenLen = ParseRepeatPattern(format, i, ch);
                        if (HasForceTwoDigitYears)
                        {
                            FormatDigits(result, year, tokenLen <= 2 ? tokenLen : 2, useFarsiDigits);
                        }
                        else
                        {
                            if (tokenLen <= 2)
                            {
                                FormatDigits(result, year % 100, tokenLen, useFarsiDigits);
                            }
                            else
                            {
                                String fmtPattern = "D" + tokenLen;
                                string toAppend = year.ToString(fmtPattern, CultureInfo.InvariantCulture);
                                if (useFarsiDigits)
                                    toAppend = ReplaceWithFarsiDigits(toAppend);
                                result.Append(toAppend);
                            }
                        }

                        bTimeOnly = false;
                        break;
                    case 'z':
                        tokenLen = ParseRepeatPattern(format, i, ch);
                        FormatCustomizedTimeZone(dateTime, offset, format, tokenLen, bTimeOnly, result);
                        break;
                    case 'K':
                        tokenLen = 1;
                        FormatCustomizedRoundripTimeZone(dateTime, offset, result);
                        break;
                    case ':':
                        result.Append(DateTimeFormatInfo.CurrentInfo.TimeSeparator);
                        tokenLen = 1;
                        break;
                    case '/':
                        result.Append(DateTimeFormatInfo.CurrentInfo.DateSeparator);
                        tokenLen = 1;
                        break;
                    case '\'':
                    case '\"':
                        StringBuilder enquotedString = new StringBuilder();
                        tokenLen = ParseQuoteString(format, i, enquotedString);
                        result.Append(enquotedString);
                        break;
                    case '%':
                        // Optional format character.
                        // For example, format string "%d" will print day of month 
                        // without leading zero.  Most of the cases, "%" can be ignored.
                        nextChar = ParseNextChar(format, i);
                        // nextChar will be -1 if we already reach the end of the format string.
                        // Besides, we will not allow "%%" appear in the pattern.
                        if (nextChar >= 0 && nextChar != (int) '%')
                        {
                            result.Append(FormatCustomized(dateTime, ((char) nextChar).ToString(), offset,
                                useFarsiDigits));
                            tokenLen = 2;
                        }
                        else
                        {
                            //
                            // This means that '%' is at the end of the format string or
                            // "%%" appears in the format string.
                            //
                            throw new FormatException();
                        }

                        break;
                    case '\\':
                        // Escaped character.  Can be used to insert character into the format string.
                        // For exmple, "\d" will insert the character 'd' into the string.
                        //
                        // NOTENOTE : we can remove this format character if we enforce the enforced quote 
                        // character rule.
                        // That is, we ask everyone to use single quote or double quote to insert characters,
                        // then we can remove this character.
                        //
                        char? tmp = ParseNextChar(format, i);
                        if (tmp.HasValue)
                        {
                            nextChar = tmp.Value;
                            result.Append(nextChar);
                            tokenLen = 2;
                        }
                        else
                        {
                            //
                            // This means that '\' is at the end of the formatting string.
                            //
                            throw new FormatException();
                        }

                        break;
                    default:
                        // NOTENOTE : we can remove this rule if we enforce the enforced quote
                        // character rule.
                        // That is, if we ask everyone to use single quote or double quote to insert characters,
                        // then we can remove this default block.
                        result.Append(ch);
                        tokenLen = 1;
                        break;
                }

                i += tokenLen;
            }

            return result.ToString();

        }

        public static string ReplaceWithFarsiDigits(string str)
        {
            return new String(str.Select(s => s >= '0' && s <= '9' ? (char) ((int) farsiZero + (int) s - (int) '0') : s)
                .ToArray());
        }

        // output the 'z' famliy of formats, which output a the offset from UTC, e.g. "-07:30"
        private static void FormatCustomizedTimeZone(DateTime dateTime, TimeSpan offset, String format, Int32 tokenLen,
            Boolean timeOnly, StringBuilder result)
        {
            // See if the instance already has an offset
            Boolean dateTimeFormat = (offset == NullOffset);
            if (dateTimeFormat)
            {
                // No offset. The instance is a DateTime and the output should be the local time zone

                if (timeOnly && dateTime.Ticks < TimeSpan.TicksPerDay)
                {
                    // TODO: is it correct? TimeSpan.TicksPerDay? It was Calendar originally.
                    // For time only format and a time only input, the time offset on 0001/01/01 is less 
                    // accurate than the system's current offset because of daylight saving time.
                    offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now); // TODO: add nothrow flag
                }
                else if (dateTime.Kind == DateTimeKind.Utc)
                {
                    offset = TimeSpan.Zero;
                }
                else
                {
                    offset = TimeZoneInfo.Local.GetUtcOffset(dateTime); // TODO: add nothrow flag
                }
            }

            if (offset >= TimeSpan.Zero)
            {
                result.Append('+');
            }
            else
            {
                result.Append('-');
                // get a positive offset, so that you don't need a separate code path for the negative numbers.
                offset = offset.Negate();
            }

            if (tokenLen <= 1)
            {
                // 'z' format e.g "-7"
                result.AppendFormat(CultureInfo.InvariantCulture, "{0:0}", offset.Hours);
            }
            else
            {
                // 'zz' or longer format e.g "-07"
                result.AppendFormat(CultureInfo.InvariantCulture, "{0:00}", offset.Hours);
                if (tokenLen >= 3)
                {
                    // 'zzz*' or longer format e.g "-07:30"
                    result.AppendFormat(CultureInfo.InvariantCulture, ":{0:00}", offset.Minutes);
                }
            }
        }

        // output the 'K' format, which is for round-tripping the data
        private static void FormatCustomizedRoundripTimeZone(DateTime dateTime, TimeSpan offset, StringBuilder result)
        {

            // The objective of this format is to round trip the data in the type
            // For DateTime it should round-trip the Kind value and preserve the time zone. 
            // DateTimeOffset instance, it should do so by using the internal time zone.                        

            if (offset == NullOffset)
            {
                // source is a date time, so behavior depends on the kind.
                switch (dateTime.Kind)
                {
                    case DateTimeKind.Local:
                        // This should output the local offset, e.g. "-07:30"
                        offset = TimeZoneInfo.Local.GetUtcOffset(dateTime); // TODO: add nothrow flag
                        // fall through to shared time zone output code
                        break;
                    case DateTimeKind.Utc:
                        // The 'Z' constant is a marker for a UTC date
                        result.Append("Z");
                        return;
                    default:
                        // If the kind is unspecified, we output nothing here
                        return;
                }
            }

            if (offset >= TimeSpan.Zero)
            {
                result.Append('+');
            }
            else
            {
                result.Append('-');
                // get a positive offset, so that you don't need a separate code path for the negative numbers.
                offset = offset.Negate();
            }

            result.AppendFormat(CultureInfo.InvariantCulture, "{0:00}:{1:00}", offset.Hours, offset.Minutes);
        }


        internal static String GetRealFormat(String format)
        {
            String realFormat = null;

            DateTimeFormatInfo dtfi = culture.DateTimeFormat;

            switch (format[0])
            {
                case 'd': // Short Date
                    realFormat = dtfi.ShortDatePattern;
                    break;
                case 'D': // Long Date
                    realFormat = dtfi.LongDatePattern;
                    break;
                case 'f': // Full (long date + short time)
                    realFormat = dtfi.LongDatePattern + " " + dtfi.ShortTimePattern;
                    break;
                case 'F': // Full (long date + long time)
                    realFormat = dtfi.FullDateTimePattern;
                    break;
                case 'g': // General (short date + short time)
                    realFormat = dtfi.ShortDatePattern + " " + dtfi.ShortTimePattern;
                    break;
                case 'G': // General (short date + long time)
                    realFormat = dtfi.ShortDatePattern + " " + dtfi.LongTimePattern;
                    break;
                case 'm':
                case 'M': // Month/Day Date
                    realFormat = dtfi.MonthDayPattern;
                    break;
                case 'o':
                case 'O':
                    realFormat = RoundtripFormat;
                    break;
                case 'r':
                case 'R': // RFC 1123 Standard                    
                    realFormat = dtfi.RFC1123Pattern;
                    break;
                case 's': // Sortable without Time Zone Info
                    realFormat = dtfi.SortableDateTimePattern;
                    break;
                case 't': // Short Time
                    realFormat = dtfi.ShortTimePattern;
                    break;
                case 'T': // Long Time
                    realFormat = dtfi.LongTimePattern;
                    break;
                case 'u': // Universal with Sortable format
                    realFormat = dtfi.UniversalSortableDateTimePattern;
                    break;
                case 'U': // Universal with Full (long date + long time) format
                    realFormat = dtfi.FullDateTimePattern;
                    break;
                case 'y':
                case 'Y': // Year/Month Date
                    realFormat = dtfi.YearMonthPattern;
                    break;
                default:
                    throw new FormatException();
            }

            return (realFormat);
        }


        // Expand a pre-defined format string (like "D" for long date) to the real format that
        // we are going to use in the date time parsing.
        // This method also convert the dateTime if necessary (e.g. when the format is in Universal time),
        // and change dtfi if necessary (e.g. when the format should use invariant culture).
        //
        private static String ExpandPredefinedFormat(String format, ref DateTime dateTime, ref TimeSpan offset)
        {
            switch (format[0])
            {
                case 's': // Sortable without Time Zone Info
                    break;
                case 'u': // Universal time in sortable format.
                    if (offset != NullOffset)
                    {
                        // Convert to UTC invariants mean this will be in range
                        dateTime = dateTime - offset;
                    }
                    else if (dateTime.Kind == DateTimeKind.Local)
                    {
                        //InvalidFormatForLocal(format, dateTime);
                        throw new FormatException("Invalid format for local time.");
                    }

                    break;
                case 'U': // Universal time in culture dependent format.
                    if (offset != NullOffset)
                    {
                        // This format is not supported by DateTimeOffset
                        throw new FormatException();
                    }

                    dateTime = dateTime.ToUniversalTime();
                    break;
            }

            format = GetRealFormat(format);
            return (format);
        }

        public static String Format(DateTime dateTime, String format, bool useFarsiDigits)
        {
            return Format(dateTime, format, NullOffset, useFarsiDigits);
        }


        internal static String Format(DateTime dateTime, String format, TimeSpan offset, bool useFarsiDigits)
        {
            if (string.IsNullOrEmpty(format))
            {
                // If the time is less than 1 day, consider it as time of day.
                // Just print out the short time format.
                //
                // This is a workaround for VB, since they use ticks less then one day to be 
                // time of day.  In cultures which use calendar other than Gregorian calendar, these
                // alternative calendar may not support ticks less than a day.
                // For example, Japanese calendar only supports date after 1868/9/8.
                // This will pose a problem when people in VB get the time of day, and use it
                // to call ToString(), which will use the general format (short date + long time).
                // Since Japanese calendar does not support Gregorian year 0001, an exception will be
                // thrown when we try to get the Japanese year for Gregorian year 0001.
                // Therefore, the workaround allows them to call ToString() for time of day from a DateTime by
                // formatting as ISO 8601 format.
                bool timeOnlySpecialCase = dateTime.Ticks < TicksPerDay;
                if (offset == NullOffset)
                {
                    // Default DateTime.ToString case.
                    if (timeOnlySpecialCase)
                    {
                        format = "s";
                    }
                    else
                    {
                        format = "G";
                    }
                }
                else
                {
                    // Default DateTimeOffset.ToString case.
                    if (timeOnlySpecialCase)
                    {
                        format = RoundtripDateTimeUnfixed;
                    }
                    else
                    {
                        format = GetDateTimeOffsetPattern(culture.DateTimeFormat);
                    }
                }

            }

            if (format.Length == 1)
            {
                switch (format[0])
                {
                    case 'O':
                    case 'o':
                        return FastFormatRoundtrip(dateTime, offset, useFarsiDigits).ToString();
                    case 'R':
                    case 'r':
                        return FastFormatRfc1123(dateTime, offset);
                }

                format = ExpandPredefinedFormat(format, ref dateTime, ref offset);
            }

            return FormatCustomized(dateTime, format, offset, useFarsiDigits);
        }

        internal static string GetDateTimeOffsetPattern(DateTimeFormatInfo dtfi)
        {
            return (string) dtfi?.GetType()?.GetProperty("DateTimeOffsetPattern")?.GetValue(dtfi) ?? "";
        }

        internal static string FastFormatRfc1123(DateTime dateTime, TimeSpan offset)
        {
            // only a placeholder
            return dateTime.ToString("R");
        }

        internal static StringBuilder FastFormatRoundtrip(DateTime dateTime, TimeSpan offset, bool useFarsiDigits)
        {
            // yyyy-MM-ddTHH:mm:ss.fffffffK
            const int roundTripFormatLength = 28;
            StringBuilder result = new StringBuilder(roundTripFormatLength);

            int year, month, day;
            GetDatePart(dateTime, out year, out month, out day);

            AppendNumber(result, year, 4, useFarsiDigits);
            result.Append('-');
            AppendNumber(result, month, 2, useFarsiDigits);
            result.Append('-');
            AppendNumber(result, day, 2, useFarsiDigits);
            result.Append('T');
            AppendHHmmssTimeOfDay(result, dateTime, useFarsiDigits);
            result.Append('.');

            long fraction = dateTime.Ticks % TimeSpan.TicksPerSecond;
            AppendNumber(result, fraction, 7, useFarsiDigits);

            FormatCustomizedRoundripTimeZone(dateTime, offset, result);

            return result;
        }

        private static void GetDatePart(DateTime dateTime, out int year, out int month, out int day)
        {
            year = dateTime.Year;
            month = dateTime.Month;
            day = dateTime.Day;
        }

        private static void AppendHHmmssTimeOfDay(StringBuilder result, DateTime dateTime, bool useFarsiDigits)
        {
            // HH:mm:ss
            AppendNumber(result, dateTime.Hour, 2, useFarsiDigits);
            result.Append(':');
            AppendNumber(result, dateTime.Minute, 2, useFarsiDigits);
            result.Append(':');
            AppendNumber(result, dateTime.Second, 2, useFarsiDigits);
        }

        internal static void AppendNumber(StringBuilder builder, long val, int digits, bool useFarsiDigits)
        {
            char zero = useFarsiDigits ? farsiZero : '0';
            for (int i = 0; i < digits; i++)
            {
                builder.Append(zero);
            }

            int index = 1;
            while (val > 0 && index <= digits)
            {
                builder[builder.Length - index] = (char) (zero + (val % 10));
                val = val / 10;
                index++;
            }

            //BCLDebug.Assert(val == 0, "DateTimeFormat.AppendNumber(): digits less than size of val");
            if (val != 0)
                throw new Exception();
        }

        internal static String[] GetAllDateTimes(DateTime dateTime, char format, bool useFarsiDigits)
        {
            String[] allFormats = null;
            String[] results = null;

            switch (format)
            {
                case 'd':
                case 'D':
                case 'f':
                case 'F':
                case 'g':
                case 'G':
                case 'm':
                case 'M':
                case 't':
                case 'T':
                case 'y':
                case 'Y':
                    allFormats = DateTimeFormatInfo.CurrentInfo.GetAllDateTimePatterns(format);
                    results = new String[allFormats.Length];
                    for (int i = 0; i < allFormats.Length; i++)
                    {
                        results[i] = Format(dateTime, allFormats[i], useFarsiDigits);
                    }

                    break;
                case 'U':
                    DateTime universalTime = dateTime.ToUniversalTime();
                    allFormats = DateTimeFormatInfo.CurrentInfo.GetAllDateTimePatterns(format);
                    results = new String[allFormats.Length];
                    for (int i = 0; i < allFormats.Length; i++)
                    {
                        results[i] = Format(universalTime, allFormats[i], useFarsiDigits);
                    }

                    break;
                //
                // The following ones are special cases because these patterns are read-only in
                // DateTimeFormatInfo.
                //
                case 'r':
                case 'R':
                case 'o':
                case 'O':
                case 's':
                case 'u':
                    results = new String[] {Format(dateTime, new String(new char[] {format}), useFarsiDigits)};
                    break;
                default:
                    throw new FormatException();

            }

            return (results);
        }

        internal static String[] GetAllDateTimes(DateTime dateTime, bool useFarsiDigits)
        {
            List<String> results = new List<String>(DEFAULT_ALL_DATETIMES_SIZE);

            for (int i = 0; i < allStandardFormats.Length; i++)
            {
                String[] strings = GetAllDateTimes(dateTime, allStandardFormats[i], useFarsiDigits);
                for (int j = 0; j < strings.Length; j++)
                {
                    results.Add(strings[j]);
                }
            }

            String[] value = new String[results.Count];
            results.CopyTo(0, value, 0, results.Count);
            return (value);
        }
    }
}