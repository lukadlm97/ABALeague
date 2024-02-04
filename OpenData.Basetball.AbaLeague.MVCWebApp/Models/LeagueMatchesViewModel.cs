namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class LeagueMatchesViewModel
    {
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public IList<LeagueRoundMatchesViewModel> RoundMathes { get; set; }
    }
}
