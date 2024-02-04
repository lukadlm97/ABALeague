namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class ScoreItemViewModel
    {
        public int? Id { get; set; }
        public int Round { get; set; }
        public int MatchNo { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public int? HomeTeamPoints { get; set; }
        public int? AwayTeamPoints { get; set; }
        public int? Attendency { get; set; }
        public string? Venue { get; set; }
        public DateTime DateTime { get; set; }
    }
}
