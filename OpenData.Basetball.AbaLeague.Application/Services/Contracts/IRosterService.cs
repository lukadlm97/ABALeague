using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface IRosterService
    {
        Task<IEnumerable<RosterEntryDto>> GetDraftRoster(int teamId, int leagueId,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<RosterEntryDto>> GetWholeDraftRoster(int leagueId,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<RosterItemDto>> GetWholeRosterItemDraftRoster(int leagueId,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<DraftRosterEntry>> Get(int leagueId, int teamId,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<DraftRosterEntry>> Add(int teamId, IEnumerable<DraftRosterEntry> entries,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<int>> Add(IEnumerable<AddRosterItemDto> entries,
            CancellationToken cancellationToken = default);
    }
}
