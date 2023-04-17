using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Application.Contracts;

namespace OpenData.Basetball.AbaLeague.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T:class
    {
        protected readonly AbaLeagueDbContext _dbContext;

        public GenericRepository(AbaLeagueDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> Add(T entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(entity,cancellationToken);
            return entity;
        }

        public async Task<IEnumerable<T>> Add(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddRangeAsync(entities,cancellationToken);
            return entities;
        }

        public async Task Delete(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task<bool> Exists(int id, CancellationToken cancellationToken = default)
        {
            var entity = await Get(id,cancellationToken);
            return entity != null;
        }

        public async Task<T> Get(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.FindAsync<T>(id,cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task Update(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }


    }
}

