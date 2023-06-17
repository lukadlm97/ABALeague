using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;

namespace OpenData.Basketball.AbaLeague.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly ILeagueService _leagueService;

        public CalendarController(ILeagueService leagueService)
        {
            _leagueService = leagueService;
        }
        [HttpGet("draft/{leagueId}")]
        public async Task<IActionResult> Get(int leagueId,CancellationToken cancellationToken)
        {
            var games = await _leagueService.GetCalendarDraft(leagueId, cancellationToken);

            return Ok(games);
        }
    }
}
