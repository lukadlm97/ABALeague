
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Models;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using System.Text.RegularExpressions;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System.Collections.Generic;
using System.Globalization;

namespace OpenData.Basketball.AbaLeague.Application.Services.Implementation
{
    public class BoxScoreService:IBoxScoreService
    {
        private readonly IWebPageProcessor _webPageProcessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEuroleagueProcessor _euroleagueProcessor;

        public BoxScoreService(IWebPageProcessor  webPageProcessor,IUnitOfWork unitOfWork, IEuroleagueProcessor euroleagueProcessor)
        {
            _webPageProcessor = webPageProcessor;
            _unitOfWork = unitOfWork;
            _euroleagueProcessor = euroleagueProcessor;
        }
        public async Task<(IEnumerable<BoxScoreDto> homePlayers, IEnumerable<BoxScoreDto> awayPlayers, IEnumerable<string> missingPlayers)> GetScore(int leagueId, int matchNo, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId,cancellationToken);
            var match = await _unitOfWork.CalendarRepository.SearchByMatchNo(leagueId, matchNo, cancellationToken);
            var rosterHomeTeam = await _unitOfWork.TeamRepository.GetRoster(match.HomeTeamId??0, cancellationToken);
            var rosterAwayTeam = await _unitOfWork.TeamRepository.GetRoster(match.AwayTeamId??0, cancellationToken);

            var statUrl = string.Format(league.BoxScoreUrl, match.MatchNo);
            var (homeTeamStats, awayTeamStats) = await _webPageProcessor.GetBoxScore(statUrl, cancellationToken);
            List<BoxScoreDto> homePlayersStat = new List<BoxScoreDto>();
            List<BoxScoreDto> awayPlayersStat = new List<BoxScoreDto>();
            List<string> missingPlayerAtRoster = new List<string>();

            foreach (var homeTeamStat in homeTeamStats)
            {
                if (rosterHomeTeam.RosterItems.Any(x => x.Player.Name == homeTeamStat.Name))
                {
                    var rosterItem = rosterHomeTeam.RosterItems.First(x => x.Player.Name == homeTeamStat.Name);
                    if (rosterItem == null)
                    {
                        missingPlayerAtRoster.Add(rosterHomeTeam.Name + " -> " + homeTeamStat.Name);
                        continue;
                    }
                    var newBoxScore = new BoxScoreDto(rosterItem.Id, match.Id, rosterItem.Player.Name, match.Round,
                        match.MatchNo, homeTeamStat.Minutes,
                        homeTeamStat.Points, homeTeamStat.ShotPrc, homeTeamStat.ShotMade2Pt,
                        homeTeamStat.ShotAttempted2Pt, homeTeamStat.ShotPrc2Pt, homeTeamStat.ShotMade3Pt,
                        homeTeamStat.ShotAttempted3Pt, homeTeamStat.shotPrc3Pt, homeTeamStat.ShotMade1Pt,
                        homeTeamStat.ShotAttempted1Pt, homeTeamStat.ShotPrc1Pt, homeTeamStat.DefensiveRebounds,
                        homeTeamStat.OffensiveRebounds, homeTeamStat.TotalRebounds, homeTeamStat.Assists,
                        homeTeamStat.Steals, homeTeamStat.Turnover, homeTeamStat.InFavoureOfBlock,
                        homeTeamStat.AgainstBlock, homeTeamStat.CommittedFoul, homeTeamStat.ReceivedFoul,
                        homeTeamStat.PointFromPain, homeTeamStat.PointFrom2ndChance, homeTeamStat.PointFromFastBreak,
                        homeTeamStat.PlusMinus, homeTeamStat.RankValue);
                    homePlayersStat.Add(newBoxScore);
                }
                else
                {
                    missingPlayerAtRoster.Add(rosterHomeTeam.Name+" -> "+ homeTeamStat.Name);
                }
                
            }

