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

        [HttpPost("/byRound/{leagueId}")]
        public async Task<IActionResult> Add(int leagueId, [FromBody] BoxScoreRequest request, CancellationToken cancellationToken)
        {
            var games = await _boxScoreService.AddScore(leagueId, request.Entries, cancellationToken);

            return Ok(new
                {
                    Stats = games
                }
            );
        }
        [HttpGet("euroleague/{leagueId}/draft/byMatch/{matchId}")]
        public async Task<IActionResult> GetEuroleagueMatch(int leagueId, int matchId, CancellationToken cancellationToken)
        {
            var games = await _boxScoreService.GetEuroleagueMatchBoxScore(leagueId, matchId, cancellationToken);
            return Ok(new
            {
                existing = games.playersScore,
                missing = games.missingPlayers
            });
        }
        [HttpGet("euroleague/{leagueId}/draft/byRound/{roundId}")]
        public async Task<IActionResult> GetEuroleagueRound(int leagueId, int roundId, CancellationToken cancellationToken)
        {
            var games = await _boxScoreService.GetEuroleagueRoundBoxScore(leagueId, roundId, cancellationToken);

            return Ok(new
            {
                existing = games.playersScore,
                missing = games.missingPlayers
            });
        }
        [HttpGet("euroleague/{leagueId}/draft/byRound/{roundId}/step/{step}")]
        public async Task<IActionResult> GetEuroleagueRound(int leagueId, int roundId, int step, CancellationToken cancellationToken)
        {
            List<BoxScoreItemDto> boxscores = new List<BoxScoreItemDto>();
            List<string> missing = new List<string>();
            for (int i = roundId; i <= roundId+step; i++)
            {
                var (result, missingPlayers) = 
                    await _boxScoreService.GetEuroleagueRoundBoxScore(leagueId, i, cancellationToken);
                boxscores.AddRange(result);
                missing.AddRange(missingPlayers);
            }
            return Ok(new
            {
                existing = boxscores.ToList(),
                missing = missing.ToList()
            });
        }
        public record BoxScoreRequest(IEnumerable<AddBoxScoreDto> Entries);
    }
}
