namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models.Boxscore
{
    public class AdvancedBoxscoreStatsViewModel
    {
        public int GamePlayed { get; set; }
        public int HomeGamesPlayed { get; set; }
        public int GamesWin { get; set; }
        public int HomeGamesWin { get; set; }
        public int TotalSpectators { get; set; }
        public int AverageSpectators { get; set; }
        public int? HomeGameScoredPoints { get; set; }
        public int? AwayGameScoredPoints { get; set; }
    }
}
