using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basketball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class StatsPropertyRepository : IStatsPropertyRepository
    {
        private readonly AbaLeagueDbContext _abaLeagueDbContext;

        public StatsPropertyRepository(AbaLeagueDbContext abaLeagueDbContext)
        {
            _abaLeagueDbContext = abaLeagueDbContext;
        }
        public IQueryable<StatsProperty> Get()
        {
           return _abaLeagueDbContext.StatsProperties.AsQueryable();
        }
    }
}
