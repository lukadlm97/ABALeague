using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basetball.AbaLeague.Persistence.Migrations;
using OpenData.Basketball.AbaLeague.Application.DTOs.Country;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Application.Features.Countries.Queries.GetCountries;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayers;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetExistingRostersByTeam;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterByTeamId;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Commands.CreateTeam;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Commands.UpdateTeam;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamById;
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
                new UpdateTeamCommand(upsertTeamViewModel.Id??0, upsertTeamViewModel.Name, upsertTeamViewModel.ShortCode,
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

        public async Task<IActionResult> Index(string filter = null, int pageNumber = 1, int pageSize = 50, CancellationToken cancellationToken = default)
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
                Teams = teams.Value.Teams.Select(x=> new MVCWebApp.Models.TeamDto()
                {
                    Name = x.Name,
                    ShortName = x.ShortName,
                    Id = x.Id??0,
                    Country = ReturnCountryName(countries.Value.Countries, x.CountryId??0)
                }).ToList(),
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
                    Id = x.Id ?? 0,
                    Country = ReturnCountryName(countries.Value.Countries, x.CountryId ?? 0)
                }).Where(x=>x.Name.ToLower().Contains(model.Filter.ToLower())).ToList(),
                Filter = model.Filter,
                Number = 1,
                Size = 50
            };
            return View("Index",indexViewModel);
        }

        string ReturnCountryName(IEnumerable<CountryDto> countries, int id)
        {
            return countries?.FirstOrDefault(x => x.CountryId == id)?.Name;
        }
        string ReturnPlayerName(IEnumerable<PlayerDTO> players, int id)
        {
            return players?.FirstOrDefault(x => x.Id == id)?.Name;
        }
        string ReturnPlayerCountryName(IEnumerable<PlayerDTO> players, IEnumerable<CountryDto> countries, int id)
        {
            return countries.FirstOrDefault(x=>players?.FirstOrDefault(x => x.Id == id)?.CountryId==x.CountryId)?.Name;
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
            var team   = await _sender.Send(new GetTeamByIdQuery(teamId ?? 0), cancellationToken);

            if (team.HasNoValue)
            {

                return View("Error", new InfoDescriptionViewModel() { Description = "cant load team details" });
            }

            var leagues = await _sender.Send(new GetExistingRostersByTeamQuery(team.Value.Id ?? 0), cancellationToken);
            if (leagues.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "cant load team details" });
            }

            TeamDetailsViewModel? detailsViewModel = null;

            if (!leagues.Value.LeagueIds.Any())
            {
                 detailsViewModel = new TeamDetailsViewModel()
                {
                    Id = team.Value.Id ?? 0,
                    ShortName = team.Value.ShortName,
                    Country = ReturnCountryName(countries.Value.Countries, team.Value.CountryId ?? 0),
                    Name = team.Value.Name,
                    RosterItems = new List<PlayerViewModel>()
                };
                return View(detailsViewModel);
            }
            var rosterItems =
                await _sender.Send(new GetRosterByTeamIdQuery(team.Value.Id ?? 0, leagues.Value.LeagueIds.Max()), cancellationToken);

            if (rosterItems.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "cant load team details" });
            }

            var players = await _sender.Send(new GetPlayersQuery(), cancellationToken);
            if (players.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "cant load team details" });
            }

            List<PlayerViewModel> list = new List<PlayerViewModel>();

            foreach (var rosterItem in rosterItems.Value.Items)
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
                        Country = ReturnCountryName(countries.Value.Countries, player.CountryId)
                    });
                }
            }

            detailsViewModel = new TeamDetailsViewModel()
            {
                Id = team.Value.Id ?? 0,
                ShortName = team.Value.ShortName,
                Country = ReturnCountryName(countries.Value.Countries, team.Value.CountryId ?? 0),
                Name = team.Value.Name,
                RosterItems = list
            };
            
            return View(detailsViewModel);
        }
    }
}
