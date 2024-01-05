using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface ISelectorResourcesRepository 
    {
        Task<ResourceSelector?> GetByLeagueIdAndSelectorType(int leagueId,
                                                                short selectorType,
                                                                CancellationToken cancellationToken = default); 
    }
}
