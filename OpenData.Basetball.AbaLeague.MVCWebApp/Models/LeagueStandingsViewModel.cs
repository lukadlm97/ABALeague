namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class LeagueStandingsViewModel
    {
        public int LeagueId { get; set; }
        public string LeagueName{ get; set;}
        public int TotalRounds { get; set; }
        public int PlayedRounds { get; set; }
        public IList<LeagueStandingItemViewModel> StandingItems { get; set; }
        public IList<LeagueStandingItemViewModel> HomeStandingItems { get; set; }
        public IList<LeagueStandingItemViewModel> AwayStandingItems { get; set; }
    }
}
