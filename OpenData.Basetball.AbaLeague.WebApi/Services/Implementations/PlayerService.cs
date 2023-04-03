using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.WebApi.Repository;
using OpenData.Basetball.AbaLeague.WebApi.Services.Contracts;

namespace OpenData.Basetball.AbaLeague.WebApi.Services.Implementations
{
    public record PlayerService(DataContext DataContext):IPlayerService
    {
        public async Task<IEnumerable<Player>> GetPlayers(CancellationToken cancellationToken)
        {
            await Task.Delay(100,cancellationToken);
            return DataContext.Players;
        }

        public async Task<Player?> GetPlayer(int id, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);
            return null;
        }
    }
}
