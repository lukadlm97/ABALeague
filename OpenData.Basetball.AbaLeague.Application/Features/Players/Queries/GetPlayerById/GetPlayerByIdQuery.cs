using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayerById
{
    public class GetPlayerByIdQuery : IQuery<Maybe<PlayerItemDto>>
    {
        public GetPlayerByIdQuery(int playerId)
        {
            PlayerId = playerId;
        }
        public int PlayerId { get; set; }
    }
}
