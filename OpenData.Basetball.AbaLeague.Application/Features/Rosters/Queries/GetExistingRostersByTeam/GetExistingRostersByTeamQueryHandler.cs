using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetExistingRostersByTeam
{
    internal class GetExistingRostersByTeamQueryHandler : IQueryHandler<GetExistingRostersByTeamQuery, Maybe<LeaguesRoster>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetExistingRostersByTeamQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<LeaguesRoster>> Handle(GetExistingRostersByTeamQuery request, CancellationToken cancellationToken)
        {
            if (request.TeamId <= 0)
            {
                return Maybe<LeaguesRoster>.None;
            }

            var team = await _unitOfWork.TeamRepository.Get(request.TeamId, cancellationToken);
            if (team == null)
            {
                return Maybe<LeaguesRoster>.None;
            }

            var leagues = await _unitOfWork.SeasonResourcesRepository.GetAll(cancellationToken);
            if (!leagues.Any())
            {
                return Maybe<LeaguesRoster>.None;
            }

            return new LeaguesRoster(leagues.Where(x => x.TeamId == team.Id).Select(x=>x.LeagueId));
        }
    }
}
