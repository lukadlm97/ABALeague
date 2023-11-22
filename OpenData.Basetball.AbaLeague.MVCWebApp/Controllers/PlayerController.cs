using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basetball.AbaLeague.MVCWebApp.Utilities;
using OpenData.Basketball.AbaLeague.Application.Features.Countries.Queries.GetCountries;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Commands.CreatePlayer;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Commands.UpdatePlayer;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayer;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayers;
using OpenData.Basketball.AbaLeague.Application.Features.Positions.Queries.GetPositions;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterHistoryByPlayer;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Commands.CreateTeam;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Commands.UpdateTeam;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamById;
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
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var players = await _sender.Send(new GetPlayersQuery(), cancellationToken);
            var countries = await _sender.Send(new GetCountriesQuery(), cancellationToken);

            if (players.HasNoValue || countries.HasNoValue)
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
                    Country = player.CountryId.ResolveCountryName(countries.Value.Countries)
                });
            }

            return View(new IndexPlayerViewModel
            {
                Players = list
            });
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
                    Country = player.Value.CountryId.ResolveCountryName(countries.Value.Countries),
                    Position = player.Value.Position.ToString(),
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

    }
}
