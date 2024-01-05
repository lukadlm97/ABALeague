using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Schedules.Queries.GetScheduleByLeagueId
{
    public class GetScheduleByLeagueIdQuery : IQuery<Maybe<ScheduleDto>>
    {
        public GetScheduleByLeagueIdQuery(int leagueId)
        {
            LeagueId = leagueId;
        }

        public int LeagueId { get; private set; }
    }
}
