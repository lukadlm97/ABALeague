using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface ITeamService
    {
        Task<(IEnumerable<(Team, Team)> existingResulution, IEnumerable<Team> newly)> Get(int leagueId,CancellationToken cancellationToken);
        Task<IEnumerable<Team>> GetExisting(CancellationToken cancellationToken);
        Task Add(Team team,CancellationToken cancellationToken);
        Task Add(IEnumerable<Team> teams,CancellationToken cancellationToken);
        Task Update(Team team,CancellationToken cancellationToken);
    }
}
