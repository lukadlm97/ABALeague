using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Application.Utilities;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System.Linq;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterByTeamId
{
    internal class GetRosterByTeamIdQueryHandler : IQueryHandler<GetRosterByTeamIdQuery,Maybe<RosterDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRosterByTeamIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<RosterDto>> Handle(GetRosterByTeamIdQuery request, CancellationToken cancellationToken)
        {
            if (request.LeagueId <= 0 || request.TeamId <= 0)
            {
                return Maybe<RosterDto>.None;
            }

            var team = await _unitOfWork.TeamRepository.GetRoster(request.TeamId, cancellationToken);
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            var players = _unitOfWork.PlayerRepository.Get().ToList();
            var countries = _unitOfWork.CountryRepository.Get().ToList();
            if (team == null || league == null)
            {
                return Maybe<RosterDto>.None;
            }

            var roster = await _unitOfWork.RosterRepository.GetAll(cancellationToken);
            var currentRosters = roster.Where(x => x.LeagueId == league.Id && x.TeamId == request.TeamId);
            return new RosterDto(currentRosters.Select(
                x => new RosterItemDto(x.PlayerId,
                x.LeagueId,
                x.DateOfInsertion,
                x.EndOfActivePeriod, 
                players?.FirstOrDefault(y=>y.Id == x.PlayerId)?.Name!, 
                players.FirstOrDefault(y=>y.Id == x.PlayerId).PositionEnum,
                players.FirstOrDefault(y=>y.Id == x.PlayerId).Height,
                players.FirstOrDefault(y=>y.Id == x.PlayerId).DateOfBirth,
                DistanceCalculator
                .CalculateAge(DateOnly.FromDateTime(players.FirstOrDefault(y => y.Id == x.PlayerId).DateOfBirth), DateOnly.FromDateTime(DateTime.UtcNow)),
                players.FirstOrDefault(y=>y.Id == x.PlayerId).CountryId,
                countries.FirstOrDefault(z=>z.Id == players.FirstOrDefault(y=>y.Id == x.PlayerId).CountryId).Name
                )));
        }
    }
}
