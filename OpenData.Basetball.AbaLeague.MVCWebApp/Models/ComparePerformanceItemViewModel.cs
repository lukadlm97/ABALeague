using OpenData.Basetball.AbaLeague.Domain.Enums;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class ComparePerformanceItemViewModel
    {
        public int TotalPoints { get; init; }
        public decimal AvgPoints { get; init; }
        public int TotalRebounds { get; init; }
        public decimal AvgRebounds { get; init; }
        public int TotalAssists { get; init; }
        public decimal AvgAssists { get; init; }
        public int TotalTurnovers { get; init; }
        public decimal AvgTurnovers { get; init; }
        public int TotalSteals { get; init; }
        public decimal AvgSteals { get; init; }
        public int TotalOffensiveRebounds { get; init; }
        public decimal AvgOffensiveRebounds { get; init; }
        public int TotalDefensiveRebounds { get; init; }
        public decimal AvgDefensiveRebounds { get; init; }
        public int TotalBlocksOn { get; init; }
        public decimal AvgBlocksOn { get; init; }
        public int TotalBlocksMade { get; init; }
        public decimal AvgBlocksMade { get; init; }
        public int GamesPlayed { get; init; }
        public int GamesToPlay { get; init; }
        public IList<TotalAndParticipatePerformanceByPositionItemViewModel> PerformanceByPositions { get; set; }
    }
}
