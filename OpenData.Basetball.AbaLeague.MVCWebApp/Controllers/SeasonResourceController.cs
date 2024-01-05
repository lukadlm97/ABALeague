using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basetball.AbaLeague.Persistence.Migrations;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetTeamsByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.SeasonResources.Commands.CreateSeasonResources;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeams;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Controllers
{
    [Route("[controller]/[action]")]
    public class SeasonResourceController : Controller
    {
        private readonly ILogger<BoxscoreController> _logger;
        private readonly ISender _sender;

        public SeasonResourceController(ILogger<BoxscoreController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }
        public async Task<IActionResult>
            RegisterTeamForCompetition([FromQuery] int leagueId,
                                        [FromQuery] int teamId,
                                        CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetTeamsByLeagueIdQuery(leagueId), cancellationToken);
            if (result.HasNoValue)
            {
                return Redirect("Error");
            }

            var selectedTeam = result.Value.FirstOrDefault(x => x.TeamId == teamId);
            if (selectedTeam == null)
            {
                return Redirect("Error");
            }
            var decorativeTeamName = selectedTeam.Name;
            var resultOfAdding = await _sender.Send(new CreateSeasonResourcesCommand(new List<AddSeasonResourceDto> { new AddSeasonResourceDto(selectedTeam.TeamId ?? 0, leagueId, selectedTeam.Url, decorativeTeamName, selectedTeam.TeamUrl, selectedTeam.IncrowdUrl) }), cancellationToken);
            if (result.HasNoValue)
            {
                return Redirect("Error");
            }
            return RedirectToAction("SeasonResources", "League", new
            {
                LeagueId = 8
            });
        }

        [HttpPost]
        public async Task<IActionResult>
           RegisterTeamsForCompetition(SeasonResourcesViewModel seasonResourcesViewModel,
                                       CancellationToken cancellationToken = default)
        {
            /*var result = await _sender.Send(new GetTeamsByLeagueIdQuery(seasonResourcesViewModel.LeagueId), cancellationToken);
            if (result.HasNoValue)
            {
                return Redirect("Error");
            }

            var selectedTeam = result.Value.FirstOrDefault(x => x.TeamId == teamId);
            if (selectedTeam == null)
            {
                return Redirect("Error");
            }
            var decorativeTeamName = selectedTeam.Name;
            */
            List<AddSeasonResourceDto> list = new List<AddSeasonResourceDto> ();

            foreach(var item in seasonResourcesViewModel.NotExistingResourcesTeams)
            {
                list.Add(new AddSeasonResourceDto(item.TeamId,
                    item.LeagueId, 
                    item.Url,
                    item.TeamName, 
                    item.TeamUrl, 
                    item.IncrowdUrl))
                    ;
            }
            var resultOfAdding = await _sender
                .Send(new CreateSeasonResourcesCommand(list), cancellationToken);
            if (resultOfAdding.IsFailure)
            {
                return Redirect("Error");
            }
            return RedirectToAction("SeasonResources", "League", new
            {
                LeagueId = seasonResourcesViewModel.LeagueId
            });
        }
        [HttpPost]
        public async Task<IActionResult>
           RegisterTeamForCompetitionViaForm(SeasonResourcesDeterminateViewModel model,
                                       CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetTeamsQuery("",1,100), cancellationToken);
            if (result.HasNoValue)
            {
                return Redirect("Error");
            }
            if (!int.TryParse(model.SelectedTeamId, out int teamId))
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "Unable to parse team id"
                });
            }
            var draftResources =
                await _sender.Send(new GetTeamsByLeagueIdQuery(model.LeagueId), cancellationToken);
            var selectedDrafrResource = draftResources.Value.FirstOrDefault(x => x.Name == model.TeamName);
            var selectedTeam = result.Value.Teams.FirstOrDefault(x => x.Id == teamId);
            if (selectedTeam == null || selectedDrafrResource == null)
            {
                return Redirect("Error");
            }
            var decorativeTeamName = model.TeamName;
            var resultOfAdding = await _sender.Send(new CreateSeasonResourcesCommand(new List<AddSeasonResourceDto> { new AddSeasonResourceDto(selectedTeam.Id ?? 0, model.LeagueId, selectedDrafrResource.Url, decorativeTeamName, selectedDrafrResource.TeamUrl, selectedDrafrResource.IncrowdUrl) }), cancellationToken);
            if (result.HasNoValue)
            {
                return Redirect("Error");
            }
            var url = $"League/SeasonResources/{teamId}";
            return Redirect(url);
        }

        public async Task<IActionResult>
          Determinate([FromQuery] int leagueId, [FromQuery] string refName, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetTeamsQuery("",1,100), cancellationToken);

            if (result.HasNoValue)
            {
                return Redirect("Error");
            }

            return View(new SeasonResourcesDeterminateViewModel
            {
                LeagueId = leagueId,
                TeamName = refName,
                SelectedTeamId = result.Value.Teams.FirstOrDefault().Id.ToString(),
                Teams = new SelectList(result.Value.Teams, "Id", "Name"),
            });
        }
    }
}
