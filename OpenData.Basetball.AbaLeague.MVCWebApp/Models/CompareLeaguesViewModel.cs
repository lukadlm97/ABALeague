using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class CompareLeaguesViewModel
    {
        public CompareItemViewModel HomeLeague { get; set; }
        public CompareItemViewModel AwayLeague { get; set; }
        public bool IsLoadedComparisonResult { get; set; } = false;
        public SelectList HomeLeaguesSelection { get; set; }
        public string SelectedHomeLeague { get; set; }
        public SelectList AwayLeaguesSelection { get; set; }
        public string SelectedAwayLeague { get; set; }
    }
}
