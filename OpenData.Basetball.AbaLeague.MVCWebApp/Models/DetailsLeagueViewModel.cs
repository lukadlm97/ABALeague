using OpenData.Basketball.AbaLeague.Application.DTOs.League;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class DetailsLeagueViewModel
    {
        public LeagueResponse League { get; set; }
        public bool IsLoaded { get; set; } = false;
        public bool IsError { get; set; } = false;
        public string ErrorMessage { get; set; }
    }
}
