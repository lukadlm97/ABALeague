namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class MatchResultViewModel
    {
        public string HomeTeamName { get;set; }
        public string AwayTeamName { get; set; }
        public int Attendency { get; set; }
        public string Venue { get; set; }
        public int HomeTeamPoints { get; set; }
        public int AwayTeamPoints { get; set; }
        public int MatchNo { get; set; }
        public int Round { get; set; }
    }
}
