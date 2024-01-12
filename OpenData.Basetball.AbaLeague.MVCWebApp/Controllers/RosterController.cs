using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basetball.AbaLeague.MVCWebApp.Utilities;
using OpenData.Basetball.AbaLeague.Persistence.Migrations;
using OpenData.Basketball.AbaLeague.Application.Features.Countries.Queries.GetCountries;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Commands.CreatePlayers;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayers;
using OpenData.Basketball.AbaLeague.Application.Features.Positions.Queries.GetPositions;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Commands.AddRosterItems;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterByTeamId;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterDraftByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.SeasonResources.Queries.GetSeasonResourcesByTeamAndLeague;


namespace OpenData.Basetball.AbaLeague.MVCWebApp.Controllers
{
    [Route("[controller]/[action]")]
    public class RosterController : Controller
    {
        private readonly ILogger<LeagueController> _logger;
        private readonly ISender _sender;

        public RosterController(ILogger<LeagueController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }
        public async Task<IActionResult> History(int? teamId, 
                                                    int? leagueId,
                                                    CancellationToken cancellationToken = default)
        {
            if (!teamId.HasValue || !leagueId.HasValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "missing team id" });
            }

            var rosterItems = await _sender.Send(new GetRosterByTeamIdQuery(teamId ?? 0, leagueId ?? 0), cancellationToken);

