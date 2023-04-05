using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.WebApi.Services.Contracts;

namespace OpenData.Basetball.AbaLeague.WebApi.Services.Implementations
{
    public record PlayerService(IUnitOfWork UnitOfWork):IPlayerService
    {
        public async Task<IEnumerable<Player>> GetPlayers(CancellationToken cancellationToken)
        {
            await Task.Delay(100,cancellationToken);
            return await UnitOfWork.PlayerRepository.GetAll();
        }

        public async Task<Player?> GetPlayer(int id, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);
            return null;
        }
    }
}
