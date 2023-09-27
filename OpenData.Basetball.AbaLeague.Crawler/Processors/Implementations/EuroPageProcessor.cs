using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Implementation;
using OpenData.Basetball.AbaLeague.Crawler.Models;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Utilities;

namespace OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations
{
    public class EuroPageProcessor : IEuroleagueProcessor
    {
        private readonly IDocumentFetcher _documentFetcher;

        public EuroPageProcessor(IDocumentFetcher documentFetcher)
        {
            _documentFetcher = documentFetcher;
        }
        public async Task<IReadOnlyList<(string Name, string Url)>> GetTeams(string leagueUrl, CancellationToken cancellationToken = default)
        {
            var webDocument = await _documentFetcher
                .FetchDocument(leagueUrl, cancellationToken);

            var teams = new List<(string, string)>();
            var teamElements = webDocument.QuerySelectorAll(".complex-stat-table_row__1P6us");

            bool initalCycle = true;
            foreach (var teamElement in teamElements)
            {
                if (initalCycle)
                {
                    initalCycle = false;
                    continue;
                }
                var name = teamElement
                    .QuerySelectorAll(".complex-stat-table_mainClubName__3IMZJ")[0]
                    .InnerHtml
                    .Trim();
                name = name.Substring(0,name.IndexOf('<'));

                var url = teamElement
                    .QuerySelectorAll(".complex-stat-table_clubWrap__2i3fk")[0]
                    .GetAttribute("href")
                    .Trim();


                teams.Add((name, url));
            }

            return teams;
        }

        public async Task<IReadOnlyList<(int? No, string Name, string Position, decimal Height, DateTime DateOfBirth, string Nationality, DateTime Start, DateTime? End)>> GetRoster(string teamUrl, CancellationToken cancellationToken = default)
        {
            List<(int? No, string Name, string Position, decimal Height, DateTime DateOfBirth, string Nationality, DateTime Start, DateTime? End)>
                list =
                    new List<(int? No, string Name, string Position, decimal Height, DateTime DateOfBirth, string Nationality, DateTime Start, DateTime? End)> ();
            var client = new HttpClient();

            var response = await client.GetAsync(teamUrl, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var myDeserializedClass = JsonSerializer.Deserialize<List<RosterItemDto>>(content);

                foreach (var rosterItemDto in myDeserializedClass)
                {
                    if (rosterItemDto.Type == "J")
                    {
                        list.Add( (null,
                            ExtractName(rosterItemDto.Person.Name),
                            rosterItemDto.PositionName,
                            rosterItemDto.Person.Height??0,
                            rosterItemDto.Person.BirthDate,
                            rosterItemDto.Person.Country.Code,
                            rosterItemDto.StartDate,
                            rosterItemDto.EndDate));
                    }
                }
            }

            return list;
        }

        public async Task<IReadOnlyList<(int? Round, string HomeTeamName, string AwayTeamName, int? HomeTeamPoints, int? AwayTeamPoints, DateTime? Date, int? MatchNo)>> GetRegularSeasonCalendar(int round, string calendarUrl, CancellationToken cancellationToken = default)
        {
            var webDocument = await _documentFetcher
                .FetchDocumentBySelenium(calendarUrl, cancellationToken);

            var teams = new List<(int? Round, string HomeTeamName, string AwayTeamName, int? HomeTeamPoints, int? AwayTeamPoints, DateTime? Date, int? MatchNo)>();
            var teamElements = webDocument.QuerySelectorAll(".game-center-group_li__Hr15J");


            foreach (var teamElement in teamElements)
            {
               var time = teamElement.QuerySelectorAll(".game-card-view_time__Po6MA")[0]
                    .GetAttribute("datetime")
                    .Trim();
               var homeTeam = teamElement.QuerySelectorAll(".game-card-view_name__H_Dy2")[0]
                    .InnerHtml
                    .Trim();
               var awayTeam = teamElement.QuerySelectorAll(".game-card-view_name__H_Dy2")[2]
                   .InnerHtml
                   .Trim();

               var homeTeamPoints = teamElement.QuerySelectorAll(".game-score_scoreWrapper__jCR8C.game-score__home__K8NwK");


               int? homePoints = null;
                if (homeTeamPoints.Any() && !string.IsNullOrWhiteSpace(homeTeamPoints.First().InnerHtml))
                {
                    homePoints = homeTeamPoints.First().InnerHtml.Trim().PointsFromSpan();
                }

                var awayTeamPoints =
                    teamElement.QuerySelectorAll(".game-score_scoreWrapper__jCR8C.game-score__away__mGtZB");
               int? awayPoints = null;
               if (awayTeamPoints.Any() && !string.IsNullOrWhiteSpace(awayTeamPoints.First().InnerHtml))
               {
                   awayPoints = awayTeamPoints.First().InnerHtml.Trim().PointsFromSpan();
               }

               var url = teamElement.QuerySelectorAll(".game-card-view_linkWrap__EABj1")[0]
                   .GetAttribute("href")
                   .Trim();
               var rightTime = time.ParseDateTimeFromEuroleagueFormat();
               var matchNo= url.ParesMatchNoFromEuroleagueUrl();

               teams.Add(new(round, homeTeam, awayTeam, homePoints, awayPoints, rightTime, matchNo));
            }

            return teams;
        }

