using MediatR;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class LeagueViewModel
    {
        public IList<LeagueResponse> Leagues { get; set; }
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; } = "There is no leagues to display.";
    }
}
