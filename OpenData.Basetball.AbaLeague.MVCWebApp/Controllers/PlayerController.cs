using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basetball.AbaLeague.MVCWebApp.Utilities;
using OpenData.Basetball.AbaLeague.Persistence.Migrations;
using OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreByPlayerIdAndLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Countries.Queries.GetCountries;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Commands.AddAnotherName;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Commands.CreatePlayer;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Commands.UpdatePlayer;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayer;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayerAnotherNames;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayerGames;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayers;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayersByCountryId;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.SearchPlayers;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.SearchPlayersByCountryId;
using OpenData.Basketball.AbaLeague.Application.Features.Positions.Queries.GetPositions;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterHistoryByPlayer;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Commands.CreateTeam;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Commands.UpdateTeam;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamById;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamGamesByLeagueId;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Controllers
{
    public class PlayerController : Controller
    {
        private readonly ILogger<LeagueController> _logger;
        private readonly ISender _sender;

        public PlayerController(ILogger<LeagueController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int playerId, CancellationToken cancellationToken = default)
        {
            var player = await _sender.Send(new GetPlayerQuery(playerId), cancellationToken);
            var countries = await _sender.Send(new GetCountriesQuery(), cancellationToken);

            if (player.HasNoValue || countries.HasNoValue)
            {
                return View("Error");
            }

            ViewBag.Title = "Player - Details";

            var rosterItemsValue  = await _sender.Send(new GetRosterHistoryByPlayerQuery(playerId), cancellationToken);

            if (rosterItemsValue.HasNoValue)
            {
                return View("Error");
            }


            List<RosterItemHistoryViewModel> list = new List<RosterItemHistoryViewModel>();

            foreach(var item in rosterItemsValue.Value)
            {
                list.Add(new RosterItemHistoryViewModel
                {
                    LeagueId = item.LeagueId,
                    LeagueName = item.LeagueName,
                    TeamName = item.TeamName,
                    Url = item.Url,
                    TeamId = item.TeamId
                });
            }

            return View(new DetailsPlayerViewModel
            {
                PlayerId = playerId,
                PlayerDetails = new PlayerViewModel
                {
                    Id = player.Value.Id,
                    Name = player.Value.Name,
                    Height = player.Value.Height,
                    DateOfBirth = player.Value.DateOfBirth,
                    Country = player.Value.CountryName,
                    Position = player.Value.Position.ToString(),
                    PositionColor = player.Value.Position.ConvertPositionEnumToColor()
                },
                Rosters = list
            });
        }

        public async Task<IActionResult> Upsert(int? playerId, CancellationToken cancellationToken = default)
        {
            var countries = await _sender.Send(new GetCountriesQuery(), cancellationToken);
            var positions = await _sender.Send(new GetPositionQuery(), cancellationToken);
            if (countries.HasNoValue || positions.HasNoValue)
            {
                return View("Error");
            }
            var viewModel = new UpsertPlayerViewModel()
            {
                Countries = new SelectList(countries.Value.Countries, "CountryId", "Name"),
                Positions = new SelectList(positions.Value, "Id", "Name"),
                DateOfBirth = DateTime.Now.Date.AddYears(-20)
            };

            if (playerId.HasValue)
            {
                var existingPlayer = await _sender.Send(new GetPlayerQuery(playerId.Value), cancellationToken);
                if (existingPlayer.HasNoValue)
                {
                    return View("Error", new InfoDescriptionViewModel()
                    {
                        Description = "Unable to find existing team"
                    });
                }

                viewModel.Name = existingPlayer.Value.Name;
                viewModel.DateOfBirth = existingPlayer.Value.DateOfBirth;
                viewModel.Height = existingPlayer.Value.Height;
                viewModel.Id = existingPlayer.Value.Id;
                viewModel.SelectedCountryId = existingPlayer.Value.CountryId.ToString();
                viewModel.SelectedPositionId = ((int)existingPlayer.Value.Position).ToString();
            }

            return View(viewModel);
        }

        public async Task<IActionResult> UpsertWithName(string playerName, int leagueId, CancellationToken cancellationToken = default)
        {
            var countries = await _sender.Send(new GetCountriesQuery(), cancellationToken);
            var positions = await _sender.Send(new GetPositionQuery(), cancellationToken);
            if (countries.HasNoValue || positions.HasNoValue)
            {
                return View("Error");
            }
            var viewModel = new UpsertPlayerViewModel()
            {
                Countries = new SelectList(countries.Value.Countries, "CountryId", "Name"),
                Positions = new SelectList(positions.Value, "Id", "Name"),
                DateOfBirth = DateTime.Now.Date.AddYears(-20)
            };

            viewModel.Name = playerName;
            viewModel.ComplexRouting = true;
            viewModel.LeagueId = leagueId;

            return View("Upsert",viewModel);
        }

        public async Task<IActionResult> AddAnotherName(int playerId,
            CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetPlayerAnotherNamesQuery(playerId), cancellationToken);

            if (result.HasValue)
            {
                var addAnotherNameViewModel = new AddAnotherNameViewModel
                {
                    PlayerId = playerId,
                    ExistingAnotherNames = result.Value.ExistingNames.Select(x => x.Name).ToList(),
                    Name = result.Value.OriginalName
                };
                return View(addAnotherNameViewModel);
            }
            return View("Error", new InfoDescriptionViewModel()
            {
                Description = "unable to find player"
            });

        }

        public async Task<IActionResult> SaveAnotherName(AddAnotherNameViewModel addAnotherNameViewModel, 
            CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new AddAnotherNameCommand(addAnotherNameViewModel.PlayerId, addAnotherNameViewModel.DraftAnotherName), cancellationToken);

            if(result.IsSuccess)
            {
                return View("Success", new InfoDescriptionViewModel
                {
                    Description  = "Successfully saved antoher name"
                });
            }
            return View("Error", new InfoDescriptionViewModel()
            {
                Description = result.Error.Message
            });

        }
        public async Task<IActionResult> Save(UpsertPlayerViewModel upsertPlayerViewModel, CancellationToken cancellationToken = default)
        {
            if (!int.TryParse(upsertPlayerViewModel.SelectedCountryId, out int countryId))
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "Unable to parse country id"
                });
            }

            if (!int.TryParse(upsertPlayerViewModel.SelectedPositionId, out int positionId))
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "Unable to parse position id"
                });
            }
            Result? result = null;
            if(upsertPlayerViewModel.Id == null)
            {
                result = await _sender.Send(new CreatePlayerCommand(upsertPlayerViewModel.Name, positionId, upsertPlayerViewModel.Height, upsertPlayerViewModel.DateOfBirth, countryId), cancellationToken);

                if (result.IsFailure)
                {
                    return View("Error", new InfoDescriptionViewModel()
                    {
                        Description = result.Error.Message
                    });
                }

                if (upsertPlayerViewModel.ComplexRouting)
                {
                    string redirectUrl = $"/Roster/Draft/{upsertPlayerViewModel.LeagueId}";
                    return Redirect(redirectUrl);
                }

                return View("Success", new InfoDescriptionViewModel()
                {
                    Description = "Successfully saved new player!"
                });
            }

            result = await _sender.Send(
               new UpdatePlayerCommand(upsertPlayerViewModel.Id ?? 0, upsertPlayerViewModel.Name, positionId, upsertPlayerViewModel.Height, upsertPlayerViewModel.DateOfBirth, countryId), cancellationToken);
            if (result.IsFailure)
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = result.Error.Message
                });
            }

            return View("Success", new InfoDescriptionViewModel()
            {
                Description = "Successfully saved player's update!"
            });
        }

        public async Task<IActionResult> Performance(int playerId, int leagueId, int teamId, CancellationToken cancellationToken = default)
        { 
            var result = await _sender
                .Send(new GetBoxscoreByPlayerIdTeamIdAndLeagueIdQuery(playerId, leagueId, teamId), cancellationToken);

            if (result.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "not found anything"
                });
            }

            var gamePerformanceItems = result.Value.Games.Select(x => new GamePerformanceViewModel
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
                Minutes = x.Minutes,
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
                ShotPrc = x.ShotPrc,
                ShotPrc1Pt = x.ShotPrc1Pt,
                ShotPrc2Pt = x.ShotPrc2Pt,
                ShotPrc3Pt = x.ShotPrc3Pt,
                Steals = x.Steals,
                TotalRebounds = x.TotalRebounds,
                Turnover = x.Turnover,
                Venue = x.Venue,
                Result = x.Result,
                MatchResultId = x.MatchResultId ?? 0
            });

            return View(new BoxscoreByPlayerViewModel
            {
                GamePerformance = gamePerformanceItems.ToList(),
                LeagueId = leagueId,
                LeagueName = result.Value.LeagueName,
                PlayerId = result.Value.PlayerId,
                PlayerName = result.Value.PlayerName,
                TeamId = result.Value.TeamId,
                TeamName = result.Value.TeamName,
                AverageBoxscoreStatsView = new AverageBoxscoreStatsViewModel
                {
                    AvgAssists = result.Value.AverageBoxscoreCalcuations.AvgAssists,
                    AvgMinutes = result.Value.AverageBoxscoreCalcuations.AvgMinutes,
                    AvgPlusMinus = result.Value.AverageBoxscoreCalcuations.AvgPlusMinus,
                    AvgPoints = result.Value.AverageBoxscoreCalcuations.AvgPoints,
                    AvgRankValue = result.Value.AverageBoxscoreCalcuations.AvgRankValue,
                    AvgSteals = result.Value.AverageBoxscoreCalcuations.AvgSteals,
                    AvgTotalRebounds = result.Value.AverageBoxscoreCalcuations.AvgTotalRebounds,
                    AvgTurnover = result.Value.AverageBoxscoreCalcuations.AvgTurnover,
                    
                },
                AdvancedBoxscoreStatsView = new AdvancedBoxscoreStatsViewModel
                {
                    AverageSpectators = result.Value.AdvancedMatchCalcuations.AverageSpectators,
                    AwayGameScoredPoints = result.Value.AdvancedMatchCalcuations.AwayGameScoredPoints,
                    GamePlayed = result.Value.AdvancedMatchCalcuations.GamePlayed,
                    GamesWin = result.Value.AdvancedMatchCalcuations.GamesWin,
                    HomeGameScoredPoints = result.Value.AdvancedMatchCalcuations.HomeGameScoredPoints,
                    HomeGamesPlayed = result.Value.AdvancedMatchCalcuations.HomeGamesPlayed,
                    HomeGamesWin = result.Value.AdvancedMatchCalcuations.HomeGamesWin,
                    TotalSpectators = result.Value.AdvancedMatchCalcuations.TotalSpectators

                }
            });
        }

        public async Task<IActionResult> Matches(int playerId,
                                                       int leagueId,
                                                       int teamId,
                                                       CancellationToken cancellationToken = default)
        {
            var gamesResult =
                await _sender.Send(new GetPlayerGamesQuery(leagueId, teamId, playerId), cancellationToken);
            var playerResult = await _sender.Send(new GetPlayerQuery(playerId), cancellationToken);
            if (gamesResult.HasNoValue || playerResult.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "not found any peformance" });
            }

            return View(new PlayerGameViewModel
            {
                PlayerId = playerResult.Value.Id,
                PlayerName = playerResult.Value.Name,
                LeagueId = gamesResult.Value.LeagueId,
                TeamId = gamesResult.Value.TeamId,
                LeagueName = gamesResult.Value.LeagueName,
                TeamName = gamesResult.Value.TeamName,
                TeamScoreItems = gamesResult.Value.GameStatsItems
                                                .Select(x => new TeamGameViewModel()
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
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1,
                                                int size = 10000,
                                                CancellationToken cancellationToken = default)
        {
            var players = await _sender.Send(new GetPlayersQuery(page, size), cancellationToken);

            if (players.HasNoValue)
            {
                return View("Error");
            }

            ViewBag.Title = "Players";

            List<PlayerViewModel> list = new List<PlayerViewModel>();

            foreach (var player in players.Value.Players)
            {
                list.Add(new PlayerViewModel()
                {
                    Id = player.Id,
                    Name = player.Name,
                    Height = player.Height,
                    DateOfBirth = player.DateOfBirth,
                    Country = player.CountryName,
                    Age = player.Age,
                });
            }

            return View(new PlayerIndexViewModel
            {
                Players = list.OrderBy(x => x.Name).ToList()
            });
        }

        [HttpGet]
        public async Task<IActionResult> SearchPlayer(int page = 1,
                                                int size = 10,
                                                string searchName = "",
                                                CancellationToken cancellationToken = default)
        {
            var players = await _sender.Send(new SearchPlayersQuery(page, size, searchName), cancellationToken);

            if (players.HasNoValue)
            {
                return View("Error");
            }

            ViewBag.Title = "Players";

            List<PlayerViewModel> list = new List<PlayerViewModel>();

            foreach (var player in players.Value.Players)
            {
                list.Add(new PlayerViewModel()
                {
                    Id = player.Id,
                    Name = player.Name,
                    Height = player.Height,
                    DateOfBirth = player.DateOfBirth,
                    Country = player.CountryName,
                    Age = player.Age,
                });
            }

            return View(new PlayerIndexViewModel
            {
                Players = list.OrderBy(x => x.Name).ToList()
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> SearchPlayerByCountry(int page = 1,
                                                  int size = 10000,
                                                  string selectedCountryId = "",
                                                  CancellationToken cancellationToken = default)
        {
            if(!int.TryParse(selectedCountryId, out var countryId))
            {
                return View("Error");
            }
            var players = await _sender.Send(new SearchPlayersByCountryIdQuery(page, size, countryId), cancellationToken);

            if (players.HasNoValue)
            {
                return View("Error");
            }

            ViewBag.Title = "Players";

            List<PlayerViewModel> list = new List<PlayerViewModel>();

            foreach (var player in players.Value.Players)
            {
                list.Add(new PlayerViewModel()
                {
                    Id = player.Id,
                    Name = player.Name,
                    Height = player.Height,
                    DateOfBirth = player.DateOfBirth,
                    Country = player.CountryName,
                    Age = player.Age,
                });
            }

            return View(new PlayerIndexViewModel
            {
                Players = list.OrderBy(x => x.Name).ToList()
            });
        }


        [HttpGet]
        public async Task<IActionResult> PlayersByCountry(string? selectedCountryId= null,
            CancellationToken cancellationToken = default)
        {
            var countries = await _sender.Send(new GetCountriesQuery(), cancellationToken);
            if (countries.HasNoValue)
            {
                return View("Error");
            }
            var viewModel = new PlayersByCountryViewModel
            {
                Countries = new SelectList(countries.Value.Countries, "CountryId", "Name"),
            };
            if (!int.TryParse(selectedCountryId, out int countryId))
            {
                return View(viewModel);
            }

            var players = await _sender.Send(new GetPlayersByCountryIdQuery(countryId), cancellationToken);
            if (players.HasNoValue)
            {
                return View("Error");
            }
            
            viewModel.SelectedCountryId = countryId.ToString();
            viewModel.PlayerItems = players.Value.Players.Select(x => new PlayerByCountryViewModel
            {
                PlayerId = x.Player.Id,
                PlayerName = x.Player.Name,
                Position = x.Player.Position.ToString(),
                PositionColor = x.Player.Position.ConvertPositionEnumToColor(),
                TeamId = x.TeamId,
                TeamName = x.TeamName,
                Minutes = x.Minutes,
                Points = (int?) x.Points,
                Assists = (int?) x.Assists,
                DefensiveRebounds = (int?) x.DefensiveRebounds,
                OffensiveRebounds = (int?) x.OffensiveRebounds,
                TotalRebounds = (int?) x.TotalRebounds,
                InFavoureOfBlock = (int?)x.InFavoureOfBlock,
                AgainstBlock = (int?) x.AgainstBlock,
                Steals = (int?) x.Steals,
                Turnover = (int?) x.Turnover,
                RankValue = (int?) x.RankValue,
                PlusMinus = (int?) x.PlusMinus,
                Age = x.Player.Age,
                Height = x.Player.Height,
                DatePerformance = x.PerformanceDate
            }).OrderByDescending(x => x.Points)
                .ThenBy(x => x.Age)
              .ToList();

            return View(viewModel);
        }


    }
}
