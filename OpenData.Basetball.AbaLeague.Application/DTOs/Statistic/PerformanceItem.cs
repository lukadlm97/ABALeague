using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Statistic
{
    public record PerformanceItem(
        int TotalPoints,
        decimal ParticipationPoints,
        int TotalRebounds,
        decimal ParticipationRebounds,
        int TotalAssists,
        decimal ParticipationAssists,
        int TotalTurnovers,
        decimal ParticipationTurnovers,
        int TotalSteals,
        decimal ParticipationSteals,
        int TotalOffensiveRebounds,
        decimal ParticipationOffensiveRebounds,
        int TotalDefensiveRebounds,
        decimal ParticipationDefensiveRebounds,
        int TotalBlocksOn,
        decimal ParticipationBlocksOn,
        int TotalBlocksMade,
        decimal ParticipationBlocksMade
    );
}
