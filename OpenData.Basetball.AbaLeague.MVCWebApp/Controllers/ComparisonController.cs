using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models.Boxscore;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models.Partial;
using OpenData.Basetball.AbaLeague.MVCWebApp.Utilities;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueComparisonByLeagueIds;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetRegistredTeamsByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetTeamsByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayers;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetExistingRostersByTeam;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterByTeamId;
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
                    AwayLeaguesSelection = new SelectList(leagues.Value.LeagueItems, "Id", "OfficialName"),
                    HomeLeaguesSelection = new SelectList(leagues.Value.LeagueItems, "Id", "OfficialName"),
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
                    LeagueSelection = new SelectList(leagues.Value.LeagueItems, "Id", "OfficialName"),
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
                                },
                                AdvancedBoxscoreStatsView = new AdvancedBoxscoreStatsViewModel
                                {
                                    AverageSpectators = homeTeam.MatchSuccessCalcuationDto.AverageSpectators,
                                    TotalSpectators = homeTeam.MatchSuccessCalcuationDto.TotalSpectators,
                                    AwayGameScoredPoints = homeTeam.MatchSuccessCalcuationDto.AwayGameScoredPoints,
                                    GamePlayed = homeTeam.MatchSuccessCalcuationDto.GamePlayed,
                                    GamesWin = homeTeam.MatchSuccessCalcuationDto.GamesWin,
                                    HomeGameScoredPoints = homeTeam.MatchSuccessCalcuationDto.HomeGameScoredPoints,
                                    HomeGamesPlayed = homeTeam.MatchSuccessCalcuationDto.HomeGamesPlayed,
                                    HomeGamesWin = homeTeam.MatchSuccessCalcuationDto.HomeGamesWin,
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
                                },
                                AdvancedBoxscoreStatsView = new AdvancedBoxscoreStatsViewModel
                                {
                                    AverageSpectators = awayTeam.MatchSuccessCalcuationDto.AverageSpectators,
                                    TotalSpectators = awayTeam.MatchSuccessCalcuationDto.TotalSpectators,
                                    AwayGameScoredPoints = awayTeam.MatchSuccessCalcuationDto.AwayGameScoredPoints,
                                    GamePlayed = awayTeam.MatchSuccessCalcuationDto.GamePlayed,
                                    GamesWin = awayTeam.MatchSuccessCalcuationDto.GamesWin,
                                    HomeGameScoredPoints = awayTeam.MatchSuccessCalcuationDto.HomeGameScoredPoints,
                                    HomeGamesPlayed = awayTeam.MatchSuccessCalcuationDto.HomeGamesPlayed,
                                    HomeGamesWin = awayTeam.MatchSuccessCalcuationDto.HomeGamesWin,
                                }
                            }
                        }); ;
                    }
                   
                    var teams = await _sender.Send(new GetRegistredTeamsByLeagueIdQuery(leagueId), cancellationToken);
                    if (teams.HasNoValue)
                    {
                        return View("Error");
                    }
                    return View(new CompareTeamsViewModel
                    {
                        IsLoadedComparisonResult = false,
                        IsLeagueSelected = true,
                        AwayTeamsSelection = new SelectList(teams.Value.Teams, "Id","Name"),
                        HomeTeamsSelection = new SelectList(teams.Value.Teams, "Id", "Name"),
                        SelectedAwayTeam = teams.Value.Teams.FirstOrDefault().Id.ToString(),
                        SelectedHomeTeam = teams.Value.Teams.LastOrDefault().Id.ToString(),
                    });
                    //TODO populate teams selection
                }

            }
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> ComparePlayers([FromQuery] string? SelectedLeagueId = null,
                                                        [FromQuery] string? SelectedHomeTeamId = null,
                                                        [FromQuery] string? SelectedAwayTeamId = null,
                                                        [FromQuery] string? SelectedHomePlayerId = null,
                                                        [FromQuery] string? SelectedAwayPlayerId = null,
                                                        CancellationToken cancellationToken = default)
        {
            ViewBag.Title = "Comparison Players";
            int leagueId = 0;
            int homeTeamId = 0;
            int homePlayerId = 0;
            int awayTeamId = 0;
            int awayPlayerId = 0;
            if (SelectedLeagueId == null)
            {
                var leagues = await _sender.Send(new GetLeagueQuery(), cancellationToken);

                if (leagues.HasNoValue)
                {
                    return View("Error");
                }

                return View(new ComparePlayersViewModel
                {
                    AvailableLeagues = new SelectList(leagues.Value.LeagueItems, "Id", "OfficialName"),
                    SelectedLeagueId = null
                });
            }
            else
            {
                if (string.IsNullOrWhiteSpace(SelectedHomeTeamId) &&
                    string.IsNullOrWhiteSpace(SelectedAwayTeamId) &&
                    int.TryParse(SelectedLeagueId, out leagueId))
                {
                    var teams = 
                        await _sender.Send(new GetRegistredTeamsByLeagueIdQuery(leagueId), cancellationToken);
                    
                    if (teams.HasNoValue)
                    {
                        return View("Error");
                    }

                    return View(new ComparePlayersViewModel
                    {
                        SelectedLeagueId = SelectedLeagueId,
                        AvailableAwayTeams = new SelectList(teams.Value.Teams, "Id", "Name"),
                        AvailableHomeTeams = new SelectList(teams.Value.Teams, "Id", "Name"),
                        SelectedAwayTeamId = teams.Value.Teams.FirstOrDefault()!.Id.ToString(),
                        SelectedHomeTeamId = teams.Value.Teams.LastOrDefault()!.Id.ToString(),
                    });
                }

                if (!string.IsNullOrWhiteSpace(SelectedHomeTeamId) &&
                    !string.IsNullOrWhiteSpace(SelectedAwayTeamId) &&
                    int.TryParse(SelectedLeagueId, out leagueId) &&
                    int.TryParse(SelectedHomeTeamId, out homeTeamId) &&
                    int.TryParse(SelectedAwayTeamId, out awayTeamId)
                    )
                {
                    var homeTeamPlayers = await _sender
                               .Send(new GetRosterByTeamIdQuery(homeTeamId, leagueId),
                                       cancellationToken);
                    var awayTeamPlayers = await _sender
                              .Send(new GetRosterByTeamIdQuery(awayTeamId, leagueId),
                                      cancellationToken);

                    if (homeTeamPlayers.HasNoValue || awayTeamPlayers.HasNoValue)
                    {
                        return View("Error");
                    }

                    return View(new ComparePlayersViewModel
                    {
                        SelectedLeagueId = SelectedLeagueId,
                        AvailableAwayPlayers = new SelectList(awayTeamPlayers.Value.Items, "Id", "Name"),
                        AvailableHomePlayers = new SelectList(homeTeamPlayers.Value.Items, "Id", "Name"),
                        SelectedAwayPlayerId = awayTeamPlayers.Value.Items.First().Id.ToString(),
                        SelectedHomePlayerId = homeTeamPlayers.Value.Items.First().Id.ToString(),
                    });
                }

                if (!string.IsNullOrWhiteSpace(SelectedHomeTeamId) &&
                    !string.IsNullOrWhiteSpace(SelectedAwayTeamId) &&
                    int.TryParse(SelectedLeagueId, out leagueId) &&
                    int.TryParse(SelectedHomeTeamId, out homeTeamId) &&
                    int.TryParse(SelectedAwayTeamId, out awayTeamId) &&
                    int.TryParse(SelectedHomePlayerId, out homePlayerId) &&
                    int.TryParse(SelectedAwayPlayerId, out awayPlayerId) 
                    )
                {
                    var teams =
                        await _sender.Send(new GetRegistredTeamsByLeagueIdQuery(leagueId), cancellationToken);

                    if (teams.HasNoValue)
                    {
                        return View("Error");
                    }

                    return View(new ComparePlayersViewModel
                    {
                        SelectedLeagueId = SelectedLeagueId,
                        AvailableAwayTeams = new SelectList(teams.Value.Teams, "Id", "Name"),
                        AvailableHomeTeams = new SelectList(teams.Value.Teams, "Id", "Name"),
                        SelectedAwayTeamId = teams.Value.Teams.FirstOrDefault()!.Id.ToString(),
                        SelectedHomeTeamId = teams.Value.Teams.LastOrDefault()!.Id.ToString(),
                    });
                }
            }


            return View(new ComparePlayersViewModel
            {

            });
        }

        [HttpGet]
        public async Task<PartialViewResult> 
            GetPlayersByTeamAndLeague([FromQuery] string? SelectedTeamId = null,
                                        [FromQuery] string? SelectedLeagueId = null,
                                        CancellationToken cancellationToken = default)
        {
            if(string.IsNullOrWhiteSpace(SelectedTeamId) || 
                string.IsNullOrWhiteSpace(SelectedLeagueId))
            {
                return PartialView("Error");
            }
            if(int.TryParse(SelectedTeamId, out var selectedTeamId) &&
                int.TryParse(SelectedLeagueId, out var selectedLeagueId))
            {
                var players = await _sender
                                .Send(new GetRosterByTeamIdQuery(selectedTeamId, selectedLeagueId), 
                                        cancellationToken);

                return PartialView("_PlayersPartial", new PlayerItemsViewModelPartial
                {
                    AvailablePlayers = new SelectList(players.Value.Items, "Id", "Name"),
                    SelectedPlayerId = players.Value.Items.First().Id.ToString(),
                });
            }
            return PartialView("Error");
        }
    }
}
