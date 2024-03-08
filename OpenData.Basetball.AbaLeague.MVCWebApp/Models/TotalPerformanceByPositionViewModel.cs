using OpenData.Basetball.AbaLeague.Domain.Enums;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class TotalPerformanceByPositionViewModel
    {
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public IList<TotalPerformanceByPositionItemViewModel> PerformanceByPosition { get; set; }
    }
}
