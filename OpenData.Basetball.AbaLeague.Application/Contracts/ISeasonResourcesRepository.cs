using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface ISeasonResourcesRepository:
        IGenericRepository<SeasonResources>
    {
        Task<IReadOnlyList<SeasonResources>> SearchByLeague(int leagueId,
            CancellationToken cancellationToken=default);
        Task<IReadOnlyList<SeasonResources>> SearchByTeam(int teamId,
            CancellationToken cancellationToken = default);
    }
}
