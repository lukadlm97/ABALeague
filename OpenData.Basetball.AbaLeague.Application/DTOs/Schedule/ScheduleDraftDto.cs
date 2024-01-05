using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Schedule
{
    public record ScheduleDraftDto(IEnumerable<ScheduleItemDto> DraftScheduleItems,
                                IEnumerable<ScheduleItemDto> PlanedScheduleItems,
                                IEnumerable<ScheduleItemDto> ExistingScheduleItems,
                                IEnumerable<string> MissingTeams);
}
