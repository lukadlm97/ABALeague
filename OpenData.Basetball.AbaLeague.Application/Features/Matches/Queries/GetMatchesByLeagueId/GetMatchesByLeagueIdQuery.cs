using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Game;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Matches.Queries.GetMatchesByLeagueId
{
    public class GetMatchesByLeagueIdQuery 
        : IQuery<Maybe<FrozenSet<ScheduleItemDto>>>
    {
        public readonly int LeagueId;

        public GetMatchesByLeagueIdQuery(int leagueId)
        {
            LeagueId = leagueId;
        }
    }
}
