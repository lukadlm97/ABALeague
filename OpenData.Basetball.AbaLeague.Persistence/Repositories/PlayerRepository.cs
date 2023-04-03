using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Persistence.Repositories
{
    public class PlayerRepository: GenericRepository<Player>,IPlayerRepository
    {
        public PlayerRepository(AbaLeagueDbContext dbContext) : base(dbContext)
        {
        }
    }
}
