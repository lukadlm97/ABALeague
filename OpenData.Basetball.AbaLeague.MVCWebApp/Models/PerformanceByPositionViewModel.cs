using OpenData.Basetball.AbaLeague.Domain.Enums;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class PerformanceByPositionViewModel
    {
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public IList<PerformanceByPositionItemViewModel> PerformanceByPosition { get; set; }
    }
}
