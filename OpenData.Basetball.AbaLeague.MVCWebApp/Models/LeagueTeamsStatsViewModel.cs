namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class LeagueTeamsStatsViewModel
    {
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public IList<LeagueTeamsStatsItemViewModel> StatsItem { get; set; }
    }
}
