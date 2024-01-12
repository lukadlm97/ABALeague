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
    public class CompetitionOrganizationRepository : ICompetitionOrganizationRepository
    {
        private readonly AbaLeagueDbContext _dbContext;

        public CompetitionOrganizationRepository(AbaLeagueDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<CompetitionOrganization> Get()
        {
            return _dbContext.CompetitionOrganizations.AsQueryable();
        }
    }
}
