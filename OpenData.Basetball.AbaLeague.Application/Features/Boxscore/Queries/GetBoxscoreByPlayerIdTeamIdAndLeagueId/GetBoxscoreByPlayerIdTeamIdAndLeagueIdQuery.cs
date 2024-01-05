using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreByPlayerIdAndLeagueId
{
    public class GetBoxscoreByPlayerIdTeamIdAndLeagueIdQuery : IQuery<Maybe<BoxscoreByPlayerDto>>
    {
        public GetBoxscoreByPlayerIdTeamIdAndLeagueIdQuery(int playerId, int leagueId, int teamId)
        {
            PlayerId = playerId;
            LeagueId = leagueId;
            TeamId = teamId;
        }

        public int PlayerId { get; private set; }
        public int LeagueId { get; private set; }
        public int TeamId { get; private set; }
    }
}
