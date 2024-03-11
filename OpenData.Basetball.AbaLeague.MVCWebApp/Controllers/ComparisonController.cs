using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Controllers
{
    public class ComparisonController(ILogger<ComparisonController> _logger, 
                                        ISender _sender) : Controller
    {


        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            ViewBag.Title = "Comparison Page";

            return View(new ComparisonIndexViewModel
            {

            });
        }

        [HttpGet]
        public async Task<IActionResult> CompareLeagues(string? selectedHomeLeagueId, 
                                                        string? selectedAwayLeagueId, 
                                                        CancellationToken cancellationToken = default)
        {
            ViewBag.Title = "Comparison Leagues";

            if(selectedHomeLeagueId != null && selectedAwayLeagueId != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                var leagues = await _sender.Send(new GetLeagueQuery(), cancellationToken);

                if (leagues.HasNoValue)
                {
                    return View("Error");
                }

                return View(new CompareLeaguesViewModel
                {
                    IsLoadedComparisonResult = false,
                    AwayLeaguesSelection = new SelectList(leagues.Value.LeagueResponses, "Id", "OfficialName"),
                    HomeLeaguesSelection = new SelectList(leagues.Value.LeagueResponses, "Id", "OfficialName"),
                    SelectedAwayLeague = 1.ToString(),
                    SelectedHomeLeague = 1.ToString()

                });
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> CompareTeams(CancellationToken cancellationToken = default)
        {
            ViewBag.Title = "Comparison Leagues";

            return View(new CompareLeaguesViewModel
            {

            });
        }

        [HttpGet]
        public async Task<IActionResult> ComparePlayers(CancellationToken cancellationToken = default)
        {
            ViewBag.Title = "Comparison Leagues";

            return View(new CompareLeaguesViewModel
            {

            });
        }
    }
}
