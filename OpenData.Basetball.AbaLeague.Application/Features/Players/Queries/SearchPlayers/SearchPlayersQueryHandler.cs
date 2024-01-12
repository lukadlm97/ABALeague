using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayers;
using OpenData.Basketball.AbaLeague.Application.Utilities;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.SearchPlayers
{
    public class SearchPlayersQueryHandler : IQueryHandler<SearchPlayersQuery, Maybe<PlayersDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchPlayersQueryHandler(IUnitOfWork  unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Maybe<PlayersDto>> Handle(SearchPlayersQuery request, CancellationToken cancellationToken)
        {
            var players = _unitOfWork.PlayerRepository
                                        .Get()
                                        .Where(x => x.Name.ToLower().Contains(request.Filter.ToLower()))
                                        .ToList();
            var countries = _unitOfWork.CountryRepository.Get().ToList();

            if (!players.Any())
            {
                return Maybe<PlayersDto>.None;
            }

            return new PlayersDto(players
                .Select(x =>new PlayerItemDto(x.Id,
                                x.Name,
                                x.PositionEnum,
                                x.Height,
                                x.DateOfBirth,
                                DistanceCalculator
                                .CalculateAge(DateOnly.FromDateTime(x.DateOfBirth), DateOnly.FromDateTime(DateTime.UtcNow)),
                                x.CountryId,
                                countries?.FirstOrDefault(y => y.Id == x.CountryId)?.Name!))
               .OrderBy(x => x.Name)
               .Skip((request.PageNumber - 1) * request.PageSize)
               .Take(request.PageSize).ToFrozenSet());
        }
    }
}
