using OpenData.Basetball.AbaLeague.Domain.Enums;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class PlayerGameStatsViewModel : GameStatsViewModel
    {
        public int RosterItemId { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string PositionColor { get; set; }
    }
}
