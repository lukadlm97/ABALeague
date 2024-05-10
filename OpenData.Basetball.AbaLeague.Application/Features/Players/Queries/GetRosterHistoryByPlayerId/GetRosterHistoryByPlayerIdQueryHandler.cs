using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Application.Utilities;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetRosterHistoryByPlayerId
{
    internal class GetRosterHistoryByPlayerIdQueryHandler : 
        IQueryHandler<GetRosterHistoryByPlayerIdQuery, Maybe<PlayerRosterHistoryItem>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRosterHistoryByPlayerIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<PlayerRosterHistoryItem>> Handle(GetRosterHistoryByPlayerIdQuery request, CancellationToken cancellationToken)
        {
            var player = _unitOfWork.PlayerRepository.Get()
                                                    .FirstOrDefault(x => x.Id == request.PlayerId);
            if(player == null)
            {
                return Maybe<PlayerRosterHistoryItem>.None;
            }
            var selectedRosterItems =  _unitOfWork.RosterRepository.Get()
                                        .Where(x => x.PlayerId == player.Id)
                                        .ToList();
            if(!selectedRosterItems.Any())
            {
                return Maybe<PlayerRosterHistoryItem>.None;
            }

            var leagues = _unitOfWork.LeagueRepository.Get()
                            .Where(x => selectedRosterItems.Select(y => y.LeagueId).Contains(x.Id))
                            .ToList();
            var teams = _unitOfWork.LeagueRepository.Get()
                            .Where(x => selectedRosterItems.Select(y => y.TeamId).Contains(x.Id))
                            .ToList();
            var country = _unitOfWork.CountryRepository.Get()
                                    .FirstOrDefault(x => x.Id == player.CountryId);
            if(!leagues.Any() || !teams.Any() || country == null) 
            {
                return Maybe<PlayerRosterHistoryItem>.None;
            }

            List<RosterItemDto> rosterItems = new List<RosterItemDto>();
            foreach(var item in selectedRosterItems)
            {
                var selectedLeague = leagues.FirstOrDefault(x=>x.Id == item.LeagueId);
                var selectedTeam = teams.FirstOrDefault(x=>x.Id == item.LeagueId);
                if(selectedLeague == null ||  selectedTeam == null)
                {
                    continue;
                }
                rosterItems.Add(new RosterItemDto( selectedLeague.Id,
                    selectedLeague.OfficalName,
                    player.Id,
                    player.Name,
                    selectedTeam.Id,
                    selectedLeague.OfficalName
                ));
            }
            return new PlayerRosterHistoryItem(
                new PlayerItemDto(player.Id, player.Name, 
                player.PositionEnum, player.Height, 
                player.DateOfBirth, 
                DistanceCalculator.CalculateAge(DateOnly.FromDateTime(player.DateOfBirth), DateOnly.FromDateTime(DateTime.UtcNow)),
                country.Id, country.Name), 
                rosterItems.ToFrozenSet());
        }
    }
}
