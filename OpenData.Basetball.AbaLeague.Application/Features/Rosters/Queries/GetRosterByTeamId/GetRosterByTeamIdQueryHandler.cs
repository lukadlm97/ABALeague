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
            if (team == null || league == null || players == null)
            {
                return Maybe<RosterDto>.None;
            }

            var roster = await _unitOfWork.RosterRepository.SearchByLeagueId(league.Id, cancellationToken);
            var currentRosters = roster.Where(x => x.TeamId == request.TeamId);
            List<RosterItemDto> list = new List<RosterItemDto>();

            foreach (var item in currentRosters)
            {
                var selectedPlayer = players.FirstOrDefault(y => y.Id == item.PlayerId);
                if (selectedPlayer == null)
                {
                    continue;
                }
                var newItem = new RosterItemDto(item.PlayerId,
                item.LeagueId,
               item.DateOfInsertion,
                item.EndOfActivePeriod,
                selectedPlayer.Name,
                selectedPlayer.PositionEnum,
                selectedPlayer.Height,
                selectedPlayer.DateOfBirth,
                DistanceCalculator
                .CalculateAge(DateOnly.FromDateTime(selectedPlayer.DateOfBirth), DateOnly.FromDateTime(DateTime.UtcNow)),
                selectedPlayer.CountryId,
                countries?.FirstOrDefault(z => z.Id == selectedPlayer.CountryId)?.Name!
                );
                list.Add(newItem);
            }

            return new RosterDto(list);
        }
    }
}
