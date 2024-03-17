namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class CompareTeamViewModel : CompareItemViewModel
    {
        public int LeagueId { get; set; }
        public int LeagueName { get; set; }
        public int CurrentRang { get; set; }
        public RosterItemsByPositionsViewModel RosterItems { get; set; }

    }
}
