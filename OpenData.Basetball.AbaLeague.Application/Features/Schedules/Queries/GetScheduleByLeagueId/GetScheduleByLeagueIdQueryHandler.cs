using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Schedules.Queries.GetScheduleByLeagueId
{
    public class GetScheduleByLeagueIdQueryHandler :
        IQueryHandler<GetScheduleByLeagueIdQuery, Maybe<ScheduleDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetScheduleByLeagueIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public  async Task<Maybe<ScheduleDto>> 
            Handle(GetScheduleByLeagueIdQuery request, CancellationToken cancellationToken)
        {
            var scheduledMatches = await _unitOfWork.CalendarRepository
                .SearchByLeague(request.LeagueId, cancellationToken);
            List<ScheduleItemDto> items = new List<ScheduleItemDto>();
            foreach(var match in scheduledMatches)
            {
                items.Add(new ScheduleItemDto(match.Id, match.HomeTeamId ?? 0, match.AwayTeamId ?? 0, match.HomeTeam.Name, match.AwayTeam.Name, match.Round, match.MatchNo, match.DateTime));
            }


            return new ScheduleDto(items.ToFrozenSet());
        }
    }
}
