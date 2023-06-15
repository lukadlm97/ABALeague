using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basetball.AbaLeague.Crawler.Utilities
{
    public static class StringExtensions
    {
        public static DateTime ConvertToDateTime(this string value)
        {
            if(string.IsNullOrWhiteSpace( value)) return DateTime.MinValue;
            return DateTime.TryParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime dateTime)
                ? dateTime
                : DateTime.MinValue;
        }
        public static decimal ConvertToDecimal(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return decimal.MinValue;
            return decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        public static int? ConvertToNullableInt(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return int.TryParse(value, out int result) ? result : null;
        }
    }
}
