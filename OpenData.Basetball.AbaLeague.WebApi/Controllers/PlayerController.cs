using Microsoft.AspNetCore.Mvc;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;

namespace OpenData.Basetball.AbaLeague.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly IPlayerService _playerService;

        public PlayerController(ILogger<PlayerController> logger,IPlayerService playerService)
        {
            _logger = logger;
            _playerService = playerService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken=default)
        {
            return Ok(await _playerService.Get(cancellationToken));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id,CancellationToken cancellationToken = default)
        {
            var player = await _playerService.Get(id, cancellationToken);
            return player!=null? 
                Ok(player):
                NotFound();
        }

        [HttpPost()]
        public async Task<IActionResult> Add([FromBody] AddPlayerDto addPlayerDto, CancellationToken cancellationToken = default)
        {
            var player = await _playerService.Add(addPlayerDto, cancellationToken);
            return player != null ?
                Ok(player) :
                NotFound();
        }


        [HttpPost("multiple")]
        public async Task<IActionResult> AddCollection([FromBody] AddPlayersRequest request, CancellationToken cancellationToken = default)
        {
            var players = await _playerService.Add(request.PlayersDtoList, cancellationToken);
            return players != null ?
                Ok(players) :
                NotFound();
        }

        public record AddPlayersRequest(IEnumerable<AddPlayerDto> PlayersDtoList);

        [HttpGet("logging")]
        public async Task<IActionResult> Test(CancellationToken cancellationToken = default)
        {
            _logger.LogCritical("LogCritical");
            _logger.LogInformation("LogInformation");
            _logger.LogError("LogError");
            _logger.LogCritical("LogCritical"); 
            _logger.LogDebug("LogDebug");
            return Ok();
        }

    }
}
