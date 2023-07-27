using Microsoft.AspNetCore.Mvc;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;

namespace OpenData.Basketball.AbaLeague.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeasonController : ControllerBase
    {
        private readonly ISeasonResourcesService _seasonResourcesService;

        public SeasonController(ISeasonResourcesService seasonResourcesService)
        {
            _seasonResourcesService = seasonResourcesService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            var resources = await _seasonResourcesService.Get(cancellationToken);
            return Ok(resources);
        }

        [HttpGet("resources/draft/{leagueId}")]
        public async Task<IActionResult> GetDraft(int leagueId,CancellationToken cancellationToken = default)
        {
            var resources = await _seasonResourcesService.GetSeasonResourcesDraft(leagueId, cancellationToken);
            return Ok(resources);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]AddSeasonResourceDto seasonResourceDto,
            CancellationToken cancellationToken = default)
        {
            var resource = await _seasonResourcesService
                .Add(seasonResourceDto, cancellationToken);
            return Ok(resource);
        }

        [HttpPost("multiple")]
        public async Task<IActionResult> Add([FromBody] IEnumerable<AddSeasonResourceDto> resources,
            CancellationToken cancellationToken = default)
        {
            var resource = await _seasonResourcesService
                .Add(resources, cancellationToken);
            return Ok(resource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int resourceId,[FromQuery] string seasonResourceUrl,
            CancellationToken cancellationToken = default)
        {
            var resource = await _seasonResourcesService
                .UpdateUrl(resourceId,seasonResourceUrl, cancellationToken);
            return Ok(resource);
        }

        [HttpGet("{leagueId}")]
        public async Task<IActionResult> Get(int leagueId,CancellationToken cancellationToken = default)
        {
            var resources = await _seasonResourcesService.GetTeams(leagueId,cancellationToken);
            return Ok(resources);
        }



    }
}
