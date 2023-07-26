using Microsoft.AspNetCore.Mvc;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;

namespace OpenData.Basketball.AbaLeague.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RosterController : ControllerBase
    {
        private readonly IRosterService _teamService;

        public RosterController(IRosterService teamService)
        {
            _teamService = teamService;
        }
        [HttpGet("players/{leagueId}/{teamId}")]
        public async Task<IActionResult> GetPlayersByLeagueAndTeam(int leagueId,int teamId, CancellationToken cancellationToken)
        {
            var teams = await _teamService.GetDraftRoster( teamId, leagueId, cancellationToken);

            return Ok(teams);
        }

        [HttpGet("league/players/{leagueId}")]
        public async Task<IActionResult> GetLeagueRoster(int leagueId, CancellationToken cancellationToken)
        {
            var teams = await _teamService.GetWholeDraftRoster(leagueId, cancellationToken);

            return Ok(teams);
        }

        [HttpGet("draft/{leagueId}/{teamId}")]
        public async Task<IActionResult> Get(int leagueId, int teamId, CancellationToken cancellationToken)
        {
            var roster = await _teamService.Get(leagueId, teamId, cancellationToken);

            return Ok(roster);
        }

        [HttpGet("draft/{leagueId}")]
        public async Task<IActionResult> Get(int leagueId, CancellationToken cancellationToken)
        {
            var roster = await _teamService.GetWholeRosterItemDraftRoster(leagueId,  cancellationToken);

            return Ok(roster);
        }

        [HttpPost("roster/")]
        public async Task<IActionResult> Add(IEnumerable<AddRosterItemDto> entries, CancellationToken cancellationToken)
        {
            var roster = await _teamService.Add(entries, cancellationToken);

            return Ok(roster);
        }

        [HttpPost("roster/{teamId}")]
        public async Task<IActionResult> Add( int teamId, IEnumerable<DraftRosterEntry> entries,CancellationToken cancellationToken)
        {
            var roster = await _teamService.Add(teamId, entries, cancellationToken);

            return Ok(roster);
        }

    }
}
