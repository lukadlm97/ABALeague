namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class GameDetailsViewModel
    {
        public MatchResultViewModel MatchResult { get; set; }
        public IList<PlayerGameStatsViewModel> HomaTeamPlayers { get; set; }
        public IList<PlayerGameStatsViewModel> AwayTeamPlayers { get; set; }
        public GameStatsViewModel HomeTeamTotal { get; set; }
        public GameStatsViewModel AwayTeamTotal { get; set; }
    }
}
