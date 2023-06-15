

namespace OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources
{
    public record AddSeasonResourceDto(int TeamId, int LeagueId, string Url);
    public record SeasonResourceDto(int Id, int LeagueId, string LeagueName, int TeamId, string TeamName, string Url);

}
