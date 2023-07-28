

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Roster
{
    public record RosterItemDto(
        int PlayerId,
        int LeagueId,
        int TeamId,
        DateTime Start,
        DateTime? End);
}
