using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Game;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Matches.Queries.GetByMatchId
{
    internal class GetByMatchIdQueryHandler : 
        IQueryHandler<GetByMatchIdQuery, Maybe<ScheduleItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByMatchIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<ScheduleItemDto>> Handle(GetByMatchIdQuery request, CancellationToken cancellationToken)
        {
            var roundMatch = _unitOfWork.CalendarRepository.Get()
                                        .FirstOrDefault(x=>x.Id == request.MatchId);
            if (roundMatch == null || roundMatch.HomeTeamId == null || roundMatch.AwayTeamId == null)
            {
                return Maybe<ScheduleItemDto>.None;
            }

            var teams = _unitOfWork.TeamRepository.Get(new List<int>
            {
                roundMatch.HomeTeamId ?? -1,
                roundMatch.AwayTeamId ?? -1
            });
            var homeTeam = teams.FirstOrDefault(x=>x.Id == roundMatch.HomeTeamId);
            var awayTeam = teams.FirstOrDefault(x=>x.Id == roundMatch.AwayTeamId);
            if (homeTeam == null || awayTeam == null)
            {
                return Maybe<ScheduleItemDto>.None;
            }

            return new ScheduleItemDto(roundMatch.Id, 
                homeTeam.Id, awayTeam.Id, 
                homeTeam.Name, awayTeam.Name, 
                roundMatch.Round, 
                roundMatch.MatchNo, roundMatch.DateTime);
        }
    }
}
