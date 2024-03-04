using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.DTOs.Statistic;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface IStatisticsCalculator
    {
        FrozenSet<CategoriesItemDto>
         Calcualate(IEnumerable<BoxScore> boxScores,
                    FrozenSet<PositionEnum> positions);
        FrozenSet<CategoriesItemByMatchDto>
         CalcualateByMatch(IEnumerable<BoxScore> boxScores,
                    FrozenSet<PositionEnum> positions);
    }
}
