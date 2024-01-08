namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class LeagueRangeStatsViewModel
    {
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public List<LeagueTeamRangeStatsViewModel> TeamItems { get; set; }
    }
}
