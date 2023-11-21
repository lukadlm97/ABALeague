namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class DetailsPlayerViewModel
    {
        public int PlayerId { get; set; }
        public int CurrentTeamId { get; set; }
        public PlayerViewModel PlayerDetails { get; set; }
        public IList<RosterItemHistoryViewModel> Rosters { get; set; }
    }
}
