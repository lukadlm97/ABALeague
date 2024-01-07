namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class LeaguePlayersStatsItemViewModel
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public decimal? Points { get; set; }
        public decimal? ShotMade2Pt { get; set; }
        public decimal? ShotAttempted2Pt { get; set; }
        public decimal? ShotMade3Pt { get; set; }
        public decimal? ShotAttempted3Pt { get; set; }
        public decimal? ShotMade1Pt { get; set; }
        public decimal? ShotAttempted1Pt { get; set; }
        public decimal? DefensiveRebounds { get; set; }
        public decimal? OffensiveRebounds { get; set; }
        public decimal? TotalRebounds { get; set; }
        public decimal? Assists { get; set; }
        public decimal? Steals { get; set; }
        public decimal? Turnover { get; set; }
        public decimal? InFavoureOfBlock { get; set; }
        public decimal? AgainstBlock { get; set; }
        public decimal? CommittedFoul { get; set; }
        public decimal? ReceivedFoul { get; set; }
        public decimal? PointFromPain { get; set; }
        public decimal? PointFrom2ndChance { get; set; }
        public decimal? PointFromFastBreak { get; set; }
        public decimal? PlusMinus { get; set; }
        public decimal? RankValue { get; set; }
        public TimeSpan? Minutes { get; set; }
    }
}
