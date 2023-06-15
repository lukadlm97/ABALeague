

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Team
{
    public record TeamSugestionDTO(int TeamId,string Name, string Url, int LeagueId,string ShortName = null);
}
