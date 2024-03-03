using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface IBoxScoreRepository : IGenericRepository<BoxScore>
    {
        IQueryable<BoxScore> Get();
        IQueryable<BoxScore> GetWithRoundMatchIncluded();
        Task<bool> Exist(int roundMatchId, int rosterItemId, CancellationToken cancellationToken = default);
        Task<IEnumerable<BoxScore>> GetByRosterItemId(int rosterItemId, CancellationToken cancellationToken = default);
        Task<IEnumerable<BoxScore>> SearchByLeagueId(int leagueId, CancellationToken cancellationToken = default);
        IQueryable<BoxScore> SearchByMatchRoundId(int roundMatchId, 
                                                            CancellationToken cancellationToken = default);
        IQueryable<BoxScore> SearchByMatchRoundAndRosterIds(int roundMatchId,
                                                            IEnumerable<int> rosterItemIds);
    }
}
