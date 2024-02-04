namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class GamePerformanceViewModel : GameStatsViewModel
    {
        public int OponentId { get; set; }
        public string OponentName { get; set; }
        public DateTime Date { get; set; }
        public int Round { get; set; }
        public bool IsHomeGame { get; set; }
        public bool IsWinTheGame { get; set; }
        public string Venue { get; set; }
        public int Attendency { get; set; }
        public int MatchNo { get; set; }
        public string? Result { get; set; } = null;
        public int MatchResultId { get; set; }
    }
}
