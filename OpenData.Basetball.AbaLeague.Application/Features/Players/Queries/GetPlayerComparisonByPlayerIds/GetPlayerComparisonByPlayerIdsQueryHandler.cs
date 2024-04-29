using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Application.DTOs.Statistic;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetAvailableRosterHistoryByTeamId;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Application.Utilities;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayerComparisonByPlayerIds
{
    public class GetPlayerComparisonByPlayerIdsQueryHandler(IUnitOfWork _unitOfWork, 
                                                            IStatisticsCalculator _statisticsCalculator,
                                                            IGameService _gameService) :
        IQueryHandler<GetPlayerComparisonByPlayerIdsQuery, Maybe<PlayerCompareDto>>
    {
        public async Task<Maybe<PlayerCompareDto>> 
            Handle(GetPlayerComparisonByPlayerIdsQuery request, CancellationToken cancellationToken)
        {
            var league = _unitOfWork.LeagueRepository
                                    .Get()
                                    .FirstOrDefault(x=>x.Id == request.LeagueId);
            if(league == null)
            {
                return Maybe<PlayerCompareDto>.None;
            }

            var rosterItemsByLeague = _unitOfWork.RosterRepository
                                                    .GetWithLeagueAndTeam()
                                                    .Where(x => x.LeagueId == request.LeagueId).ToList();

            List<(int rosterItemId, int playerId, int teamId, Player player)> rosterItemPlayerAndTeamIds = 
                                                        new List<(int rosterItemId, int playerId, int teamId, Player player)>();   
            foreach(var (teamId, playerId) in request.RosterDestination)
            {
                if(rosterItemsByLeague.Any(x => x.TeamId == teamId && x.PlayerId == playerId))
                {
                    var rosterItem = rosterItemsByLeague.FirstOrDefault(x => x.TeamId == teamId && x.PlayerId == playerId);
                    if(rosterItem != null)
                    {
                        rosterItemPlayerAndTeamIds.Add((rosterItem.Id, playerId, teamId, rosterItem.Player));
                    }
                }
            }

            List<PlayerCompareItemDto> list= new List<PlayerCompareItemDto>();
            TotalAndAveragePerformanceDto totalAndAveragePerformanceDto = null;
            foreach((int rosterItemId, int playerId, int teamId, Player player) in rosterItemPlayerAndTeamIds)
            {
                var boxscores = await _unitOfWork.BoxScoreRepository
                                                    .GetByRosterItemId(rosterItemId, cancellationToken);
                if (boxscores == null)
                {
                    continue;
                }
                totalAndAveragePerformanceDto = _statisticsCalculator
                                .CalcualteTotalAndAveragePerformance(boxscores.ToFrozenSet());

                var results = await _unitOfWork.ResultRepository
                            .SearchByLeague(request.LeagueId, cancellationToken);
                var resultWherePlayed = results
                    .Where(res => boxscores
                                    .Select(x => x.RoundMatchId).Contains(res.RoundMatchId)); 

                var advancedMatchStats = _gameService
                                                .CalculateAdvancedMatch(resultWherePlayed.ToFrozenSet(), teamId);

                list.Add(new PlayerCompareItemDto(teamId, 
                    string.Empty, 
                    new PlayerItemDto(player.Id, 
                                        player.Name, 
                                        player.PositionEnum, 
                                        player.Height, 
                                        player.DateOfBirth, 
                                        DistanceCalculator
                                        .CalculateAge(DateOnly.FromDateTime(player.DateOfBirth), 
                                                        DateOnly.FromDateTime(DateTime.UtcNow)), 
                                        player?.Country?.Id??0, 
                                        player?.Country?.Name), 
                    totalAndAveragePerformanceDto, 
                    advancedMatchStats));
            }
            return new PlayerCompareDto(league.Id, league.OfficalName, list.ToFrozenSet());
        }
    }
}