            foreach (var awayTeamStat in awayTeamStats)
            {
                if (rosterAwayTeam.RosterItems.Any(x => x.Player.Name == awayTeamStat.Name))
                {
                    var rosterItem = rosterAwayTeam.RosterItems.First(x => x.Player.Name == awayTeamStat.Name); 
                    if (rosterItem == null)
                    {
                        missingPlayerAtRoster.Add(rosterAwayTeam.Name + " -> " + awayTeamStat.Name);
                        continue;
                    }
                    var newBoxScore = new BoxScoreDto(rosterItem.Id, match.Id, rosterItem.Player.Name, match.Round,
                        match.MatchNo, awayTeamStat.Minutes,
                        awayTeamStat.Points, awayTeamStat.ShotPrc, awayTeamStat.ShotMade2Pt,
                        awayTeamStat.ShotAttempted2Pt, awayTeamStat.ShotPrc2Pt, awayTeamStat.ShotMade3Pt,
                        awayTeamStat.ShotAttempted3Pt, awayTeamStat.shotPrc3Pt, awayTeamStat.ShotMade1Pt,
                        awayTeamStat.ShotAttempted1Pt, awayTeamStat.ShotPrc1Pt, awayTeamStat.DefensiveRebounds,
                        awayTeamStat.OffensiveRebounds, awayTeamStat.TotalRebounds, awayTeamStat.Assists,
                        awayTeamStat.Steals, awayTeamStat.Turnover, awayTeamStat.InFavoureOfBlock,
                        awayTeamStat.AgainstBlock, awayTeamStat.CommittedFoul, awayTeamStat.ReceivedFoul,
                        awayTeamStat.PointFromPain, awayTeamStat.PointFrom2ndChance, awayTeamStat.PointFromFastBreak,
                        awayTeamStat.PlusMinus, awayTeamStat.RankValue);
                    awayPlayersStat.Add(newBoxScore);
                }
                else
                {
                    missingPlayerAtRoster.Add(rosterAwayTeam.Name + " -> " + awayTeamStat.Name);
                }

            }



            return (homePlayersStat, awayPlayersStat,missingPlayerAtRoster);
        }

        public async Task<(IEnumerable<BoxScoreDto> playersScore, IEnumerable<string> missingPlayers)> GetRoundBoxScore(int leagueId, int roundNo, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            var matches = await _unitOfWork.CalendarRepository.SearchByRoundNo(leagueId, roundNo, cancellationToken);

            List<BoxScoreDto> outputCollection = new List<BoxScoreDto>();
            List<string> missingPlayerScoreDto = new List<string>();

            foreach (var matchNo in matches.Select(x=>x.MatchNo))
            {
                var result = await GetScore(leagueId, matchNo, cancellationToken);
                if (result.missingPlayers.Any())
                {
                    missingPlayerScoreDto.AddRange(result.missingPlayers);
                    continue;
                }
                outputCollection.AddRange(result.homePlayers);
                outputCollection.AddRange(result.awayPlayers);

            }

            return (outputCollection, missingPlayerScoreDto);

        }

        public async Task<(IEnumerable<BoxScoreDto> playersScore, IEnumerable<string> missingPlayers)> GetEuroleagueRoundBoxScore(int leagueId, int roundNo, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            var matches = await _unitOfWork.CalendarRepository.SearchByRoundNo(leagueId, roundNo, cancellationToken);

            List<BoxScoreDto> outputCollection = new List<BoxScoreDto>();
            List<string> missingPlayerScoreDto = new List<string>();

            foreach (var matchNo in matches.Select(x => x.MatchNo))
            {
                var result = await GetEuroleagueMatchBoxScore(leagueId, matchNo, cancellationToken);
                if (result.missingPlayers.Any())
                {
                    missingPlayerScoreDto.AddRange(result.missingPlayers);
                }
                outputCollection.AddRange(result.playersScore);

            }

            return (outputCollection, missingPlayerScoreDto);
        }

