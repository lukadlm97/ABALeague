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

namespace OpenData.Basketball.AbaLeague.Application.Features.Score.Queries.GetScoreById
{
    internal class GetScoreByIdQueryHandler
        : IQueryHandler<GetScoreByIdQuery, Maybe<ScoreItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetScoreByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<ScoreItemDto>> 
            Handle(GetScoreByIdQuery request, CancellationToken cancellationToken)
        {
            var roundMatch = _unitOfWork.CalendarRepository.GetWithTeamsIncluded()
                                        .FirstOrDefault(x => x.Id == request.GameId);
            var score = _unitOfWork.ResultRepository.Get()
                                    .FirstOrDefault(x => x.RoundMatchId == request.GameId);
            if (roundMatch == null || 
                score == null || 
                roundMatch.HomeTeam == null || 
                roundMatch.AwayTeam == null) 
            {
                return Maybe<ScoreItemDto>.None;
            }

            return new ScoreItemDto(roundMatch.Id, roundMatch.MatchNo, 
                roundMatch.HomeTeam.Id, roundMatch.AwayTeam.Id, 
                roundMatch.HomeTeam.Name, roundMatch.AwayTeam.Name, 
                score.Attendency, score.Venue, 
                score.HomeTeamPoints, score.AwayTeamPoint, 
                roundMatch.Round, score.Id, 
                roundMatch.DateTime);
        }
    }
}
