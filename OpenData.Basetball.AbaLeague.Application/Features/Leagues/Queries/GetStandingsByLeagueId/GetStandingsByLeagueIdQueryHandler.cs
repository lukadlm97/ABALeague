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
using System.Collections.Frozen;
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
            var seasonResourcesByLeague = await _unitOfWork.SeasonResourcesRepository
                                                            .SearchByLeague(request.LeagueId, cancellationToken);
            if (league == null || results == null || seasonResourcesByLeague == null) 
            {
                return Maybe<StandingsDto>.None;
            }

            var list = await _standingsService.GetByLeagueId(request.LeagueId, cancellationToken);
            if (list == null || !list.Any())
            {
                return Maybe<StandingsDto>.None;
            }

            List<GroupStandingsDto> groupStandings = new List<GroupStandingsDto>();
            switch (league.CompetitionOrganizationEnum)
            {
                case Domain.Enums.CompetitionOrganizationEnum.Groups:
                    var groups = seasonResourcesByLeague.Select(x=>x.Group).Distinct().ToList();
                    foreach(var group in groups)
                    {
                        var teamIds = seasonResourcesByLeague.Where(x => x.Group == group)
                                                                .Select(x => x.TeamId);
                        var standings = list.Where(x => teamIds.Contains(x.TeamId));
                        groupStandings.Add(new GroupStandingsDto(group, standings.ToFrozenSet()));
                    }
                    break;
                default:
                case Domain.Enums.CompetitionOrganizationEnum.League:
                    break;
            }
            

            return new StandingsDto(league.Id, 
                                    league.OfficalName,
                                    league.CompetitionOrganizationEnum,
                                    league.RoundsToPlay ?? 0, 
                                    results.Select(x => x.RoundMatch.Round).Distinct().Count(), 
                                    list
                                    .OrderByDescending(x => x.WonGames)
                                        .ThenBy(x => x.LostGames)
                                            .ThenByDescending(x => x.PointDifference)
                                    .ToList(),
                                    groupStandings.ToFrozenSet());
        }
    }
}
