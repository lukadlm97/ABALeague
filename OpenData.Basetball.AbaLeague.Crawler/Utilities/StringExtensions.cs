﻿using System;
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
            if (string.IsNullOrWhiteSpace(value)) return DateTime.MinValue;
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
        public static TimeSpan? ConvertToNullableTimeSpan(this string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains(':')) return new TimeSpan(0,0,0,0);
           var array = value.Split(':');
           var minutes = 0;
           var seconds = 0;
           int.TryParse(array[0], out minutes);
           int.TryParse(array[1], out seconds);

           return new TimeSpan(0, 0, minutes, seconds);

        }

        public static decimal? ConvertToNullableDecimal(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            if(decimal.TryParse(value, CultureInfo.InvariantCulture,out decimal result))
            {

                return result;
            }
            return null;
        }

        public static int? ConvertToNullableInt(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return int.TryParse(value, out int result) ? result : null;
        }

        public static IEnumerable<string> ParseTeamNames(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)||
                !value.Contains(":") ||
                !value.Contains("<span>") ||
                !value.Contains("</span>")) return Array.Empty<string>();
            
            var factorizedValue = value.Split(">:<").Select(x => x.Trim()).ToArray();
            if (factorizedValue.Length != 2) return Array.Empty<string>();

            var indexOfWhitespace = 7;

            factorizedValue[0] = factorizedValue[0].Substring(0, factorizedValue[0].Length- indexOfWhitespace).Trim();

             var indexOfBracket = factorizedValue[1].IndexOf('>');

             factorizedValue[1] = factorizedValue[1].Substring(indexOfBracket+1, factorizedValue[1].Length - indexOfBracket-1).Trim();

             return factorizedValue;
        }

        public static IEnumerable<int> ParseTeamPoints(this string value)
        {
            if (string.IsNullOrWhiteSpace(value) ||
                !value.Contains(":")) return Array.Empty<int>();

            var factorizedValue = value.Split(':').Select(x => x.Trim()).ToArray();
            if (factorizedValue.Length != 2) return Array.Empty<int>();

            List<int> items = new List<int>();
            if (int.TryParse(factorizedValue[0], out int homeTeamPoint))
            {
                items.Add(homeTeamPoint);
            }
            if (int.TryParse(factorizedValue[1], out int awayTeamPoint))
            {
                items.Add(awayTeamPoint);
            }

            return items;
        }

        public static int? ParesMatchNoFromUrl(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            var bracketStart = 4;
            var bracketEnd = 5;
            var bracketCounter = 0;
            StringBuilder output = new StringBuilder();

            for (var counter = 0; counter < value.Length; counter++)
            {
                if (value[counter] == '/')
                {
                    bracketCounter++;
                }

                if (bracketStart == bracketCounter && bracketEnd > bracketCounter)
                {
                    output.Append(value[counter]);
                }
            }


            return output.ToString().TrimStart('/').ConvertToNullableInt();
        }

        public static DateTime? ParseDateTimeFromAbaFormat(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            var withoutDay = value.Substring(value.IndexOf(',') + 1).Trim();
            var spplited = withoutDay.Split(' ');
            if (spplited.Length != 3)
            {
                return null;
            }

            var date = spplited[0];
            var time = spplited[1];

            if (DateTime.TryParseExact(date + " " + time, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime dateOut))
            {
                return dateOut;
            }
            return null;
        }

        public static string ExtractVenue(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            value = value.Trim();
            var lastIndexOfBr = value.LastIndexOf("<br>");
            if (lastIndexOfBr == -1)
            {
                return null;
            }

            var justVenue = value.Substring(lastIndexOfBr + 3).Trim();

            return justVenue.Substring(justVenue.IndexOf(":")+1).Trim();
        }

        public static int? ExtractAttendance(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            value = value.Trim();
            var lastIndexOfBr = value.LastIndexOf("<br>");
            if (lastIndexOfBr == -1)
            {
                return null;
            }
            var lastIndexOfI = value.IndexOf("</i>")+4;
            var justVenue = value.Substring(lastIndexOfI, lastIndexOfBr-lastIndexOfI).Trim();

            return ConvertToNullableInt(justVenue);
        }


        public static string ExtractNameFromUrl(this string valueUrl)
        {
            if (string.IsNullOrWhiteSpace(valueUrl)) return null;
            valueUrl = valueUrl.Trim();

            int indexOfSecondBackSlashFromBack = -1;

            for (var i = valueUrl.Length - 2; i >= 0; i--)
            {
                if (valueUrl[i] == '/')
                {
                    indexOfSecondBackSlashFromBack = i;
                    break;
                }
            }

            var playerName = valueUrl.Substring(indexOfSecondBackSlashFromBack+1);
            playerName = playerName.Replace('-', ' ');
            return playerName.TrimEnd('/').ToTitleCase();

        }

        public static string ToTitleCase(this string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
        }
    }
}