﻿using OpenData.Basetball.AbaLeague.Application.Contracts;
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
            if (team == null || league == null)
            {
                return Maybe<RosterDto>.None;
            }

            var roster = _unitOfWork.RosterRepository
                                    .GetByLeagueIdAndTeamId(league.Id, team.Id)
                                    .ToList();

            List<RosterItemDto> list = new List<RosterItemDto>();
            foreach (var item in roster)
            {
                var newItem = 
                new RosterItemDto(item.PlayerId,
                                item.LeagueId,
                                item.DateOfInsertion,
                                item.EndOfActivePeriod,
                                item.Player.Name,
                                item.Player.PositionEnum,
                                item.Player.Height,
                                item.Player.DateOfBirth,
                                DistanceCalculator
                                .CalculateAge(DateOnly.FromDateTime(item.Player.DateOfBirth), 
                                                DateOnly.FromDateTime(DateTime.UtcNow)),
                                item.Player.CountryId,
                                item.Player.Country?.Name!
                                );

                list.Add(newItem);
            }

            return new RosterDto(list);
        }
    }
}
