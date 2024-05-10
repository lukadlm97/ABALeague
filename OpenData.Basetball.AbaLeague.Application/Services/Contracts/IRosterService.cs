using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface IRosterService
    {
        Task<IEnumerable<RosterEntryDto>> GetDraftRoster(int teamId, int leagueId,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<RosterEntryDto>> GetWholeDraftRoster(int leagueId,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<PlayerRosterItemDto>> GetWholeRosterItemDraftRoster(int leagueId,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<DraftRosterItemDto>> Get(int leagueId, int teamId,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<DraftRosterItemDto>> Add(int teamId, IEnumerable<DraftRosterItemDto> entries,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<int>> Add(IEnumerable<AddRosterItemDto> entries,
            CancellationToken cancellationToken = default);
    }
}
