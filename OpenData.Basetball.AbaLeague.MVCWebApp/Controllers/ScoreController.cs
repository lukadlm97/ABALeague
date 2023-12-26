using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Application.DTOs.Score;
using OpenData.Basketball.AbaLeague.Application.Features.Schedules.Commands.AddScheduleByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Schedules.Queries.GetScheduleByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Score.Commands.AddScoreByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Score.Queries.GetScoreDraftByLeagueId;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Controllers
{
    [Route("[controller]/[action]")]
    public class ScoreController : Controller
    {
        private readonly ILogger<ContentController> _logger;
        private readonly ISender _sender;

        public ScoreController(ILogger<ContentController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        [HttpGet("score/{leagueId:int}")]
        public async Task<IActionResult> Index(int leagueId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetScoreDraftByLeagueIdQuery(leagueId), cancellationToken);

            if(result.HasValue)
            {
                var scoreViewModel = new ScoreViewModel
                {
                    ExistingScores = result.Value.ExistingScoreItems.Select(x => new ScoreItemViewModel
                    {
                        Id = x.MatchId,
                        MatchNo = x.MatchNo,
                        HomeTeamId = x.HomeTeamId,
                        HomeTeamName = x.HomeTeamName,
                        HomeTeamPoints = x.HomeTeamPoints,
                        AwayTeamId = x.AwayTeamId,
                        Attendency = x.Attendency,
                        AwayTeamName = x.AwayTeamName,
                        AwayTeamPoints = x.AwayTeamPoints,
                        Venue = x.Venue
                    }).ToList(),
                    OnWaitingScores = result.Value.DraftScoreItems.Select(x => new ScoreItemViewModel
                    {
                        Id = x.MatchId,
                        MatchNo = x.MatchNo,
                        HomeTeamId = x.HomeTeamId,
                        HomeTeamName = x.HomeTeamName,
                        HomeTeamPoints = x.HomeTeamPoints,
                        AwayTeamId = x.AwayTeamId,
                        Attendency = x.Attendency,
                        AwayTeamName = x.AwayTeamName,
                        AwayTeamPoints = x.AwayTeamPoints,
                        Venue = x.Venue
                    }).ToList(),
                    ToBePlayed = result.Value.PlannedToPlayItems.Select(x => new ScoreItemViewModel
                    {
                        Id = x.MatchId,
                        MatchNo = x.MatchNo,
                        HomeTeamId = x.HomeTeamId,
                        HomeTeamName = x.HomeTeamName,
                        HomeTeamPoints = x.HomeTeamPoints,
                        AwayTeamId = x.AwayTeamId,
                        Attendency = x.Attendency,
                        AwayTeamName = x.AwayTeamName,
                        AwayTeamPoints = x.AwayTeamPoints,
                        Venue = x.Venue
                    }).ToList(),
                    LeagueId = leagueId
                };

                return View("Index", scoreViewModel);
            }

            return View("Error",new InfoDescriptionViewModel
            {
                Description = "Unable to find draft scores"
            });
        }
        [Route("{scoreLeagueId:int}")]
        public async Task<IActionResult> AddScores(int scoreLeagueId,
                                                      CancellationToken cancellationToken = default)
        {
            var results = await _sender.Send(new GetScoreDraftByLeagueIdQuery(scoreLeagueId), cancellationToken);

            if (results.HasNoValue)
            {
                return View("Error");
            }

            var addingResult = await _sender
                .Send(new AddScoreByLeagueIdCommand
                    (scoreLeagueId, results.Value.DraftScoreItems.Select(x => new AddScoreDto(x.MatchId, x.HomeTeamId, x.AwayTeamId, x.HomeTeamPoints??0, x.AwayTeamPoints??0, x.Attendency??0, x.Venue))), cancellationToken);

            InfoDescriptionViewModel infoViewModel = null;
            if (addingResult.IsFailure)
            {
                infoViewModel = new InfoDescriptionViewModel
                {
                    Description = addingResult.Error.Message
                };
                return View("Error", infoViewModel);
            }

            ViewBag.Title = "Score Managment";
            infoViewModel = new InfoDescriptionViewModel
            {
                Description = "Added score items"
            };


            return View("Success", infoViewModel);
        }
    }
}
