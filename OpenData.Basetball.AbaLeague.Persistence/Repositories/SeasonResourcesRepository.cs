using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basetball.AbaLeague.Persistence.Repositories;
using OpenData.Basketball.AbaLeague.Application.Contracts;

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class SeasonResourcesRepository:GenericRepository<SeasonResources>,ISeasonResourcesRepository
    {
        public SeasonResourcesRepository(AbaLeagueDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<SeasonResources>> SearchByLeague(int leagueId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.SeasonResources
                .Where(x => x.LeagueId == leagueId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<SeasonResources>> SearchByTeam(int teamId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.SeasonResources
                .Where(x => x.TeamId == teamId)
                .ToListAsync(cancellationToken);
        }
    }
}
