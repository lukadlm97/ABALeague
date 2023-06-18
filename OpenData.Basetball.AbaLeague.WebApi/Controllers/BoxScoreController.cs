using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;

namespace OpenData.Basketball.AbaLeague.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoxScoreController : ControllerBase
    {
        private readonly IBoxScoreService _boxScoreService;

        public BoxScoreController(IBoxScoreService boxScoreService)
        {
            _boxScoreService = boxScoreService;
        }
        [HttpGet("draft/{leagueId}/{matchId}")]
        public async Task<IActionResult> Get(int leagueId,int matchId, CancellationToken cancellationToken)
        {
            var games = await _boxScoreService.GetScore(leagueId, matchId, cancellationToken);

            return Ok( new
                {
                    HomeTeamStats= games.homePlayers,
                    AwayTeamStats= games.awayPlayers,
                    Missing= games.missingPlayers,
                }
            );
        }

        [HttpGet("draft/byRound/{leagueId}/{roundNo}")]
        public async Task<IActionResult> GetByRound(int leagueId, int roundNo, CancellationToken cancellationToken)
        {
            var games = await _boxScoreService.GetRoundBoxScore(leagueId, roundNo, cancellationToken);

            return Ok(new
                {
                    Stats = games.playersScore,
                    Missing = games.missingPlayers,
                }
            );
        }

        [HttpPost("/byRound/{leagueId}/{roundNo}")]
        public async Task<IActionResult> Add(int leagueId, int roundNo, [FromBody] BoxScoreRequest request, CancellationToken cancellationToken)
        {
            var games = await _boxScoreService.AddScore(leagueId, roundNo,request.Entries, cancellationToken);

            return Ok(new
                {
                    Stats = games
                }
            );
        }

        public record BoxScoreRequest(IEnumerable<AddBoxScoreDto> Entries);
    }
}
