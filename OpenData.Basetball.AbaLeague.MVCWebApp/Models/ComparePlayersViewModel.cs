using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models.Partial;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class ComparePlayersViewModel
    {
        public SelectList AvailableLeagues { get; set; }
        public string SelectedLeagueId { get; set; }
        public SelectList AvailableHomeTeams { get; set; }
        public string SelectedHomeTeamId { get; set; }
        public SelectList AvailableAwayTeams { get; set; }
        public string SelectedAwayTeamId { get; set; }
        public SelectList AvailableHomePlayers { get; set; }
        public string SelectedHomePlayerId { get; set; }
        public SelectList AvailableAwayPlayers { get; set; }
        public string SelectedAwayPlayerId { get; set; }
        public bool IsPlayerComperisonLoaded { get; set; }
        public ComparePlayerViewModel HomePlayer { get; set; }
        public ComparePlayerViewModel AwayPlayer { get; set; }
    }
}
