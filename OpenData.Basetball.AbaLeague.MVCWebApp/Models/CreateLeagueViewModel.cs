using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Models
{
    public class CreateLeagueViewModel
    {
        [BindProperty] public LeagueDto League { get; set; } = default!;
        public string SelectedProcessorTypeId { get; set; }
        public SelectList ProcessorTypes { get; set; }
    }
}
