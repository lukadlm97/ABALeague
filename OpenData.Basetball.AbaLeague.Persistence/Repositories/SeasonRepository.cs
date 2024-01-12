using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basetball.AbaLeague.Persistence.Repositories;
using OpenData.Basketball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class SeasonRepository : ISeasonRepository
    {
        private readonly AbaLeagueDbContext _dbContext;

        public SeasonRepository(AbaLeagueDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<Season> Get()
        {
            return _dbContext.Seasons.AsQueryable();
        }
    }
}
