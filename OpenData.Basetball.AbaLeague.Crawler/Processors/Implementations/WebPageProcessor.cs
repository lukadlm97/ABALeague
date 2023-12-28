﻿using System.Globalization;
using AngleSharp.Html.Dom;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Implementation;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Utilities;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using System.Xml.Linq;
using System.Numerics;
using System.Text;
using OpenData.Basetball.AbaLeague.Crawler.Models;
using AngleSharp.Dom;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;
using Microsoft.Extensions.Logging;

namespace OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations
{
    public class WebPageProcessor : IWebPageProcessor
    {
        private readonly IDocumentFetcher _documentFether;
        private readonly ILoggerFactory _loggerFactory;

        public WebPageProcessor(IDocumentFetcher documentFether, ILoggerFactory loggerFactory)
        {
            _documentFether = documentFether;
            _loggerFactory = loggerFactory;
        }
        public async Task<IReadOnlyList<(string Name, string Url)>> GetTeams(string leagueUrl,
            CancellationToken cancellationToken = default)
        {
            var webDocument = await _documentFether
                .FetchDocument(leagueUrl, cancellationToken);

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

                name = RemoveAccents(name);
                teams.Add((name, url));
            }

            return teams;

        }
        string RemoveAccents(string input)
        {
            string normalized = input.Normalize(NormalizationForm.FormKD);
            Encoding removal = Encoding.GetEncoding(Encoding.ASCII.CodePage,
                new EncoderReplacementFallback(""),
                new DecoderReplacementFallback(""));
            byte[] bytes = removal.GetBytes(normalized);
            return Encoding.ASCII.GetString(bytes);
        }

        public async Task<IReadOnlyList<(int? No, string Name, string Position, decimal Height, DateTime DateOfBirth, string Nationality, DateTime Start, DateTime? End)>> GetRoster(string teamUrl, CancellationToken cancellationToken = default)
        {
            var webDocument = await _documentFether
                .FetchDocument(teamUrl, cancellationToken);

            var players = new List<(int? No, string Name, string Position, decimal Height, DateTime DateOfBirth, string Nationality, DateTime Start, DateTime? End)>();



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

                name = name.ReplaceSpecialCharactersWithZ()
                                            .ReplaceSpecialCharactersWithC()
                                            .ReplaceSpecialCharactersWithS()
                                            .ReplaceSpecialCharactersWithDJ()
                                            .ToTitleCase();
                players.Add( (no, name, position, height, dateOfBirth, natinality, DateTime.Now.ToUniversalTime(), null));
            }

