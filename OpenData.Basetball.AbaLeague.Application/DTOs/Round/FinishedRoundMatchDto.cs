using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Round
{
    public record FinishedRoundMatchDto(int HomeTeamId,
                                        int AwayTeamId,
                                        string HomeTeamName,
                                        string AwayTeamName,
                                        int? HomeTeamPoints,
                                        int? AwayTeamPoints,
                                        int Round,
                                        int MatchNo,
                                        DateTime DateTime,
                                        int? Attendency,
                                        string? Venue,
                                        BoxscoreTotalDto HomeTeamTotals,
                                        BoxscoreTotalDto AwayTeamTotals) 
        : RoundMatchDto(HomeTeamId, 
                        AwayTeamId,
                        HomeTeamName, 
                        AwayTeamName,
                        HomeTeamPoints,
                        AwayTeamPoints, 
                        Round, 
                        MatchNo, 
                        DateTime);
}
