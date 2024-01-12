

namespace OpenData.Basketball.AbaLeague.Application.DTOs.League
{
    public record LeagueItemDto(int? Id, 
                                string OfficialName, 
                                string ShortName,
                                string StandingUrl,
                                string CalendarUrl,
                                string MatchUrl,
                                string BoxScoreUrl,
                                string BaseUrl,
                                string RosterUrl,
                                Domain.Enums.ProcessorType ProcessorType,
                                int SeasonId,
                                string SeasonName,
                                int? RoundsToPlay
                                );

    public record LeaguesDto(IEnumerable<LeagueItemDto> LeagueResponses);

    public record LeaguesRoster(IEnumerable<int> LeagueIds);
}
