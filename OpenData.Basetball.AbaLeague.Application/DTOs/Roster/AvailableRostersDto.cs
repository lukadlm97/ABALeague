

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Roster
{
    public record AvailableRostersDto(IEnumerable<(int leagueId, string name)> Leagues);
}
