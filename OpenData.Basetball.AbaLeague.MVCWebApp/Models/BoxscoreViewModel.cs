namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class BoxscoreViewModel
    {
        public int LeagueId { get; set; }
        public int MatchRoundId { get; set; }
        public IList<BoxscoreItemViewModel> DraftBoxscoreItems { get; set; }
        public IList<BoxscoreItemViewModel> ExistingBoxscoreItems { get; set; }
        public IList<DraftRosterItemViewModel> DraftRosterItems { get; set; }
        public IList<string> MissingPlayerItems { get; set; }

    }
}
