using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Game;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayerGames
{
    public class GetPlayerGamesQuery : IQuery<Maybe<MatchesDto>>
    {
        public GetPlayerGamesQuery(int leagueId, int teamId, int playerId)
        {
            LeagueId = leagueId;
            TeamId = teamId;
            PlayerId = playerId;
        }

        public int LeagueId { get; private set; }
        public int TeamId { get; private set; }
        public int PlayerId { get; private set; }
    }
}
