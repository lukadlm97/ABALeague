using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Score
{
    public record AddScoreDto(int MatchId,
                                        int HomeTeamId,
                                        int AwayTeamId,
                                        int HomeTeamPoints,
                                        int AwayTeamPoints,
                                        int Attendency,
                                        string Venue);
}
