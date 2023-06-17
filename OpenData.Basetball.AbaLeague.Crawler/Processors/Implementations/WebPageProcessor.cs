using System.Globalization;
using AngleSharp.Html.Dom;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Implementation;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Utilities;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using System.Xml.Linq;

namespace OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations
{
    public class WebPageProcessor:IWebPageProcessor
    {
        private readonly ILeagueFetcher _leagueFetcher;

        public WebPageProcessor(ILeagueFetcher leagueFetcher)
        {
            _leagueFetcher = leagueFetcher;
        }
        public async Task<IReadOnlyList<(string Name, string Url)>> GetTeams(string leagueUrl,
            CancellationToken cancellationToken = default)
        {
            var webDocument = await _leagueFetcher
                .FetchTeams(leagueUrl, cancellationToken);

            var teams = new List<(string,string)>();
            var teamElements = webDocument.QuerySelectorAll("table > tbody > tr");

            foreach (var teamElement in teamElements)
            {
                var name = teamElement
                    .QuerySelectorAll("td")[1]
                    .QuerySelectorAll("a")[0]
                    .InnerHtml
                    .Trim();

                var url = teamElement
                    .QuerySelectorAll("td")[1]
                    .QuerySelectorAll("a")[0]
                    .GetAttribute("href")
                    .Trim();
              

                teams.Add((name, url));
            }

            return teams;

        }

        public async Task<IReadOnlyList<(int? No, string Name, string Position, decimal Height, DateTime DateOfBirth, string Nationality)>> GetRoster(string teamUrl, CancellationToken cancellationToken = default)
        {
            var webDocument = await _leagueFetcher
                .FetchTeams(teamUrl, cancellationToken);

            var players = new List<(int? No, string Name, string Position, decimal height, DateTime DateOfBirth, string Nationality)>();



            var playerElements = webDocument.QuerySelectorAll("table#team_roster_table > tbody > tr");
            
            foreach (var teamElement in playerElements)
            {
                var no = teamElement
                    .QuerySelectorAll("td")[0]
                    .InnerHtml
                    .Trim()
                    .ConvertToNullableInt();

                var name = teamElement
                    .QuerySelectorAll("td")[2]
                    .QuerySelectorAll("a")[0]
                    .InnerHtml
                    .Trim();

                var position = teamElement
                    .QuerySelectorAll("td")[3]
                    .InnerHtml
                    .Trim();

                var height = teamElement
                    .QuerySelectorAll("td")[4]
                    .InnerHtml
                    .Trim()
                    .ConvertToDecimal();

                var dateOfBirth = teamElement
                    .QuerySelectorAll("td")[5]
                    .InnerHtml
                    .Trim()
                    .ConvertToDateTime();


                var natinality = teamElement
                    .QuerySelectorAll("td")[6]
                    .InnerHtml
                    .Trim();

                players.Add( (no, name, position, height, dateOfBirth, natinality));
            }

            return players;
        }

        public async Task<IReadOnlyList<(int? Round, string HomeTeamName, string AwayTeamName, int HomeTeamPoints, int AwayTeamPoints, DateTime? Date,int? MatchNo)>> GetRegularSeasonCalendar(string calendarUrl, CancellationToken cancellationToken = default)
        {
            var webDocument = await _leagueFetcher
                .FetchTeams(calendarUrl, cancellationToken);

            var calendarItems = new List<(int? Round, string HomeTeamName, string AwayTeamName, int HomeTeamPoints, int AwayTeamPoints, DateTime? Date,int? MatchNo)>();



            var calendarItem = webDocument.QuerySelectorAll(".panel");

            foreach (var item in calendarItem)
            {
                var roundRaw = item.QuerySelectorAll(".panel-title")[0]
                    .QuerySelectorAll("a")[0]
                    .InnerHtml
                    .Trim(); ;
                var round = roundRaw.Substring(0, roundRaw.IndexOf('<')).Trim();
                if (!round.ToLower(CultureInfo.InvariantCulture).Contains("round"))
                {
                    continue;
                }
                round = round.Substring(round.IndexOf(' '),round.Length-round.IndexOf(' ')).Trim();
                var roundInt = round.ConvertToNullableInt();
                if (!roundInt.HasValue)
                {

                    continue;
                }
                var homeTeam = string.Empty;
                var awayTeam = string.Empty;
                var homeTeamPoints = (int)0;
                var awayTeamPoints = (int)0;
                int? matchNo = (int?)0;
                DateTime? dateTime = null;
                Console.WriteLine(round);
                var matchdayItems = item.QuerySelectorAll("table > tbody > tr");
                foreach (var matchdayItem in matchdayItems)
                {
                    var columns = matchdayItem
                        .QuerySelectorAll("td");
                    int i = 0;
                    foreach (var col
                              in columns)
                    {
                        if (i == 0)
                        {
                            var matchNoUrl = col.QuerySelectorAll("a")[1].GetAttribute("href")
                                .Trim();
                            matchNo = matchNoUrl.ParesMatchNoFromUrl();

                            var  name = col.QuerySelectorAll("a")[1].InnerHtml.Trim();
                            
                            var teamName = name.ParseTeamNames();
                            if (!teamName.Any())
                            {
                                //todo default
                            }

                             homeTeam = teamName.FirstOrDefault();
                             awayTeam = teamName.LastOrDefault();

                            i++;continue;
                        }
                        if (i == 1)
                        {
                            var points = col.QuerySelectorAll("a")[0].InnerHtml.Trim();
                            var teamsPoint = points.ParseTeamPoints();
                             homeTeamPoints = teamsPoint.FirstOrDefault();
                             awayTeamPoints = teamsPoint.LastOrDefault();

                            i++; continue;
                        }

                        if (i == 2)
                        {
                            dateTime = col.InnerHtml.ParseDateTimeFromAbaFormat();
                            break;
                        }
                     
                    }

                    calendarItems.Add((roundInt,homeTeam,awayTeam,homeTeamPoints,awayTeamPoints,dateTime, matchNo));
                }
            }

            return calendarItems;
        }
    }
}
