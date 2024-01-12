using OpenData.Basketball.AbaLeague.Domain.Enums;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Team
{
    public record AddTeamsDto(IEnumerable<AddTeamDto> Teams);
}
