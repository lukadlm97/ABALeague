
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Application.Contracts
{
    public interface IPositionRepository
    {
        Task<Position?> Get(int id,CancellationToken  cancellationToken=default);
    }
}