            return players;
        }

        public async Task<IReadOnlyList<(string HomeTeamName, string AwayTeamName, int? HomeTeamPoints, int? AwayTeamPoints, DateTime? Date,int? MatchNo, int? Round)>> GetRegularSeasonCalendar(string calendarUrl, CancellationToken cancellationToken = default)
        {
            var webDocument = await _documentFether
                .FetchDocument(calendarUrl, cancellationToken);

            var calendarItems = new List<(string HomeTeamName, string AwayTeamName, int? HomeTeamPoints, int? AwayTeamPoints, DateTime? Date,int? MatchNo, int? Round)>();

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
                var homeTeamPoints = (int?) null;
                var awayTeamPoints = (int?) null;
                int? matchNo = (int?) null;
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
                            matchNo = matchNoUrl.ParesMatchNoFromAbaUrl();

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
                            if (col.QuerySelectorAll("a").Any())
                            {
                                var points = col.QuerySelectorAll("a")[0].InnerHtml.Trim();
                                var teamsPoint = points.ParseTeamPoints();
                                homeTeamPoints = teamsPoint.FirstOrDefault();
                                awayTeamPoints = teamsPoint.LastOrDefault();
                            }

                            i++; continue;
                        }

                        if (i == 2)
                        {
                            dateTime = col.InnerHtml.ParseDateTimeFromAbaFormat();
                            break;
                        }
                     
                    }

                    calendarItems.Add
                        (
                            (
                            homeTeam.ReplaceSpecialCharactersWithZ()
                            .ReplaceSpecialCharactersWithC()
                            .ReplaceSpecialCharactersWithS()
                            .ReplaceSpecialCharactersWithDJ()
                                .CapitalizeFirstLetter(), 
                            awayTeam.ReplaceSpecialCharactersWithZ()
                            .ReplaceSpecialCharactersWithC()
                            .ReplaceSpecialCharactersWithS()
                            .ReplaceSpecialCharactersWithDJ()
                                .CapitalizeFirstLetter(), homeTeamPoints, awayTeamPoints, dateTime, matchNo, roundInt
                            )
                        );
                }
            }

            return calendarItems;
        }

        public async Task<IReadOnlyList<(int? MatchNo, int? Attendency, string? Venue, int? HomeTeamPoint, int? AwayTeamPoint)>>
            GetMatchScores(IEnumerable<(int matchNo, string url)> matchResources, CancellationToken cancellationToken = default)
        {
            var matchDetails = new List<(int? MatchNo, int? Attendency, string? Venue, int? HomeTeamPoint, int? AwayTeamPoint)>();

            List<Task<(int no,IDocument? document)>> documents = new List<Task<(int no, IDocument? document)>>();
            foreach (var (no, url) in matchResources)
            {
                documents.Add(GetScore(no, url));
            }

            var matchNoDocumentPair = await Task.WhenAll(documents);

            if (matchNoDocumentPair.Any(x => x.document == null))
            {
                return Array.Empty<(int? MatchNo, int? Attendency, string? Venue, int? HomeTeamPoint, int? AwayTeamPoint)>();
            }

            foreach (var (no, document) in matchNoDocumentPair)
            {
                if (document == null)
                {
                    matchDetails.Add((no, null, null, null, null));
                    continue;
                }
                try
                {
                    var result = document.QuerySelectorAll("table > tbody > tr > td.gameScore")[0];
                    var points = result.InnerHtml.ParseTeamPoints();
                    if (points == null || !points.Any())
                    {
                        matchDetails.Add((no, null, null, null, null));
                        continue;
                    }
                    int homeTeamPoints = points.First();
                    int awayTeamPoints = points.Last();

                    var venueAndAttendencyRaw = document.QuerySelectorAll(".dateAndVenue_container")[0].InnerHtml;
                    var venue = venueAndAttendencyRaw.ExtractVenue();
                    var attendance = venueAndAttendencyRaw.ExtractAttendance();

                    matchDetails.Add((no, attendance, venue, homeTeamPoints, awayTeamPoints));
                }
                catch(Exception ex)
                {
                    var logger = _loggerFactory.CreateLogger("Values");
                    logger.LogError("{0} occurred for match no {1}", ex.Message, no);
                }
                
            }

            return matchDetails;
        }

        async Task<(int no, IDocument? document)> GetScore(int no, string url, CancellationToken cancellationToken = default)
        {
            try
            {
                var document = await _documentFether.FetchDocument(url, cancellationToken);
                return (no, document);
            }
            catch(Exception ex)
            {
                return (no, null);
            }
        }


        public async Task<(IReadOnlyList<PlayerScore>
            HomeTeam,
            IReadOnlyList<PlayerScore>
            AwayTeam)> GetBoxScore(string matchUrl, CancellationToken cancellationToken = default)
        {
            var homeBoxScore = new List<PlayerScore>();
            var awayBoxScore = new List<PlayerScore>();

         
                var webDocument = await _documentFether
                    .FetchDocument(matchUrl, cancellationToken);

                var homeTeamPlayers = webDocument.QuerySelectorAll("table.match_boxscore_team_table")[0]
                    .QuerySelectorAll("tbody > tr");

                foreach (var playerRow in homeTeamPlayers)
                {
                    var columns = playerRow.QuerySelectorAll("td");
                    var name = columns[1].QuerySelectorAll("a")[0].GetAttribute("href")
                                            .ExtractNameFromUrl()
                                            .ReplaceSpecialCharactersWithZ()
                                            .ReplaceSpecialCharactersWithC()
                                            .ReplaceSpecialCharactersWithS()
                                            .ReplaceSpecialCharactersWithDJ();
                    var minutes = columns[2].InnerHtml;
                    var min = minutes.ConvertToNullableTimeSpan();
                    if (minutes == "00:00")
                    {
                        homeBoxScore.Add(new PlayerScore(name, min));
                        continue;
                        
                    }
                    var points = columns[3].InnerHtml.ConvertToNullableInt();
                    var shotPrc = columns[4].InnerHtml.ConvertToNullableDecimal();
                    var shotMade2Pt = columns[5].InnerHtml.ConvertToNullableInt();
                    var shotAttempted2Pt = columns[6].InnerHtml.ConvertToNullableInt();
                    var shotPrc2Pt = columns[7].InnerHtml.ConvertToNullableDecimal();
                    var shotMade3Pt = columns[8].InnerHtml.ConvertToNullableInt();
                    var shotAttempted3Pt = columns[9].InnerHtml.ConvertToNullableInt();
                    var shotPrc3Pt = columns[10].InnerHtml.ConvertToNullableDecimal();
                    var shotMade1Pt = columns[11].InnerHtml.ConvertToNullableInt();
                    var shotAttempted1Pt = columns[12].InnerHtml.ConvertToNullableInt();
                    var shotPrc1Pt = columns[13].InnerHtml.ConvertToNullableDecimal();
                    var defensiveRebounds = columns[14].InnerHtml.ConvertToNullableInt();
                    var offensiveRebounds = columns[15].InnerHtml.ConvertToNullableInt();
                    var totalRebounds = columns[16].InnerHtml.ConvertToNullableInt();  
                    var assists = columns[17].InnerHtml.ConvertToNullableInt();
                    var steals = columns[18].InnerHtml.ConvertToNullableInt();
                    var turnover = columns[19].InnerHtml.ConvertToNullableInt();
                    var inFavoureOfBlock = columns[20].InnerHtml.ConvertToNullableInt();
                    var againstBlock = columns[21].InnerHtml.ConvertToNullableInt();
                    var committedFoul = columns[22].InnerHtml.ConvertToNullableInt();
                    var receivedFoul = columns[23].InnerHtml.ConvertToNullableInt();
                    var pointFromPain = columns[24].InnerHtml.ConvertToNullableInt();
                    var pointFrom2ndChance = columns[25].InnerHtml.ConvertToNullableInt();
                    var pointFromFastBreak = columns[26].InnerHtml.ConvertToNullableInt();
                    var plusMinus = columns[27].InnerHtml.ConvertToNullableInt();
                    var rankValue = columns[28].InnerHtml.ConvertToNullableInt();

                    homeBoxScore.Add(new PlayerScore(name,min,points,shotPrc,shotMade2Pt,shotAttempted2Pt,shotPrc2Pt,shotMade3Pt,shotAttempted3Pt,shotPrc3Pt,shotMade1Pt,shotAttempted1Pt,shotPrc1Pt,defensiveRebounds,offensiveRebounds,totalRebounds,assists,steals,turnover,inFavoureOfBlock,againstBlock,committedFoul,receivedFoul,pointFromPain,pointFrom2ndChance,pointFromFastBreak,plusMinus,rankValue));
                }
                var awayTeamPlayers = webDocument.QuerySelectorAll("table.match_boxscore_team_table")[1]
                    .QuerySelectorAll("tbody > tr");

                foreach (var playerRow in awayTeamPlayers)
                {
                    var columns = playerRow.QuerySelectorAll("td");
                    var name = columns[1].QuerySelectorAll("a")[0].GetAttribute("href")
                                            .ExtractNameFromUrl()
                                            .ReplaceSpecialCharactersWithZ()
                                            .ReplaceSpecialCharactersWithC()
                                            .ReplaceSpecialCharactersWithS()
                                            .ReplaceSpecialCharactersWithDJ();
                    var minutes = columns[2].InnerHtml;
                    var min = minutes.ConvertToNullableTimeSpan();
                    if (minutes == "00:00")
                    {
                        awayBoxScore.Add(new PlayerScore(name, min));
                        continue;

                    }
                    var points = columns[3].InnerHtml.ConvertToNullableInt();
                    var shotPrc = columns[4].InnerHtml.ConvertToNullableDecimal();
                    var shotMade2Pt = columns[5].InnerHtml.ConvertToNullableInt();
                    var shotAttempted2Pt = columns[6].InnerHtml.ConvertToNullableInt();
                    var shotPrc2Pt = columns[7].InnerHtml.ConvertToNullableDecimal();
                    var shotMade3Pt = columns[8].InnerHtml.ConvertToNullableInt();
                    var shotAttempted3Pt = columns[9].InnerHtml.ConvertToNullableInt();
                    var shotPrc3Pt = columns[10].InnerHtml.ConvertToNullableDecimal();
                    var shotMade1Pt = columns[11].InnerHtml.ConvertToNullableInt();
                    var shotAttempted1Pt = columns[12].InnerHtml.ConvertToNullableInt();
                    var shotPrc1Pt = columns[13].InnerHtml.ConvertToNullableDecimal();
                    var defensiveRebounds = columns[14].InnerHtml.ConvertToNullableInt();
                    var offensiveRebounds = columns[15].InnerHtml.ConvertToNullableInt();
                    var totalRebounds = columns[16].InnerHtml.ConvertToNullableInt();
                    var assists = columns[17].InnerHtml.ConvertToNullableInt();
                    var steals = columns[18].InnerHtml.ConvertToNullableInt();
                    var turnover = columns[19].InnerHtml.ConvertToNullableInt();
                    var inFavoureOfBlock = columns[20].InnerHtml.ConvertToNullableInt();
                    var againstBlock = columns[21].InnerHtml.ConvertToNullableInt();
                    var committedFoul = columns[22].InnerHtml.ConvertToNullableInt();
                    var receivedFoul = columns[23].InnerHtml.ConvertToNullableInt();
                    var pointFromPain = columns[24].InnerHtml.ConvertToNullableInt();
                    var pointFrom2ndChance = columns[25].InnerHtml.ConvertToNullableInt();
                    var pointFromFastBreak = columns[26].InnerHtml.ConvertToNullableInt();
                    var plusMinus = columns[27].InnerHtml.ConvertToNullableInt();
                    var rankValue = columns[28].InnerHtml.ConvertToNullableInt();

                    awayBoxScore.Add(new PlayerScore(name, min, points, shotPrc, shotMade2Pt, shotAttempted2Pt, shotPrc2Pt, shotMade3Pt, shotAttempted3Pt, shotPrc3Pt, shotMade1Pt, shotAttempted1Pt, shotPrc1Pt, defensiveRebounds, offensiveRebounds, totalRebounds, assists, steals, turnover, inFavoureOfBlock, againstBlock, committedFoul, receivedFoul, pointFromPain, pointFrom2ndChance, pointFromFastBreak, plusMinus, rankValue));
            }
            

            return (homeBoxScore,awayBoxScore);
        }

        public Task<IReadOnlyList<(string HomeTeamName, string AwayTeamName, int? HomeTeamPoints, int? AwayTeamPoints, DateTime? Date, int? MatchNo, int? Round)>> GetRegularSeasonCalendar(int round, string calendarUrl, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
