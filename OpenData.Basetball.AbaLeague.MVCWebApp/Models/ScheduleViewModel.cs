namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class ScheduleViewModel
    {
        public int LeagueId { get; set; }
        public IList<ScheduleItemViewModel> ExistingScheduleItems { get; set; }
        public IList<ScheduleItemViewModel> NewScheduleItems { get; set; }
        public IList<ScheduleItemViewModel> PlanedItems { get; set; }
        public IList<string> MissingTeams { get; set; }
    }
}
