using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface IResourceSelectorRepository : IGenericRepository<ResourceSelector>
    {
        Task<ResourceSelector?> GetByLeagueIdAndSelectorType(int leagueId,
                                                                short selectorType,
                                                                CancellationToken cancellationToken = default); 
        IQueryable<ResourceSelector> Get();
    }
}
