
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayers
{
    internal class GetPlayersQueryHandler :IQueryHandler<GetPlayersQuery, Maybe<PlayerResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPlayersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<PlayerResponse>> Handle(GetPlayersQuery request, CancellationToken cancellationToken)
        {
            var players = await _unitOfWork.PlayerRepository.GetAll(cancellationToken);

            if (!players.Any())
            {
                return Maybe<PlayerResponse>.None;
            }

            return new PlayerResponse(players.Select(x =>
                new PlayerDTO(x.Id, x.Name, x.PositionEnum, x.Height, x.DateOfBirth, x.CountryId)));
        }
    }
}
