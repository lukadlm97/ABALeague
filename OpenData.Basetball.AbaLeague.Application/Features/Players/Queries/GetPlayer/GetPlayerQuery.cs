using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayer
{
    public class GetPlayerQuery : IQuery<Maybe<PlayerItemDto>>
    {
        public GetPlayerQuery(int playerId)
        {
            PlayerId = playerId;
        }
        public int PlayerId { get; set; }
    }
}
