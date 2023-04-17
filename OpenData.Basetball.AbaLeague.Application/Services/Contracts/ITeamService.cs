using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface ITeamService
    {
        Task<(IEnumerable<(Team, Team)> existingResulution, IEnumerable<Team> newly)> Get(int leagueId,CancellationToken cancellationToken);
        Task<IEnumerable<Team>> GetExisting(CancellationToken cancellationToken);
        Task<Team> Add(TeamDto team,CancellationToken cancellationToken);
        Task<IEnumerable<Team>> Add(IEnumerable<TeamDto> teams,CancellationToken cancellationToken);
        Task<Team> Update(int id,TeamDto team,CancellationToken cancellationToken);
    }
}
