using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("draft/{leagueId}/{teamId}")]
        public async Task<IActionResult> Get(int leagueId,int teamId, CancellationToken cancellationToken)
        {
            var teams = await _teamService.GetDraftRoster( teamId, leagueId, cancellationToken);

            return Ok(teams);
        }
    }
}
