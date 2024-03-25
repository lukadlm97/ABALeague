namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models.Boxscore
{
    public class BoxscoreItemViewModel
    {
        public int RosterItemId { get; set; }
        public int MatchRoundId { get; set; }
        public string PlayerName { get; set; }
        public string TeamName { get; set; }
        public int Round { get; set; }
        public int MatchNo { get; set; }
        public TimeSpan? Minutes { get; set; }
        public int? Points { get; set; }
        public decimal? ShotPrc { get; set; }
        public int? ShotMade2Pt { get; set; }
        public int? ShotAttempted2Pt { get; set; }
        public decimal? ShotPrc2Pt { get; set; }
        public int? ShotMade3Pt { get; set; }
        public int? ShotAttempted3Pt { get; set; }
        public decimal? shotPrc3Pt { get; set; }
        public int? ShotMade1Pt { get; set; }
        public int? ShotAttempted1Pt { get; set; }
        public decimal? ShotPrc1Pt { get; set; }
        public int? DefensiveRebounds { get; set; }
        public int? OffensiveRebounds { get; set; }
        public int? TotalRebounds { get; set; }
        public int? Assists { get; set; }
        public int? Steals { get; set; }
        public int? Turnover { get; set; }
        public int? InFavoureOfBlock { get; set; }
        public int? AgainstBlock { get; set; }
        public int? CommittedFoul { get; set; }
        public int? ReceivedFoul { get; set; }
        public int? PointFromPain { get; set; }
        public int? PointFrom2ndChance { get; set; }
        public int? PointFromFastBreak { get; set; }
        public int? PlusMinus { get; set; }
        public int? RankValue { get; set; }
    }
}
