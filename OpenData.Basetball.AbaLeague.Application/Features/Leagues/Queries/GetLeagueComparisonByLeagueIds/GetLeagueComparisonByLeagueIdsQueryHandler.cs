using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
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
            foreach(var league in leagues)
            {
                var rosterItemIds = _unitOfWork.RosterRepository.Get()
                                                .Where(x => x.LeagueId == league.Id)
                                                .Select(x => x.Id)
                                                .ToList();

                var boxscores = _unitOfWork.BoxScoreRepository.Get()
                                            .Where(x => rosterItemIds.Contains(x.RosterItemId))
                                            .ToList();

                throw new NotImplementedException();

            }

        }
    }
}
