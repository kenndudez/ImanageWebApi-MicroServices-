using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared
{
    public static class DateExtensions
    {
        public static DateTime GetDateUtcNow(this DateTime now)
        {
            return DateTime.UtcNow;
        }

        public static DateTime ToInvariantDateTime(this String value, String format)
        {
            DateTimeFormatInfo dtfi = DateTimeFormatInfo.InvariantInfo;
            var result = DateTime.TryParseExact(value, format, dtfi, DateTimeStyles.None, out DateTime newValue);
            return newValue;
        }

        public static string ToDateString(this DateTime dt, String format)
        {
            return dt.ToString(format, DateTimeFormatInfo.InvariantInfo);
        }
    }
}
