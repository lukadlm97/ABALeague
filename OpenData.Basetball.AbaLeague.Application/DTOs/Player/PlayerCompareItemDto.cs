using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Application.DTOs.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Player
{
    public record PlayerCompareItemDto(int TeamId, 
                                        string TeamName, 
                                        PlayerItemDto PlayerItem, 
                                        TotalAndAveragePerformanceDto? TotalAndAveragePerformance = null,
                                        AdvancedMatchCalcuationDto? MatchSuccessCalcuationDto = null);
}