            if(rosterItems.HasNoValue)
            {
                return View("Error", new InfoDescriptionViewModel() { Description = "cant load team details" });
            }
            List<PlayerViewModel> list = new List<PlayerViewModel>();
            foreach(var rosterItem in rosterItems.Value.Items)
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
            var resources= await _sender
                .Send(new GetSeasonResourcesByTeamAndLeagueQuery(teamId ?? 0, leagueId ?? 0), cancellationToken);

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
            };

            return View("History", viewModel);
        }
        [HttpGet("{leagueId:int}")]
        public async Task<IActionResult> Draft(int leagueId, 
                                                CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetRosterDraftByLeagueIdQuery(leagueId), cancellationToken);

            if(!result.HasValue)
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "Unable to find roster items"
                });
            }

            var countries = await _sender.Send(new GetCountriesQuery(), cancellationToken);
            if (countries.HasNoValue)
            {
                return View("Error");
            }


            var draftViewModel = new DraftRosterViewModel()
            {
                LeagueId = leagueId,
                MissingPlayerItems = result.Value.MissingPlayers.Select(x=> new PlayerViewModel
                {
                    DateOfBirth = x.DateOfBirth,
                    Height = x.Height,
                    Name = x.Name,
                    Position = (x.Position).ToString(),
                    Country = x.NationalityId?.ResolveCountryName(countries.Value.Countries)
                }).ToList(),
                DraftRosterItems = result.Value.DraftRosterItems.Select(x=>new DraftRosterItemViewModel
                {
                    LeagueId = x.LeagueId,
                    LeagueName= x.LeagueName,
                    PlayerId = x.PlayerId,
                    PlayerName = x.PlayerName,
                    TeamId= x.TeamId,
                    TeamName = x.TeamName
                }).ToList(),
                ExistingRosterItems = result.Value.ExistingRosterItems.Select(x => new DraftRosterItemViewModel
                {
                    LeagueId = x.LeagueId,
                    LeagueName = x.LeagueName,
                    PlayerId = x.PlayerId,
                    PlayerName = x.PlayerName,
                    TeamId = x.TeamId,
                    TeamName = x.TeamName
                }).ToList(),
            };

            return View(draftViewModel);
        }


        [HttpPost("")]
        public async Task<IActionResult> SaveDraftRosterItems(int leagueId, 
                                                                CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetRosterDraftByLeagueIdQuery(leagueId), cancellationToken);
            if (!result.HasValue)
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "Unable to find roster items"
                });
            }
            if(result.HasValue)
            {
                if (result.Value.DraftRosterItems.Any())
                {
                    var addRosterResult = await _sender.Send(
                        new AddRosterItemsCommand(leagueId, 
                        result.Value.DraftRosterItems.Select(x => 
                        new Basketball.AbaLeague.Application.DTOs.Roster.AddRosterItemDto(x.PlayerId, 
                                                                                            x.LeagueId, 
                                                                                            x.TeamId, 
                                                                                            DateTime.UtcNow,
                                                                                            null))), cancellationToken);

                    if(addRosterResult.IsSuccess)
                    {
                        string redirectUrl = $"/Roster/Draft/{leagueId}";
                        return Redirect(redirectUrl);
                    }
                    return View("Error", new InfoDescriptionViewModel()
                    {
                        Description = addRosterResult.Error.Message
                    });
                }
            }
            return View("Error", new InfoDescriptionViewModel()
            {
                Description = "Unable to get collection"
            });

        }

        [HttpPost("")]
        public async Task<IActionResult> SaveDraftRosterItem([FromQuery] int leagueId,
                                                                [FromQuery] int teamId, 
                                                                [FromQuery] int playerId,
                                                                CancellationToken cancellationToken = default)
        {
            var addRosterResult = await _sender
                .Send(new AddRosterItemsCommand(leagueId, 
                    new List<Basketball.AbaLeague.Application.DTOs.Roster.AddRosterItemDto>
                    {
                        new Basketball.AbaLeague.Application.DTOs.Roster.AddRosterItemDto(playerId, 
                                                                                            leagueId,
                                                                                            teamId,
                                                                                            DateTime.UtcNow, null)
                    }), cancellationToken);

            if (addRosterResult.IsSuccess)
            {
                string redirectUrl = $"/Roster/Draft/{leagueId}";
                return Redirect(redirectUrl);
            }

            return View("Error", new InfoDescriptionViewModel()
            {
                Description = addRosterResult.Error.Message
            });

        }

        public async Task<IActionResult> SaveHistoryDraftRosterItem([FromQuery] int leagueId,
                                                                    [FromQuery] int teamId, 
                                                                    [FromQuery] int playerId,
                                                                    CancellationToken cancellationToken = default)
        {
            var addRosterResult = await _sender.Send(new AddRosterItemsCommand(leagueId,
                    new List<Basketball.AbaLeague.Application.DTOs.Roster.AddRosterItemDto>
                    {
                        new Basketball.AbaLeague.Application.DTOs.Roster.AddRosterItemDto(playerId,
                                                                                            leagueId,
                                                                                            teamId,
                                                                                            DateTime.UtcNow,
                                                                                            DateTime.UtcNow)
                    }), cancellationToken);

            if (addRosterResult.IsSuccess)
            {
                string redirectUrl = $"/Roster/Draft/{leagueId}";
                return Redirect(redirectUrl);
            }

            return View("Error", new InfoDescriptionViewModel()
            {
                Description = addRosterResult.Error.Message
            });

        }

        [HttpPost("")]
        public async Task<IActionResult> AddMissingPlayers(int leagueId,
                                                            CancellationToken cancellationToken = default)
        {
            var result = await _sender
                                    .Send(new GetRosterDraftByLeagueIdQuery(leagueId), cancellationToken);
            if (!result.HasValue)
            {
                return View("Error", new InfoDescriptionViewModel()
                {
                    Description = "Unable to find roster items"
                });
            }
            if (result.HasValue)
            {
                if (result.Value.MissingPlayers.Any())
                {
                    var addPlayersResult = await _sender
                        .Send(new CreatePlayersCommand(result.Value.MissingPlayers.Select(x => new Basketball.AbaLeague.Application.DTOs.Player.AddPlayerDto(x.Name,
                                                                                    x.Position,
                                                                                    x.Height,
                                                                                    x.DateOfBirth,
                                                                                    x.NationalityId))), cancellationToken);

                    if (addPlayersResult.IsSuccess)
                    {
                        string redirectUrl = $"/Roster/Draft/{leagueId}";
                        return Redirect(redirectUrl);
                    }
                    return View("Error", new InfoDescriptionViewModel()
                    {
                        Description = addPlayersResult.Error.Message
                    });
                }
            }
            return View("Error", new InfoDescriptionViewModel()
            {
                Description = "Unable to get collection"
            });

        }
    }
}
