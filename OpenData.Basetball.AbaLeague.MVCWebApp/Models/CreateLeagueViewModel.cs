using Microsoft.AspNetCore.Mvc;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class CreateLeagueViewModel
    {
        [BindProperty] public LeagueDto League { get; set; } = default!;
    }
}
