using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayer
{
    public class GetPlayerQueryHandler : IQueryHandler<GetPlayerQuery, Maybe<PlayerDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPlayerQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Maybe<PlayerDTO>> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
        {
            var player= await _unitOfWork.PlayerRepository.Get(request.PlayerId, cancellationToken);

            if (player == null)
            {
                return Maybe<PlayerDTO>.None;
            }

            return new PlayerDTO(player.Id, player.Name, player.PositionEnum, player.Height, player.DateOfBirth, player.CountryId);
        }
    }
}
