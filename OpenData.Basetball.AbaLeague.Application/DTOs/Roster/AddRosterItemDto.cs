
namespace OpenData.Basketball.AbaLeague.Application.DTOs.Roster
{

    public record AddRosterItemDto(int PlayerId, int LeagueId,int TeamId, DateTime Start, DateTime? End);
}
