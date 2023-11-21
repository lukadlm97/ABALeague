
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Persistence;

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly AbaLeagueDbContext _abaLeagueDbContext;

        public PositionRepository(AbaLeagueDbContext abaLeagueDbContext)
        {
            _abaLeagueDbContext = abaLeagueDbContext;
        }
        public async Task<Position?> Get(int id, CancellationToken cancellationToken = default)
        {
            return await _abaLeagueDbContext.Positions.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Position>> Get(CancellationToken cancellationToken = default)
        {
            return await _abaLeagueDbContext.Positions.ToListAsync(cancellationToken);
        }
    }
}
