namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class StandingsViewModel
    {
        public string Title { get; set; }
        public IList<LeagueStandingItemViewModel> StandingItems { get; set; }
        public int LeagueId { get; set; }
    }
}
