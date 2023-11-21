
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Application.Contracts
{
    public interface IPositionRepository
    {
        Task<IEnumerable<Position>> Get(CancellationToken cancellationToken=default);
        Task<Position?> Get(int id,CancellationToken  cancellationToken=default);
    }
}