        public async Task<(IEnumerable<BoxScoreDto> playersScore, IEnumerable<string> missingPlayers)> GetEuroleagueMatchBoxScore(int leagueId, int matchNo, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            var match = await _unitOfWork.CalendarRepository.SearchByMatchNo(leagueId, matchNo, cancellationToken);
            var rosterHomeTeam = await _unitOfWork.TeamRepository.GetRoster(match.HomeTeamId ?? 0, cancellationToken);
            var rosterAwayTeam = await _unitOfWork.TeamRepository.GetRoster(match.AwayTeamId ?? 0, cancellationToken);

            var statUrl = string.Format(league.BaseUrl+league.BoxScoreUrl, match.MatchNo);

            var (homeTeamStats, awayTeamStats) = await _euroleagueProcessor.GetBoxScore(statUrl, cancellationToken);
           
            List<BoxScoreDto> homePlayersStat = new List<BoxScoreDto>();
            List<BoxScoreDto> awayPlayersStat = new List<BoxScoreDto>();
            List<string> missingPlayerAtRoster = new List<string>();

            foreach (var homeTeamStat in homeTeamStats)
            {
                if (rosterHomeTeam.RosterItems.Any(x => x.Player.Name.Trim().ToLower(CultureInfo.InvariantCulture) ==
                                                        homeTeamStat.Name.Trim().ToLower(CultureInfo.InvariantCulture) && x.LeagueId == leagueId))
                {
                    var rosterItem = rosterHomeTeam.RosterItems.FirstOrDefault(x => x.Player.Name.Trim().ToLower(CultureInfo.InvariantCulture) == 
                        homeTeamStat.Name.Trim().ToLower(CultureInfo.InvariantCulture) && x.LeagueId == leagueId);
                    if (rosterItem == null)
                    {
                        missingPlayerAtRoster.Add(rosterHomeTeam.Name + " -> " + homeTeamStat.Name);
                        continue;
                    }
                    var newBoxScore = new BoxScoreDto(rosterItem.Id, match.Id, rosterItem.Player.Name, match.Round,
                        match.MatchNo, homeTeamStat.Minutes,
                        homeTeamStat.Points, homeTeamStat.ShotPrc, homeTeamStat.ShotMade2Pt,
                        homeTeamStat.ShotAttempted2Pt, homeTeamStat.ShotPrc2Pt, homeTeamStat.ShotMade3Pt,
                        homeTeamStat.ShotAttempted3Pt, homeTeamStat.shotPrc3Pt, homeTeamStat.ShotMade1Pt,
                        homeTeamStat.ShotAttempted1Pt, homeTeamStat.ShotPrc1Pt, homeTeamStat.DefensiveRebounds,
                        homeTeamStat.OffensiveRebounds, homeTeamStat.TotalRebounds, homeTeamStat.Assists,
                        homeTeamStat.Steals, homeTeamStat.Turnover, homeTeamStat.InFavoureOfBlock,
                        homeTeamStat.AgainstBlock, homeTeamStat.CommittedFoul, homeTeamStat.ReceivedFoul,
                        homeTeamStat.PointFromPain, homeTeamStat.PointFrom2ndChance, homeTeamStat.PointFromFastBreak,
                        homeTeamStat.PlusMinus, homeTeamStat.RankValue);
                    homePlayersStat.Add(newBoxScore);
                }
                else
                {
                    missingPlayerAtRoster.Add(rosterHomeTeam.Name + " -> " + homeTeamStat.Name);
                }

            }

            foreach (var awayTeamStat in awayTeamStats)
            {
                if (rosterAwayTeam.RosterItems.Any(x => x.Player.Name.Trim().ToLower(CultureInfo.InvariantCulture) ==
                                                        awayTeamStat.Name.Trim().ToLower(CultureInfo.InvariantCulture) && x.LeagueId == leagueId))
                {
                    var rosterItem = rosterAwayTeam.RosterItems.FirstOrDefault(x => x.Player.Name.Trim().ToLower(CultureInfo.InvariantCulture) == 
                        awayTeamStat.Name.Trim().ToLower(CultureInfo.InvariantCulture) && x.LeagueId == leagueId);
                    if (rosterItem == null)
                    {
                        missingPlayerAtRoster.Add(rosterAwayTeam.Name + " -> " + awayTeamStat.Name);
                        continue;
                    }
                    var newBoxScore = new BoxScoreDto(rosterItem.Id, match.Id, rosterItem.Player.Name, match.Round,
                        match.MatchNo, awayTeamStat.Minutes,
                        awayTeamStat.Points, awayTeamStat.ShotPrc, awayTeamStat.ShotMade2Pt,
                        awayTeamStat.ShotAttempted2Pt, awayTeamStat.ShotPrc2Pt, awayTeamStat.ShotMade3Pt,
                        awayTeamStat.ShotAttempted3Pt, awayTeamStat.shotPrc3Pt, awayTeamStat.ShotMade1Pt,
                        awayTeamStat.ShotAttempted1Pt, awayTeamStat.ShotPrc1Pt, awayTeamStat.DefensiveRebounds,
                        awayTeamStat.OffensiveRebounds, awayTeamStat.TotalRebounds, awayTeamStat.Assists,
                        awayTeamStat.Steals, awayTeamStat.Turnover, awayTeamStat.InFavoureOfBlock,
                        awayTeamStat.AgainstBlock, awayTeamStat.CommittedFoul, awayTeamStat.ReceivedFoul,
                        awayTeamStat.PointFromPain, awayTeamStat.PointFrom2ndChance, awayTeamStat.PointFromFastBreak,
                        awayTeamStat.PlusMinus, awayTeamStat.RankValue);
                    awayPlayersStat.Add(newBoxScore);
                }
                else
                {
                    missingPlayerAtRoster.Add(rosterAwayTeam.Name + " -> " + awayTeamStat.Name);
                }

            }


            homePlayersStat.AddRange(awayPlayersStat.AsEnumerable());
            return (homePlayersStat, missingPlayerAtRoster);
        }

