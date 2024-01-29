using Microsoft.Extensions.Logging;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Models;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations
{
    public class KlsPageProcessor : IWebPageProcessor
    {
        private readonly IDocumentFetcher _documentFether;
        private readonly ILoggerFactory _loggerFactory;

        public KlsPageProcessor(IDocumentFetcher documentFether, ILoggerFactory loggerFactory)
        {
            _documentFether = documentFether;
            _loggerFactory = loggerFactory;
        }
        public async Task<(IReadOnlyList<PlayerScore> HomeTeam, 
            IReadOnlyList<PlayerScore> AwayTeam)> 
            GetBoxScore(string matchUrl, 
                        CancellationToken cancellationToken = default)
        {
            var homeBoxScore = new List<PlayerScore>();
            var awayBoxScore = new List<PlayerScore>();

            var webDocument = await _documentFether
                .FetchDocument(matchUrl, cancellationToken);

            var homeTeamPlayers = webDocument.QuerySelectorAll("table.table-stats")[0]
                .QuerySelectorAll("tbody > tr");
            var name = string.Empty;


            foreach (var playerRow in homeTeamPlayers)
            {
                TimeSpan? min = null;
                int? points = null;
                decimal? shotPrc = null;
                int? shotMade2Pt = null;
                int? shotAttempted2Pt = null;
                decimal? shotPrc2Pt = null;
                int? shotMade3Pt = null;
                int? shotAttempted3Pt = null;
                decimal? shotPrc3Pt = null;
                int? shotMade1Pt = null;
                int? shotAttempted1Pt = null;
                decimal? shotPrc1Pt = null;
                int? defensiveRebounds = null;
                int? offensiveRebounds = null;
                int? totalRebounds = null;
                int? assists = null;
                int? steals = null;
                int? turnover = null;
                int? inFavoureOfBlock = null;
                int? againstBlock = null;
                int? committedFoul = null;
                int? receivedFoul = null;
                int? pointFromPain = null;
                int? pointFrom2ndChance = null;

                int? pointFromFastBreak = null;
                int? plusMinus = null;
                int? rankValue = null;

                try
                {
                    var columns = playerRow.QuerySelectorAll("td");
                    name = columns[0].QuerySelectorAll("a")[0].InnerHtml
                                            .SkipNumbers()
                                           .Trim().ReplaceSpaceChars()
                                            .SwapFirstAndLastNameForBalkanPlayer()
                                            .Trim()
                                            .ReplaceSpecialCharactersWithZ()
                                            .ReplaceSpecialCharactersWithC()
                                            .ReplaceSpecialCharactersWithS()
                                            .ReplaceSpecialCharactersWithDJ()
                                            .ToTitleCase()
                                            .CheckWellKnownName()
                                            .TrimStars()
                                            ;
                    var minutes = columns[16].InnerHtml;
                    min = minutes.ConvertFromKlsTimeToNullableTimeSpan();
                    if (minutes == "0")
                    {
                        homeBoxScore.Add(new PlayerScore(name, min));
                        continue;

                    }
                    rankValue = columns[1].InnerHtml.ConvertToNullableInt();
                    points = columns[2].InnerHtml.ConvertToNullableInt();
                    shotPrc = columns[10].InnerHtml.ConvertToNullableDecimal();

                    (shotMade1Pt, shotAttempted1Pt) = columns[3].InnerHtml.UnpackSlashSeparatedValues();
                    shotPrc1Pt = columns[4].InnerHtml.ConvertToNullableDecimal();
                    (shotMade2Pt, shotAttempted2Pt) = columns[5].InnerHtml.UnpackSlashSeparatedValues();
                    shotPrc2Pt = columns[6].InnerHtml.ConvertToNullableDecimal();
                    (shotMade3Pt, shotAttempted3Pt) = columns[7].InnerHtml.UnpackSlashSeparatedValues();
                    shotPrc3Pt = columns[8].InnerHtml.ConvertToNullableDecimal();
                    shotPrc = columns[10].InnerHtml.ConvertToNullableDecimal();
                    (offensiveRebounds, defensiveRebounds) = columns[11].InnerHtml.UnpackSlashSeparatedValues();
                    totalRebounds = offensiveRebounds + defensiveRebounds;
                    (steals, turnover) = columns[12].InnerHtml.UnpackSlashSeparatedValues();
                    assists = columns[13].InnerHtml.ConvertToNullableInt();
                    (inFavoureOfBlock, againstBlock) = columns[14].InnerHtml.UnpackSlashSeparatedValues();
                    (committedFoul, receivedFoul) = columns[15].InnerHtml.UnpackSlashSeparatedValues();

                    homeBoxScore.Add(new PlayerScore(name,
                                                        min,
                                                        points,
                                                        shotPrc,
                                                        shotMade2Pt,
                                                        shotAttempted2Pt,
                                                        shotPrc2Pt,
                                                        shotMade3Pt,
                                                        shotAttempted3Pt,
                                                        shotPrc3Pt,
                                                        shotMade1Pt,
                                                        shotAttempted1Pt,
                                                        shotPrc1Pt,
                                                        defensiveRebounds,
                                                        offensiveRebounds,
                                                        totalRebounds,
                                                        assists,
                                                        steals,
                                                        turnover,
                                                        inFavoureOfBlock,
                                                        againstBlock,
                                                        committedFoul,
                                                        receivedFoul,
                                                        pointFromPain,
                                                        pointFrom2ndChance,
                                                        pointFromFastBreak,
                                                        plusMinus,
                                                        rankValue));
                }
                catch (Exception ex)
                {
                    var logger = _loggerFactory.CreateLogger("Values");
                    logger.LogError("Unable to found boxscore for {0} at team {1} with message {2}", name, name, ex.Message);
                    homeBoxScore.Add(new PlayerScore(name,
                                                        min,
                                                        points,
                                                        shotPrc,
                                                        shotMade2Pt,
                                                        shotAttempted2Pt,
                                                        shotPrc2Pt,
                                                        shotMade3Pt,
                                                        shotAttempted3Pt,
                                                        shotPrc3Pt,
                                                        shotMade1Pt,
                                                        shotAttempted1Pt,
                                                        shotPrc1Pt,
                                                        defensiveRebounds,
                                                        offensiveRebounds,
                                                        totalRebounds,
                                                        assists,
                                                        steals,
                                                        turnover,
                                                        inFavoureOfBlock,
                                                        againstBlock,
                                                        committedFoul,
                                                        receivedFoul,
                                                        pointFromPain,
                                                        pointFrom2ndChance,
                                                        pointFromFastBreak,
                                                        plusMinus,
                                                        rankValue));
                }

            }
            var awayTeamPlayers = webDocument.QuerySelectorAll("table.table-stats")[1]
                .QuerySelectorAll("tbody > tr");

            foreach (var playerRow in awayTeamPlayers)
            {
                TimeSpan? min = null;
                int? points = null;
                decimal? shotPrc = null;
                int? shotMade2Pt = null;
                int? shotAttempted2Pt = null;
                decimal? shotPrc2Pt = null;
                int? shotMade3Pt = null;
                int? shotAttempted3Pt = null;
                decimal? shotPrc3Pt = null;
                int? shotMade1Pt = null;
                int? shotAttempted1Pt = null;
                decimal? shotPrc1Pt = null;
                int? defensiveRebounds = null;
                int? offensiveRebounds = null;
                int? totalRebounds = null;
                int? assists = null;
                int? steals = null;
                int? turnover = null;
                int? inFavoureOfBlock = null;
                int? againstBlock = null;
                int? committedFoul = null;
                int? receivedFoul = null;
                int? pointFromPain = null;
                int? pointFrom2ndChance = null;

                int? pointFromFastBreak = null;
                int? plusMinus = null;
                int? rankValue = null;

                try
                {
                    var columns = playerRow.QuerySelectorAll("td");
                    name = columns[0].QuerySelectorAll("a")[0].InnerHtml
                                            .SkipNumbers()
                                           .Trim().ReplaceSpaceChars()
                                            .SwapFirstAndLastNameForBalkanPlayer()
                                            .Trim()
                                            .ReplaceSpecialCharactersWithZ()
                                            .ReplaceSpecialCharactersWithC()
                                            .ReplaceSpecialCharactersWithS()
                                            .ReplaceSpecialCharactersWithDJ()
                                            .ToTitleCase()
                                            .CheckWellKnownName()
                                            .TrimStars()
                                            ;
                    var minutes = columns[16].InnerHtml;
                    min = minutes.ConvertFromKlsTimeToNullableTimeSpan();
                    if (minutes == "0")
                    {
                        awayBoxScore.Add(new PlayerScore(name, min));
                        continue;

                    }
                    rankValue = columns[1].InnerHtml.ConvertToNullableInt();
                    points = columns[2].InnerHtml.ConvertToNullableInt();
                    shotPrc = columns[10].InnerHtml.ConvertToNullableDecimal();

                    (shotMade1Pt, shotAttempted1Pt) = columns[3].InnerHtml.UnpackSlashSeparatedValues();
                    shotPrc1Pt = columns[4].InnerHtml.ConvertToNullableDecimal();
                    (shotMade2Pt, shotAttempted2Pt) = columns[5].InnerHtml.UnpackSlashSeparatedValues();
                    shotPrc2Pt = columns[6].InnerHtml.ConvertToNullableDecimal();
                    (shotMade3Pt, shotAttempted3Pt) = columns[7].InnerHtml.UnpackSlashSeparatedValues();
                    shotPrc3Pt = columns[8].InnerHtml.ConvertToNullableDecimal();
                    shotPrc = columns[10].InnerHtml.ConvertToNullableDecimal();
                    (offensiveRebounds, defensiveRebounds) = columns[11].InnerHtml.UnpackSlashSeparatedValues();
                    totalRebounds = offensiveRebounds + defensiveRebounds;
                    (steals, turnover) = columns[12].InnerHtml.UnpackSlashSeparatedValues();
                    assists = columns[13].InnerHtml.ConvertToNullableInt();
                    (inFavoureOfBlock, againstBlock) = columns[14].InnerHtml.UnpackSlashSeparatedValues();
                    (committedFoul, receivedFoul) = columns[15].InnerHtml.UnpackSlashSeparatedValues();

                    awayBoxScore.Add(new PlayerScore(name,
                                                        min,
                                                        points,
                                                        shotPrc,
                                                        shotMade2Pt,
                                                        shotAttempted2Pt,
                                                        shotPrc2Pt,
                                                        shotMade3Pt,
                                                        shotAttempted3Pt,
                                                        shotPrc3Pt,
                                                        shotMade1Pt,
                                                        shotAttempted1Pt,
                                                        shotPrc1Pt,
                                                        defensiveRebounds,
                                                        offensiveRebounds,
                                                        totalRebounds,
                                                        assists,
                                                        steals,
                                                        turnover,
                                                        inFavoureOfBlock,
                                                        againstBlock,
                                                        committedFoul,
                                                        receivedFoul,
                                                        pointFromPain,
                                                        pointFrom2ndChance,
                                                        pointFromFastBreak,
                                                        plusMinus,
                                                        rankValue));
                }
                catch (Exception ex)
                {
                    var logger = _loggerFactory.CreateLogger("Values");
                    logger.LogError("Unable to found boxscore for {0} at team {1} with message {2}", name, name, ex.Message);
                    awayBoxScore.Add(new PlayerScore(name,
                                                        min,
                                                        points,
                                                        shotPrc,
                                                        shotMade2Pt,
                                                        shotAttempted2Pt,
                                                        shotPrc2Pt,
                                                        shotMade3Pt,
                                                        shotAttempted3Pt,
                                                        shotPrc3Pt,
                                                        shotMade1Pt,
                                                        shotAttempted1Pt,
                                                        shotPrc1Pt,
                                                        defensiveRebounds,
                                                        offensiveRebounds,
                                                        totalRebounds,
                                                        assists,
                                                        steals,
                                                        turnover,
                                                        inFavoureOfBlock,
                                                        againstBlock,
                                                        committedFoul,
                                                        receivedFoul,
                                                        pointFromPain,
                                                        pointFrom2ndChance,
                                                        pointFromFastBreak,
                                                        plusMinus,
                                                        rankValue));
                }

            }


            return (homeBoxScore, awayBoxScore);
        }

        public async Task<IReadOnlyList<(int? MatchNo, 
            int? Attendency,
            string? Venue, 
            int? HomeTeamPoint,
            int? AwayTeamPoint)>>
            GetMatchScores(IEnumerable<(int matchNo, string url)> matchResources,
            CancellationToken cancellationToken = default)
        {
            
            List<(int? MatchNo,
            int? Attendency,
            string? Venue,
            int? HomeTeamPoint,
            int? AwayTeamPoint)> values = new List<(int? MatchNo,
                                                    int? Attendency,
                                                    string? Venue,
                                                    int? HomeTeamPoint,
                                                    int? AwayTeamPoint)>();
            foreach(var (matchNo, url) in matchResources)
            {
                var webDocument = await _documentFether.FetchDocument(url, cancellationToken);
                var administrationContent = webDocument.QuerySelectorAll("span")[0];

                if(administrationContent == null)
                {
                    continue;
                }

                var lastIndexOfDots =
                administrationContent.InnerHtml.Trim().LastIndexOf(':');
                var attendencyRaw = administrationContent.InnerHtml.Trim()
                    .Substring(administrationContent.InnerHtml.Trim().LastIndexOf(':') + 1).Trim();

                var attendency = attendencyRaw.ConvertToNullableInt();
                var scoresRaw = webDocument.QuerySelectorAll(".event-team__score-wrap");
                var homeTeamPointsRaw = scoresRaw[0].QuerySelectorAll("span")[0].InnerHtml.Trim();
                var awayTeamPointsRaw = scoresRaw[1].QuerySelectorAll("span")[0].InnerHtml.Trim();
                var homePoints = homeTeamPointsRaw.PointsFromSpan();
                var awayPoints = awayTeamPointsRaw.PointsFromSpan();
                values.Add((matchNo, attendency, null, homePoints, awayPoints));
            }

            return values;
        }

        public async Task<IReadOnlyList<(string HomeTeamName, 
                                            string AwayTeamName, 
                                            int? HomeTeamPoints,
                                            int? AwayTeamPoints, 
                                            DateTime? Date, 
                                            int? MatchNo, 
                                            int? Round)>>
            GetRegularSeasonCalendar(string calendarUrl, CancellationToken cancellationToken = default)
        {
            var webDocument = await _documentFether
              .FetchDocument(calendarUrl, cancellationToken);

            var calendarItems = new List<(string HomeTeamName, 
                                            string AwayTeamName,
                                            int? HomeTeamPoints, 
                                            int? AwayTeamPoints, 
                                            DateTime? Date,
                                            int? MatchNo,
                                            int? Round)>();

            var calendarItem = webDocument.QuerySelectorAll(".table");
            int matchNoIterator = 1;
            foreach (var item in calendarItem)
            {
                var roundRaw = item.QuerySelectorAll("table > tbody > tr > td")[0]
                    .QuerySelectorAll("b")[0]
                    .InnerHtml
                    .Trim(); 

                var round = roundRaw.Substring(0, roundRaw.IndexOf('.')).Trim();
                var roundInt = round.ConvertToNullableInt();
                if (!roundInt.HasValue)
                {

                    continue;
                }
                var homeTeam = string.Empty;
                var awayTeam = string.Empty;
                var homeTeamPoints = (int?)null;
                var awayTeamPoints = (int?)null;
                int? matchNo = (int?)null;
                DateTime? dateTime = null;
                Console.WriteLine(round);
                var matchdayItems = item.QuerySelectorAll("table > tbody > tr");
                bool inialIteration = true;
                foreach (var matchdayItem in matchdayItems)
                {
                    if (inialIteration)
                    {
                        inialIteration = false; 
                        continue;
                    }
                    var columns = matchdayItem
                        .QuerySelectorAll("td");
                    int i = 0;
                    foreach (var col
                              in columns)
                    {
                        if (i == 0)
                        {
                            homeTeam = col.QuerySelectorAll("a")[0]
                                                .InnerHtml
                                                .Trim()
                                                .ToLower()
                                                .CapitalizeFirstLetter();
                            awayTeam = col.QuerySelectorAll("a")[1]
                                                .InnerHtml
                                                .ToLower()
                                                .CapitalizeFirstLetter();

                            i++; 
                            continue;
                        }
                        if (i == 1)
                        {
                            dateTime = col.InnerHtml.ParseDateTimeFromKlsFormat();
                            break;
                        }

                    }
                    matchNo = matchNoIterator++;
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

        public Task<IReadOnlyList<(string HomeTeamName, string AwayTeamName, int? HomeTeamPoints, int? AwayTeamPoints, DateTime? Date, int? MatchNo, int? Round)>> GetRegularSeasonCalendar(int round, string calendarUrl, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<(int? No,
                                    string Name, 
                                    string Position,
                                    decimal Height, 
                                    DateTime DateOfBirth,
                                    string Nationality, 
                                    DateTime Start, 
                                    DateTime? End)>> 
            GetRoster(string teamUrl,
                        CancellationToken cancellationToken = default)
        {
            var webDocument = await _documentFether
               .FetchDocument(teamUrl, cancellationToken);

            var players = new List<(int? No, 
                                    string Name, 
                                    string Position, 
                                    decimal Height, 
                                    DateTime DateOfBirth, 
                                    string Nationality, 
                                    DateTime Start, 
                                    DateTime? End)>();

            var postElements = webDocument.QuerySelectorAll(".post-content");
            var panelElement = postElements[0].QuerySelectorAll(".fusion-fullwidth");
            var rowsElements = panelElement[1].QuerySelectorAll(".fusion-layout-column");
            bool isInialCycle = true;
            foreach ( var row in rowsElements) 
            {
                if( isInialCycle)
                {
                    isInialCycle = false;
                    continue;
                }
                var nameRaw = row.QuerySelector(".fusion-title");
                if(nameRaw ==null || nameRaw.QuerySelectorAll("h3").Count()<2) 
                {
                    continue;
                }
                var heandingThree = nameRaw.QuerySelectorAll("h3")[1];
                var name = heandingThree
                  .InnerHtml
                  .Trim();

                if (name.StartsWith("<strong>"))
                {
                    name = name.ExtractNameFromString().Trim();
                }
                else
                {
                    name = name.Substring(0, name.IndexOf('<'));
                }
                var bio = row.QuerySelector(".fusion-text")
                  .InnerHtml
                  .Trim();

                name = name.Trim().ReplaceSpaceChars()
                                    .SwapFirstAndLastNameForBalkanPlayer()
                                    .Trim()
                                    .ReplaceSpecialCharactersWithZ()
                                    .ReplaceSpecialCharactersWithC()
                                    .ReplaceSpecialCharactersWithS()
                                    .ReplaceSpecialCharactersWithDJ()
                                    .ToTitleCase()
                                    .CheckWellKnownName();
                /*
                name = name.SwapFirstAndLastNameForBalkanPlayer();
                name = name.Trim();
                name = name.ReplaceSpecialCharactersWithZ()
                                         .ReplaceSpecialCharactersWithC()
                                         .ReplaceSpecialCharactersWithS()
                                         .ReplaceSpecialCharactersWithDJ()
                                         .ToTitleCase();
                name = name.CheckWellKnownName();
                */

                var (no, year, height, position)  = bio.ExtractItems();
                var poistionEnum = position.ConvertPositionFromSerbian();
                var noExact = no.ConvertToNullableInt();
                var heightExact = height.ConvertToNullableInt();
                var yearExact = year.ConvertToNullableInt();
                var yearOfBirth = new DateTime(yearExact ?? DateTime.Now.Year, 1, 1);

                players.Add((noExact, 
                                name, 
                                poistionEnum.ToString(), 
                                heightExact??0, 
                                yearOfBirth,
                                name.DeterminateIsoCode(), 
                                DateTime.Now.ToUniversalTime(), 
                                null));
            }

            return players;
        }

        public async Task<IReadOnlyList<(string Name, string Url)>> GetTeams(string leagueUrl,
            string? standingsTableSelector = null, 
            string? standingsTableRowNameSelector = null, 
            string? standingsTableRowUrlSelector = null,
            CancellationToken cancellationToken = default)
        {
            var teams = new List<(string, string)>();
            if (string.IsNullOrWhiteSpace(leagueUrl) || string.IsNullOrWhiteSpace(standingsTableSelector))
            {
                return teams;
            }

            var webDocument = await _documentFether
              .FetchDocument(leagueUrl, cancellationToken);

            var teamElements = webDocument.QuerySelectorAll(standingsTableSelector);

            foreach (var teamElement in teamElements)
            {
                var name = teamElement
                    .QuerySelectorAll("td")[0]
                    .QuerySelectorAll("a")[0]
                    .QuerySelectorAll("b")[0]
                    .InnerHtml
                    .Trim()
                    .ToLower()
                    .CapitalizeFirstLetter();

                var url = teamElement
                    .QuerySelectorAll("td")[0]
                    .QuerySelectorAll("a")[0]
                    .GetAttribute("href")
                    .Trim();

                name = name.RemoveAccents();
                teams.Add((name, url));
            }

            return teams;
        }
    }
}
