
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface ITeamRepository:IGenericRepository<Team>
    {
        IEnumerable<Team> Get(IEnumerable<int> ids,CancellationToken  cancellationToken=default);
        Task<Team?> GetRoster(int teamId, CancellationToken cancellationToken = default);
    }
}
