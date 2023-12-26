namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class ScheduleItemViewModel
    {
        public int? Id { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public int Round { get; set; }
        public int MatchNo { get; set; }
        public DateTime DateTime { get; set; }
    }
}
