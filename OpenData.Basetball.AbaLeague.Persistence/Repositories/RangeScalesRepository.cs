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
    public class RangeScalesRepository : IRangeScalesRepository
    {
        private readonly AbaLeagueDbContext _abaLeagueDbContext;

        public RangeScalesRepository(AbaLeagueDbContext abaLeagueDbContext)
        {
            _abaLeagueDbContext = abaLeagueDbContext;
        }
        public IQueryable<RangeScale> Get()
        {
            return _abaLeagueDbContext.RangeScales.AsQueryable();
        }
    }
}
