
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface IResultRepository : IGenericRepository<Result>
    {
        Task<bool> Exist(int matchRoundId, CancellationToken cancellationToken = default);
        Task<Result?> GetByMatchRound(int matchRoundId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Result>> SearchByLeague(int leagueId, CancellationToken cancellationToken = default);
        IQueryable<Result> SearchByLeagueAndRoundNo(int leagueId, 
                                                            int roundNo, 
                                                            CancellationToken cancellationToken = default);

        Task<Result?> GetByMatchResult(int matchResultId, CancellationToken cancellationToken = default);
        IQueryable<Result> Get();
    }
}
