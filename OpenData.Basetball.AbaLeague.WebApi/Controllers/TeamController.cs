using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;

namespace OpenData.Basketball.AbaLeague.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly IWebPageProcessor _webPageProcessor;

        public TeamController(IWebPageProcessor webPageProcessor)
        {
            _webPageProcessor = webPageProcessor;
        }
        [HttpGet]
        public async Task<IActionResult> Get(string leagueUrl,CancellationToken cancellationToken)
        {
            var teams = await _webPageProcessor
                .GetTeams(leagueUrl,cancellationToken);

            return Ok(teams);
        }
    }
}
