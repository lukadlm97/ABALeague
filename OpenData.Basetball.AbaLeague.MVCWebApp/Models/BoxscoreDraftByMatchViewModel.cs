namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class BoxscoreDraftByMatchViewModel
    {
        public int LeagueId { get; set; }
        public int MatchResultId { get; set; }
        public MatchResultViewModel MatchResult { get; set; }
        public int HomeTeamPoints { get; set; }
        public bool HomeTeamMatch { get; set; }
        public int AwayTeamPoints { get; set; }
        public bool AwayTeamMatch { get; set; }
        public IList<BoxscoreItemViewModel> HomeDraftBoxscoreItems { get; set; }
        public IList<BoxscoreItemViewModel> AwayDraftBoxscoreItems { get; set; }
        public IList<BoxscoreItemViewModel> HomeExistingBoxscoreItems { get; set; }
        public IList<BoxscoreItemViewModel> AwayExistingBoxscoreItems { get; set; }
        public IList<DraftRosterItemViewModel> DraftRosterItems { get; set; }
        public IList<string> MissingPlayerItems { get; set; }
        //= HomeDraftBoxscoreItems.Any() || AwayDraftBoxscoreItems.Any();
        public bool AvailableDraftBoxscoreItems { get; set; } 
        public bool AvailableDraftRosterItems { get; set; } 
        public bool AvailableMissingPlayerItems { get; set; } 

    }
}
