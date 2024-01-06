using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Application.Services.Implementation;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetStandingsByLeagueId
{
    public class GetStandingsByLeagueIdQueryHandler 
        : IQueryHandler<GetStandingsByLeagueIdQuery, Maybe<StandingsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStandingsService _standingsService;

        public GetStandingsByLeagueIdQueryHandler(IUnitOfWork unitOfWork, IStandingsService standingsService)
        {
            _unitOfWork = unitOfWork;
            _standingsService = standingsService;
        }
        public async Task<Maybe<StandingsDto>> 
            Handle(GetStandingsByLeagueIdQuery request, CancellationToken cancellationToken)
        {
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            var results = await _unitOfWork.ResultRepository.SearchByLeague(request.LeagueId, cancellationToken);
            if(league == null || results == null) 
            {
                return Maybe<StandingsDto>.None;
            }
            var list = await _standingsService.GetByLeagueId(request.LeagueId, cancellationToken);
            if(list == null || !list.Any())
            {
                return Maybe<StandingsDto>.None;
            }

            return new StandingsDto(league.Id, 
                                    league.OfficalName,
                                    league.RoundsToPlay ?? 0, 
                                    results.Select(x=>x.RoundMatch.Round).Distinct().Count(), 
                                    list);
        }
    }
}
