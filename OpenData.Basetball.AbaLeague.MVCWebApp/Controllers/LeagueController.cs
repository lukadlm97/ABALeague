using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Commands.CreateLeague;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueById;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetStandingsByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetTeamsByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Positions.Queries.GetPositions;
using OpenData.Basketball.AbaLeague.Application.Features.ProcessorTypes.Queries;
using OpenData.Basketball.AbaLeague.Domain.Enums;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Controllers
{
    [Route("[controller]/[action]")]
    public class LeagueController : Controller
    {
        private readonly ILogger<LeagueController> _logger;
        private readonly ISender _sender;

        public LeagueController(ILogger<LeagueController> logger, ISender sender)
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
            var leagueViewModel = new LeagueViewModel()
            {
                Leagues = results.Value.LeagueResponses.ToList(),
                IsSuccess = true
            };
            return View(leagueViewModel);
        }

        public async Task<IActionResult> Upsert(int? leagueId, CancellationToken cancellationToken = default)
        {
            var processorType = await _sender.Send(new GetProcessorTypeQuery(), cancellationToken);
            if (processorType.HasNoValue)
            {
                return View("Error");
            }
            var leagueViewModel = new CreateLeagueViewModel()
            {
                League = new LeagueDto(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,string.Empty, string.Empty, 1),
                ProcessorTypes = new SelectList(processorType.Value, "Id", "Name"),
            };
            var modelName = "Insert";
            if (leagueId == null)
            {
                ViewBag.Title = modelName;
                return View(leagueViewModel);
            }
            var results = await _sender.Send(new GetLeagueByIdQuery(leagueId??0), cancellationToken);
            if (results.HasNoValue)
            {
                return View("Error");
            }

            leagueViewModel.League = new LeagueDto(results.Value.OfficialName, results.Value.ShortName,
                results.Value.Season, results.Value.StandingUrl, results.Value.CalendarUrl, results.Value.MatchUrl,
                results.Value.BoxScoreUrl, results.Value.BaseUrl, results.Value.RosterUrl, results.Value.ProcessorTypeId??1);
            leagueViewModel.SelectedProcessorTypeId =  (results.Value.ProcessorTypeId ?? 1).ToString();
           

            ViewBag.Title = "Update";
            return View(leagueViewModel);
        }

        public async Task<IActionResult> Details(int leagueId, CancellationToken cancellationToken = default)
        {
            var results = await _sender.Send(new GetLeagueByIdQuery(leagueId), cancellationToken);

            var modelName =" Details";
            
            if (results.HasNoValue)
            {
                ViewBag.Title = modelName;
                return View("Error");
            }

            var leagueDetailsViewModel = new DetailsLeagueViewModel()
            {
                League = results.Value,
                IsLoaded = true,
            };
            ViewBag.Title = leagueDetailsViewModel.League.ShortName+" -"+modelName;
            return View(leagueDetailsViewModel);
        }
        public async Task<IActionResult> Save(CreateLeagueViewModel model, CancellationToken cancellationToken = default)
        {
                if (!short.TryParse(model.SelectedProcessorTypeId, out short processor))
                {
                    return View("Error", new InfoDescriptionViewModel()
                    {
                        Description = "Unable to parse processory type id"
                    });
                }
                var result = await _sender.Send(new CreateLeagueCommand(model.League.OfficialName, model.League.ShortName, model.League.Season, model.League.StandingUrl, model.League.CalendarUrl, model.League.MatchUrl, model.League.BoxScoreUrl, model.League.BaseUrl, model.League.RosterUrl, processor));

                if (result.IsSuccess)
                {

                    string redirectUrl = $"/League/Index";
                    return Redirect(redirectUrl);
                }
                return RedirectToAction("Error");
            

            // If the model state is not valid, you can return the view with validation errors
            return View("Upsert", model);
        }

        public async Task<IActionResult> SeasonResources(int leagueId, CancellationToken cancellationToken = default)
        {
            var results = await _sender.Send(new GetTeamsByLeagueIdQuery(leagueId), cancellationToken);
            var league = await _sender.Send(new GetLeagueByIdQuery(leagueId), cancellationToken);

            var modelName = " Resources";

            if (results.HasNoValue || league.HasNoValue)
            {
                ViewBag.Title = modelName;
                return View("Error");
            }

            var existingResources = results.Value.Where(x=>x.MaterializationStatus==MaterializationStatus.Exist).Select(x =>
                new AddSeasonResourceDraftDto(x.TeamId??0, league.Value.Id, x.Url, x.Name, x.TeamUrl, x.IncrowdUrl));
            var notExistingTeams = results.Value.Where(x => x.MaterializationStatus == MaterializationStatus.TeamNoExist).Select(x =>
                new AddSeasonResourceDraftDto(x.TeamId ?? 0, league.Value.Id, x.Url, x.Name, x.TeamUrl, x.IncrowdUrl));
            var notExistingResources = results.Value.Where(x => x.MaterializationStatus == MaterializationStatus.NotExist).Select(x =>
                new AddSeasonResourceDraftDto(x.TeamId ?? 0, league.Value.Id, x.Url, x.Name, x.TeamUrl, x.IncrowdUrl));
            var leagueDetailsViewModel = new SeasonResourcesViewModel()
            {
                ExistingTeams = existingResources.ToList(),
                MissingTeams = notExistingTeams.ToList(),
                NotExistingResourcesTeams = notExistingResources.ToList(),
                LeagueId = leagueId
            };
            ViewBag.Title = league.Value.ShortName + " -" + modelName;
            return View(leagueDetailsViewModel);
        }

        public async Task<IActionResult> Standings(int leagueId, CancellationToken cancellationToken = default)
        {
            var results = await _sender.Send(new GetStandingsByLeagueIdQuery(leagueId), cancellationToken);

            var modelName = " Standings";

            if (results.HasNoValue)
            {
                ViewBag.Title = modelName;
                return View("Error");
            }

            var leagueDetailsViewModel = new LeagueStandingsViewModel()
            {
                LeagueId = results.Value.LeagueId,
                LeagueName = results.Value.LeagueName,
                PlayedRounds = results.Value.PlayedRounds,
                TotalRounds = results.Value.TotalRounds,
                StandingItems = results.Value.StandingItems.Select(x => new LeagueStandingItemViewModel
                {
                    CountryCode = x.CountryCode,
                    CountryId = x.CountryId,
                    LostAwayGames = x.LostAwayGames,
                    LostGames = x.LostGames,
                    LostHomeGames = x.LostHomeGames,
                    PlayedAwayGames = x.PlayedAwayGames,
                    PlayedGames = x.PlayedGames,
                    PlayedHomeGames = x.PlayedHomeGames,
                    PointDifference = x.PointDifference,
                    ReceivedAwayPoints = x.ReceivedAwayPoints,
                    ReceivedHomePoints = x.ReceivedHomePoints,
                    ReceivedPoints = x.ReceivedPoints,
                    ScoredAwayPoints = x.ScoredAwayPoints,
                    ScoredHomePoints = x.ScoredHomePoints,
                    ScoredPoints = x.ScoredPoints,
                    TeamId = x.TeamId,
                    TeamName = x.TeamName,
                    WonAwayGames = x.WonAwayGames,  
                    WonGames = x.WonGames,
                    WonHomeGames = x.WonHomeGames,
                    RecentForm = x.RecentForm.ToList(),
                }).ToList(),
                HomeStandingItems = results.Value.StandingItems.OrderByDescending(x => x.WonHomeGames)
                                                                    .ThenBy(x => x.LostHomeGames)
                                                                    .ThenByDescending(x =>
                                                                        (x.ScoredHomePoints - x.ReceivedHomePoints)).Select(x => new LeagueStandingItemViewModel
                {
                    CountryCode = x.CountryCode,
                    CountryId = x.CountryId,
                    LostAwayGames = x.LostAwayGames,
                    LostGames = x.LostGames,
                    LostHomeGames = x.LostHomeGames,
                    PlayedAwayGames = x.PlayedAwayGames,
                    PlayedGames = x.PlayedGames,
                    PlayedHomeGames = x.PlayedHomeGames,
                    PointDifference = x.PointDifference,
                    ReceivedAwayPoints = x.ReceivedAwayPoints,
                    ReceivedHomePoints = x.ReceivedHomePoints,
                    ReceivedPoints = x.ReceivedPoints,
                    ScoredAwayPoints = x.ScoredAwayPoints,
                    ScoredHomePoints = x.ScoredHomePoints,
                    ScoredPoints = x.ScoredPoints,
                    TeamId = x.TeamId,
                    TeamName = x.TeamName,
                    WonAwayGames = x.WonAwayGames,
                    WonGames = x.WonGames,
                    WonHomeGames = x.WonHomeGames,
                    HomeRecentForm = x.HomeRecentForm.ToList()

                }).ToList(),
                 AwayStandingItems = results.Value.StandingItems.OrderByDescending(x=>x.WonAwayGames)
                                                                    .ThenBy(x => x.LostAwayGames)
                                                                    .ThenByDescending(x => 
                                                                        (x.ScoredAwayPoints-x.ReceivedAwayPoints))
                 .Select(x => new LeagueStandingItemViewModel
                 {
                     CountryCode = x.CountryCode,
                     CountryId = x.CountryId,
                     LostAwayGames = x.LostAwayGames,
                     LostGames = x.LostGames,
                     LostHomeGames = x.LostHomeGames,
                     PlayedAwayGames = x.PlayedAwayGames,
                     PlayedGames = x.PlayedGames,
                     PlayedHomeGames = x.PlayedHomeGames,
                     PointDifference = x.PointDifference,
                     ReceivedAwayPoints = x.ReceivedAwayPoints,
                     ReceivedHomePoints = x.ReceivedHomePoints,
                     ReceivedPoints = x.ReceivedPoints,
                     ScoredAwayPoints = x.ScoredAwayPoints,
                     ScoredHomePoints = x.ScoredHomePoints,
                     ScoredPoints = x.ScoredPoints,
                     TeamId = x.TeamId,
                     TeamName = x.TeamName,
                     WonAwayGames = x.WonAwayGames,
                     WonGames = x.WonGames,
                     WonHomeGames = x.WonHomeGames,
                     AwayRecentForm = x.AwayRecentForm.ToList()
                 }).ToList(),
                 
            };
            ViewBag.Title = leagueDetailsViewModel.LeagueName + " -" + modelName;
            return View(leagueDetailsViewModel);
        }
    }
}
