using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreByPlayerIdAndLeagueId
{
    public class GetBoxscoreByPlayerIdTeamIdAndLeagueIdQueryHandler :
        IQueryHandler<GetBoxscoreByPlayerIdTeamIdAndLeagueIdQuery, Maybe<BoxscoreByPlayerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBoxscoreByPlayerIdTeamIdAndLeagueIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<BoxscoreByPlayerDto>> 
            Handle(GetBoxscoreByPlayerIdTeamIdAndLeagueIdQuery request, CancellationToken cancellationToken)
        {
            var player = await _unitOfWork.PlayerRepository.Get(request.PlayerId, cancellationToken);
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            var team = await _unitOfWork.TeamRepository.Get(request.TeamId, cancellationToken);
            var results  = await _unitOfWork.ResultRepository.GetAll(cancellationToken);
            var rosterItem = await _unitOfWork.RosterRepository
                .Get(request.LeagueId, request.PlayerId, request.TeamId, cancellationToken);

            if(rosterItem == null || player== null || league ==null) 
            {
                return Maybe<BoxscoreByPlayerDto>.None;
            }
            var boxscores = await _unitOfWork.BoxScoreRepository.GetByRosterItemId(rosterItem.Id, cancellationToken);

            if(boxscores == null || !boxscores.Any())
            {
                return Maybe<BoxscoreByPlayerDto>.None;
            }
            List<GameStatsByPlayerItemDto> list = new List<GameStatsByPlayerItemDto>();
            int homeGamesPlayed = 0;
            int homeGamesWon = 0;
            int totalGamesPlayed = 0;
            int totalGamesWon = 0;
            int totalSpectators = 0;
            int homeGamesPointScored = 0;
            int awayGamesPointScored = 0;
         
            foreach(var score in boxscores)
            {
                var matchResult = results.FirstOrDefault(x => x.RoundMatchId == score.RoundMatchId);
                if (matchResult == null)
                {
                    continue;
                }
                totalGamesPlayed++;
                var isHomeGame = score.RoundMatch.HomeTeamId == rosterItem.TeamId;
                bool winTheGame = false;
                if (isHomeGame)
                {
                    homeGamesPlayed++;
                    winTheGame = matchResult.HomeTeamPoints > matchResult.AwayTeamPoint;
                    if (winTheGame)
                    {
                        homeGamesWon++;
                        totalGamesWon++;
                    }
                }
                else
                {
                    winTheGame = matchResult.HomeTeamPoints < matchResult.AwayTeamPoint;
                    if (winTheGame)
                    {
                        totalGamesWon++;
                    }
                }
                totalSpectators += matchResult.Attendency;
                homeGamesPointScored += isHomeGame ? score.Points??0:0;
                awayGamesPointScored += !isHomeGame ? score.Points ?? 0 :0;
                var result = $"{matchResult.HomeTeamPoints}:{matchResult.AwayTeamPoint}";
                list.Add(new GameStatsByPlayerItemDto(matchResult.RoundMatchId,
                    isHomeGame ?  score.RoundMatch.AwayTeam.Id: score.RoundMatch.HomeTeam.Id ,
                    isHomeGame ? score.RoundMatch.AwayTeam.Name: score.RoundMatch.HomeTeam.Name , 
                    score.RoundMatch.DateTime, 
                    score.RoundMatch.Round,
                    score.RoundMatch.MatchNo,
                    isHomeGame,
                    winTheGame,
                    matchResult.Venue, 
                    matchResult.Attendency, 
                    score.Minutes,
                    score.Points,
                    score.ShotPrc,
                    score.ShotMade2Pt,
                    score.ShotAttempted2Pt,
                    score.ShotPrc2Pt,
                    score.ShotMade3Pt,
                     score.ShotAttempted3Pt,
                    score.shotPrc3Pt,
                    score.ShotMade1Pt,
                     score.ShotAttempted1Pt,
                    score.ShotPrc1Pt,
                    score.DefensiveRebounds,
                    score.OffensiveRebounds,
                    score.TotalRebounds, 
                    score.Assists, 
                    score.Steals,
                    score.Turnover, 
                    score.InFavoureOfBlock, 
                    score.AgainstBlock, 
                    score.CommittedFoul, 
                    score.ReceivedFoul,
                    score.PointFromPain,
                    score.PointFrom2ndChance,
                    score.PointFromFastBreak, 
                    score.PlusMinus,
                    score.RankValue,
                    result));
            }

            var advancedCalculation = new AdvancedMatchCalcuationDto(totalGamesPlayed, homeGamesPlayed, totalGamesWon, homeGamesWon, totalSpectators, totalSpectators / totalGamesPlayed, homeGamesPointScored, awayGamesPointScored);
            var averageCalculation = new AverageBoxscoreCalcuationDto(
                GetTimeSpanAverage(boxscores.Select(x => x.Minutes??TimeSpan.FromSeconds(0)).ToList())??TimeSpan.FromSeconds(0),
                Math.Round(boxscores.Average(x => x.Points) ?? 0,2),
                Math.Round(boxscores.Average(x => x.TotalRebounds) ?? 0, 2),
                Math.Round(boxscores.Average(x => x.Assists) ?? 0, 2),
                Math.Round(boxscores.Average(x => x.Steals) ?? 0, 2),
                Math.Round(boxscores.Average(x => x.Turnover) ?? 0, 2),
                Math.Round(boxscores.Average(x => x.PlusMinus) ?? 0, 2),
                Math.Round(boxscores.Average(x => x.RankValue) ?? 0, 2));
            return new BoxscoreByPlayerDto(request.PlayerId, 
                                            player.Name, 
                                            rosterItem.Team.Id, 
                                            rosterItem.Team.Name,
                                            league.Id, 
                                            league.OfficalName,
                                            list,
                                            averageCalculation, 
                                            advancedCalculation);
        }

        private TimeSpan? GetTimeSpanAverage(List<TimeSpan> sourceList)
        {
            double doubleAverageTicks = sourceList.Average(timeSpan => timeSpan.Ticks);
            long longAverageTicks = Convert.ToInt64(doubleAverageTicks);

            return new TimeSpan(longAverageTicks);
        }
    }
}
