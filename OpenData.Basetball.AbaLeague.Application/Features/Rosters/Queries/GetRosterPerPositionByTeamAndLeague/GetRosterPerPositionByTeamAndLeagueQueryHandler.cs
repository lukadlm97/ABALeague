using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Application.DTOs.Statistic;
using OpenData.Basketball.AbaLeague.Application.Utilities;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterPerPositionByTeamAndLeague
{
    public class GetRosterPerPositionByTeamAndLeagueQueryHandler : 
        IQueryHandler<GetRosterPerPositionByTeamAndLeagueQuery, Maybe<RosterByPositionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRosterPerPositionByTeamAndLeagueQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<RosterByPositionDto>> 
            Handle(GetRosterPerPositionByTeamAndLeagueQuery request, CancellationToken cancellationToken)
        {
            var team = _unitOfWork.TeamRepository.Get()
                                 .FirstOrDefault(x => x.Id == request.TeamId);
            var league = _unitOfWork.LeagueRepository.Get()
                                    .FirstOrDefault(x => x.Id == request.LeagueId);
            if (team == null || league == null)
            {
                return Maybe<RosterByPositionDto>.None;
            }
            var rosterItems = await _unitOfWork.RosterRepository
                .GetTeamRosterByLeagueId(league.Id, team.Id, cancellationToken);

            var playerIds = rosterItems.Select(x => x.PlayerId).ToList();
            var players = _unitOfWork.PlayerRepository.Get()
                                    .Where(x => playerIds.Contains(x.Id))
                                    .ToList();
            var playersByPositions = players.GroupBy(x => x.PositionEnum)
                .Select(y => (y.Key,
                    y.Select(z =>
                                new RosterItemDto(z.Id, 
                                league.Id,
                                rosterItems.FirstOrDefault(x=>x.PlayerId == z.Id).DateOfInsertion,
                                rosterItems.FirstOrDefault(x => x.PlayerId == z.Id).EndOfActivePeriod, 
                                z.Name,
                                z.PositionEnum,
                                z.Height,
                                z.DateOfBirth,
                                  DistanceCalculator
                                .CalculateAge(DateOnly.FromDateTime(z.DateOfBirth), DateOnly.FromDateTime(DateTime.UtcNow)),
                                z.CountryId,
                                string.Empty)
                                ).ToFrozenSet())
                    ).ToDictionary();

            return new RosterByPositionDto(1, string.Empty, 1, string.Empty, playersByPositions.ToFrozenDictionary());
        }
    }
}
