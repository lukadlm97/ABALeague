using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface ITeamService
    {
        Task<(IEnumerable<(TeamSugestionDTO, TeamSugestionDTO)> existingResulution, IEnumerable<TeamSugestionDTO> newly)> GetAba(int leagueId,CancellationToken cancellationToken);
        Task<(IEnumerable<(TeamSugestionDTO, TeamSugestionDTO)> existingResulution, IEnumerable<TeamSugestionDTO> newly)> GetEuro(int leagueId, CancellationToken cancellationToken);
        Task<(IEnumerable<(TeamSugestionDTO, TeamSugestionDTO)> existingResulution, IEnumerable<TeamSugestionDTO> newly)> GetNational(int leagueId, CancellationToken cancellationToken);
        Task<IEnumerable<Team>> GetExisting(CancellationToken cancellationToken);

        Task<Team> Add(AddTeamDto team,CancellationToken cancellationToken);
        Task<IEnumerable<Team>> Add(IEnumerable<AddTeamDto> teams,CancellationToken cancellationToken);
        Task<Team> Update(int id, AddTeamDto team,CancellationToken cancellationToken);
        Task<Team> AddRoster(IEnumerable<RosterEntryDto>  entries,CancellationToken cancellationToken);
    }
}
