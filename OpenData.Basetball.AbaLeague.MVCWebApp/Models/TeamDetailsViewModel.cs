namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class TeamDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Country { get; set; }
        public IList<PlayerViewModel> RosterItems { get; set; }
        public IList<SeasonResourceViewModel> RosterHistory { get; set; }
    }
}
