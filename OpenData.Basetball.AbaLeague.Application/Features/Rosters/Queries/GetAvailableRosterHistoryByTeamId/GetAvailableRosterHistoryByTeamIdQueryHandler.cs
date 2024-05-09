using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System.Collections.Frozen;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetAvailableRosterHistoryByTeamId
{
    public class GetAvailableRosterHistoryByTeamIdQueryHandler :  IQueryHandler<GetAvailableRosterHistoryByTeamIdQuery, Maybe<LeagueIdentifiers>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAvailableRosterHistoryByTeamIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<LeagueIdentifiers>> 
            Handle(GetAvailableRosterHistoryByTeamIdQuery request, 
                    CancellationToken cancellationToken)
        {
            if (request.TeamId <= 0)
            {
                return Maybe<LeagueIdentifiers>.None;
            }

            var team = _unitOfWork.TeamRepository
                                    .Get()
                                    .FirstOrDefault(x => x.Id == request.TeamId);
            if (team == null)
            {
                return Maybe<LeagueIdentifiers>.None;
            }

            var leagues = await _unitOfWork.SeasonResourcesRepository.GetAll(cancellationToken);
            if (!leagues.Any())
            {
                return Maybe<LeagueIdentifiers>.None;
            }

            return new LeagueIdentifiers(leagues.Where(x => x.TeamId == team.Id)
                                                .Select(x => x.LeagueId)
                                                .ToFrozenSet());
        }
    }
}
