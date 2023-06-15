
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Persistence.Repositories
{
    public class PlayerRepository: GenericRepository<Player>,IPlayerRepository
    {
        public PlayerRepository(AbaLeagueDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> Exist(string name, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Players.AnyAsync(x => x.Name == name, cancellationToken);
        }

        public async Task<Player> Get(string name, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Players.SingleOrDefaultAsync(x => x.Name == name, cancellationToken);
        }
    }
}
