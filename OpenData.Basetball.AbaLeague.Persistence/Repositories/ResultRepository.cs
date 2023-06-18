using OpenData.Basketball.AbaLeague.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basetball.AbaLeague.Persistence.Repositories;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class ResultRepository: GenericRepository<Result>,IResultRepository
    {
        public ResultRepository(AbaLeagueDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> Exist(int matchRoundId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Results.AnyAsync(x => x.RoundMatchId == matchRoundId, cancellationToken);
        }
    }
}
