using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Game;
using OpenData.Basketball.AbaLeague.Application.DTOs.Score;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamGamesByLeagueId
{
    public class GetTeamGamesByLeagueIdQuery : IQuery<Maybe<MatchesDto>>
    {
        public GetTeamGamesByLeagueIdQuery(int leagueId, int teamId)
        {
            LeagueId = leagueId;
            TeamId = teamId;
        }

        public int LeagueId { get; private set; }
        public int TeamId { get; private set; }
    }
}
