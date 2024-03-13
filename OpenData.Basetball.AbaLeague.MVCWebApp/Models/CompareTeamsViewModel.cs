using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class CompareTeamsViewModel
    {
        public CompareTeamViewModel HomeTeam { get; set; }
        public CompareTeamViewModel AwayTeam { get; set; }
        public bool IsLoadedComparisonResult { get; set; } = false;
        public bool IsLeagueSelected { get; set; } = false;
        public SelectList LeagueSelection { get; set; }
        public string SelectedLeague { get; set; }
        public SelectList HomeTeamsSelection { get; set; }
        public string SelectedHomeTeam { get; set; }
        public SelectList AwayTeamsSelection { get; set; }
        public string SelectedAwayTeam { get; set; }
    }
}
