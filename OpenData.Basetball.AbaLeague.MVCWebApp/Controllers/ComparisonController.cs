using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basetball.AbaLeague.MVCWebApp.Utilities;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueComparisonByLeagueIds;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetTeamsByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamsComparisonByTeamIds;
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
                                PerformanceByPositions = homeLeague.Items.Select(x => new TotalAndParticipatePerformanceByPositionItemViewModel
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
                                PerformanceByPositions = awayLeague.Items.Select(x => new TotalAndParticipatePerformanceByPositionItemViewModel
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
                        PositionPlaceholderItems = new List<(PositionEnum key, string color, string name)>()
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
        public async Task<IActionResult> CompareTeams([FromQuery] string SelectedLeague, 
                                                        [FromQuery] string SelectedHomeTeam, 
                                                        [FromQuery] string SelectedAwayTeam,
                                                        [FromQuery] bool IsLeagueSelected,
                                                        CancellationToken cancellationToken = default)
        {
            ViewBag.Title = "Comparison Teams";
            if (string.IsNullOrWhiteSpace(SelectedLeague))
            {
                var leagues = await _sender.Send(new GetLeagueQuery(), cancellationToken);

                if (leagues.HasNoValue)
                {
                    return View("Error");
                }

                return View(new CompareTeamsViewModel
                {
                    IsLoadedComparisonResult = false,
                    IsLeagueSelected = false,
                    LeagueSelection = new SelectList(leagues.Value.LeagueResponses, "Id", "OfficialName"),
                    SelectedLeague = 1.ToString(),

                });
            }
            else
            {
                if(string.IsNullOrWhiteSpace(SelectedLeague) ||
                    !int.TryParse(SelectedLeague, out int leagueId))
                {
                    return View("Error");
                }
                else
                {
                    if (IsLeagueSelected)
                    {
                        if (string.IsNullOrWhiteSpace(SelectedHomeTeam) ||
                   string.IsNullOrWhiteSpace(SelectedAwayTeam) ||
                   !int.TryParse(SelectedHomeTeam, out int homeTeamId) ||
                   !int.TryParse(SelectedAwayTeam, out int awayTeamId))
                        {
                            return View("Error");
                        }
                        var result = await _sender.Send(new GetTeamsComparisonByTeamIdsQuery(new List<(int, int)>()
                {
                    (homeTeamId, leagueId),
                    (awayTeamId, leagueId)
                }.ToFrozenSet()), cancellationToken);

                        if (result.HasNoValue)
                        {
                            return View("Error");
                        }

                        var homeTeam = result.Value.TeamCompareItems.FirstOrDefault();
                        var awayTeam = result.Value.TeamCompareItems.LastOrDefault();
                        if (homeTeam == null || awayTeam == null)
                        {
                            return View("Error");
                        }

                        return View(new CompareTeamsViewModel
                        {
                            PositionPlaceholderItems = 
                                new List<(PositionEnum key, string color, string name)>()
                                {
                                    (PositionEnum.Guard, PositionEnum.Guard.ConvertPositionEnumToColor(), PositionEnum.Guard.ToString()),
                                    (PositionEnum.ShootingGuard, PositionEnum.ShootingGuard.ConvertPositionEnumToColor(), PositionEnum.ShootingGuard.ToString()),
                                    (PositionEnum.Forward, PositionEnum.Forward.ConvertPositionEnumToColor(), PositionEnum.Forward.ToString()),
                                    (PositionEnum.PowerForward, PositionEnum.PowerForward.ConvertPositionEnumToColor(), PositionEnum.PowerForward.ToString()),
                                    (PositionEnum.Center, PositionEnum.Center.ConvertPositionEnumToColor(), PositionEnum.Center.ToString()),
                                }
                            ,
                            IsLoadedComparisonResult = true,
                            HomeTeam = new CompareTeamViewModel
                            {
                                Id = homeTeam.TeamItem.Id,
                                Name = homeTeam.TeamItem.Name,
                                RosterItems = new RosterItemsByPositionsViewModel
                                {
                                    RosterItems = homeTeam.RosterEntriesByPosition.Select(keyValuePair => new RosterItemByPositionViewModel
                                    {
                                        Players = keyValuePair.Value.Select(x => new PlayerAtRosterViewModel
                                        {
                                            Id = x.Id,
                                            IsActive = x.End == null,
                                            Name = x.Name,
                                            Position = x.Position.ToString()
                                        }).ToList(),
                                        Position = keyValuePair.Key,
                                        PositionColor = keyValuePair.Key.ConvertPositionEnumToColor(),
                                        PositionName = keyValuePair.Key.ToString()
                                    }).ToList(),

                                },
                                CorePerformance = new ComparePerformanceItemViewModel
                                {
                                    AvgAssists = homeTeam.TotalAndAveragePerformance?.AverageAssists ?? 0,
                                    AvgBlocksMade = homeTeam.TotalAndAveragePerformance?.AverageBlocksMade ?? 0,
                                    AvgBlocksOn = homeTeam.TotalAndAveragePerformance?.AverageBlocksOn ?? 0,
                                    AvgDefensiveRebounds = homeTeam.TotalAndAveragePerformance?.AverageDefensiveRebounds ?? 0,
                                    AvgOffensiveRebounds = homeTeam.TotalAndAveragePerformance?.AverageOffensiveRebounds ?? 0,
                                    AvgPoints = homeTeam.TotalAndAveragePerformance?.AveragePoints ?? 0,
                                    AvgRebounds = homeTeam.TotalAndAveragePerformance?.AverageRebounds ?? 0,
                                    AvgSteals = homeTeam.TotalAndAveragePerformance?.AverageSteals ?? 0,
                                    AvgTurnovers = homeTeam.TotalAndAveragePerformance?.AverageTurnovers ?? 0,
                                    GamesPlayed = homeTeam.TotalAndAveragePerformance?.GamesPlayed ?? 0,
                                    GamesToPlay = 0,
                                    TotalAssists = homeTeam.TotalAndAveragePerformance?.TotalAssists ?? 0,
                                    TotalBlocksMade = homeTeam.TotalAndAveragePerformance?.TotalBlocksMade ?? 0,
                                    TotalBlocksOn = homeTeam.TotalAndAveragePerformance?.TotalBlocksOn ?? 0,
                                    TotalDefensiveRebounds = homeTeam.TotalAndAveragePerformance?.TotalDefensiveRebounds ?? 0,
                                    TotalPoints = homeTeam.TotalAndAveragePerformance?.TotalPoints ?? 0,
                                    TotalOffensiveRebounds = homeTeam.TotalAndAveragePerformance?.TotalOffensiveRebounds ?? 0,
                                    TotalRebounds = homeTeam.TotalAndAveragePerformance?.TotalRebounds ?? 0,
                                    TotalSteals = homeTeam.TotalAndAveragePerformance?.TotalSteals ?? 0,
                                    TotalTurnovers = homeTeam.TotalAndAveragePerformance?.TotalTurnovers ?? 0,
                                    PerformanceByPositions =
                                    homeTeam.Items.Select(x => new TotalAndParticipatePerformanceByPositionItemViewModel
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
                                    }).OrderBy(x => x.Position)
                                    .ToList()
                                }
                            },
                            AwayTeam = new CompareTeamViewModel
                            {
                                Id = awayTeam.TeamItem.Id,
                                Name = awayTeam.TeamItem.Name,
                                RosterItems = new RosterItemsByPositionsViewModel
                                {
                                    RosterItems = awayTeam.RosterEntriesByPosition.Select(keyValuePair => new RosterItemByPositionViewModel
                                    {
                                        Players = keyValuePair.Value.Select(x => new PlayerAtRosterViewModel
                                        {
                                            Id = x.Id,
                                            IsActive = x.End == null,
                                            Name = x.Name,
                                            Position = x.Position.ToString()
                                        }).ToList(),
                                        Position = keyValuePair.Key,
                                        PositionColor = keyValuePair.Key.ConvertPositionEnumToColor(),
                                        PositionName = keyValuePair.Key.ToString()
                                    }).ToList(),
                                },

                                CorePerformance = new ComparePerformanceItemViewModel
                                {
                                    AvgAssists = awayTeam.TotalAndAveragePerformance?.AverageAssists ?? 0,
                                    AvgBlocksMade = awayTeam.TotalAndAveragePerformance?.AverageBlocksMade ?? 0,
                                    AvgBlocksOn = awayTeam.TotalAndAveragePerformance?.AverageBlocksOn ?? 0,
                                    AvgDefensiveRebounds = awayTeam.TotalAndAveragePerformance?.AverageDefensiveRebounds ?? 0,
                                    AvgOffensiveRebounds = awayTeam.TotalAndAveragePerformance?.AverageOffensiveRebounds ?? 0,
                                    AvgPoints = awayTeam.TotalAndAveragePerformance?.AveragePoints ?? 0,
                                    AvgRebounds = awayTeam.TotalAndAveragePerformance?.AverageRebounds ?? 0,
                                    AvgSteals = awayTeam.TotalAndAveragePerformance?.AverageSteals ?? 0,
                                    AvgTurnovers = awayTeam.TotalAndAveragePerformance?.AverageTurnovers ?? 0,
                                    GamesPlayed = awayTeam.TotalAndAveragePerformance?.GamesPlayed ?? 0,
                                    GamesToPlay = 0,
                                    TotalAssists = awayTeam.TotalAndAveragePerformance?.TotalAssists ?? 0,
                                    TotalBlocksMade = awayTeam.TotalAndAveragePerformance?.TotalBlocksMade ?? 0,
                                    TotalBlocksOn = awayTeam.TotalAndAveragePerformance?.TotalBlocksOn ?? 0,
                                    TotalDefensiveRebounds = awayTeam.TotalAndAveragePerformance?.TotalDefensiveRebounds ?? 0,
                                    TotalPoints = awayTeam.TotalAndAveragePerformance?.TotalPoints ?? 0,
                                    TotalOffensiveRebounds = awayTeam.TotalAndAveragePerformance?.TotalOffensiveRebounds ?? 0,
                                    TotalRebounds = awayTeam.TotalAndAveragePerformance?.TotalRebounds ?? 0,
                                    TotalSteals = awayTeam.TotalAndAveragePerformance?.TotalSteals ?? 0,
                                    TotalTurnovers = awayTeam.TotalAndAveragePerformance?.TotalTurnovers ?? 0,
                                    PerformanceByPositions =
                                    awayTeam.Items.Select(x => new TotalAndParticipatePerformanceByPositionItemViewModel
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
                                    }).OrderBy(x => x.Position)
                                    .ToList()
                                }
                            }
                        }); ;
                    }
                   
                    var teams = await _sender.Send(new GetTeamsByLeagueIdQuery(leagueId), cancellationToken);
                    if (teams.HasNoValue)
                    {
                        return View("Error");
                    }
                    return View(new CompareTeamsViewModel
                    {
                        IsLoadedComparisonResult = false,
                        IsLeagueSelected = true,
                        AwayTeamsSelection = new SelectList(teams.Value.ExistingTeamSeasonResourcesItems, "TeamId","Name"),
                        HomeTeamsSelection = new SelectList(teams.Value.ExistingTeamSeasonResourcesItems, "TeamId","Name"),
                        SelectedAwayTeam = teams.Value.ExistingTeamSeasonResourcesItems.FirstOrDefault().TeamId.ToString(),
                        SelectedHomeTeam = teams.Value.ExistingTeamSeasonResourcesItems.LastOrDefault().TeamId.ToString(),
                    });
                    //TODO populate teams selection
                }

            }
            throw new NotImplementedException();
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
