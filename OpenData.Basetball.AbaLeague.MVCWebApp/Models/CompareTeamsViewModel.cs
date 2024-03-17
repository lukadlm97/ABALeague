using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basetball.AbaLeague.Domain.Enums;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class CompareTeamsViewModel
    {
        public IList<(PositionEnum key, string color, string name)> PositionPlaceholderItems { get; set; }
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
