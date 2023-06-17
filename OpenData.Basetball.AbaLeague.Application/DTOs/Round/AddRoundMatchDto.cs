

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Round
{
    public record AddRoundMatchDto(int HomeTeamId, int AwayTeamId,  int HomeTeamPoints, int AwayTeamPoints, int Round,
        int MatchNo, DateTime DateTime);
}
