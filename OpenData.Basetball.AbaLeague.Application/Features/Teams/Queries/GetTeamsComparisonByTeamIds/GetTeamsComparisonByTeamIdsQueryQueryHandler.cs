using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Common;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Application.Utilities;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamsComparisonByTeamIds
{
    public class GetTeamsComparisonByTeamIdsQueryQueryHandler(IUnitOfWork _unitOfWork, 
                                                                IStatisticsCalculator _statisticsCalculator,
                                                                IGameService _gameService)
        : IQueryHandler<GetTeamsComparisonByTeamIdsQuery, Maybe<TeamCompareDto>>
    {
        public async Task<Maybe<TeamCompareDto>> 
            Handle(GetTeamsComparisonByTeamIdsQuery request, CancellationToken cancellationToken)
        {
            var teams = _unitOfWork.TeamRepository.GetWithCountry()
                                    .Where(x => request.TeamAndLeagueIds.Select(x=>x.teamId).Contains(x.Id))
                                    .ToList();

            var list = new List<TeamCompareItemDto>();
            foreach(var team in teams)
            {
                var leagueIdItem = request.TeamAndLeagueIds?.FirstOrDefault(x=>x.teamId== team.Id)!.leagueId;
                if(leagueIdItem == null)
                {
                    throw new NotImplementedException();
                }
                var rosterItems = _unitOfWork.RosterRepository.Get()
                                                .Where(x => x.TeamId == team.Id &&
                                                            x.LeagueId == leagueIdItem).ToList();
                var rosterItemIds = rosterItems.Select(x => x.Id)
                                                .ToList();

                var boxscores = _unitOfWork.BoxScoreRepository.GetWithRosterItemDetailsIncluded()
                                            .Where(x => rosterItemIds.Contains(x.RosterItemId))
                                            .ToList();

                var performanceByPosition = _statisticsCalculator.CalcualatePerPosition(boxscores,
                                           new List<PositionEnum>()
                                           {
                                                PositionEnum.Guard,
                                                PositionEnum.Forward,
                                                PositionEnum.ShootingGuard,
                                                PositionEnum.PowerForward,
                                                PositionEnum.Center
                                           }.ToFrozenSet());

                var totalAndAveragePerformance = _statisticsCalculator
                                                .CalcualteTotalAndAveragePerformance(boxscores.ToFrozenSet());
                
                var results = await _unitOfWork.ResultRepository.SearchByLeague(leagueIdItem ?? 0, cancellationToken);
                var advancedMatchStats = _gameService.CalculateAdvancedMatch(results.ToFrozenSet(), team.Id);

                var playerIds = _unitOfWork.RosterRepository.Get()
                                          .Where(x => rosterItemIds.Contains(x.Id))
                                          .Select(x => x.PlayerId)
                                          .ToList();
                var players = _unitOfWork.PlayerRepository.Get()
                                        .Where(x => playerIds.Contains(x.Id))
                                        .ToList();

                var playersByPositions = players.GroupBy(x => x.PositionEnum)
                    .Select(y => (y.Key,
                        y.Select(z =>
                        new RosterItemDto(z.Id,
                                    rosterItems.Select(x=>x.LeagueId).Distinct().FirstOrDefault(),
                        rosterItems.FirstOrDefault(x => x.PlayerId == z.Id).DateOfInsertion,
                                    rosterItems.FirstOrDefault(x => x.PlayerId == z.Id).EndOfActivePeriod,
                                    z.Name,
                                    z.PositionEnum,
                                    z.Height,
                                    z.DateOfBirth,
                                      DistanceCalculator
                                    .CalculateAge(DateOnly.FromDateTime(z.DateOfBirth), DateOnly.FromDateTime(DateTime.UtcNow)),
                                    z.CountryId,
                                    string.Empty)
                                    ).ToFrozenSet())
                        ).ToDictionary();
                list.Add(new TeamCompareItemDto
                (
                    new TeamItemDto(team.Id, team.Name, team.ShortCode, team.CountryId, team.Country?.Name ?? string.Empty),
                    performanceByPosition,
                    playersByPositions.ToFrozenDictionary(),
                    totalAndAveragePerformance,
                    advancedMatchStats
                ));
            }

            return new TeamCompareDto(list.ToFrozenSet());
        }
    }
}
