

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Roster
{
    public record DraftRosterEntry(int PlayerId, string PlayerName, int LeagueId, string LeagueName, DateTime Start, DateTime? End);
}
