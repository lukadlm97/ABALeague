

namespace OpenData.Basketball.AbaLeague.Application.DTOs.League
{
    public record LeagueDto(string OfficialName, 
        string ShortName,
        string Season,
        string StandingUrl,
        string CalendarUrl,
        string MatchUrl,
        string BoxScoreUrl,
        string BaseUrl
        );
}
