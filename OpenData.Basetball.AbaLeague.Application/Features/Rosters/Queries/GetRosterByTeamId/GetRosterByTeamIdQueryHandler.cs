using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterByTeamId
{
    internal class GetRosterByTeamIdQueryHandler : IQueryHandler<GetRosterByTeamIdQuery,Maybe<RosterResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRosterByTeamIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<RosterResponse>> Handle(GetRosterByTeamIdQuery request, CancellationToken cancellationToken)
        {
            if (request.LeagueId <= 0 || request.TeamId <= 0)
            {
                return Maybe<RosterResponse>.None;
            }

            var team = await _unitOfWork.TeamRepository.GetRoster(request.TeamId, cancellationToken);
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            if (team == null || league == null)
            {
                return Maybe<RosterResponse>.None;
            }

            var roster = await _unitOfWork.RosterRepository.GetAll(cancellationToken);
            var currentRosters = roster.Where(x => x.LeagueId == league.Id && x.TeamId == request.TeamId);
            return new RosterResponse(currentRosters.Select(
                x => new RosterItemDto(x.PlayerId, x.LeagueId, x.DateOfInsertion, x.EndOfActivePeriod)));
        }
    }
}
