using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class LeagueUpsertViewModel
    {
        [BindProperty] public LeagueItemViewModel League { get; set; } = default!;
        
    }
}
