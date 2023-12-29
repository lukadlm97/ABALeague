namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class BoxscoreDraftByRoundViewModel
    {
        public int LeagueId { get; set; }
        public string Name { get; set; }
        public int RoundNo { get; set; }
        public IList<BoxscoreDraftByMatchViewModel> Matches { get; set; }
    }
}
