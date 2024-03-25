namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models.Boxscore
{
    public class AverageBoxscoreStatsViewModel
    {
        public TimeSpan AvgMinutes { get; set; }
        public double? AvgPoints { get; set; }
        public double? AvgTotalRebounds { get; set; }
        public double? AvgAssists { get; set; }
        public double? AvgSteals { get; set; }
        public double? AvgTurnover { get; set; }
        public double? AvgPlusMinus { get; set; }
        public double? AvgRankValue { get; set; }
    }
}
