using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Application.Features.Schedules.Queries.GetScheduleByTeamId;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Schedules.Queries.GetScheduleByLeagueIdAndTeamId
{
    public class GetScheduleByLeagueIdAndTeamIdQueryHandler
        : IQueryHandler<GetScheduleByLeagueIdAndTeamIdQuery, Maybe<ScheduleDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetScheduleByLeagueIdAndTeamIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<ScheduleDto>> Handle(GetScheduleByLeagueIdAndTeamIdQuery request, CancellationToken cancellationToken)
        {
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            if(league == null)
            {
                return Maybe<ScheduleDto>.None;
            }

            var scheduledMatches = await _unitOfWork.CalendarRepository
                            .SearchByLeagueIdAndTeamId(request.LeagueId, request.TeamId, cancellationToken);

            if(scheduledMatches == null || !scheduledMatches.Any())
            {
                return Maybe<ScheduleDto>.None;
            }
            List<ScheduleItemDto> list = new List<ScheduleItemDto>();
            foreach(var scheduleItem in scheduledMatches)
            {
                list.Add(new ScheduleItemDto(scheduleItem.Id, 
                                                scheduleItem.HomeTeamId ?? 0, 
                                                scheduleItem.AwayTeamId ?? 0, 
                                                scheduleItem.HomeTeam.Name, 
                                                scheduleItem.AwayTeam.Name, 
                                                scheduleItem.Round, 
                                                scheduleItem.MatchNo,
                                                scheduleItem.DateTime));
            }

            return new ScheduleDto(list);
        }
    }
}
