using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Schedules.Queries.GetScheduleByLeagueId
{
    public class GetScheduleDraftByLeagueIdQuery : IQuery<Maybe<ScheduleDraftDto>>
    {
        public int LeagueId { get; }
        public GetScheduleDraftByLeagueIdQuery(int leagueId) => LeagueId = leagueId;
    }
}
