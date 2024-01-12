using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Application.Utilities;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.SearchPlayersByCountryId
{
    public class SearchPlayersByCountryIdQueryHandler : 
        IQueryHandler<SearchPlayersByCountryIdQuery, Maybe<PlayersDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchPlayersByCountryIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<PlayersDto>> 
            Handle(SearchPlayersByCountryIdQuery request, CancellationToken cancellationToken)
        {
            var players = _unitOfWork.PlayerRepository.Get()
                                                        .Where(x=>x.CountryId == request.CountryId)
                                                        .ToList();
            var country = _unitOfWork.CountryRepository.Get()
                                                        .FirstOrDefault(x=>x.Id == request.CountryId);
            if (!players.Any() || country==null)
            {
                return Maybe<PlayersDto>.None;
            }
            return new PlayersDto(players
                .Select(x => new PlayerItemDto(x.Id,
                                x.Name,
                                x.PositionEnum,
                                x.Height,
                                x.DateOfBirth,
                                DistanceCalculator
                                .CalculateAge(DateOnly.FromDateTime(x.DateOfBirth), DateOnly.FromDateTime(DateTime.UtcNow)),
                                x.CountryId,
                                country.Name))
               .OrderBy(x => x.Name)
               .Skip((request.PageNumber - 1) * request.PageSize)
               .Take(request.PageSize).ToFrozenSet());
        }
    }
}
