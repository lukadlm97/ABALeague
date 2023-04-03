using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.WebApi.Services.Contracts;
using System.Threading;

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
            return Ok(await _playerService.GetPlayers(cancellationToken));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id,CancellationToken cancellationToken = default)
        {
            var player = await _playerService.GetPlayer(id, cancellationToken);
            return player!=null? 
                Ok(player):
                NotFound();
        }
    }
}
