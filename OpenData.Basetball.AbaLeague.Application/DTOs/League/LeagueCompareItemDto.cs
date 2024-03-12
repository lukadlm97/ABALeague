using OpenData.Basketball.AbaLeague.Application.DTOs.Statistic;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.League
{
    public record LeagueCompareItemDto(int Id,
                                         string Name,
                                         int TotalPoints ,
                                         decimal AvgPoints ,
                                         int TotalRebounds ,
                                         decimal AvgRebounds ,
                                         int TotalAssists ,
                                         decimal AvgAssists ,
                                         int TotalTurnovers ,
                                         decimal AvgTurnovers ,
                                         int TotalSteals ,
                                         decimal AvgSteals ,
                                         int TotalOffensiveRebounds ,
                                         decimal AvgOffensiveRebounds ,
                                         int TotalDefensiveRebounds,
                                         decimal AvgDefensiveRebounds,
                                         int TotalBlocksOn ,
                                         decimal AvgBlocksOn,
                                         int TotalBlocksMade,
                                         decimal AvgBlocksMade,
                                         int GamesPlayed,
                                         int GamesToPlay,
                                         FrozenSet<CategoriesItemDto> Items);
}
