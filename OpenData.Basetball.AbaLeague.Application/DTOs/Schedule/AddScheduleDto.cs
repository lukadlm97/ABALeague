using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Schedule
{
    public record AddScheduleDto(int HomeTeamId,
                                            int AwayTeamId,
                                            int Round,
                                            int MatchNo,
                                            DateTime DateTime);
}
