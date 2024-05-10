using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeaguesBySeasonAndTeamId
{
    internal class GetLeaguesBySeasonAndTeamIdQueryHandler : 
        IQueryHandler<GetLeaguesBySeasonAndTeamIdQuery, Maybe<LeagueIdentifiers>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLeaguesBySeasonAndTeamIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<LeagueIdentifiers>> 
            Handle(GetLeaguesBySeasonAndTeamIdQuery request, CancellationToken cancellationToken)
        {
            var selectedSeason = _unitOfWork.SeasonRepository.Get()
                                                            .FirstOrDefault(x => x.Id == request.SeasonId);
            if(selectedSeason == null)
            {
                return Maybe<LeagueIdentifiers>.None;
            }

            var leagues = _unitOfWork.LeagueRepository.Get()
                                                        .Where(x=>x.SeasonId == selectedSeason.Id);
            if(leagues == null || !leagues.Any())
            {
                return Maybe<LeagueIdentifiers>.None;
            }

            return new LeagueIdentifiers(leagues.Select(x => x.Id).ToFrozenSet());
        }
    }
}
