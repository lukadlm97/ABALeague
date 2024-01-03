namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class BoxscoreByPlayerViewModel
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public IList<GamePerformanceViewModel> GamePerformance { get; set; }
        public AdvancedBoxscoreStatsViewModel AdvancedBoxscoreStatsView { get; set; }
        public AverageBoxscoreStatsViewModel AverageBoxscoreStatsView { get; set; }
    }
}
