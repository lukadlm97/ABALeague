using Microsoft.EntityFrameworkCore;
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
    public class GameLengthRepository : IGameLengthRepository
    {
        private readonly AbaLeagueDbContext _dbContext;

        public GameLengthRepository(AbaLeagueDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<GameLength> Get()
        {
            return _dbContext.GameLengths.AsQueryable();
        }
    }
}
