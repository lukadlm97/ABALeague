using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Application.Contracts
{
    public interface IPlayerRepository:IGenericRepository<Player>
    {
        Task<bool> Exist(string name, CancellationToken cancellationToken = default);
        Task<Player> Get(string name, CancellationToken cancellationToken = default);
        Task<bool> AddAnotherName(int playerId, string name, CancellationToken cancellationToken = default);
        Task<bool> ExistAnotherName(int playerId, string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<AnotherNameItem>> GetAnotherNames(int playerId, CancellationToken cancellationToken= default);
        Task<AnotherNameItem?> GetAnotherNamePlayerByAnotherName(string name, CancellationToken cancellationToken= default);
        Task<Player?> GetPlayerByAnotherName(string name, CancellationToken cancellationToken = default);
    }
}
