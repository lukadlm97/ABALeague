using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore
{
    public record AdvancedMatchCalcuationDto(int GamePlayed, 
                                                int HomeGamesPlayed, 
                                                int GamesWin,
                                                int HomeGamesWin, 
                                                int TotalSpectators,
                                                int AverageSpectators,
                                                int? HomeGameScoredPoints,
                                                int? AwayGameScoredPoints);
}
