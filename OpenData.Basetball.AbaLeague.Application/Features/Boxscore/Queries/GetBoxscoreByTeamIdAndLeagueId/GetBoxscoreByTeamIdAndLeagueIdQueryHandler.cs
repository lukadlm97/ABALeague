﻿using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreByTeamIdAndLeagueId
{
    public class GetBoxscoreByTeamIdAndLeagueIdQueryHandler :
        IQueryHandler<GetBoxscoreByTeamIdAndLeagueIdQuery, Maybe<BoxscoreByTeamAndLeagueDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBoxscoreByTeamIdAndLeagueIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<BoxscoreByTeamAndLeagueDto>> 
            Handle(GetBoxscoreByTeamIdAndLeagueIdQuery request, CancellationToken cancellationToken)
        {
            var team = await _unitOfWork.TeamRepository.Get(request.TeamId, cancellationToken);
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            var boxscores = await _unitOfWork.BoxScoreRepository.GetAll(cancellationToken);
            var roundMatches = await _unitOfWork.CalendarRepository.SearchByLeague(request.LeagueId, cancellationToken);
            var results = await _unitOfWork.ResultRepository.SearchByLeague(request.LeagueId, cancellationToken);
            if(team == null || league == null || boxscores == null) 
            {
                return Maybe<BoxscoreByTeamAndLeagueDto>.None;
            }

            var rosterItems = await _unitOfWork.RosterRepository
                .GetTeamRosterByLeagueId(request.LeagueId, request.TeamId, cancellationToken);
            roundMatches = roundMatches.Where(x=> x.AwayTeamId == request.TeamId || x.HomeTeamId == request.TeamId);

            List<GameStats> list = new List<GameStats>();
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
            foreach (var item in roundMatches)
            {
                var matchResult = results.FirstOrDefault(x => x.RoundMatchId == item.Id);
                if (matchResult == null)
                {
                    continue;
                }
                var selectedBoxscores = boxscores
                    .Where(x => x.RoundMatchId == item.Id && 
                                rosterItems.Select(x=>x.Id).Contains(x.RosterItemId));
                var teamId = rosterItems.Select(x => x.TeamId).Distinct().First();
                if(item.HomeTeamId == teamId)
                {
                    homeGame = true;
                    oponentId = item.AwayTeamId??0;
                    oponentName= item.AwayTeam.Name;
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
                    wonTheGame = matchResult.HomeTeamPoints < matchResult.AwayTeamPoint;
                    awayGamesPointScored += matchResult.AwayTeamPoint;
                }
                totalGamesPlayed++;
                totalGamesWon += wonTheGame ? 1 : 0;
                totalSpectators += matchResult.Attendency;
                var avg1pt = Math.Round((decimal)selectedBoxscores.Sum(x => x.ShotMade1Pt) * 100 /
                    (selectedBoxscores.Sum(x => x.ShotAttempted1Pt) is null or 0 ? 
                    1 : 
                    selectedBoxscores.Sum(x => x.ShotAttempted1Pt)??1), 2);

                var avg2pt = Math.Round((decimal)selectedBoxscores.Sum(x => x.ShotMade2Pt) * 100 /
                    (selectedBoxscores.Sum(x => x.ShotAttempted2Pt) is null or 0 ?
                    1 :
                    selectedBoxscores.Sum(x => x.ShotAttempted2Pt) ?? 1), 2);
                var avg3pt = Math.Round((decimal)selectedBoxscores.Sum(x => x.ShotMade3Pt) * 100 /
                    (selectedBoxscores.Sum(x => x.ShotAttempted3Pt) is null or 0 ?
                    1 :
                    selectedBoxscores.Sum(x => x.ShotAttempted3Pt) ?? 1), 2);
                list.Add(new GameStats(oponentId,
                  oponentName,
                  item.DateTime,
                  item.Round,
                  homeGame,
                  wonTheGame,
                  matchResult.Venue,
                  matchResult.Attendency,
                  item.MatchNo,
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
                  $"{matchResult.HomeTeamPoints}:{matchResult.AwayTeamPoint}"
                  ));


            }
            var subsetBoxscores = boxscores
                  .Where(x => rosterItems.Select(x => x.Id).Contains(x.RosterItemId));

            var advancedCalculation = new AdvancedMatchCalcuationDto(totalGamesPlayed??0, homeGamesPlayed, totalGamesWon, homeGamesWon, totalSpectators, totalSpectators / totalGamesPlayed == 0?1:totalGamesPlayed??1, homeGamesPointScored, awayGamesPointScored);
            var averageCalculation = new AverageBoxscoreCalcuationDto(
                TimeSpan.FromSeconds(0),
                Math.Round((double)subsetBoxscores.Sum(x => x.Points)/ totalGamesPlayed  ?? 1, 2),
                Math.Round((double)subsetBoxscores.Sum(x => x.TotalRebounds) / totalGamesPlayed ?? 1, 2),
                Math.Round((double)subsetBoxscores.Sum(x => x.Assists) / totalGamesPlayed ?? 1, 2),
                Math.Round((double)subsetBoxscores.Sum(x => x.Steals) / totalGamesPlayed ?? 1, 2),
                Math.Round((double)subsetBoxscores.Sum(x => x.Turnover) / totalGamesPlayed ?? 1, 2),
                Math.Round((double)subsetBoxscores.Sum(x => x.PlusMinus) / totalGamesPlayed ?? 1, 2),
                Math.Round( (double)subsetBoxscores.Sum(x => x.RankValue) / totalGamesPlayed ?? 1, 2));
            return new BoxscoreByTeamAndLeagueDto(team.Id, team.Name, league.Id, league.OfficalName, list, averageCalculation, advancedCalculation);
        }
    }
}