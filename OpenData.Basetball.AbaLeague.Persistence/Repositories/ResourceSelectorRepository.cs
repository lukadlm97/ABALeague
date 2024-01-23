using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basketball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class ResourceSelectorRepository : IResourceSelectorRepository
    {
        private readonly AbaLeagueDbContext _dbContext;

        public ResourceSelectorRepository(AbaLeagueDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResourceSelector> Add(ResourceSelector entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.ResourceSelector.AddAsync(entity);
            return entity;
        }

        public Task<IEnumerable<ResourceSelector>> Add(IEnumerable<ResourceSelector> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Delete(ResourceSelector entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResourceSelector> Get(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ResourceSelector> Get()
        {
            return _dbContext.ResourceSelector;
        }

        public Task<IReadOnlyList<ResourceSelector>> GetAll(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ResourceSelector?> 
            GetByLeagueIdAndSelectorType(int leagueId, short selectorType, CancellationToken cancellationToken = default)
        {
            var selectedSelectors =  _dbContext.ResourceSelector
                .Where(x => x.LeagueId == leagueId && x.HtmlQuerySelectorId == selectorType);
            if(selectedSelectors == null || selectedSelectors.Count() != 1)
            {
                return null;
            }
            return selectedSelectors.First();
        }

        public Task Update(ResourceSelector entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
