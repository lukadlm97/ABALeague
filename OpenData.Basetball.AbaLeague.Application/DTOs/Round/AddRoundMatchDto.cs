

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Round
{
    public record AddRoundMatchDto(int HomeTeamId, int AwayTeamId,   int Round,
        int MatchNo, DateTime DateTime);
}
