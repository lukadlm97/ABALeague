namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class LeaguePlayersStatsViewModel
    {
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public IList<LeaguePlayersStatsItemViewModel> StatsItem { get; set; }
    }
}
