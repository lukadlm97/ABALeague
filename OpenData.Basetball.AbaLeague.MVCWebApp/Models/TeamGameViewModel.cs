namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class TeamGameViewModel
    {
        public int MatchId { get; set; }
        public int Round { get; set; }
        public int MatchNo { get; set; }
        public bool IsHomeGame { get; set; }
        public int OponentId { get; set; }
        public string OponentName { get; set; }
        public int OponentCurrentRanking { get; set; }
        public IList<bool> OponentRecentForm { get; set; }
        public DateTime Date { get; set; }

        public int? ResultId { get; set; }
        public bool? WinTheGame { get; set; } = null;
        public int? Attendency { get; set; }
        public string? Venue { get; set; }
        public string? Result { get; set; }
    }
}
