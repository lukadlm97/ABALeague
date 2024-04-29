using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayerComparisonByPlayerIds
{
    public class GetPlayerComparisonByPlayerIdsQuery : IQuery<Maybe<PlayerCompareDto>>
    {
        public GetPlayerComparisonByPlayerIdsQuery(int leagueId, 
                                                    IReadOnlyList<(int teamId, int playerId)> rosterDestination)
        {
            LeagueId = leagueId;
            RosterDestination = rosterDestination;
        }

        public int LeagueId { get; }
        public IReadOnlyList<(int teamId, int playerId)> RosterDestination { get; }
    }

}
