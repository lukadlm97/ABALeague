using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Statistic
{
    public record CategoriesItemDto(PositionEnum PositionEnum,
                                    PerformanceItem BoxScoreItemDto);
}
