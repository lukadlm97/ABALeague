using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore
{
    public record AverageBoxscoreCalcuationDto(TimeSpan AvgMinutes, 
                                            double? AvgPoints,
                                            double? AvgTotalRebounds,
                                            double? AvgAssists,
                                            double? AvgSteals,
                                            double? AvgTurnover,
                                            double? AvgPlusMinus,
                                            double? AvgRankValue);
}
