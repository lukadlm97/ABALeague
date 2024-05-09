using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetExistingRostersByTeam
{
    internal class GetExistingLeagueIdRostersByTeamQueryHandler : IQueryHandler<GetExistingLeagueIdRostersByTeamQuery, Maybe<LeaguesRoster>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetExistingLeagueIdRostersByTeamQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<LeaguesRoster>> Handle(GetExistingLeagueIdRostersByTeamQuery request, CancellationToken cancellationToken)
        {
            if (request.TeamId <= 0)
            {
                return Maybe<LeaguesRoster>.None;
            }

            var team =  _unitOfWork.TeamRepository.Get().FirstOrDefault(x=>x.Id == request.TeamId);
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
