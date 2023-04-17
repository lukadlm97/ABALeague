using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface ISeasonResourcesService
    {
        Task<IEnumerable<SeasonResources>> Get(CancellationToken  cancellationToken=default);
        Task<IEnumerable<SeasonResources>> Get(int teamId, 
            CancellationToken cancellationToken=default);
        Task<IEnumerable<SeasonResources>> GetTeams(int leagueId,
            CancellationToken cancellationToken = default);
        Task<SeasonResources> Add(SeasonResourceDto seasonResourceDto, 
            CancellationToken cancellationToken = default);
        Task<IEnumerable<SeasonResources>> Add(IEnumerable<SeasonResourceDto> seasonResourceDto,
            CancellationToken cancellationToken = default);
        Task<SeasonResources> UpdateUrl(int resourceId, 
            string url,CancellationToken cancellationToken=default);

    }
}
