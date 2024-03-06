using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Statistic;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Statistics.Queries.GetByPositionsPerLeague
{
    public class GetByPositionsPerLeagueQueryHandler(IUnitOfWork _unitOfWork,
                                                     IStatisticsCalculator _statisticsCalculator) 
        : IQueryHandler<GetByPositionsPerLeagueQuery, Maybe<CategoriesByPositionPerLeagueDto>>
    {

        public async Task<Maybe<CategoriesByPositionPerLeagueDto>> 
            Handle(GetByPositionsPerLeagueQuery request, 
                    CancellationToken cancellationToken)
        {
            var league = _unitOfWork.LeagueRepository.Get()
                                    .FirstOrDefault(x => x.Id == request.LeagueId);
            if (league == null)
            {
                return Maybe<CategoriesByPositionPerLeagueDto>.None;
            }

            var boxscore = _unitOfWork.BoxScoreRepository.GetWithRosterItemDetailsIncluded()
                               .Where(x => x.RosterItem.LeagueId == league.Id)
                               .ToList();

            var performanceByPosition = _statisticsCalculator.Calcualate(boxscore, new List<PositionEnum>()
            {
                PositionEnum.Guard,
                PositionEnum.Forward,
                PositionEnum.ShootingGuard,
                PositionEnum.PowerForward,
                PositionEnum.Center
            }.ToFrozenSet());

            return new CategoriesByPositionPerLeagueDto(league.Id,
                                                        league.OfficalName,
                                                        performanceByPosition);
        }
    }
}
