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
    public class LeagueGameLengthRepository : ILeagueGameLengthRepository
    {
        private readonly AbaLeagueDbContext _abaLeagueDbContext;

        public LeagueGameLengthRepository(AbaLeagueDbContext abaLeagueDbContext)
        {
            _abaLeagueDbContext = abaLeagueDbContext;
        }
        public IQueryable<LeagueGameLength> Get()
        {
            return _abaLeagueDbContext.LeagueGameLengths.AsQueryable();
        }
    }
}
