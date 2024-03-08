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

namespace OpenData.Basketball.AbaLeague.Application.Features.Statistics.Queries.GetByPositionsPerLeagues
{
    public class GetByPositionsPerLeaguesQueryHandler 
        : IQueryHandler<GetByPositionsPerLeaguesQuery, Maybe<CategoriesByPositionPerLeaguesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStatisticsCalculator _statisticsCalculator;

        public GetByPositionsPerLeaguesQueryHandler(IUnitOfWork unitOfWork, 
                                                    IStatisticsCalculator statisticsCalculator)
        {
            _unitOfWork = unitOfWork;
            _statisticsCalculator = statisticsCalculator;
        }
        public async Task<Maybe<CategoriesByPositionPerLeaguesDto>> 
            Handle(GetByPositionsPerLeaguesQuery request, 
                    CancellationToken cancellationToken)
        {
            var leagues = _unitOfWork.LeagueRepository.Get().ToList();
            if (leagues == null)
            {
                return Maybe<CategoriesByPositionPerLeaguesDto>.None;
            }

            List<CategoriesByPositionPerLeagueDto> list = new List<CategoriesByPositionPerLeagueDto>();
            foreach (var league in leagues)
            {
                var boxscore = _unitOfWork.BoxScoreRepository
                                            .GetWithRosterItemDetailsIncluded()
                                               .Where(x => x.RosterItem.LeagueId == league.Id)
                                               .ToList();
                if(boxscore == null || !boxscore.Any())
                {
                    continue;
                }
                var performanceByPosition = _statisticsCalculator.Calcualate(boxscore,
                                            new List<PositionEnum>()
                                            {
                                                PositionEnum.Guard,
                                                PositionEnum.Forward,
                                                PositionEnum.ShootingGuard,
                                                PositionEnum.PowerForward,
                                                PositionEnum.Center
                                            }.ToFrozenSet());

                list.Add(new CategoriesByPositionPerLeagueDto(league.Id,
                                                            league.OfficalName,
                                                            performanceByPosition));
            }

            return new CategoriesByPositionPerLeaguesDto(list.ToFrozenSet());
        }
    }
}
