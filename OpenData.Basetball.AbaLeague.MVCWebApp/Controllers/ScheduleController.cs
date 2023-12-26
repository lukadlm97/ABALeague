using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Application.Features.Schedules.Commands.AddScheduleByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Schedules.Queries.GetScheduleByLeagueId;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Controllers
{
    [Route("[controller]/[action]")]
    public class ScheduleController : Controller
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly ISender _sender;

        public ScheduleController(ILogger<ScheduleController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }
        [HttpGet("{leagueId:int}")]
        public async Task<IActionResult> Index(int leagueId, CancellationToken cancellationToken = default)
        {
            var results = await _sender.Send(new GetScheduleDraftByLeagueIdQuery(leagueId), cancellationToken);

            if (results.HasNoValue)
            {
                return View("Error");
            }
            ViewBag.Title = "Schedule Managment";
            var scheduleViewModel = new ScheduleViewModel()
            {
                ExistingScheduleItems = results.Value.ExistingScheduleItems
                                                .Select(x => new ScheduleItemViewModel()
                                                {
                                                    Id = x.Id,
                                                    AwayTeamId = x.AwayTeamId,
                                                    AwayTeamName = x.AwayTeamName,
                                                    HomeTeamId = x.HomeTeamId,
                                                    HomeTeamName = x.HomeTeamName,
                                                    DateTime = x.DateTime,
                                                    MatchNo = x.MatchNo,
                                                    Round = x.Round
                                                }).ToList(),
                NewScheduleItems = results.Value.DraftScheduleItems
                                                .Select(x => new ScheduleItemViewModel()
                                                {
                                                    Id = x.Id,
                                                    AwayTeamId = x.AwayTeamId,
                                                    AwayTeamName = x.AwayTeamName,
                                                    HomeTeamId = x.HomeTeamId,
                                                    HomeTeamName = x.HomeTeamName,
                                                    DateTime = x.DateTime,
                                                    MatchNo = x.MatchNo,
                                                    Round = x.Round
                                                }).ToList(),
                PlanedItems = results.Value.PlanedScheduleItems
                                                .Select(x => new ScheduleItemViewModel()
                                                {
                                                    Id = x.Id,
                                                    AwayTeamId = x.AwayTeamId,
                                                    AwayTeamName = x.AwayTeamName,
                                                    HomeTeamId = x.HomeTeamId,
                                                    HomeTeamName = x.HomeTeamName,
                                                    DateTime = x.DateTime,
                                                    MatchNo = x.MatchNo,
                                                    Round = x.Round
                                                }).ToList(),
                MissingTeams = results.Value.MissingTeams.ToList(),
                LeagueId = leagueId
            };

            return View(scheduleViewModel);
        }

        [HttpPost("{scheduleLeagueId:int}")]
        public async Task<IActionResult> AddSchedule(int scheduleLeagueId,
                                                       CancellationToken cancellationToken = default)
        {
            var results = await _sender.Send(new GetScheduleDraftByLeagueIdQuery(scheduleLeagueId), cancellationToken);

            if (results.HasNoValue)
            {
                return View("Error");
            }

            var addingResult = await _sender
                .Send(new AddScheduleByLeagueIdCommand
                    (results.Value.DraftScheduleItems.Select(x => new AddScheduleDto(x.HomeTeamId, x.AwayTeamId, x.Round, x.MatchNo, x.DateTime)),
                    scheduleLeagueId,
                    false), cancellationToken);
            InfoDescriptionViewModel infoViewModel = null;
            if (addingResult.IsFailure)
            {
                infoViewModel = new InfoDescriptionViewModel
                {
                    Description = addingResult.Error.Message
                };
                return View("Error", infoViewModel);
            }

            ViewBag.Title = "Schedule Managment";
            infoViewModel = new InfoDescriptionViewModel
            {
                Description = "Added schedule items"
            };


            return View("Success", infoViewModel);
        }
    }
}
