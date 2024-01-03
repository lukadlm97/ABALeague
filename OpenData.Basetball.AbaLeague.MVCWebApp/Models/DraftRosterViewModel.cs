namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class DraftRosterViewModel
    {
        public int LeagueId { get; set; } 
        public IList<DraftRosterItemViewModel> DraftRosterItems { get; set; }
        public IList<DraftRosterItemViewModel> ExistingRosterItems { get; set; }
        public IList<DraftRosterItemViewModel> ExistingRosterItemsWithEndedContract { get; set; }
        public IList<PlayerViewModel> MissingPlayerItems { get; set; }
    }
}
