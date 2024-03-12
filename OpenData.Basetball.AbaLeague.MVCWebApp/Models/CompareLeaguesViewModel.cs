using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basetball.AbaLeague.Domain.Enums;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class CompareLeaguesViewModel
    {
        public IList<(PositionEnum positionEnum, string positionColor, string name)> PositionsWithColors { get; set; }
        public CompareItemViewModel HomeLeague { get; set; }
        public CompareItemViewModel AwayLeague { get; set; }
        public bool IsLoadedComparisonResult { get; set; } = false;
        public SelectList HomeLeaguesSelection { get; set; }
        public string SelectedHomeLeague { get; set; }
        public SelectList AwayLeaguesSelection { get; set; }
        public string SelectedAwayLeague { get; set; }
    }
}
