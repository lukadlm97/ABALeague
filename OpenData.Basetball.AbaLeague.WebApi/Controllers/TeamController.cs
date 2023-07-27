using Microsoft.AspNetCore.Mvc;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;

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
        [HttpGet("draft/aba/{leagueId}")]
        public async Task<IActionResult> GetAba(int leagueId,CancellationToken cancellationToken)
        {
            var teams = await _teamService
                .GetAba(leagueId, cancellationToken);

            return Ok(new {Existing=teams.existingResulution.Select(x=>x.Item1.Name+"("+x.Item1.Url+")"+"="+x.Item2.Name + "(" + x.Item2.Url + ")"),New=teams.newly.Select(x=>new 
            {
                Name=x.Name,
                Url=x.Url
            })});
        }
        [HttpGet("draft/euro/{leagueId}")]
        public async Task<IActionResult> GetEuro(int leagueId, CancellationToken cancellationToken)
        {
            var teams = await _teamService
                .GetEuro(leagueId, cancellationToken);

            return Ok(new
            {
                Existing = teams.existingResulution.Select(x => x.Item1.Name + "(" + x.Item1.Url + ")" + "=" + x.Item2.Name + "(" + x.Item2.Url + ")"),
                New = teams.newly.Select(x => new
                {
                    Name = x.Name,
                    Url = x.Url
                })
            });
        }

        [HttpGet("existing")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var teams = await _teamService
                .GetExisting(cancellationToken);

            return Ok(teams);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TeamDto teamDto, CancellationToken cancellationToken = default)
        {
           var team =  await _teamService
               .Add(teamDto, cancellationToken);

           return Ok(team);
        }

        [HttpPost("multiple")]
        public async Task<IActionResult> Add([FromBody] IEnumerable<TeamDto> teams,
            CancellationToken cancellationToken = default)
        {
            var createdTeams = await _teamService
                .Add(teams, cancellationToken);

            return Ok(createdTeams);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody] TeamDto teamDto, CancellationToken cancellationToken = default)
        {
            var team = await _teamService
                .Update(id,teamDto, cancellationToken);

            return Ok(team);
        }


    }
}
