using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Domain.Enums;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamRangeStatsByLeagueId
{
    public class GetTeamRangeStatsByLeagueIdQuery : 
        IQuery<Maybe<TeamRangeStatsDto>>
    {
        public GetTeamRangeStatsByLeagueIdQuery(int leagueId, FrozenSet<StatsPropertyEnum> statsProperties)
        {
            LeagueId = leagueId;
            StatsProperties = statsProperties;
        }

        public int LeagueId { get; private set; }
        public FrozenSet<StatsPropertyEnum> StatsProperties { get; private set; }
    }
}
