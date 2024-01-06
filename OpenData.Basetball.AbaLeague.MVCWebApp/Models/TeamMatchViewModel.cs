namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class TeamMatchViewModel
    {
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public IList<TeamGameViewModel> TeamScheduledItems { get; set; }
        public IList<TeamGameViewModel> TeamScoreItems { get; set; }
    }
}
