using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basetball.AbaLeague.Persistence.Repositories;
using OpenData.Basketball.AbaLeague.Application.Contracts;

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class TeamRepository: GenericRepository<Team>,ITeamRepository
    {
        public TeamRepository(AbaLeagueDbContext dbContext) : base(dbContext)
        {
        }
    }
}
