using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Application.Contracts
{
    public interface IPlayerRepository:IGenericRepository<Player>
    {
        Task<bool> Exist(string name, CancellationToken cancellationToken = default);
        Task<Player> Get(string name, CancellationToken cancellationToken = default);
    }
}
