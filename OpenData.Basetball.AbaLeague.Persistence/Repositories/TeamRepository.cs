using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basetball.AbaLeague.Persistence.Repositories;
using OpenData.Basketball.AbaLeague.Application.Contracts;

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {
        public TeamRepository(AbaLeagueDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Team> Get(IEnumerable<int> ids, CancellationToken cancellationToken = default)
        {
            return  _dbContext.Teams.Where(x => ids.Contains(x.Id));
        }

        public async Task<Team?> GetRoster(int teamId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Teams.Include(x=>x.RosterItems)
                    .ThenInclude(x=>x.Player)
                .FirstOrDefaultAsync(x => x.Id == teamId, cancellationToken);
        }
    }
}
