using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface ILeagueRepository : IGenericRepository<League>
    {
        League? SearchLeagueByRoundMatchId(int roundMatchId);
        IQueryable<League> Get();
    }
}
