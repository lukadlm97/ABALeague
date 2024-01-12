using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models;

public class LeagueRangeStatsViewModel
{
    public int LeagueId { get; set; }
    public string LeagueName { get; set; }
    public List<LeagueTeamRangeStatsViewModel> TeamItems { get; set; }
    public string SelectedStatsPropertyId { get; set; }
    public SelectList SelectedStatsProperties { get; set; }
}
