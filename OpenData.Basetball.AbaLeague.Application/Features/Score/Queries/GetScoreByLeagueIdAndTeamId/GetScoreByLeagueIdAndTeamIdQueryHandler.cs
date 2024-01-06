using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Application.DTOs.Score;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Score.Queries.GetScoreByLeagueIdAndTeamId
{
    public class GetScoreByLeagueIdAndTeamIdQueryHandler :
        IQueryHandler<GetScoreByLeagueIdAndTeamIdQuery, Maybe<ScoreDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetScoreByLeagueIdAndTeamIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<ScoreDto>>
            Handle(GetScoreByLeagueIdAndTeamIdQuery request, CancellationToken cancellationToken)
        {
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            if (league == null)
            {
                return Maybe<ScoreDto>.None;
            }

            var scheduledMatches = await _unitOfWork.CalendarRepository
                            .SearchByLeagueIdAndTeamId(request.LeagueId, request.TeamId, cancellationToken);

            if (scheduledMatches == null || !scheduledMatches.Any())
            {
                return Maybe<ScoreDto>.None;
            }

            List<ScoreItemDto> list = new List<ScoreItemDto>();
            foreach (var scheduleItem in scheduledMatches)
            {
                var result = await _unitOfWork.ResultRepository.GetByMatchRound(scheduleItem.Id, cancellationToken);
                if(result == null)
                {
                    continue;
                }
                list.Add(new ScoreItemDto(scheduleItem.Id, 
                                                scheduleItem.MatchNo,
                                                scheduleItem.HomeTeamId ?? 0,
                                                scheduleItem.AwayTeamId ?? 0,
                                                scheduleItem.HomeTeam.Name,
                                                scheduleItem.AwayTeam.Name,
                                                result.Attendency,
                                                result.Venue,
                                                result.HomeTeamPoints,
                                                result.AwayTeamPoint,
                                                scheduleItem.Round,
                                                null));
            }

            return new ScoreDto(list);
        }
    }
}
