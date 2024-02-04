namespace OpenData.Basketball.AbaLeague.Application.DTOs.Round
{
    public record RoundMatchDto(int HomeTeamId, 
                                int AwayTeamId, 
                                string HomeTeamName, 
                                string AwayTeamName, 
                                int? HomeTeamPoints, 
                                int? AwayTeamPoints, 
                                int Round, 
                                int MatchNo, 
                                DateTime DateTime);
}
