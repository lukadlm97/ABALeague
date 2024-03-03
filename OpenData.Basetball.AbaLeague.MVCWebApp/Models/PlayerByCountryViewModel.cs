namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class PlayerByCountryViewModel : GameStatsViewModel
    {
        public int RosterItemId { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string PositionColor { get; set; }
        public string Position { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
    }
}
