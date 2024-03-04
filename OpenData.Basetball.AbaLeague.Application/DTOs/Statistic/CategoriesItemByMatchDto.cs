using OpenData.Basetball.AbaLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Statistic
{
    public record CategoriesItemByMatchDto(int matchId, 
                                            PositionEnum PositionEnum,
                                            PerformanceItem BoxScoreItemDto) 
    : CategoriesItemDto(PositionEnum, BoxScoreItemDto);
}
