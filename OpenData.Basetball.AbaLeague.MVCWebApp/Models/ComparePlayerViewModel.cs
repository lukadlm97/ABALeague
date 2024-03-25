using OpenData.Basetball.AbaLeague.Domain.Enums;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class ComparePlayerViewModel : CompareItemViewModel
    {
        public int Age { get; set; }
        public PositionEnum Position { get; set; }
        public string PositionColor { get; set; }
        public string PositionName { get; set; }
        public int CurrentTeamId { get; set; }
        public string CurrentTeamName { get; set; }
        public string CurrentTeamRang { get; set; }
        public string CurrentTeamWon { get; set; }
        public string CurrentTeamLost { get; set; }
        public MatchResultViewModel AdvancedBoxscoreStatsView { get; set; }

    }
}
