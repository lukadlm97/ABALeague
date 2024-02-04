using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Score;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Score.Queries.GetScoresByLeagueId
{
    public class GetScoresByLeagueIdQueryHandler : 
        IQueryHandler<GetScoresByLeagueIdQuery, Maybe<ScoreDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetScoresByLeagueIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<ScoreDto>>
            Handle(GetScoresByLeagueIdQuery request, CancellationToken cancellationToken)
        {
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            if (league == null)
            {
                return Maybe<ScoreDto>.None;
            }

            var results = await _unitOfWork.ResultRepository.SearchByLeague(league.Id, cancellationToken);
            var matches = await _unitOfWork.CalendarRepository.SearchByLeague(league.Id, cancellationToken);
            List<ScoreItemDto> list = new List<ScoreItemDto>();
            foreach(var result in results)
            {
                var match = matches.FirstOrDefault(x => x.Id == result.RoundMatchId);
                if(match == null)
                {
                    continue;
                }
                list.Add(new ScoreItemDto(match.Id, 
                    match.MatchNo,
                    match.HomeTeamId ?? 0,
                    match.AwayTeamId ?? 0, 
                    match.HomeTeam.Name,
                    match.AwayTeam.Name, 
                    result.Attendency, 
                    result.Venue, 
                    result.HomeTeamPoints, 
                    result.AwayTeamPoint, 
                    match.Round,
                    result.Id, 
                    match.DateTime));
            }

            return new ScoreDto(list.ToFrozenSet());
        }
    }
}
