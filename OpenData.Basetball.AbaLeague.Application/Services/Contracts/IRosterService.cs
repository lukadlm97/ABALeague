
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface IRosterService
    {
        Task<IEnumerable<RosterEntryDto>> GetDraftRoster(int teamId, int leagueId,
            CancellationToken cancellationToken = default);
    }
}
