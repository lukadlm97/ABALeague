using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basketball.AbaLeague.Application.DTOs.Country;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreByTeamIdAndLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Countries.Queries.GetCountries;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueById;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayers;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetExistingRostersByTeam;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterByTeamId;
using OpenData.Basketball.AbaLeague.Application.Features.Schedules.Queries.GetScheduleByTeamId;
using OpenData.Basketball.AbaLeague.Application.Features.Score.Queries.GetScoreByLeagueIdAndTeamId;
using OpenData.Basketball.AbaLeague.Application.Features.SeasonResources.Queries.GetSeasonResourcesByTeam;
using OpenData.Basketball.AbaLeague.Application.Features.Statistics.Queries.GetByPositions;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Commands.CreateTeam;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Commands.UpdateTeam;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamById;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamGamesByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeams;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Controllers
{
    public class TeamController : Controller
    {
        private readonly ILogger<LeagueController> _logger;
        private readonly ISender _sender;

        public TeamController(ILogger<LeagueController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        public async Task<IActionResult> Upsert(int? teamId, string? teamName, CancellationToken cancellationToken = default)
        {
            var countries = await _sender.Send(new GetCountriesQuery(), cancellationToken);

            if (countries.HasNoValue)
            {
                return View("Error");
            }
            var viewModel = new UpsertTeamViewModel()
            {
                Name = teamName,
                Countries = new SelectList(countries.Value.Countries, "CountryId", "Name")
            };
            if (teamId.HasValue)
            {
                var existingTeam = await _sender.Send(new GetTeamByIdQuery(teamId.Value), cancellationToken);
                if (existingTeam.HasNoValue)
                {
                    return View("Error", new InfoDescriptionViewModel()
                    {
                        Description = "Unable to find existing team"
                    });
                }

                viewModel.Name = existingTeam.Value.Name;
                viewModel.ShortCode = existingTeam.Value.ShortName;
                viewModel.Id = existingTeam.Value.Id;
                viewModel.SelectedCountryId = existingTeam.Value.CountryId.ToString();
            }

            return View(viewModel);
        }
        public async Task<IActionResult> Save(UpsertTeamViewModel upsertTeamViewModel, CancellationToken cancellationToken = default)
        {
            if (!int.TryParse(upsertTeamViewModel.SelectedCountryId, out int countryId))
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "Unable to parse country id"
                });
            }
            Result? result = null;
            if (upsertTeamViewModel.Id == null)
            {
                result =
                   await _sender.Send(
                       new CreateTeamCommand(upsertTeamViewModel.Name, upsertTeamViewModel.ShortCode,
                           countryId), cancellationToken);

                if (result.IsFailure)
                {
                    return View("Error", new InfoDescriptionViewModel()
                    {
                        Description = result.Error.Message
                    });
                }

                return View("Success", new InfoDescriptionViewModel()
                {
                    Description = "Successfully saved new team!"
                });
            }


            result = await _sender.Send(
                new UpdateTeamCommand(upsertTeamViewModel.Id ?? 0, upsertTeamViewModel.Name, upsertTeamViewModel.ShortCode,
                    countryId), cancellationToken);
            if (result.IsFailure)
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = result.Error.Message
                });
            }

            return View("Success", new InfoDescriptionViewModel()
            {
                Description = "Successfully saved team's update!"
            });
        }

        public async Task<IActionResult> Index(string filter = null, int pageNumber = 1, int pageSize = 1000, CancellationToken cancellationToken = default)
        {
            var countries = await _sender.Send(new GetCountriesQuery(), cancellationToken);

            if (countries.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "cant load countries"
                });
            }

            var teams = await _sender.Send(new GetTeamsQuery("", pageNumber, pageSize), cancellationToken);

            if (teams.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "cant load teams"
                });
            }

            var indexViewModel = new TeamIndexViewModel()
            {
                Teams = teams.Value.Teams.Select(x => new MVCWebApp.Models.TeamDto()
                {
                    Name = x.Name,
                    ShortName = x.ShortName,
                    Id = x.Id,
                    Country = x.CountryName
                }).OrderBy(x => x.Name).ToList(),
                Filter = filter,
                Number = pageNumber,
                Size = pageSize
            };
            return View(indexViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Search(TeamIndexViewModel model, CancellationToken cancellationToken = default)
        {
            var countries = await _sender.Send(new GetCountriesQuery(), cancellationToken);

            if (countries.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "cant load countries"
                });
            }

            var teams = await _sender.Send(new GetTeamsQuery(model.Filter, model.Number, model.Size), cancellationToken);

            if (teams.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "cant load teams"
                });
            }

            var indexViewModel = new TeamIndexViewModel()
            {
                Teams = teams.Value.Teams.Select(x => new MVCWebApp.Models.TeamDto()
                {
                    Name = x.Name,
                    ShortName = x.ShortName,
                    Id = x.Id,
                    Country = x.CountryName
                }).Where(x => x.Name.ToLower().Contains(model.Filter.ToLower())).ToList(),
                Filter = model.Filter,
                Number = 1,
                Size = 50
            };
            return View("Index", indexViewModel);
        }


        public async Task<IActionResult> Details(int? teamId, CancellationToken cancellationToken = default)
        {
            if (!teamId.HasValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "missing team id" });
            }
            var countries = await _sender.Send(new GetCountriesQuery(), cancellationToken);
            if (countries.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "cant load countries" });
            }
            var team = await _sender.Send(new GetTeamByIdQuery(teamId ?? 0), cancellationToken);

            if (team.HasNoValue)
            {

                return View("Error", new InfoDescriptionViewModel() { Description = "cant load team details" });
            }

            var leagues = await _sender.Send(new GetExistingRostersByTeamQuery(team.Value.Id), cancellationToken);
            if (leagues.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "cant load team details" });
            }

            var rosterResources = await _sender.Send(new GetSeasonResourcesByTeamQuery(team.Value.Id), cancellationToken);
            if (leagues.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "cant load team details" });
            }

            TeamDetailsViewModel? detailsViewModel = null;

            if (!leagues.Value.LeagueIds.Any())
            {
                detailsViewModel = new TeamDetailsViewModel()
                {
                    Id = team.Value.Id,
                    ShortName = team.Value.ShortName,
                    Country = team.Value.CountryName,
                    Name = team.Value.Name,
                    RosterItems = new List<PlayerViewModel>()
                };
                return View(detailsViewModel);
            }

            Maybe<RosterDto>? rosterItems = null;
            var listOfValues = leagues.Value.LeagueIds.ToList();
            int? leagueId = null;
            for (int i = listOfValues.Count() - 1; i > -1; i--)
            {
                rosterItems = await _sender.Send(new GetRosterByTeamIdQuery(team.Value.Id, listOfValues[i]), cancellationToken);
                if (rosterItems.HasValue && rosterItems.Value.Items.Any())
                {
                    leagueId = i;
                    break;
                }
            }


            if (rosterItems.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "cant load team details" });
            }

            List<PlayerViewModel> list = new List<PlayerViewModel>();

            foreach (var rosterItem in rosterItems.Value.Items)
            {
                list.Add(new PlayerViewModel()
                {
                    Name = rosterItem.Name,
                    DateOfBirth = rosterItem.DateOfBirth,
                    Id = rosterItem.Id,
                    Height = rosterItem.Height,
                    Country = rosterItem.CountryName
                });
                
            }
            List<SeasonResourceViewModel> history = new List<SeasonResourceViewModel>();

            foreach (var rosterItem in rosterResources.Value)
            {

                if (rosterItem != null)
                {
                    history.Add(new SeasonResourceViewModel()
                    {
                        LeagueId = rosterItem.LeagueId,
                        LeagueName = rosterItem.LeagueName,
                        ShortName = rosterItem.ShortName,
                        TeamName = rosterItem.TeamName,
                        Url = rosterItem.Url
                    });
                }
            }



            detailsViewModel = new TeamDetailsViewModel()
            {
                Id = team.Value.Id,
                ShortName = team.Value.ShortName,
                Country = team.Value.CountryName,
                Name = team.Value.Name,
                RosterItems = list,
                RosterHistory = history,
                LeagueIdForLatestAvailableRoster = leagueId
            };

            return View(detailsViewModel);
        }

        public async Task<IActionResult> Performance(int leagueId,
                                                        int teamId,
                                                        CancellationToken cancellationToken = default)
        {
            var gamePerformance = await _sender
                .Send(new GetBoxscoreByTeamIdAndLeagueIdQuery(leagueId, teamId), cancellationToken);
            var perforamanceByPostion = await _sender
                .Send(new GetByPositionsQuery(teamId, leagueId), cancellationToken);


            if (gamePerformance.HasNoValue || perforamanceByPostion.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "not found any peformance" });
            }

            var gamePerformanceItems = gamePerformance.Value.Games.Select(x => new BoxscoreByTeamViewModel
            {
                AgainstBlock = x.AgainstBlock,
                Assists = x.Assists,
                Attendency = x.Attendency,
                CommittedFoul = x.CommittedFoul,
                Date = x.Date,
                DefensiveRebounds = x.DefensiveRebounds,
                IsHomeGame = x.HomeGame,
                IsWinTheGame = x.WinTheGame,
                InFavoureOfBlock = x.InFavoureOfBlock,
                MatchNo = x.MatchNo,
                OffensiveRebounds = x.OffensiveRebounds,
                OponentId = x.OponentId,
                OponentName = x.OponentName,
                PlusMinus = x.PlusMinus,
                PointFrom2ndChance = x.PointFrom2ndChance,
                PointFromFastBreak = x.PointFromFastBreak,
                PointFromPain = x.PointFromPain,
                Points = x.Points,
                RankValue = x.RankValue,
                ReceivedFoul = x.ReceivedFoul,
                Round = x.Round,
                ShotAttempted1Pt = x.ShotAttempted1Pt,
                ShotAttempted2Pt = x.ShotAttempted2Pt,
                ShotAttempted3Pt = x.ShotAttempted3Pt,
                ShotMade1Pt = x.ShotMade1Pt,
                ShotMade2Pt = x.ShotMade2Pt,
                ShotMade3Pt = x.ShotMade3Pt,
                Steals = x.Steals,
                TotalRebounds = x.TotalRebounds,
                Turnover = x.Turnover,
                Venue = x.Venue,
                Result = x.Result,
                ShotPrc1Pt = x.ShotPrc1Pt,
                ShotPrc2Pt = x.ShotPrc2Pt,
                ShotPrc3Pt = x.ShotPrc3Pt,
                MatchResultId = x.ResultId??0
            });

            
            return View(new PerformanceViewModel
            {
                LeagueId = gamePerformance.Value.LeagueId,
                LeagueName = gamePerformance.Value.LeagueName,
                TeamId = gamePerformance.Value.TeamId,
                TeamName = gamePerformance.Value.TeamName,
                StatsByRounds = gamePerformanceItems.ToList(),
                AverageBoxscoreStats = new AverageBoxscoreStatsViewModel
                {
                    AvgAssists = gamePerformance.Value.AverageBoxscoreCalcuations.AvgAssists,
                    AvgMinutes = gamePerformance.Value.AverageBoxscoreCalcuations.AvgMinutes,
                    AvgPlusMinus = gamePerformance.Value.AverageBoxscoreCalcuations.AvgPlusMinus,
                    AvgPoints = gamePerformance.Value.AverageBoxscoreCalcuations.AvgPoints,
                    AvgRankValue = gamePerformance.Value.AverageBoxscoreCalcuations.AvgRankValue,
                    AvgSteals = gamePerformance.Value.AverageBoxscoreCalcuations.AvgSteals,
                    AvgTotalRebounds = gamePerformance.Value.AverageBoxscoreCalcuations.AvgTotalRebounds,
                    AvgTurnover = gamePerformance.Value.AverageBoxscoreCalcuations.AvgTurnover,

                },
                AdvancedBoxscoreStats = new AdvancedBoxscoreStatsViewModel
                {
                    AverageSpectators = gamePerformance.Value.AdvancedMatchCalcuations.AverageSpectators,
                    AwayGameScoredPoints = gamePerformance.Value.AdvancedMatchCalcuations.AwayGameScoredPoints,
                    GamePlayed = gamePerformance.Value.AdvancedMatchCalcuations.GamePlayed,
                    GamesWin = gamePerformance.Value.AdvancedMatchCalcuations.GamesWin,
                    HomeGameScoredPoints = gamePerformance.Value.AdvancedMatchCalcuations.HomeGameScoredPoints,
                    HomeGamesPlayed = gamePerformance.Value.AdvancedMatchCalcuations.HomeGamesPlayed,
                    HomeGamesWin = gamePerformance.Value.AdvancedMatchCalcuations.HomeGamesWin,
                    TotalSpectators = gamePerformance.Value.AdvancedMatchCalcuations.TotalSpectators
                },
                PerformanceByPosition = new PerformanceByPositionViewModel
                {
                    PerformanceByPosition = perforamanceByPostion.Value.Items
                    .OrderBy(x=>x.PositionEnum)
                    .Select(x=>new PerformanceByPositionItemViewModel
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
                        PositionName = x.PositionEnum.ToString(),
                        TotalAssists = x.BoxScoreItemDto.TotalAssists,
                        TotalBlocksMade = x.BoxScoreItemDto.TotalBlocksMade,
                        TotalBlocksOn = x.BoxScoreItemDto.TotalBlocksOn,
                        TotalDefensiveRebounds = x.BoxScoreItemDto.TotalDefensiveRebounds,
                        TotalOffensiveRebounds = x.BoxScoreItemDto.TotalOffensiveRebounds,
                        TotalPoints = x.BoxScoreItemDto.TotalPoints,
                        TotalRebounds = x.BoxScoreItemDto.TotalRebounds,
                        TotalSteals = x.BoxScoreItemDto.TotalSteals,
                        TotalTurnovers = x.BoxScoreItemDto.TotalTurnovers,
                        
                    }).ToList()
                }
            });
        }

        public async Task<IActionResult> Matches(int leagueId,
                                                       int teamId,
                                                       CancellationToken cancellationToken = default)
        {
            var gamesResult = 
                await _sender.Send(new GetTeamGamesByLeagueIdQuery(leagueId, teamId), cancellationToken);

            if (gamesResult.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "not found any peformance" });
            }

            return View(new TeamMatchViewModel
            {
                LeagueId = gamesResult.Value.LeagueId,
                TeamId = gamesResult.Value.TeamId,
                LeagueName = gamesResult.Value.LeagueName,
                TeamName = gamesResult.Value.TeamName,
                TeamScoreItems = gamesResult.Value.GameStatsItems
                                                .Select(x=>new TeamGameViewModel()
                                                {
                                                    WinTheGame = x.WinTheGame,
                                                    Attendency = x.Attendency,
                                                    OponentName = x.OponentName,
                                                    OponentId = x.OponentId,    
                                                    IsHomeGame = x.HomeGame,
                                                    Date = x.Date,
                                                    MatchId = x.RoundMatchId,
                                                    MatchNo = x.MatchNo,
                                                    Result = x.Result,
                                                    ResultId = x.ResultId,
                                                    Round = x.Round,
                                                    Venue = x.Venue
                                                }).ToList(),
                TeamScheduledItems = gamesResult.Value.MatchItems
                                                .Select(x => new TeamGameViewModel()
                                                {
                                                    OponentName = x.OponentName,
                                                    OponentId = x.OponentId,
                                                    OponentCurrentRanking = x.OponentCurrentRanking,
                                                    IsHomeGame = x.HomeGame,
                                                    Date = x.Date,
                                                    MatchId = x.MatchNo,
                                                    MatchNo = x.MatchNo,
                                                    Round = x.Round,
                                                    OponentRecentForm = x.OponentRecentForm.ToList()
                                                }).ToList(),
            });
        }
    }
}
