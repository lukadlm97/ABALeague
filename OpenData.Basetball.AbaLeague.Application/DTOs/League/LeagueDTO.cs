

namespace OpenData.Basketball.AbaLeague.Application.DTOs.League
{
    public record LeagueDto(string OfficialName, 
        string ShortName,
        string Season,
        string StandingUrl,
        string CalendarUrl,
        string MatchUrl,
        string BoxScoreUrl,
        string BaseUrl,
        string RosterUrl
        );
    public record LeagueResponse(int Id,
        string OfficialName,
        string ShortName,
        string Season,
        string StandingUrl,
        string CalendarUrl,
        string MatchUrl,
        string BoxScoreUrl,
        string RosterUrl,
        string BaseUrl
    );

    public record LeaguesResponse(IEnumerable<LeagueResponse> LeagueResponses);

    public record LeaguesRoster(IEnumerable<int> LeagueIds);
}
