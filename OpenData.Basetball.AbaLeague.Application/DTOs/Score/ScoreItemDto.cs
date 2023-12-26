using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Score
{
    public record ScoreItemDto(int MatchId,
                                int MatchNo,
                                int HomeTeamId,
                                int AwayTeamId,
                                string HomeTeamName,
                                string AwayTeamName,
                                int? Attendency,
                                string? Venue,
                                int? HomeTeamPoints,
                                int? AwayTeamPoints);
}
