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
    public class LevelsOfScaleRepository : ILevelsOfScaleRepository
    {
        private readonly AbaLeagueDbContext _abaLeagueDbContext;

        public LevelsOfScaleRepository(AbaLeagueDbContext abaLeagueDbContext)
        {
            _abaLeagueDbContext = abaLeagueDbContext;
        }
        public IQueryable<LevelOfScale> Get()
        {
            return _abaLeagueDbContext.LevelsOfScale.AsQueryable();
        }
    }
}
