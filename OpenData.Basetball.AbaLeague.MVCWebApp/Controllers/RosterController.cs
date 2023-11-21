using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basetball.AbaLeague.MVCWebApp.Utilities;
using OpenData.Basketball.AbaLeague.Application.Features.Countries.Queries.GetCountries;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayers;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterByTeamId;
using OpenData.Basketball.AbaLeague.Application.Features.SeasonResources.Queries.GetSeasonResourcesByTeamAndLeague;


namespace OpenData.Basetball.AbaLeague.MVCWebApp.Controllers
{
    public class RosterController : Controller
    {
        private readonly ILogger<LeagueController> _logger;
        private readonly ISender _sender;

        public RosterController(ILogger<LeagueController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        public async Task<IActionResult> History(int? teamId, int? leagueId, CancellationToken cancellationToken = default)
        {
            if (!teamId.HasValue || !leagueId.HasValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "missing team id" });
            }

            var players = await _sender.Send(new GetPlayersQuery(), cancellationToken);
            var countries = await _sender.Send(new GetCountriesQuery(), cancellationToken);
            var rosterItems = await _sender.Send(new GetRosterByTeamIdQuery(teamId ?? 0, leagueId ?? 0), cancellationToken);

            if(players.HasNoValue ||  rosterItems.HasNoValue || countries.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "cant load team details" });
            }
            List<PlayerViewModel> list = new List<PlayerViewModel>();
            foreach(var rosterItem in rosterItems.Value.Items)
            {
                var player = players.Value.Players.FirstOrDefault(x => x.Id == rosterItem.PlayerId);
                if (player != null)
                {
                    list.Add(new PlayerViewModel()
                    {
                        Name = player.Name,
                        DateOfBirth = player.DateOfBirth,
                        Id = player.Id,
                        Height = player.Height,
                        Country = player.CountryId.ResolveCountryName(countries.Value.Countries)
                    });
                }
            }
            var resources= await _sender.Send(new GetSeasonResourcesByTeamAndLeagueQuery(teamId??0, leagueId??0), cancellationToken);

            if (resources.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "cant load team details" });
            }

            var viewModel = new RosterHistoryViewModel()
            {
                LeagueName = resources.Value.LeagueName,
                TeamName = resources.Value.TeamName,
                RosterItems = list,
                TeamId = teamId ?? 0,
                LeagueId = leagueId ?? 0,
            }
            ;

            return View("History", viewModel);
        }
    }
}
