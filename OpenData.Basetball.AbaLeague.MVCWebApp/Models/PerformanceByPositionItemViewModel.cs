using OpenData.Basetball.AbaLeague.Domain.Enums;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class PerformanceByPositionItemViewModel
    {
        public PositionEnum Position { get; set; }
        public string PositionName { get; set; }
        public int TotalPoints { get; init; }
        public decimal ParticipationPoints { get; init; }
        public int TotalRebounds { get; init; }
        public decimal ParticipationRebounds { get; init; }
        public int TotalAssists { get; init; }
        public decimal ParticipationAssists { get; init; }
        public int TotalTurnovers { get; init; }
        public decimal ParticipationTurnovers { get; init; }
        public int TotalSteals { get; init; }
        public decimal ParticipationSteals { get; init; }
        public int TotalOffensiveRebounds { get; init; }
        public decimal ParticipationOffensiveRebounds { get; init; }
        public int TotalDefensiveRebounds { get; init; }
        public decimal ParticipationDefensiveRebounds { get; init; }
        public int TotalBlocksOn { get; init; }
        public decimal ParticipationBlocksOn { get; init; }
        public int TotalBlocksMade { get; init; }
        public decimal ParticipationBlocksMade { get; init; }
    }
}
