using AngleSharp.Dom;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

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

        public static int? ParesMatchNoFromAbaUrl(this string value)
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

        public static int? ParesMatchNoFromEuroleagueUrl(this string value)
        {
            // Split the URL path by '/'
            string[] segments = value.Split('/');

            // Iterate through the segments to find a number
            foreach (var segment in segments)
            {
                if (int.TryParse(segment, out int number))
                {
                    return number;
                }
            }

            return null; // Return null if no number is found
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
        public static DateTime? ParseDateTimeFromKlsFormat(this string value)
        {
            if (DateTime.TryParseExact(value, "dd.MM.yyyy. HH:mm", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime dateOut))
            {
                return dateOut;
            }
            return null;
        }

        public static DateTime? ParseDateTimeFromEuroleagueFormat(this string value)
        {
            string format = "yyyy-MM-ddTHH:mm:ss.fffZ";

            // Parse the string
            if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out DateTime dateTime))
            {
                return dateTime;
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

        public static int? ExtractEuroleagueAttendance(this string value)
        {
            if (!value.Contains(':'))
            {
                return null;
            }

            var justAttendance = value.Split(':')[1].Trim();

            return ConvertToNullableInt(justAttendance);
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

        public static string CapitalizeFirstLetter(this string source)
        {
            if (string.IsNullOrWhiteSpace(source)) return null;
            source = source.Trim();

            List<string> result = new List<string>();
            foreach (var segments in source.Split(' '))
            {
                result.Add(segments.ToTitleCase());
            }

            return string.Join(' ', result);

        }

        public static string ToTitleCase(this string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
        }

        public static int? PointsFromSpan(this string pointsRaw)
        {
            var startBoundary = '>';
            var endBoundary = '<';
            var startIndex = pointsRaw.IndexOf(startBoundary);
            var endIndex = pointsRaw.LastIndexOf(endBoundary);

            var points = pointsRaw.Substring(startIndex+1, endIndex - startIndex - 1);
            return ConvertToNullableInt(points);

        }

        public static string ReplaceSpecialCharactersWithC(this string input)
        {
            // Define the characters to be replaced
            char[] charactersToReplace = { 'Ć', 'Č', 'ć', 'č' };

            // Use LINQ to replace characters
            input = new string(input.Select(c => charactersToReplace.Contains(c) ? 'c' : c).ToArray());

            return input;
        }
        public static string ReplaceSpecialCharactersWithZ(this string input)
        {
            // Define the characters to be replaced
            char[] charactersToReplace = { 'Ž', 'ž' };

            // Use LINQ to replace characters
            input = new string(input.Select(c => charactersToReplace.Contains(c) ? 'z' : c).ToArray());

            return input;
        }
        public static string ReplaceSpecialCharactersWithS(this string input)
        {
            // Define the characters to be replaced
            char[] charactersToReplace = { 'Š', 'š' };

            // Use LINQ to replace characters
            input = new string(input.Select(c => charactersToReplace.Contains(c) ? 's' : c).ToArray());

            return input;
        }
        public static string ReplaceSpecialCharactersWithDJ(this string input)
        {
            // Use LINQ to replace characters
            input = input.Replace("đ", "dj", StringComparison.OrdinalIgnoreCase);

            return input;
        }

        public static string SwapFirstAndLastNameForBalkanPlayer(this string input)
        {
            var specialChars = new List<string>
            {
                "đ", "Š", "š", "Ž", "ž", "Ć", "Č", "ć", "č",  "Đ"
            };
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            bool isHomeName = false;
            foreach(var inputChar in input)
            {
                if (specialChars.Contains(inputChar.ToString()))
                {
                    isHomeName = true;
                    break;
                }
            }
            if(!isHomeName )
            {
                return input;
            }
            // Split the input into parts using space as the separator
            string[] nameParts = input.Split(' ');

            // Check if there are at least two parts (first name and last name)
            if (nameParts.Length >= 2)
            {
                // Swap the first and last names
                string swappedName = $"{nameParts[1]} {nameParts[0]}";

                return swappedName;
            }

            // If there's only one part or no parts, return the original input
            return input;
        }

        public static string FixDzPlayerNames(this string input)
        {
            if(input == "Nikola Dzurisic")
            {
                return "Nikola Djurisic";
            }
            if (input == "Dzoko Salic")
            {
                return "Djoko Salic";
            }
            if (input == "Danilo Andzusic")
            {
                return "Danilo Andjusic";
            }
            if (input == "Srdzan Boskovic")
            {
                return "Srdjan Boskovic";
            }
            if (input == "Dzordzije Jovanovic")
            {
                return "Djordjije Jovanovic";
            } 
            if (input == "Nicola Dzogo")
            {
                return "Nicola Djogo";
            }

            return input;
        }

        public static string RemoveAccents(this string input)
        {
            string normalized = input.Normalize(NormalizationForm.FormKD);
            Encoding removal = Encoding.GetEncoding(Encoding.ASCII.CodePage,
                new EncoderReplacementFallback(""),
                new DecoderReplacementFallback(""));
            byte[] bytes = removal.GetBytes(normalized);
            return Encoding.ASCII.GetString(bytes);
        }

        public static (string no, string date, string height, string position) ExtractItems(this string bioInput)
        {
            // Define regular expressions for each piece of information
            Regex numberRegex = new Regex(@"Broj: (\d+)<br>");
            Regex yearOfBirthRegex = new Regex(@"Godište: (\d+)");
            Regex heightRegex = new Regex(@"Visina: (\d+)\s*cm");
            Regex heightAdvancedRegex = new Regex(@"Visina: (\d+)\s\s*cm");
            Regex positionRegex = new Regex(@"Pozicija: (.+)</p>");
            bioInput = bioInput.Replace("&nbsp;", string.Empty);

            // Extract information using regular expressions
            string number = GetMatch(bioInput, numberRegex);
            string yearOfBirth = GetMatch(bioInput, yearOfBirthRegex);
            string height = GetMatch(bioInput, heightRegex);
            string position = GetMatch(bioInput, positionRegex);
            if (string.IsNullOrEmpty(height))
            {
                height = GetMatch(bioInput, heightAdvancedRegex);
            }

            return (number, yearOfBirth, height, position);
        }

        public static string ExtractNameFromString(this string input)
        {
            // Define the regular expression
            Regex nameRegex = new Regex(@"<strong>(.+?)<\/strong>");

            // Extract the name using the regular expression
            string extractedName = GetMatch(input, nameRegex);

            // Display the extracted name
            return extractedName;
        }
        static string GetMatch(string input, Regex regex)
        {
            Match match = regex.Match(input);
            return match.Success ? match.Groups[1].Value : null;
        }

        public static string DeterminateIsoCode(this string input)
        {
            var defaultCountryCode = "SRB";
            List<string> usualUsaNames = new List<string>()
            {
                "Tyron Jumord Harris","Kangu Kevin","Terry Lee Armstrong Ii","Kenny Martel Mathews Dye",
                "Elvis Dale Harvey","Zachary Harvey","Jordan Callahan","Rashun Jarrell Davis",
                "Zachary Cleveland Simmons","Jaylen Robert Andrews","Treacy Dante","Nelson Lamar Phillips Jr",
                "Nelson Ellis Haskin","Wendell Jerome Green Jr","Dedric Lamont Boyd","Terrance Christopher Motley",
                "Jalen Tyrese Finch","Lamont Lavell West","Kobe Thomas Webster","Quinterious Trayvon Croft",
                "Penn Eral Jarred", "Scott Blakney"
            };

            List<string> usualChinaNames = new List<string>() { "Linjun Jiang", "Junwei Xu" };

            if (usualUsaNames.Contains(input))
            {
                return "USA";
            }

            if (usualChinaNames.Contains(input))
            {
                return "CNH";
            }

            return defaultCountryCode;
        }


        public static PositionEnum ConvertPositionFromSerbian(this string source)
        {
            switch(source.ToLower())
            {
                case "plejmejker":
                case " plejmejker / bek":
                    return PositionEnum.Guard;
                case "bek":
                case "bek / krilo":
                    return PositionEnum.ShootingGuard;
                case "krilo":
                case "bek/krilo":
                case "krilo krilni centar":
                case "krilo / krilni centar":
                case "krilo /krilni centar":
                    return PositionEnum.Forward;
                case "krilni centar":
                case "krilni centar/ centar":
                case "krilni centar / centar":
                    return PositionEnum.PowerForward;
                case "centar":
                    return PositionEnum.Center;
                default:
                    return PositionEnum.Coach;
            }
        }
        public static string NameSwap(this string input)
        {
            // Split the input into parts using space as the separator
            string[] nameParts = input.Split(' ');

            // Check if there are at least two parts (first name and last name)
            if (nameParts.Length >= 2)
            {
                // Swap the first and last names
                string swappedName = $"{nameParts[1]} {nameParts[0]}";

                return swappedName;
            }

            // If there's only one part or no parts, return the original input
            return input;
        }

        public static string CheckWellKnownName(this string input)
        {
            List<string> wellKnownNames= new List<string>()
            {
                "Cerovina Luka", "Barna Filip", "Nedeljkov Luka", "Sinovec Stefan",
                 "Mikavica Bogdan", "Urban Kroflic", "Jovic Dragomir", "Gavrilovic Milos",
                  "Vukas Ognjen", "Milin Matija", "Gutalj Marko", "Pilica Jasin",
                   "Shkarban Bogdan"
            };
            if (wellKnownNames.Contains(input))
            {
                return input.NameSwap();
            }
            return input;
        } 
        public static string ReplaceSpaceChars(this string input)
        {
            if (input.Contains("&nbsp;"))
            {
                input = input.Replace("&nbsp;", string.Empty);
            }

            return input;
        }

        public static string SkipNumbers(this string input)
        {
            var startIndex = 0;
            for(int i = 0; i < input.Length; i++)
            {
                if (!char.IsDigit(input[i]))
                {
                    startIndex = i;
                    break;
                }
            }

            return input.Substring(startIndex);
        }
    }
}
