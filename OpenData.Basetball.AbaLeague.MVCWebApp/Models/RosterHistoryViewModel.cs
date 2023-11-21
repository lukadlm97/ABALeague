namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class RosterHistoryViewModel
    {
        public int TeamId { get; set; }
        public int LeagueId { get; set; }
        public IList<PlayerViewModel> RosterItems { get; set; }
        public string LeagueName { get; set; }
        public string TeamName { get; set; }
    }
}
