using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basetball.AbaLeague.Persistence.Repositories;
using OpenData.Basketball.AbaLeague.Application.Contracts;

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class RosterRepository:GenericRepository<RosterItem>,IRosterRepository
    {
        public RosterRepository(AbaLeagueDbContext dbContext) : base(dbContext)
        {
        }
    }
}
