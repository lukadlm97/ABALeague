using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basketball.AbaLeague.Application.DTOs.Round;
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

        [HttpPost("/{leagueId}")]
        public async Task<IActionResult> Add(int leagueId,[FromBody] Calendar calendar, CancellationToken cancellationToken)
        {
            var games = await _leagueService.AddCalendar(leagueId,calendar.Entries,false, cancellationToken);

            return Ok(games);
        }

        public record Calendar(IEnumerable<AddRoundMatchDto> Entries);
    }
}
