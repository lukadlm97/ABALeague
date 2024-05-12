using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Score;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Score.Queries.GetScoresByRoundAndLeagueId
{
    public class GetScoresByRoundAndLeagueIdQuery : 
        IQuery<Maybe<FrozenSet<ScoreItemDto>>>
    {
        public GetScoresByRoundAndLeagueIdQuery(int round, int leagueId)
        {
            Round = round;
            LeagueId = leagueId;
        }

        public int Round { get; }
        public int LeagueId { get; }
    }
}
