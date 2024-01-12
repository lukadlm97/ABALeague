

namespace OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources
{
    public record AddSeasonResourceDto(int TeamId, int LeagueId, string Url,  string TeamName, string? TeamUrl, string? IncrowdUrl);
    public record AddSeasonResourceDraftDto(int TeamId, int LeagueId, string Url,  string TeamName, string? TeamUrl, string? IncrowdUrl);
    public record SeasonResourceDto(int Id, int LeagueId, string LeagueName, int TeamId, string TeamName, string Url);
    public record SeasonResourceByTeamDto(int LeagueId, string LeagueName, string ShortName, string TeamName, int TeamId, string Url);

}
