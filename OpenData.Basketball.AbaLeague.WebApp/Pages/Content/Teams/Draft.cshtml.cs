using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetTeamsByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.SeasonResources.Commands.CreateSeasonResources;
using OpenData.Basketball.AbaLeague.Domain.Enums;

namespace OpenData.Basketball.AbaLeague.WebApp.Pages.Content.Teams
{
    public class DraftModel : PageModel
    {
        private readonly ISender _sender;

        public DraftModel(ISender sender)
        {
            _sender = sender;
        }
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; } = "There is no leagues to display.";
        public IList<TeamItemDto> Teams { get; set; }
    }
}
