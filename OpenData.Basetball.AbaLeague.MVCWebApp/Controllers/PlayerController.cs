using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basetball.AbaLeague.MVCWebApp.Utilities;
using OpenData.Basketball.AbaLeague.Application.Features.Countries.Queries.GetCountries;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayer;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayers;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterHistoryByPlayer;

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


    }
}
