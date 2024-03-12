namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class CompareItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GamesPlayed { get; set; }
        public int TotalGames { get; set; }
        public ComparePerformanceItemViewModel CorePerformance { get; set; }
    }
}
