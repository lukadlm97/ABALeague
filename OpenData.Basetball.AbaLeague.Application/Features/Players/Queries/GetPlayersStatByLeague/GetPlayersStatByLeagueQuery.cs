using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Game;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayersStatByLeague
{
    public class GetPlayersStatByLeagueQuery : IQuery<Maybe<PlayerStatsByLeagueDto>>
    {
        public GetPlayersStatByLeagueQuery(int leagueId)
        {
            LeagueId = leagueId;
        }

        public int LeagueId { get; private set; }
    }
}
