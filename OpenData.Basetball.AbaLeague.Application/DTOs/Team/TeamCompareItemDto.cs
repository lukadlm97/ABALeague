using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Application.DTOs.Statistic;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Team
{
    public record TeamCompareItemDto(TeamItemDto TeamItem,
                                        FrozenSet<CategoriesItemDto> Items,
                                        FrozenDictionary<PositionEnum, FrozenSet<PlayerRosterItemDto>> RosterEntriesByPosition,
                                        TotalAndAveragePerformanceDto? TotalAndAveragePerformance = null,
                                        AdvancedMatchCalcuationDto? MatchSuccessCalcuationDto = null);
}
