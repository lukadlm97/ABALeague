
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface IResultRepository:IGenericRepository<Result>
    {
        Task<bool> Exist(int matchRoundId, CancellationToken cancellationToken = default);
    }
}
