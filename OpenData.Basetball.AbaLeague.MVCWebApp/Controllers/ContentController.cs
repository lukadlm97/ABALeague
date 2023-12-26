using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;
using OpenData.Basketball.AbaLeague.Application.Features.Schedules.Commands;
using OpenData.Basketball.AbaLeague.Application.Features.Schedules.Queries;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Controllers
{
    public class ContentController : Controller
    {
        private readonly ILogger<ContentController> _logger; 
        private readonly ISender _sender;

        public ContentController(ILogger<ContentController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var results = await _sender.Send(new GetLeagueQuery(), cancellationToken);

            if (results.HasNoValue)
            {
                return View("Error");
            }

            ViewBag.Title = "Leagues";
            var indexContentViewModel = new ContentViewModel()
            {
                Leagues = results.Value.LeagueResponses
                .Select(x => new SingleLeagueViewModel()
                { Id = x.Id, Name = x.OfficialName })
                .ToList()
            };

            return View(indexContentViewModel);
        }


    }
}
