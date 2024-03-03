namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class GameStatsViewModel
    {
        public TimeSpan? Minutes { get; set; }
        public int? Points { get; set; } = null;
        public decimal? ShotPrc { get; set; } = null;
        public int? ShotMade2Pt { get; set; } = null;
        public int? ShotAttempted2Pt { get; set; } = null;
        public decimal? ShotPrc2Pt { get; set; } = null;
        public int? ShotMade3Pt { get; set; } = null;
        public int? ShotAttempted3Pt { get; set; } = null;
        public decimal? ShotPrc3Pt { get; set; } = null;
        public int? ShotMade1Pt { get; set; } = null;
        public int? ShotAttempted1Pt { get; set; } = null;
        public decimal? ShotPrc1Pt { get; set; } = null;
        public int? DefensiveRebounds { get; set; } = null;
        public int? OffensiveRebounds { get; set; } = null;
        public int? TotalRebounds { get; set; } = null;
        public int? Assists { get; set; } = null;
        public int? Steals { get; set; } = null;
        public int? Turnover { get; set; } = null;
        public int? InFavoureOfBlock { get; set; } = null;
        public int? AgainstBlock { get; set; } = null;
        public int? CommittedFoul { get; set; } = null;
        public int? ReceivedFoul { get; set; } = null;
        public int? PointFromPain { get; set; } = null;
        public int? PointFrom2ndChance { get; set; } = null;
        public int? PointFromFastBreak { get; set; } = null;
        public int? PlusMinus { get; set; } = null;
        public int? RankValue { get; set; } = null;
        public DateTime? DatePerformance { get; set; }
    }
}
