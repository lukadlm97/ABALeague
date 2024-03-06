using OpenData.Basetball.AbaLeague.Domain.Enums;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class RosterItemByPositionViewModel
    {
        public PositionEnum Position { get; set; }
        public string PositionName { get; set; }
        public string PositionColor { get; set; }
        public IList<PlayerAtRosterViewModel> Players { get; set; }
    }
}
