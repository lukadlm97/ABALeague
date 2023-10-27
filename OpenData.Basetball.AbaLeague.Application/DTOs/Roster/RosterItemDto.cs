

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Roster
{
    public record RosterItemDto(
        int PlayerId,
        int LeagueId,
        DateTime Start,
        DateTime? End);

    public record RosterResponse(IEnumerable<RosterItemDto> Items);
}
