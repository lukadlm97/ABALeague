namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class ScoreViewModel
    {
        public IList<ScoreItemViewModel> ExistingScores { get; set; }
        public IList<ScoreItemViewModel> OnWaitingScores { get; set; }
        public IList<ScoreItemViewModel> ToBePlayed { get; set; }
        public int LeagueId { get; set; }
    }
}
