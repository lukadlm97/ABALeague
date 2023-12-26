using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Schedule
{
    public record ScheduleItemDto(int? Id,
                                            int HomeTeamId, 
                                            int AwayTeamId, 
                                            string HomeTeamName,
                                            string AwayTeamName,
                                            int Round, 
                                            int MatchNo, 
                                            DateTime DateTime);
}
