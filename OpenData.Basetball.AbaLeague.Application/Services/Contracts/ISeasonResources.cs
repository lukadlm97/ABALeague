using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface ISeasonResourcesService
    {
        Task<IEnumerable<SeasonResourceDto>> Get(CancellationToken  cancellationToken=default);
        Task<IEnumerable<SeasonResources>> Get(int teamId, 
            CancellationToken cancellationToken=default);
        Task<IEnumerable<SeasonResourceDto>> GetTeams(int leagueId,
            CancellationToken cancellationToken = default);
        Task<SeasonResources> Add(AddSeasonResourceDto seasonResourceDto, 
            CancellationToken cancellationToken = default);
        Task<IEnumerable<SeasonResources>> Add(IEnumerable<AddSeasonResourceDto> seasonResourceDto,
            CancellationToken cancellationToken = default);
        Task<SeasonResources> UpdateUrl(int resourceId, 
            string url,CancellationToken cancellationToken=default);

    }
}
