using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface IRosterRepository : IGenericRepository<RosterItem>
    {
        Task<bool> Exist(int leagueId, int playerId, int teamId, CancellationToken  cancellationToken = default);
        Task<RosterItem?> Get(int leagueId, int playerId, int teamId, CancellationToken  cancellationToken = default);
        Task<RosterItem?> Get(int leagueId, int playerId, CancellationToken  cancellationToken = default);
        Task<IEnumerable<RosterItem>> 
            GetTeamRosterByLeagueId(int leagueId, int teamId, CancellationToken  cancellationToken = default);
        Task<IEnumerable<RosterItem>> SearchByLeagueId(int leagueId, CancellationToken cancellationToken = default);
        Task<IQueryable<RosterItem>> SearchRosterByLeagueAndTeamId(int leagueId, int teamId, CancellationToken cancellationToken = default);
        IQueryable<RosterItem> Get(); 
        IQueryable<RosterItem> GetWithLeagueAndTeam();
        IQueryable<RosterItem> GetByLeagueIdAndTeamId(int leagueId, int teamId);
    }
}
