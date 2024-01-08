using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Team
{
    public record TeamRangeStatsDto(int LeagueId, 
        string LeagueName,
        FrozenDictionary<(int teamId, string teamName), FrozenSet<TeamRangeStatsItemDto>> Stats);
}
