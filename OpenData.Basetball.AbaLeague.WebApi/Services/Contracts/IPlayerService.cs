using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.WebApi.Services.Contracts
{
    public interface IPlayerService
    {
        Task<IEnumerable<Player>> GetPlayers(CancellationToken cancellationToken);
        Task<Player?> GetPlayer(int id,CancellationToken cancellationToken);
    }
}
