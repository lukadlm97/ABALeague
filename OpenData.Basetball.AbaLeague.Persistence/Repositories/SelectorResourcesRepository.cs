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
    public class SelectorResourcesRepository : ISelectorResourcesRepository
    {
        private readonly AbaLeagueDbContext _dbContext;

        public SelectorResourcesRepository(AbaLeagueDbContext dbContext)
        {
            _dbContext = dbContext;
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
    }
}
