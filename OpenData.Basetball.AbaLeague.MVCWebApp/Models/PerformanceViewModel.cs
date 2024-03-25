using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models.Boxscore;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class PerformanceViewModel
    {
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public IList<BoxscoreByTeamViewModel> StatsByRounds { get; set; }
        public AverageBoxscoreStatsViewModel AverageBoxscoreStats { get; set; }
        public AdvancedBoxscoreStatsViewModel AdvancedBoxscoreStats { get; set; }
        public RosterItemsByPositionsViewModel RosterItemsByPositionsViewModel { get; set; }
        public TotalPerformanceByPositionViewModel PerformanceByPosition { get; set; }
        public IList<(PositionEnum key, string color)>  PositionPlaceholderItems { get; set; }
    }
}
