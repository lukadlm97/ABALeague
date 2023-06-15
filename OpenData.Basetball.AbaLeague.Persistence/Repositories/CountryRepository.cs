using OpenData.Basketball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Persistence;
using Microsoft.EntityFrameworkCore;

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class CountryRepository: ICountryRepository
    {
        private readonly AbaLeagueDbContext _dbContext;

        public CountryRepository(AbaLeagueDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Country?> GetById(string nationalitySo3, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Countries.SingleOrDefaultAsync(x => x.CodeIso == nationalitySo3, cancellationToken);
        }

        public async Task<Country?> GetById(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Countries.SingleOrDefaultAsync(x => x.Id==id, cancellationToken);

        }
    }
}
