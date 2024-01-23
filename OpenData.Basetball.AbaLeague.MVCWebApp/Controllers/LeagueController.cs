﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;
using OpenData.Basketball.AbaLeague.Application.Features.CompetitionOrganization.Queries.GetCompetionOrganizations;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Commands.CreateLeague;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Commands.UpdateLeague;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueById;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetStandingsByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetTeamsByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayersStatByLeague;
using OpenData.Basketball.AbaLeague.Application.Features.Positions.Queries.GetPositions;
using OpenData.Basketball.AbaLeague.Application.Features.ProcessorTypes.Queries;
using OpenData.Basketball.AbaLeague.Application.Features.Seasons.Queries.GetSeasons;
using OpenData.Basketball.AbaLeague.Application.Features.StatsProperties.Queries;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamRangeStatsByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamStatsByLeagueId;
using OpenData.Basketball.AbaLeague.Domain.Enums;
using System.Collections.Frozen;

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
            var processorTypeResult = await _sender.Send(new GetProcessorTypeQuery(), cancellationToken);
            if (processorTypeResult.HasNoValue)
            {
                return View("Error");
            }
            var seasonsResult = await _sender.Send(new GetSeasonsQuery(), cancellationToken);
            if (seasonsResult.HasNoValue)
            {
                return View("Error");
            }
            var competionOrganizationResult = await _sender.Send(new GetCompetionOrganizationsQuery(), cancellationToken);
            if (competionOrganizationResult.HasNoValue)
            {
                return View("Error");
            }
            var leagueViewModel = new LeagueUpsertViewModel()
            {
                League = new LeagueItemViewModel {
                    Id = null,
                    BaseUrl = string.Empty,
                    BoxScoreUrl = string.Empty,
                    CalendarUrl = string.Empty,
                    MatchUrl = string.Empty,
                    OfficialName = string.Empty,
                    RosterUrl = string.Empty,
                    ShortName = string.Empty,
                    StandingUrl = string.Empty,
                    SelectedProcessorTypeId = 1.ToString(),
                    SelectedSeasonId = 1.ToString(),
                    ProcessorTypes = new SelectList(processorTypeResult.Value, "Id", "Name") ,
                    Seasons = new SelectList(seasonsResult.Value.SeasonItems, "Id", "Name"),
                    RoundsToPlay = 0,
                    CompetitionOrganizations = 
                    new SelectList(competionOrganizationResult.Value.CompetionOrganizationItems,  "Id", "Name"),
                    SelectedCompetitionOrganizationId = 1.ToString(),
                    StandingRowName =  string.Empty,
                    StandingRowUrl = string.Empty,
                    StandingTableSelector = string.Empty
                },
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
            leagueViewModel.League = new LeagueItemViewModel
            {
                Id = results.Value.Id,
                BaseUrl = results.Value.BaseUrl,
                BoxScoreUrl = results.Value.BoxScoreUrl,
                CalendarUrl = results.Value.CalendarUrl,
                MatchUrl = results.Value.MatchUrl,
                OfficialName = results.Value.OfficialName,
                RosterUrl = results.Value.RosterUrl,
                ShortName = results.Value.ShortName,
                StandingUrl = results.Value.StandingUrl,
                SelectedProcessorTypeId = ((int) results.Value.ProcessorType).ToString(),
                SelectedSeasonId = ((int)results.Value.SeasonId).ToString(),
                ProcessorTypes = new SelectList(processorTypeResult.Value, "Id", "Name"),
                Seasons = new SelectList(seasonsResult.Value.SeasonItems, "Id", "Name"),
                RoundsToPlay = results.Value.RoundsToPlay ?? 0,
                CompetitionOrganizations =
                    new SelectList(competionOrganizationResult.Value.CompetionOrganizationItems, "Id", "Name"),
                SelectedCompetitionOrganizationId = ((short) (results.Value.CompetitionOrganization)).ToString(),
                StandingRowName = results.Value.StandingRowName,
                StandingRowUrl = results.Value.StandingRowUrl,
                StandingTableSelector = results.Value.StandingTableSelector
            };
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
        public async Task<IActionResult> Save(LeagueUpsertViewModel model, CancellationToken cancellationToken = default)
        {
            if (!short.TryParse(model.League.SelectedProcessorTypeId, out short processorId))
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "Unable to parse processory type id"
                });
            }
            if (!int.TryParse(model.League.SelectedSeasonId, out int seasonId))
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "Unable to parse processory type id"
                });
            }
            if (!short.TryParse(model.League.SelectedCompetitionOrganizationId, out short competitionOrganizationId))
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "Unable to parse processory type id"
                });
            }
            if (model.League.Id == null)
            {
                var result = await _sender.Send(
                    new CreateLeagueCommand(model.League.OfficialName,
                                            model.League.ShortName, 
                                            model.League.StandingUrl, 
                                            model.League.CalendarUrl, 
                                            model.League.MatchUrl, 
                                            model.League.BoxScoreUrl,
                                            model.League.BaseUrl, 
                                            model.League.RosterUrl,
                                            processorId,
                                            seasonId,
                                            model.League.RoundsToPlay,
                                            competitionOrganizationId,
                                            model.League.StandingTableSelector,
                                            model.League.StandingRowName,
                                            model.League.StandingRowUrl));

                if (result.IsSuccess)
                {
                    string redirectUrl = $"/League/Index";
                    return Redirect(redirectUrl);
                }
                return RedirectToAction("Error");
            }
            else
            {
                var result = await _sender.Send(
                    new UpdateLeagueCommand(model.League.Id ?? 0, 
                                            model.League.OfficialName,
                                            model.League.ShortName,
                                            model.League.StandingUrl,
                                            model.League.CalendarUrl,
                                            model.League.MatchUrl,
                                            model.League.BoxScoreUrl,
                                            model.League.BaseUrl,
                                            model.League.RosterUrl,
                                            processorId,
                                            seasonId,
                                            model.League.RoundsToPlay,
                                            competitionOrganizationId));
                if (result.IsSuccess)
                {
                    string redirectUrl = $"/League/Index";
                    return Redirect(redirectUrl);
                }
                return RedirectToAction("Error");
            }
           
            

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

            var draftSeasonResources = results.Value.DraftTeamSeasonResourcesItems
                                                        .Select(x => new AddSeasonResourceDraftDto(x.TeamId ?? 0, 
                                                                                                    league.Value.Id ?? -1, 
                                                                                                    x.Url, 
                                                                                                    x.Name, 
                                                                                                    x.TeamUrl, 
                                                                                                    x.IncrowdUrl));
            var notExistingTeams = results.Value.MissingTeamItems
                                                    .Select(x =>  new AddSeasonResourceDraftDto(-1,
                                                                                                league.Value.Id ?? -1, 
                                                                                                string.Empty,
                                                                                                x.Name,
                                                                                                x.Url,
                                                                                                x.IncrowdUrl));
            var existingResources = results.Value.ExistingTeamSeasonResourcesItems
                                                        .Select(x => new AddSeasonResourceDraftDto(x.TeamId ?? 0, 
                                                                                                    league.Value.Id ?? -1,
                                                                                                    x.Url, 
                                                                                                    x.Name,
                                                                                                    x.TeamUrl, 
                                                                                                    x.IncrowdUrl));
            var leagueDetailsViewModel = new SeasonResourcesViewModel()
            {
                ExistingTeams = existingResources.ToList(),
                MissingTeams = notExistingTeams.ToList(),
                DraftTeams = draftSeasonResources.ToList(),
                LeagueId = leagueId,
                IsLeagueOrganization = league.Value.CompetitionOrganization == CompetitionOrganizationEnum.League
            };
            ViewBag.Title = league.Value.ShortName + " -" + modelName;
            return View(leagueDetailsViewModel);
        }

        public async Task<IActionResult> Standings(int leagueId, CancellationToken cancellationToken = default)
        {
            var standingsResult = await _sender.Send(new GetStandingsByLeagueIdQuery(leagueId), cancellationToken);
            var leagueResult = await _sender.Send(new GetLeagueByIdQuery(leagueId), cancellationToken);

            var modelName = " Standings";

            if (standingsResult.HasNoValue || leagueResult.HasNoValue)
            {
                ViewBag.Title = modelName;
                return View("Error");
            }
            List<StandingsViewModel> standings = new List<StandingsViewModel>();

            switch (leagueResult.Value.CompetitionOrganization)
            {
                case CompetitionOrganizationEnum.League:
                    standings.Add(new StandingsViewModel
                    {
                        LeagueId = leagueId,
                        Title = "Total standings",
                        StandingItems = standingsResult.Value.StandingItems
                                          .OrderByDescending(x => x.WonGames)
                                          .ThenByDescending(x => x.PointDifference)
                                          .ThenByDescending(x => x.ScoredHomePoints)
                                          .Select(x => new LeagueStandingItemViewModel
                                          {
                                              CountryCode = x.CountryCode,
                                              CountryId = x.CountryId,
                                              LostGames = x.LostGames,
                                              PlayedGames = x.PlayedGames,
                                              PointDifference = x.PointDifference,
                                              ReceivedPoints = x.ReceivedPoints,
                                              ScoredPoints = x.ScoredPoints,
                                              TeamId = x.TeamId,
                                              TeamName = x.TeamName,
                                              WonGames = x.WonGames,
                                              RecentForm = x.RecentForm.ToList(),
                                          }).ToList()
                    });

                    standings.Add(new StandingsViewModel
                    {
                        LeagueId = leagueId,
                        Title = "Home match standings",
                        StandingItems = standingsResult.Value.StandingItems
                        .OrderByDescending(x => x.WonHomeGames)
                        .ThenBy(x => x.LostHomeGames)
                        .ThenByDescending(x => (x.ScoredHomePoints - x.ReceivedHomePoints))
                        .Select(x => new LeagueStandingItemViewModel
                        {
                            CountryCode = x.CountryCode,
                            CountryId = x.CountryId,
                            LostGames = x.LostHomeGames,
                            PlayedGames = x.PlayedHomeGames,
                            PointDifference = x.ScoredHomePoints - x.ReceivedHomePoints,
                            ReceivedPoints = x.ReceivedHomePoints,
                            ScoredPoints = x.ScoredHomePoints,
                            TeamId = x.TeamId,
                            TeamName = x.TeamName,
                            WonGames = x.WonHomeGames,
                            RecentForm = x.HomeRecentForm.ToList()
                        }).ToList()
                    });

                    standings.Add(new StandingsViewModel
                    {
                        LeagueId = leagueId,
                        Title = "Away match standings",
                        StandingItems = standingsResult.Value.StandingItems
                        .OrderByDescending(x => x.WonAwayGames)
                        .ThenBy(x => x.LostAwayGames)
                        .ThenByDescending(x => (x.ScoredAwayPoints - x.ReceivedAwayPoints))
                        .Select(x => new LeagueStandingItemViewModel
                         {
                             CountryCode = x.CountryCode,
                             CountryId = x.CountryId,
                             LostGames = x.LostAwayGames,
                             PlayedGames = x.PlayedAwayGames,
                             PointDifference = x.ScoredAwayPoints - x.ReceivedAwayPoints,
                             ReceivedPoints = x.ReceivedAwayPoints,
                             ScoredPoints = x.ScoredAwayPoints,
                             TeamId = x.TeamId,
                             TeamName = x.TeamName,
                             WonGames = x.WonAwayGames,
                             RecentForm = x.AwayRecentForm.ToList()
                         }).ToList(),
                    }); 
                    break;
                case CompetitionOrganizationEnum.Groups:
                    foreach(var item in standingsResult.Value.GroupStandingItems)
                    {
                        standings.Add(new StandingsViewModel
                        {
                            LeagueId = leagueId,
                            Title = $"Standings - {item.Name}",
                            StandingItems = item.StandingsItems
                                         .OrderByDescending(x => x.WonGames)
                                         .ThenByDescending(x => x.PointDifference)
                                         .ThenByDescending(x => x.ScoredHomePoints)
                                         .Select(x => new LeagueStandingItemViewModel
                                         {
                                             CountryCode = x.CountryCode,
                                             CountryId = x.CountryId,
                                             LostGames = x.LostGames,
                                             PlayedGames = x.PlayedGames,
                                             PointDifference = x.PointDifference,
                                             ReceivedPoints = x.ReceivedPoints,
                                             ScoredPoints = x.ScoredPoints,
                                             TeamId = x.TeamId,
                                             TeamName = x.TeamName,
                                             WonGames = x.WonGames,
                                             RecentForm = x.RecentForm.ToList(),
                                         }).ToList()
                        });
                    }


                    break;
                default: throw new NotImplementedException();
            }
          
            var leagueDetailsViewModel = new LeagueStandingsViewModel()
            {
                LeagueId = standingsResult.Value.LeagueId,
                LeagueName = standingsResult.Value.LeagueName,
                PlayedRounds = standingsResult.Value.PlayedRounds,
                TotalRounds = standingsResult.Value.TotalRounds,
                StandingItems = standings
            };
            ViewBag.Title = leagueDetailsViewModel.LeagueName + " -" + modelName;
            return View(leagueDetailsViewModel);
        }

        public async Task<IActionResult> TeamsLeaderboard(int leagueId, CancellationToken cancellationToken = default)
        {
            var results = await _sender.Send(new GetTeamStatsByLeagueIdQuery(leagueId), cancellationToken);

            var modelName = " Leaderboard";

            if (results.HasNoValue)
            {
                ViewBag.Title = modelName;
                return View("Error");
            }

            ViewBag.Title = results.Value.LeagueName + " -" + modelName;
            return View(new LeagueTeamsStatsViewModel
            {
                LeagueId = results.Value.LeagueId,
                LeagueName = results.Value.LeagueName,
                StatsItem = results.Value.StatItems.Select(x=>new LeagueTeamsStatsItemViewModel
                {
                    AgainstBlock = x.AgainstBlock,
                    Assists = x.Assists,
                    CommittedFoul = x.CommittedFoul,
                    DefensiveRebounds = x.DefensiveRebounds,
                    InFavoureOfBlock = x.InFavoureOfBlock,
                    OffensiveRebounds = x.OffensiveRebounds,
                    PlusMinus = x.PlusMinus,
                    PointFrom2ndChance = x.PointFrom2ndChance,
                    PointFromFastBreak = x.PointFromFastBreak,
                    PointFromPain = x.PointFromPain,
                    Points = x.Points,
                    RankValue = x.RankValue,
                    ReceivedFoul = x.ReceivedFoul,
                    ShotAttempted1Pt = x.ShotAttempted1Pt,
                    ShotAttempted2Pt = x.ShotAttempted2Pt,
                    ShotAttempted3Pt = x.ShotAttempted3Pt,
                    ShotMade1Pt = x.ShotMade1Pt,
                    ShotMade2Pt = x.ShotMade2Pt,
                    ShotMade3Pt = x.ShotMade3Pt,
                    Steals = x.Steals,
                    TeamId = x.TeamId,
                    TeamName = x.TeamName,
                    TotalRebounds = x.TotalRebounds,
                    Turnover = x.Turnover
                }).ToList()
            });
        }

        public async Task<IActionResult> PlayersLeaderboard(int leagueId, CancellationToken cancellationToken = default)
        {
            var results = await _sender.Send(new GetPlayersStatByLeagueQuery(leagueId), cancellationToken);

            var modelName = " Leaderboard";

            if (results.HasNoValue)
            {
                ViewBag.Title = modelName;
                return View("Error");
            }

            ViewBag.Title = results.Value.LeagueName + " -" + modelName;
            return View(new LeaguePlayersStatsViewModel
            {
                LeagueId = results.Value.LeagueId,
                LeagueName = results.Value.LeagueName,
                StatsItem = results.Value.StatItems.Select(x => new LeaguePlayersStatsItemViewModel
                {
                    TeamId =x.TeamId,
                    TeamName =x.TeamName,
                    Minutes =x.Minutes,
                    PlayerId = x.PlayerId, 
                    PlayerName = x.PlayerName,
                    AgainstBlock = x.AgainstBlock,
                    Assists = x.Assists,
                    CommittedFoul = x.CommittedFoul,
                    DefensiveRebounds = x.DefensiveRebounds,
                    InFavoureOfBlock = x.InFavoureOfBlock,
                    OffensiveRebounds = x.OffensiveRebounds,
                    PlusMinus = x.PlusMinus,
                    PointFrom2ndChance = x.PointFrom2ndChance,
                    PointFromFastBreak = x.PointFromFastBreak,
                    PointFromPain = x.PointFromPain,
                    Points = x.Points,
                    RankValue = x.RankValue,
                    ReceivedFoul = x.ReceivedFoul,
                    ShotAttempted1Pt = x.ShotAttempted1Pt,
                    ShotAttempted2Pt = x.ShotAttempted2Pt,
                    ShotAttempted3Pt = x.ShotAttempted3Pt,
                    ShotMade1Pt = x.ShotMade1Pt,
                    ShotMade2Pt = x.ShotMade2Pt,
                    ShotMade3Pt = x.ShotMade3Pt,
                    Steals = x.Steals,
                    TotalRebounds = x.TotalRebounds,
                    Turnover = x.Turnover
                }).ToList()
            });
        }


        public async Task<IActionResult> LeagueRangeStatsByLeague(LeagueRangeStatsViewModel leagueRangeStatsViewModel, 
                                                            CancellationToken cancellationToken = default)
        {
            var modelName = " Range Stats";
            var statsPropertiesResult = await _sender.Send(new GetStatsPropertiesQuery(), cancellationToken);
            if (statsPropertiesResult.HasNoValue)
            {
                ViewBag.Title = modelName;
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "Unable to parse position id"
                });
            }
            int selectedStatsProperty = 0;
            List<StatsPropertyEnum> list = new List<StatsPropertyEnum>();
            if (string.IsNullOrWhiteSpace(leagueRangeStatsViewModel.SelectedStatsPropertyId))
            {
                list.Add(StatsPropertyEnum.Points); 
                selectedStatsProperty = (int) StatsPropertyEnum.Points;
            }
            else
            {
                if (!int.TryParse(leagueRangeStatsViewModel.SelectedStatsPropertyId, out selectedStatsProperty))
                {
                    return View("Error", new InfoDescriptionViewModel()
                    {
                        Description = "Unable to parse position id"
                    });
                }
                list.Add((StatsPropertyEnum)selectedStatsProperty);
            }

            var results = await _sender.Send(new GetTeamRangeStatsByLeagueIdQuery(leagueRangeStatsViewModel.LeagueId,
               list.ToFrozenSet()), cancellationToken);
            if (results.HasNoValue)
            {
                ViewBag.Title = modelName;
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "Unable to found ranges for available scopes"
                });
            }
           
            ViewBag.Title = results.Value.LeagueName + " -" + modelName;
            return View(new LeagueRangeStatsViewModel
            {
                LeagueId = results.Value.LeagueId,
                LeagueName = results.Value.LeagueName,
                TeamItems = results.Value.Stats.OrderBy(x=>x.Key.teamName).Select(x => new LeagueTeamRangeStatsViewModel
                {
                    TeamId = x.Key.teamId,
                    TeamName = x.Key.teamName,
                    RangeStatsItems = x.Value.OrderBy(x=>x.Id).Select(y => new RangeStatsItemViewModel
                    {
                        Id = y.Id,
                        OffensiveWinCount = y.ScoredWinCount,
                        OffensiveLossCount = y.ScoredLossCount,
                        DefensiveWinCount = y.ReceivedWinCount,
                        DefensiveLossCount = y.ReceivedLossCount,
                        MaxValue = y.MaxValue,
                        MinValue = y.MinValue,
                        StatsName = y.StatsProperty.ToString()
                    }).ToList()
                }).ToList(),
                SelectedStatsProperties = new SelectList(statsPropertiesResult.Value.PropertyItems.ToList(), "Id", "Name"),
                SelectedStatsPropertyId = selectedStatsProperty.ToString()
            });
        }
    }
}
