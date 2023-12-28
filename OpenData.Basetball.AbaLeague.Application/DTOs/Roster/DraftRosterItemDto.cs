

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Roster
{
    public record DraftRosterItemDto(int PlayerId, string PlayerName, int LeagueId, string LeagueName, int TeamId, string TeamName, DateTime Start, DateTime? End);
}
