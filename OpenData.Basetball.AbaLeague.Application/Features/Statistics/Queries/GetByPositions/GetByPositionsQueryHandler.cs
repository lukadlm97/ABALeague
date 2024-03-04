using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Statistic;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Application.Services.Implementation;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Statistics.Queries.GetByPositions
{
    internal class GetByPositionsQueryHandler : IQueryHandler<GetByPositionsQuery, Maybe<CategoriesByPositionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStatisticsCalculator _statisticsCalculator;

        public GetByPositionsQueryHandler(IUnitOfWork unitOfWork, IStatisticsCalculator statisticsCalculator)
        {
            _unitOfWork = unitOfWork;
            _statisticsCalculator = statisticsCalculator;
        }
        public async Task<Maybe<CategoriesByPositionDto>> Handle(GetByPositionsQuery request,
                                                    CancellationToken cancellationToken)
        {
            var team = _unitOfWork.TeamRepository.Get()
                                    .FirstOrDefault(x => x.Id == request.TeamId);
            var league = _unitOfWork.LeagueRepository.Get() 
                                    .FirstOrDefault(x => x.Id == request.LeagueId);
            if (team == null || league == null)
            {
                return Maybe<CategoriesByPositionDto>.None;
            }
            var boxscore = _unitOfWork.BoxScoreRepository.GetWithRosterItemDetailsIncluded()
                                .Where(x => x.RosterItem.TeamId == team.Id 
                                            && x.RosterItem.LeagueId == league.Id)
                                .ToList();
            
            var performanceByPosition = _statisticsCalculator.Calcualate(boxscore, new List<PositionEnum>()
            {
                PositionEnum.Guard,
                PositionEnum.Forward,
                PositionEnum.ShootingGuard,
                PositionEnum.PowerForward,
                PositionEnum.Center
            }.ToFrozenSet());

            return new CategoriesByPositionDto(team.Id, 
                                                team.Name, 
                                                league.Id,
                                                league.OfficalName, 
                                                performanceByPosition);
        }
    }
}
