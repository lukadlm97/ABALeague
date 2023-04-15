using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Application.Services.Implementation;

namespace OpenData.Basketball.AbaLeague.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeagueController :  ControllerBase
    {
        private readonly ILeagueService _leagueService;

        public LeagueController(ILeagueService leagueService)
        {
            _leagueService = leagueService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var leagues = await _leagueService.Get(cancellationToken);

            return Ok(leagues);
        }
        [HttpGet("id")]
        public async Task<IActionResult> Get(int id,CancellationToken cancellationToken)
        {
            var leagues = await _leagueService.Get(id,cancellationToken);

            return Ok(leagues);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody]LeagueDto league, CancellationToken cancellationToken)
        {
            await _leagueService.Add(league, cancellationToken);

            return Ok(league);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _leagueService.Delete(id, cancellationToken);

            return NoContent();
        }
    }


}

