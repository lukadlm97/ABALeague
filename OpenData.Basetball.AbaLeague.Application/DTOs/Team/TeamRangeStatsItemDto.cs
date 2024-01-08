using OpenData.Basketball.AbaLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Team
{
    public record TeamRangeStatsItemDto(int Id,
                                            int? MinValue, 
                                            int? MaxValue,
                                            int Count,
                                            StatsPropertyEnum StatsProperty);
}
