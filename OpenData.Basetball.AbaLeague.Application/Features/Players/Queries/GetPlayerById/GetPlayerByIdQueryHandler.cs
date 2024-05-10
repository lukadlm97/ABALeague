using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Application.Utilities;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System.Collections.Frozen;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayerById
{
    public class GetPlayerByIdQueryHandler : IQueryHandler<GetPlayerByIdQuery, Maybe<PlayerItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPlayerByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Maybe<PlayerItemDto>> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken)
        {
            var player= await _unitOfWork.PlayerRepository.Get(request.PlayerId, cancellationToken);
            var countries = _unitOfWork.CountryRepository.Get().ToList();

            if (player == null)
            {
                return Maybe<PlayerItemDto>.None;
            }

            return new PlayerItemDto(player.Id, 
                player.Name, 
                player.PositionEnum, 
                player.Height, 
                player.DateOfBirth,
                DistanceCalculator
                .CalculateAge(DateOnly.FromDateTime(player.DateOfBirth), DateOnly.FromDateTime(DateTime.UtcNow)),
                player.CountryId,
                countries?.FirstOrDefault(y => y.Id == player.CountryId)?.Name!);
        }
    }
}
