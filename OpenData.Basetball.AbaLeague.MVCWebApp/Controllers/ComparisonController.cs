using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basetball.AbaLeague.MVCWebApp.Utilities;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueComparisonByLeagueIds;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;
using System.Collections.Frozen;

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
        public async Task<IActionResult> CompareLeagues([FromQuery]string SelectedAwayLeague,
                                                        [FromQuery] string SelectedHomeLeague,
                                                        CancellationToken cancellationToken = default)
        {
            ViewBag.Title = "Comparison Leagues";

            if(SelectedHomeLeague != null &&
                SelectedAwayLeague != null &&
                SelectedHomeLeague != SelectedAwayLeague)
            {
                if(int.TryParse(SelectedHomeLeague, out int homeLeagueId) 
                    && int.TryParse(SelectedAwayLeague, out int awayLeagueId))
                {

                    var leagues = await _sender.Send(new GetLeagueComparisonByLeagueIdsQuery(new List<int>
                    {
                        homeLeagueId, awayLeagueId
                    }.ToFrozenSet()), cancellationToken);
                    if(leagues.HasNoValue) 
                    {
                        return View("Error");
                    }
                    var homeLeague = leagues.Value.LeagueCompareItems.FirstOrDefault();
                    var awayLeague = leagues.Value.LeagueCompareItems.LastOrDefault();

                    return View(new CompareLeaguesViewModel
                    {
                        IsLoadedComparisonResult = true,
                        HomeLeague = new CompareItemViewModel
                        {
                            Id = homeLeague.Id,
                            Name = homeLeague.Name,
                            CorePerformance = new ComparePerformanceItemViewModel
                            {
                                AvgAssists = homeLeague.AvgAssists,
                                AvgBlocksMade = homeLeague.AvgBlocksMade,
                                AvgBlocksOn = homeLeague.AvgBlocksOn,
                                AvgDefensiveRebounds = homeLeague.AvgDefensiveRebounds,
                                AvgOffensiveRebounds = homeLeague.AvgOffensiveRebounds,
                                AvgPoints = homeLeague.AvgPoints,
                                AvgRebounds = homeLeague.AvgRebounds,
                                AvgSteals = homeLeague.AvgSteals,
                                AvgTurnovers = homeLeague.AvgTurnovers,
                                GamesPlayed = homeLeague.GamesPlayed,
                                GamesToPlay = homeLeague.GamesToPlay,
                                TotalAssists = homeLeague.TotalAssists,
                                TotalBlocksMade = homeLeague.TotalBlocksMade,
                                TotalBlocksOn = homeLeague.TotalBlocksOn,
                                TotalDefensiveRebounds = homeLeague.TotalDefensiveRebounds,
                                TotalOffensiveRebounds = homeLeague.TotalOffensiveRebounds,
                                TotalPoints = homeLeague.TotalPoints,
                                TotalRebounds = homeLeague.TotalRebounds,
                                TotalSteals = homeLeague.TotalSteals,
                                TotalTurnovers = homeLeague.TotalTurnovers,
                                PerformanceByPositions = homeLeague.Items.Select(x => new TotalPerformanceByPositionItemViewModel
                                {
                                    ParticipationAssists = x.BoxScoreItemDto.ParticipationAssists,
                                    ParticipationBlocksMade = x.BoxScoreItemDto.ParticipationBlocksMade,
                                    ParticipationBlocksOn = x.BoxScoreItemDto.ParticipationBlocksOn,
                                    ParticipationDefensiveRebounds = x.BoxScoreItemDto.ParticipationDefensiveRebounds,
                                    ParticipationOffensiveRebounds = x.BoxScoreItemDto.ParticipationOffensiveRebounds,
                                    ParticipationPoints  = x.BoxScoreItemDto.ParticipationPoints,
                                    ParticipationRebounds = x.BoxScoreItemDto.ParticipationRebounds,
                                    ParticipationSteals = x.BoxScoreItemDto.ParticipationSteals,
                                    ParticipationTurnovers = x.BoxScoreItemDto.ParticipationTurnovers,
                                    Position = x.PositionEnum,
                                    PositionColor = x.PositionEnum.ConvertPositionEnumToColor(),
                                    PositionName = x.PositionEnum.ToString(),
                                    TotalAssists = x.BoxScoreItemDto.TotalAssists,
                                    TotalBlocksMade = x.BoxScoreItemDto.TotalBlocksMade,
                                    TotalBlocksOn = x.BoxScoreItemDto.TotalBlocksOn,
                                    TotalDefensiveRebounds = x.BoxScoreItemDto.TotalDefensiveRebounds,
                                    TotalOffensiveRebounds = x.BoxScoreItemDto.TotalOffensiveRebounds,
                                    TotalPoints = x.BoxScoreItemDto.TotalPoints,
                                    TotalRebounds = x.BoxScoreItemDto.TotalRebounds,
                                    TotalSteals = x.BoxScoreItemDto.TotalSteals,
                                    TotalTurnovers = x.BoxScoreItemDto.TotalTurnovers
                                }).ToList(),
                            },
                            GamesPlayed = homeLeague.GamesPlayed,
                            TotalGames = homeLeague.GamesToPlay
                        },
                        AwayLeague = new CompareItemViewModel
                        {
                            Id = awayLeague.Id,
                            Name = awayLeague.Name,
                            CorePerformance = new ComparePerformanceItemViewModel
                            {
                                AvgAssists = awayLeague.AvgAssists,
                                AvgBlocksMade = awayLeague.AvgBlocksMade,
                                AvgBlocksOn = awayLeague.AvgBlocksOn,
                                AvgDefensiveRebounds = awayLeague.AvgDefensiveRebounds,
                                AvgOffensiveRebounds = awayLeague.AvgOffensiveRebounds,
                                AvgPoints = awayLeague.AvgPoints,
                                AvgRebounds = awayLeague.AvgRebounds,
                                AvgSteals = awayLeague.AvgSteals,
                                AvgTurnovers = awayLeague.AvgTurnovers,
                                GamesPlayed = awayLeague.GamesPlayed,
                                GamesToPlay = awayLeague.GamesToPlay,
                                TotalAssists = awayLeague.TotalAssists,
                                TotalBlocksMade = awayLeague.TotalBlocksMade,
                                TotalBlocksOn = awayLeague.TotalBlocksOn,
                                TotalDefensiveRebounds = awayLeague.TotalDefensiveRebounds,
                                TotalOffensiveRebounds = awayLeague.TotalOffensiveRebounds,
                                TotalPoints = awayLeague.TotalPoints,
                                TotalRebounds = awayLeague.TotalRebounds,
                                TotalSteals = awayLeague.TotalSteals,
                                TotalTurnovers = awayLeague.TotalTurnovers,
                                PerformanceByPositions = awayLeague.Items.Select(x => new TotalPerformanceByPositionItemViewModel
                                {
                                    ParticipationAssists = x.BoxScoreItemDto.ParticipationAssists,
                                    ParticipationBlocksMade = x.BoxScoreItemDto.ParticipationBlocksMade,
                                    ParticipationBlocksOn = x.BoxScoreItemDto.ParticipationBlocksOn,
                                    ParticipationDefensiveRebounds = x.BoxScoreItemDto.ParticipationDefensiveRebounds,
                                    ParticipationOffensiveRebounds = x.BoxScoreItemDto.ParticipationOffensiveRebounds,
                                    ParticipationPoints = x.BoxScoreItemDto.ParticipationPoints,
                                    ParticipationRebounds = x.BoxScoreItemDto.ParticipationRebounds,
                                    ParticipationSteals = x.BoxScoreItemDto.ParticipationSteals,
                                    ParticipationTurnovers = x.BoxScoreItemDto.ParticipationTurnovers,
                                    Position = x.PositionEnum,
                                    PositionColor = x.PositionEnum.ConvertPositionEnumToColor(),
                                    PositionName = x.PositionEnum.ToString(),
                                    TotalAssists = x.BoxScoreItemDto.TotalAssists,
                                    TotalBlocksMade = x.BoxScoreItemDto.TotalBlocksMade,
                                    TotalBlocksOn = x.BoxScoreItemDto.TotalBlocksOn,
                                    TotalDefensiveRebounds = x.BoxScoreItemDto.TotalDefensiveRebounds,
                                    TotalOffensiveRebounds = x.BoxScoreItemDto.TotalOffensiveRebounds,
                                    TotalPoints = x.BoxScoreItemDto.TotalPoints,
                                    TotalRebounds = x.BoxScoreItemDto.TotalRebounds,
                                    TotalSteals = x.BoxScoreItemDto.TotalSteals,
                                    TotalTurnovers = x.BoxScoreItemDto.TotalTurnovers
                                }).ToList(),
                            },
                            GamesPlayed = awayLeague.GamesPlayed,
                            TotalGames = awayLeague.GamesToPlay,

                        },
                        PositionsWithColors = new List<(PositionEnum, string, string)>
                        {
                            (PositionEnum.Guard, PositionEnum.Guard.ConvertPositionEnumToColor(), PositionEnum.Guard.ToString()),
                            (PositionEnum.ShootingGuard, PositionEnum.ShootingGuard.ConvertPositionEnumToColor(), PositionEnum.ShootingGuard.ToString()),
                            (PositionEnum.Forward, PositionEnum.Forward.ConvertPositionEnumToColor(), PositionEnum.Forward.ToString()),
                            (PositionEnum.PowerForward, PositionEnum.PowerForward.ConvertPositionEnumToColor(), PositionEnum.PowerForward.ToString()),
                            (PositionEnum.Center, PositionEnum.Center.ConvertPositionEnumToColor(), PositionEnum.Center.ToString()),
                        }
                    });
                }
                else
                {
                    throw new NotImplementedException();
                }
                
                
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
