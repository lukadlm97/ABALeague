using AngleSharp.Html.Dom;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Implementation;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Utilities;
using OpenData.Basetball.AbaLeague.Domain.Entities;

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
    }
}
