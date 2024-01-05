using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class SeasonResourcesDeterminateViewModel
    {
        public int LeagueId { get; set; }
        public string TeamName { get; set; }

        public string SelectedTeamId { get; set; }
        public SelectList Teams { get; set; }
    }
}
