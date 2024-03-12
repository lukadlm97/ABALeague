using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueComparisonByLeagueIds
{
    public class GetLeagueComparisonByLeagueIdsQueryHandler (IUnitOfWork _unitOfWork, 
                                                                IStatisticsCalculator _statisticsCalculator) : 
                    IQueryHandler<GetLeagueComparisonByLeagueIdsQuery, Maybe<LeagueCompareDto>>
    {

        public async Task<Maybe<LeagueCompareDto>> 
            Handle(GetLeagueComparisonByLeagueIdsQuery request, CancellationToken cancellationToken)
        {
            var leagues =  _unitOfWork.LeagueRepository.Get()
                                        .Where(x => request.LeagueIds.Contains(x.Id))
                                        .ToList();

            List<LeagueCompareItemDto> list = new List<LeagueCompareItemDto>();
            foreach(var league in leagues)
            {
                var rosterItemIds = _unitOfWork.RosterRepository.Get()
                                                .Where(x => x.LeagueId == league.Id)
                                                .Select(x => x.Id)
                                                .ToList();

                var teamIds = _unitOfWork.RosterRepository.Get()
                                                .Where(x => x.LeagueId == league.Id)
                                                .Select(x => x.TeamId)
                                                .ToList();

                var boxscores = _unitOfWork.BoxScoreRepository.GetWithRosterItemDetailsIncluded()
                                            .Where(x => rosterItemIds.Contains(x.RosterItemId))
                                            .ToList();

                var performanceByPosition = _statisticsCalculator.Calcualate(boxscores,
                                           new List<PositionEnum>()
                                           {
                                                PositionEnum.Guard,
                                                PositionEnum.Forward,
                                                PositionEnum.ShootingGuard,
                                                PositionEnum.PowerForward,
                                                PositionEnum.Center
                                           }.ToFrozenSet());

                list.Add(new LeagueCompareItemDto(league.Id,
                                                    league.OfficalName,
                                                    boxscores.Sum(x => x.Points) ?? 0,
                                                    Math.Round((decimal)boxscores.Where(x => x.Minutes != null).Average(x => x.Points), 2),
                                                    boxscores.Sum(x => x.TotalRebounds) ?? 0,
                                                    Math.Round((decimal)boxscores.Where(x => x.Minutes != null).Average(x => x.TotalRebounds), 2),
                                                    boxscores.Sum(x => x.Assists) ?? 0,
                                                    Math.Round((decimal)boxscores.Where(x => x.Minutes != null).Average(x => x.Assists), 2),
                                                    boxscores.Sum(x => x.Turnover) ?? 0,
                                                    Math.Round((decimal)boxscores.Where(x => x.Minutes != null).Average(x => x.Turnover), 2),
                                                    boxscores.Sum(x => x.Steals) ?? 0,
                                                    Math.Round((decimal)boxscores.Where(x => x.Minutes != null).Average(x => x.Steals), 2),
                                                    boxscores.Sum(x => x.OffensiveRebounds) ?? 0,
                                                    Math.Round((decimal)boxscores.Where(x => x.Minutes != null).Average(x => x.OffensiveRebounds), 2),
                                                    boxscores.Sum(x => x.DefensiveRebounds) ?? 0,
                                                    Math.Round((decimal)boxscores.Where(x => x.Minutes != null).Average(x => x.DefensiveRebounds), 2),
                                                    boxscores.Sum(x => x.AgainstBlock) ?? 0,
                                                    Math.Round((decimal)boxscores.Where(x => x.Minutes != null).Average(x => x.AgainstBlock), 2),
                                                    boxscores.Sum(x => x.InFavoureOfBlock) ?? 0,
                                                    Math.Round((decimal)boxscores.Where(x => x.Minutes != null).Average(x => x.InFavoureOfBlock), 2),
                                                    boxscores.Select(x => x.RoundMatchId).Distinct().Count(),
                                                    league.RoundsToPlay*(teamIds.Distinct().Count()/2) ?? 0,
                                                    performanceByPosition));
            }
            return new LeagueCompareDto(list.ToFrozenSet());
        }
    }
}
