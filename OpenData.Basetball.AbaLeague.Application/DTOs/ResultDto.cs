

namespace OpenData.Basketball.AbaLeague.Application.DTOs
{
    public record ResultDto(int MatchRoundId,string HomeTeam,string AwayTeam, int? Attendency, string Venue, int? HomeTeamPoint, int? AwayTeamPoint);
}
