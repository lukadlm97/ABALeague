using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basketball.AbaLeague.Application.DTOs.Game;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore
{
    public record BoxscoreByPlayerDto(int PlayerId, 
                                        string PlayerName, 
                                        int TeamId, 
                                        string TeamName, 
                                        int LeagueId, 
                                        string LeagueName,
                                        IEnumerable<GameStatsByPlayerItemDto> Games,
                                        AverageBoxscoreCalcuationDto AverageBoxscoreCalcuations,
                                        AdvancedMatchCalcuationDto AdvancedMatchCalcuations);
}
