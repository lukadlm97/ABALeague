using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface IBoxScoreRepository:IGenericRepository<BoxScore>
    {
        Task<bool> Exist(int roundMatchId, int rosterItemId, CancellationToken cancellationToken = default);
    }
}
