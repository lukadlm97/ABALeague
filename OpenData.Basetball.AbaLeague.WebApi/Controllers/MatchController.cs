using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basketball.AbaLeague.Application.DTOs;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;

namespace OpenData.Basketball.AbaLeague.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IResultService _resultService;

        public MatchController(IResultService resultService)
        {
            _resultService = resultService;
        }
        
        [HttpGet("{leagueId}/{roundId}")]
        public async Task<IActionResult> Get(int leagueId, int roundId, CancellationToken cancellationToken)
        {
            var results = await _resultService.GetResultsByRoundId(leagueId, roundId, cancellationToken);
            return Ok(results);
        }

        [HttpGet("{leagueId}")]
        public async Task<IActionResult> Get(int leagueId, CancellationToken cancellationToken)
        {
            var results = await _resultService.GetResults(leagueId,cancellationToken);
            return Ok(results);
        }
        [HttpGet("euroleague/{leagueId}/{matchNo}")]
        public async Task<IActionResult> GetEuroleagueResults(int leagueId, int matchNo, CancellationToken cancellationToken)
        {
            var results = await _resultService.GetEuroleagueResults(leagueId, matchNo, cancellationToken);
            return Ok(results);
        }

        [HttpGet("euroleague/{leagueId}")]
        public async Task<IActionResult> GetEuroleagueResults(int leagueId, CancellationToken cancellationToken)
        {
            var results = await _resultService.GetEuroleagueResults(leagueId, cancellationToken);
            return Ok(results);
        }

        [HttpPost("{leagueId}")]
        public async Task<IActionResult> Add(int leagueId,[FromBody] ResultsRequest request, CancellationToken cancellationToken)
        {
            var results = await _resultService.Add(leagueId,request.Results,cancellationToken);
            return Ok(results);
        }

        public record ResultsRequest(IEnumerable<AddResultDto> Results);
    }
}
