using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Application.DTOs.Game;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Services.Implementation
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStandingsService _standingsService;

        public GameService(IUnitOfWork  unitOfWork, IStandingsService standingsService)
        {
            _unitOfWork = unitOfWork;
            _standingsService = standingsService;
        }

       

        public async Task<(int? teamId,
                string? teamName,
                int? leagueId,
                string? leagueName,
                IEnumerable<GameStatsByTeamItemDto> games,
                AdvancedMatchCalcuationDto? advancedMatchCalcuation,
                AverageBoxscoreCalcuationDto? averageBoxscoreCalcuation)>
            GetPlayedByLeagueIdAndTeamId(int leagueId,
                                    int teamId,
                                    bool includeAdvancedCalculation = false,
                                    CancellationToken cancellationToken = default)
        {
            var team = await _unitOfWork.TeamRepository.Get(teamId, cancellationToken);
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            var boxscores = await _unitOfWork.BoxScoreRepository.GetAll(cancellationToken);
            var roundMatches = await _unitOfWork.CalendarRepository.SearchByLeague(leagueId, cancellationToken);
            var results = await _unitOfWork.ResultRepository.SearchByLeague(leagueId, cancellationToken);
            if (team == null || league == null || boxscores == null)
            {
                return (null, null, null, null, null, null, null);
            }

            var rosterItems = await _unitOfWork.RosterRepository
                .GetTeamRosterByLeagueId(leagueId, teamId, cancellationToken);
            roundMatches = roundMatches.Where(x => x.AwayTeamId == teamId || x.HomeTeamId == teamId);

            List<GameStatsByTeamItemDto> list = new List<GameStatsByTeamItemDto>();
            bool homeGame = false;
            int oponentId = 0;
            string oponentName = string.Empty;
            bool wonTheGame = false;
            int homeGamesPlayed = 0;
            int homeGamesWon = 0;
            int homeGamesPointScored = 0;
            int? totalGamesPlayed = 0;
            int totalGamesWon = 0;
            int totalSpectators = 0;
            int awayGamesPointScored = 0;
            int? oponentScore = null;
            foreach (var item in roundMatches)
            {
                var matchResult = results.FirstOrDefault(x => x.RoundMatchId == item.Id);
                if (matchResult == null)
                {
                    continue;
                }
                var selectedBoxscores = boxscores
                    .Where(x => x.RoundMatchId == item.Id &&
                                rosterItems.Select(x => x.Id).Contains(x.RosterItemId));
                var selectTeamId = rosterItems.Select(x => x.TeamId).Distinct().First();
                if (item.HomeTeamId == selectTeamId)
                {
                    homeGame = true;
                    oponentId = item.AwayTeamId ?? 0;
                    oponentName = item.AwayTeam.Name;
                    oponentScore = matchResult.AwayTeamPoint;
                    wonTheGame = matchResult.HomeTeamPoints > matchResult.AwayTeamPoint;
                    homeGamesPlayed++;
                    homeGamesWon += wonTheGame ? 1 : 0;
                    homeGamesPointScored += matchResult.HomeTeamPoints;
                }
                else
                {
                    homeGame = false;
                    oponentId = item.HomeTeamId ?? 0;
                    oponentName = item.HomeTeam.Name;
                    oponentScore = matchResult.HomeTeamPoints;
                    wonTheGame = matchResult.HomeTeamPoints < matchResult.AwayTeamPoint;
                    awayGamesPointScored += matchResult.AwayTeamPoint;
                }
                totalGamesPlayed++;
                totalGamesWon += wonTheGame ? 1 : 0;
                totalSpectators += matchResult.Attendency;
                var avg1pt = Math.Round((decimal)selectedBoxscores.Sum(x => x.ShotMade1Pt) * 100 /
                    (selectedBoxscores.Sum(x => x.ShotAttempted1Pt) is null or 0 ?
                    1 :
                    selectedBoxscores.Sum(x => x.ShotAttempted1Pt) ?? 1), 2);

                var avg2pt = Math.Round((decimal)selectedBoxscores.Sum(x => x.ShotMade2Pt) * 100 /
                    (selectedBoxscores.Sum(x => x.ShotAttempted2Pt) is null or 0 ?
                    1 :
                    selectedBoxscores.Sum(x => x.ShotAttempted2Pt) ?? 1), 2);
                var avg3pt = Math.Round((decimal)selectedBoxscores.Sum(x => x.ShotMade3Pt) * 100 /
                    (selectedBoxscores.Sum(x => x.ShotAttempted3Pt) is null or 0 ?
                    1 :
                    selectedBoxscores.Sum(x => x.ShotAttempted3Pt) ?? 1), 2);
                list.Add(new GameStatsByTeamItemDto(item.Id,
                   oponentId,
                  oponentName,
                  item.DateTime,
                  item.Round,
                  item.MatchNo,
                  homeGame,
                  wonTheGame,
                  matchResult.Venue,
                  matchResult.Attendency,
                  matchResult.Id,
                  null,
                  selectedBoxscores.Sum(x => x.Points),
                  null,
                  selectedBoxscores.Sum(x => x.ShotMade2Pt),
                  selectedBoxscores.Sum(x => x.ShotAttempted2Pt),
                  avg2pt,
                  selectedBoxscores.Sum(x => x.ShotMade3Pt),
                  selectedBoxscores.Sum(x => x.ShotAttempted3Pt),
                  avg3pt,
                  selectedBoxscores.Sum(x => x.ShotMade1Pt),
                  selectedBoxscores.Sum(x => x.ShotAttempted1Pt),
                  avg1pt,
                  selectedBoxscores.Sum(x => x.DefensiveRebounds),
                  selectedBoxscores.Sum(x => x.OffensiveRebounds),
                  selectedBoxscores.Sum(x => x.TotalRebounds),
                  selectedBoxscores.Sum(x => x.Assists),
                  selectedBoxscores.Sum(x => x.Steals),
                  selectedBoxscores.Sum(x => x.Turnover),
                  selectedBoxscores.Sum(x => x.InFavoureOfBlock),
                  selectedBoxscores.Sum(x => x.AgainstBlock),
                  selectedBoxscores.Sum(x => x.CommittedFoul),
                  selectedBoxscores.Sum(x => x.ReceivedFoul),
                  selectedBoxscores.Sum(x => x.PointFromPain),
                  selectedBoxscores.Sum(x => x.PointFrom2ndChance),
                  selectedBoxscores.Sum(x => x.PointFromFastBreak),
                  selectedBoxscores.Sum(x => x.PlusMinus),
                  selectedBoxscores.Sum(x => x.RankValue),
                  $"{matchResult.HomeTeamPoints}:{matchResult.AwayTeamPoint}",
                  oponentScore
                  ));
            }
            if (!includeAdvancedCalculation)
            {
                return (team.Id, team.Name, league.Id, league.OfficalName, list, null, null);
            }

            var subsetBoxscores = boxscores
                 .Where(x => rosterItems.Select(x => x.Id).Contains(x.RosterItemId));

            var advancedCalculation = new AdvancedMatchCalcuationDto(totalGamesPlayed ?? 0, homeGamesPlayed, totalGamesWon, homeGamesWon, totalSpectators, totalSpectators / totalGamesPlayed == 0 ? 1 : totalGamesPlayed ?? 1, homeGamesPointScored, awayGamesPointScored);
            var averageCalculation = new AverageBoxscoreCalcuationDto(
                
                TimeSpan.FromSeconds(0),
                Math.Round((double)subsetBoxscores.Sum(x => x.Points) / totalGamesPlayed ?? 1, 2),
                Math.Round((double)subsetBoxscores.Sum(x => x.TotalRebounds) / totalGamesPlayed ?? 1, 2),
                Math.Round((double)subsetBoxscores.Sum(x => x.Assists) / totalGamesPlayed ?? 1, 2),
                Math.Round((double)subsetBoxscores.Sum(x => x.Steals) / totalGamesPlayed ?? 1, 2),
                Math.Round((double)subsetBoxscores.Sum(x => x.Turnover) / totalGamesPlayed ?? 1, 2),
                Math.Round((double)subsetBoxscores.Sum(x => x.PlusMinus) / totalGamesPlayed ?? 1, 2),
                Math.Round((double)subsetBoxscores.Sum(x => x.RankValue) / totalGamesPlayed ?? 1, 2));

            return (team.Id, team.Name, league.Id, league.OfficalName, list, advancedCalculation, averageCalculation);
        }

        public async Task<(int? teamId, 
                            string? teamName, 
                            int? leagueId,
                            string? leagueName, 
                            IEnumerable<MatchItemDto> games)> 
            GetScheduledByLeagueIdAndTeamId(int leagueId, 
                                int teamId,
                                bool includePlayed = false, 
                                CancellationToken cancellationToken = default)
        {
            var team = await _unitOfWork.TeamRepository.Get(teamId, cancellationToken);
            var league = await _unitOfWork.LeagueRepository.Get(leagueId, cancellationToken);
            var roundMatches = await _unitOfWork.CalendarRepository.SearchByLeague(leagueId, cancellationToken); 
            var results = await _unitOfWork.ResultRepository.SearchByLeague(leagueId, cancellationToken);
            var currentStanding = await _standingsService.GetByLeagueId(leagueId, cancellationToken);
            if (team == null || league == null || currentStanding == null || !currentStanding.Any())
            {
                return (null, null, null, null, null);
            }

            if (!includePlayed)
            {
                roundMatches = roundMatches.Where(x => x.DateTime > DateTime.UtcNow);
            }
            roundMatches = roundMatches.Where(x => x.AwayTeamId == teamId || x.HomeTeamId == teamId);

            List<MatchItemDto> matchesOnSchedule = new List<MatchItemDto>();  
            bool homeGame = false;
            int oponentId = 0;
            string oponentName = string.Empty;
            var standingList = currentStanding.ToList();
            foreach (var item in roundMatches)
            {
                var matchResult = results.FirstOrDefault(x => x.RoundMatchId == item.Id);
                if (matchResult != null)
                {
                    continue;
                }

                if (item.HomeTeamId == teamId)
                {
                    homeGame = true;
                    oponentId = item.AwayTeamId ?? 0;
                    oponentName = item.AwayTeam.Name;
                }
                else
                {
                    homeGame = false;
                    oponentId = item.HomeTeamId ?? 0;
                    oponentName = item.HomeTeam.Name;
                }
                var currentStandingOfOponent = currentStanding.ToList().FirstOrDefault(x => x.TeamId == oponentId);
                int oponentCurrentRanking = -1;
                IEnumerable<bool> oponentRecentForm = new List<bool>();
                if(currentStandingOfOponent != null)
                {
                    oponentCurrentRanking = standingList.IndexOf(currentStandingOfOponent) + 1;
                    oponentRecentForm = currentStandingOfOponent.RecentForm;
                }

                matchesOnSchedule.Add(new MatchItemDto(item.Id,
                                                        oponentId, 
                                                        oponentName,
                                                        item.DateTime,
                                                        item.Round, 
                                                        item.MatchNo, 
                                                        homeGame,
                                                        oponentCurrentRanking,
                                                        oponentRecentForm));

            }



            return (team.Id, team.Name, league.Id, league.OfficalName, matchesOnSchedule);
        }

        public AdvancedMatchCalcuationDto? 
            CalculateAdvancedMatch(FrozenSet<Domain.Entities.Result> results, int teamId)
        {
            int homeGamesPlayed = 0;
            int homeGamesWon = 0;
            int homeGamesPointScored = 0;
            int? totalGamesPlayed = 0;
            int totalGamesWon = 0;
            int totalSpectators = 0;
            int awayGamesPointScored = 0;
            bool wonTheGame = false;
            foreach (var item in results)
            {
                var roundMatch = item.RoundMatch;
                if (roundMatch == null || (roundMatch.HomeTeamId != teamId && roundMatch.AwayTeamId != teamId))
                {
                    continue;
                }

                if (roundMatch.HomeTeamId == teamId)
                {
                    wonTheGame = item.HomeTeamPoints > item.AwayTeamPoint;
                    homeGamesPlayed++;
                    homeGamesWon += wonTheGame ? 1 : 0;
                    homeGamesPointScored += item.HomeTeamPoints;
                }
                else
                {
                    wonTheGame = item.HomeTeamPoints < item.AwayTeamPoint;
                    awayGamesPointScored += item.AwayTeamPoint;
                }
                totalGamesPlayed++;
                totalGamesWon += wonTheGame ? 1 : 0;
                totalSpectators += item.Attendency;
            }

            return new AdvancedMatchCalcuationDto(totalGamesPlayed ?? 0, 
                                                    homeGamesPlayed, 
                                                    totalGamesWon, 
                                                    homeGamesWon, 
                                                    totalSpectators, 
                                                    totalSpectators / (totalGamesPlayed == null 
                                                    ? 
                                                    1 
                                                    :
                                                    totalGamesPlayed)??0, 
                                                    homeGamesPointScored, 
                                                    awayGamesPointScored);

        }
    }
}
