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
    public class GetBoxscoreByPlayerIdAndLeagueIdQuery : IQuery<Maybe<BoxscoreByPlayerDto>>
    {
        public GetBoxscoreByPlayerIdAndLeagueIdQuery(int playerId, int leagueId)
        {
            PlayerId = playerId;
            LeagueId = leagueId;
        }

        public int PlayerId { get; private set; }
        public int LeagueId { get; private set; }
    }
}
