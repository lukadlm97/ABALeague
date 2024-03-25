using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetRegistredTeamsByLeagueId
{
    public class GetRegistredTeamsByLeagueIdQueryHandler(IUnitOfWork _unitOfWork)
        : IQueryHandler<GetRegistredTeamsByLeagueIdQuery, Maybe<TeamDto>>
    {
        public async Task<Maybe<TeamDto>> Handle(GetRegistredTeamsByLeagueIdQuery request, CancellationToken cancellationToken)
        {
            var league= _unitOfWork.LeagueRepository.Get()
                                    .FirstOrDefault(x=>x.Id==request.LeagueId);
            if (league == null)
            {
                return Maybe<TeamDto>.None;
            }

            var registerTeamIds = (await _unitOfWork.SeasonResourcesRepository.GetAll(cancellationToken))
                                                    .Where(x => x.LeagueId == request.LeagueId)
                                                    .Select(x => x.TeamId);
            if(!registerTeamIds.Any())
            {
                return Maybe<TeamDto>.None;
            }

            var teams = _unitOfWork.TeamRepository.Get()
                                    .Where(x => registerTeamIds.Contains(x.Id))
                                    .ToList();
            return new TeamDto(teams.Select(x =>
                                            new TeamItemDto(x.Id, x.Name, x.ShortCode, x.CountryId, string.Empty))
                                                .ToFrozenSet());
        }
    }
}