        public async Task<IReadOnlyList<(int MatchId, int? Attendency, string Venue, int HomeTeamPoint, int AwayTeamPoint)>> GetMatchResult(
            IEnumerable<(int MatchId, string MatchUrl)> matchUrls, CancellationToken cancellationToken = default)
        {
            List<(int MatchId, int? Attendency, string Venue, int HomeTeamPoint, int AwayTeamPoint)> list =
                new List<(int MatchId, int? Attendency, string Venue, int HomeTeamPoint, int AwayTeamPoint)>();
            foreach (var matchUrl in matchUrls)
            {
                string? venue = null;
                int? attendency = null;
                int? homeTeamPoint = null;
                int? awayTeamPoint = null;
                var webDocument = await _documentFetcher
                    .FetchDocumentBySelenium(matchUrl.MatchUrl, cancellationToken);

                var eventDetails = webDocument.QuerySelectorAll("ul.event-info_list__pEwdU > li");
                var scores =
                    webDocument.QuerySelectorAll("div.game-hero-score_scoreWrapper__dMBPQ");
                try
                {
                    if (eventDetails.Any() && eventDetails[2] != null && eventDetails[4] != null)
                    {
                        venue = eventDetails[2].InnerHtml.ToString().CapitalizeFirstLetter();
                        attendency = eventDetails[4].InnerHtml.ToString().ExtractEuroleagueAttendance();
                        homeTeamPoint = scores[0].InnerHtml.ToString().PointsFromSpan();
                        awayTeamPoint = scores[1].InnerHtml.ToString().PointsFromSpan();
                        list.Add(new(matchUrl.MatchId, attendency, venue, homeTeamPoint ?? -1, awayTeamPoint ?? -1));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(matchUrl.MatchId+":"+matchUrl.MatchUrl);
                }
               
            }

            return list;
        }

        public async Task<(IReadOnlyList<PlayerScore> HomeTeam, IReadOnlyList<PlayerScore> AwayTeam)> GetBoxScore(string matchUrl, CancellationToken cancellationToken = default)
        {
            var webDocument = await _documentFetcher
                .FetchDocumentBySelenium(matchUrl, cancellationToken);
            var boxscores = webDocument.QuerySelectorAll("div.game-box-scores-table-grouped-tab_tableGroupedWrapper__YLif_");


            foreach (var boxscore in boxscores)
            {
                Console.WriteLine(boxscore.InnerHtml);
            }

            throw new NotImplementedException();
        }

        string ExtractName(string sourceName)
        {
            var tokenized = sourceName.Split(',').Select(x => x.Trim().ToLower()).ToArray();
            if (tokenized.Count() != 2)
            {
                throw new Exception();
            }
            var firstName = char.ToUpper(tokenized[1][0]) + 
                            tokenized[1].Substring(1);
            var lastName = char.ToUpper(tokenized[0][0]) +
                           tokenized[0].Substring(1);

            return firstName + " " + lastName;
        }

    }
}
