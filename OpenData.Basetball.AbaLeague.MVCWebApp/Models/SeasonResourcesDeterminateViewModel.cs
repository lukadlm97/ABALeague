using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class SeasonResourcesDeterminateViewModel
    {
        public int LeagueId { get; set; }
        public string TeamName { get; set; }

        public string SelectedTeamId { get; set; }
        public SelectList Teams { get; set; }
        public bool IsLeague { get; set; }
        public bool IsGroup { get; set; }
        public string? GroupName { get; set; }
        public int? BaracketPosition { get; set; }
    }
}
