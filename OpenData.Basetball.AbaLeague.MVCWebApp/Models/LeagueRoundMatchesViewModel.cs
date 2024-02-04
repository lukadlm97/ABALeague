namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class LeagueRoundMatchesViewModel
    {
        public int Round { get; set; }
        public IList<ScoreItemViewModel> Scores { get; set; }
        public IList<ScheduleItemViewModel> Schedules { get; set; }
    }
}