        public Task<IEnumerable<(IEnumerable<BoxScoreDto> homePlayers, IEnumerable<BoxScoreDto> awayPlayers)>> AddScore(int leagueId, int matchId, IEnumerable<AddBoxScoreDto> homePlayers, IEnumerable<AddBoxScoreDto> awayPlayers,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<BoxScoreDto>> AddScore(int leagueId, int roundNo, IEnumerable<AddBoxScoreDto> playerScores, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            var matches = await _unitOfWork.CalendarRepository.SearchByRoundNo(leagueId, roundNo, cancellationToken);

            if (league == null || matches == null)
            {
                return Array.Empty<BoxScoreDto>();
            }

            List<BoxScoreDto> output = new List<BoxScoreDto>();
            foreach (var addBoxScoreDto in playerScores)
            {
                if (!await _unitOfWork.BoxScoreRepository.Exist(addBoxScoreDto.MatchRoundId, addBoxScoreDto.RosterItemId,
                        cancellationToken))
                {
                    var roundMatch =
                        await _unitOfWork.CalendarRepository.Get(addBoxScoreDto.MatchRoundId, cancellationToken);
                    var rosterItem =
                        await _unitOfWork.RosterRepository.Get(addBoxScoreDto.RosterItemId, cancellationToken);

                    var newItem = new BoxScore
                    {
                        RosterItem = rosterItem,
                        RosterItemId = rosterItem.Id,
                        RoundMatch = roundMatch,
                        RoundMatchId = roundMatch.Id,
                        Minutes = addBoxScoreDto.Minutes,
                        Points = addBoxScoreDto.Points,
                        ShotPrc = addBoxScoreDto.ShotPrc,
                        ShotMade2Pt = addBoxScoreDto.ShotMade2Pt,
                        ShotAttempted2Pt = addBoxScoreDto.ShotAttempted2Pt,
                        ShotPrc2Pt = addBoxScoreDto.ShotPrc2Pt,
                        ShotMade3Pt = addBoxScoreDto.ShotMade3Pt,
                        ShotAttempted3Pt = addBoxScoreDto.ShotAttempted3Pt,
                        shotPrc3Pt = addBoxScoreDto.shotPrc3Pt,
                        ShotMade1Pt = addBoxScoreDto.ShotMade1Pt,
                        ShotAttempted1Pt = addBoxScoreDto.ShotAttempted1Pt,
                        ShotPrc1Pt = addBoxScoreDto.ShotPrc1Pt,
                        DefensiveRebounds = addBoxScoreDto.DefensiveRebounds,
                        OffensiveRebounds = addBoxScoreDto.OffensiveRebounds,
                        TotalRebounds = addBoxScoreDto.TotalRebounds,
                        Assists = addBoxScoreDto.Assists,
                        Steals = addBoxScoreDto.Steals,
                        Turnover = addBoxScoreDto.Turnover,
                        InFavoureOfBlock = addBoxScoreDto.InFavoureOfBlock,
                        AgainstBlock = addBoxScoreDto.AgainstBlock,
                        CommittedFoul = addBoxScoreDto.CommittedFoul,
                        ReceivedFoul = addBoxScoreDto.ReceivedFoul,
                        PointFromPain = addBoxScoreDto.PointFromPain,
                        PointFrom2ndChance = addBoxScoreDto.PointFrom2ndChance,
                        PointFromFastBreak = addBoxScoreDto.PointFromFastBreak,
                        PlusMinus = addBoxScoreDto.PlusMinus,
                        RankValue = addBoxScoreDto.RankValue
                    };
                    await _unitOfWork.BoxScoreRepository.Add(newItem, cancellationToken);
                    await _unitOfWork.Save();

                    var outputItem = new BoxScoreDto(0, newItem.RoundMatchId, rosterItem.PlayerId.ToString(), roundMatch.Round, roundMatch.MatchNo);

                    output.Add(outputItem);
                }
            }

            return output;
        }

        public async Task<IEnumerable<BoxScoreDto>> AddScore(int leagueId, IEnumerable<AddBoxScoreDto> playerScores, CancellationToken cancellationToken = default)
        {
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            var rounds = playerScores.Select(x => x.Round).Distinct();
            foreach (var round in rounds)
            {
                var matches = await _unitOfWork.CalendarRepository.SearchByRoundNo(leagueId, round, cancellationToken);
                if (matches == null || !matches.Any())
                {
                    return Array.Empty<BoxScoreDto>();
                }
            }
            if (league == null)
            {
                return Array.Empty<BoxScoreDto>();
            }

            List<BoxScoreDto> output = new List<BoxScoreDto>();
            foreach (var addBoxScoreDto in playerScores)
            {
                if (!await _unitOfWork.BoxScoreRepository.Exist(addBoxScoreDto.MatchRoundId, addBoxScoreDto.RosterItemId,
                        cancellationToken))
                {
                    var roundMatch =
                        await _unitOfWork.CalendarRepository.Get(addBoxScoreDto.MatchRoundId, cancellationToken);
                    var rosterItem =
                        await _unitOfWork.RosterRepository.Get(addBoxScoreDto.RosterItemId, cancellationToken);

                    var newItem = new BoxScore
                    {
                        RosterItem = rosterItem,
                        RosterItemId = rosterItem.Id,
                        RoundMatch = roundMatch,
                        RoundMatchId = roundMatch.Id,
                        Minutes = addBoxScoreDto.Minutes,
                        Points = addBoxScoreDto.Points,
                        ShotPrc = addBoxScoreDto.ShotPrc,
                        ShotMade2Pt = addBoxScoreDto.ShotMade2Pt,
                        ShotAttempted2Pt = addBoxScoreDto.ShotAttempted2Pt,
                        ShotPrc2Pt = addBoxScoreDto.ShotPrc2Pt,
                        ShotMade3Pt = addBoxScoreDto.ShotMade3Pt,
                        ShotAttempted3Pt = addBoxScoreDto.ShotAttempted3Pt,
                        shotPrc3Pt = addBoxScoreDto.shotPrc3Pt,
                        ShotMade1Pt = addBoxScoreDto.ShotMade1Pt,
                        ShotAttempted1Pt = addBoxScoreDto.ShotAttempted1Pt,
                        ShotPrc1Pt = addBoxScoreDto.ShotPrc1Pt,
                        DefensiveRebounds = addBoxScoreDto.DefensiveRebounds,
                        OffensiveRebounds = addBoxScoreDto.OffensiveRebounds,
                        TotalRebounds = addBoxScoreDto.TotalRebounds,
                        Assists = addBoxScoreDto.Assists,
                        Steals = addBoxScoreDto.Steals,
                        Turnover = addBoxScoreDto.Turnover,
                        InFavoureOfBlock = addBoxScoreDto.InFavoureOfBlock,
                        AgainstBlock = addBoxScoreDto.AgainstBlock,
                        CommittedFoul = addBoxScoreDto.CommittedFoul,
                        ReceivedFoul = addBoxScoreDto.ReceivedFoul,
                        PointFromPain = addBoxScoreDto.PointFromPain,
                        PointFrom2ndChance = addBoxScoreDto.PointFrom2ndChance,
                        PointFromFastBreak = addBoxScoreDto.PointFromFastBreak,
                        PlusMinus = addBoxScoreDto.PlusMinus,
                        RankValue = addBoxScoreDto.RankValue
                    };
                    await _unitOfWork.BoxScoreRepository.Add(newItem, cancellationToken);
                    await _unitOfWork.Save();

                    var outputItem = new BoxScoreDto(0, newItem.RoundMatchId, rosterItem.PlayerId.ToString(), roundMatch.Round, roundMatch.MatchNo);

                    output.Add(outputItem);
                }
            }

            return output;
        }
    }
}
