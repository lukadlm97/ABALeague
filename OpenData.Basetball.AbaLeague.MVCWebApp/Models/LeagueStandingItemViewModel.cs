namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class LeagueStandingItemViewModel
    {
        public  int TeamId { get; set; }
        public string TeamName { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public int PlayedGames { get; set; }
        public int WonGames { get; set; }
        public int LostGames { get; set; }
        public int PlayedHomeGames { get; set; }
        public int WonHomeGames { get; set; }
        public int LostHomeGames { get; set; }
        public int PlayedAwayGames { get; set; }
        public int WonAwayGames { get; set; }
        public int LostAwayGames { get; set; }
        public int ScoredPoints { get; set; }
        public int ReceivedPoints { get; set; }
        public int PointDifference { get; set; }
        public int ScoredHomePoints { get; set; }
        public int ReceivedHomePoints { get; set; }
        public int ScoredAwayPoints { get; set; }
        public int ReceivedAwayPoints { get; set; }
        public IList<bool> RecentForm { get; set; }
        public IList<bool> HomeRecentForm { get; set; }
        public IList<bool> AwayRecentForm { get; set; }
    }
}
