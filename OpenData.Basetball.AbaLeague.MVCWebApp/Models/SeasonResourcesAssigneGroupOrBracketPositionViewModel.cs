namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class SeasonResourcesAssigneGroupOrBracketPositionViewModel
    {
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public bool IsGroup { get; set; }
        public string? GroupName { get; set; }
        public int? BaracketPosition { get; set; }
    }
}
