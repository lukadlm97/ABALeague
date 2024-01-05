using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Schedules.Queries.GetScheduleByTeamId
{
    public class GetScheduleByLeagueIdAndTeamIdQuery : IQuery<Maybe<ScheduleDto>>
    {
        public GetScheduleByLeagueIdAndTeamIdQuery(int leagueId, int teamId)
        {
            LeagueId = leagueId;
            TeamId = teamId;
        }

        public int LeagueId { get; private set; }
        public int TeamId { get; private set; }
    }
}
