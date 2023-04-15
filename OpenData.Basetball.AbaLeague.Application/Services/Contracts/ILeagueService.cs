using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface ILeagueService
    {
        Task<IEnumerable<League>> Get(CancellationToken cancellationToken = default);
        Task<League> Get(int id, CancellationToken cancellationToken = default);
        Task Add(LeagueDto league, CancellationToken cancellationToken = default);
        Task Delete(int id, CancellationToken cancellationToken = default);
    }
}
