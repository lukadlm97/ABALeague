

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Team
{
    public record AddResultDto(int MatchRoundId, string HomeTeam, string AwayTeam, int? Attendency, string Venue,
        int HomeTeamPoint, int AwayTeamPoint);
}
