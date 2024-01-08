namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class LeagueTeamRangeStatsViewModel
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public IList<RangeStatsItemViewModel> RangeStatsItems { get; set; }
    }
}
