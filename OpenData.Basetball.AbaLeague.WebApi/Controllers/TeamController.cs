using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Application.Services.Implementation;

namespace OpenData.Basketball.AbaLeague.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }
        [HttpGet("{leagueId}")]
        public async Task<IActionResult> Get(int leagueId,CancellationToken cancellationToken)
        {
            var teams = await _teamService
                .Get(leagueId, cancellationToken);

            return Ok(new {Existing=teams.existingResulution.Select(x=>x.Item1.Name+"="+x.Item2.Name),New=teams.newly});
        }
    }
}
