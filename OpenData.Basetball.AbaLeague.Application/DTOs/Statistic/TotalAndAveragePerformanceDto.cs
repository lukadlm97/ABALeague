using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Statistic
{
    public record TotalAndAveragePerformanceDto(
        int? TotalPoints = null,
        decimal? AveragePoints = null,
        int? TotalRebounds = null,
        decimal? AverageRebounds = null,
        int? TotalAssists = null,
        decimal? AverageAssists = null,
        int? TotalTurnovers = null,
        decimal? AverageTurnovers = null,
        int? TotalSteals = null,
        decimal? AverageSteals = null,
        int? TotalOffensiveRebounds = null,
        decimal? AverageOffensiveRebounds = null,
        int? TotalDefensiveRebounds = null,
        decimal? AverageDefensiveRebounds = null,
        int? TotalBlocksOn = null,
        decimal? AverageBlocksOn = null,
        int? TotalBlocksMade = null,
        decimal? AverageBlocksMade = null,
        int? GamesPlayed = null);
}
