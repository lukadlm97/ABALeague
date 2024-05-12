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

namespace OpenData.Basketball.AbaLeague.Application.Features.Score.Queries.GetScoresByRoundAndLeagueId
{
    internal class GetScoresByRoundAndLeagueIdQueryHandler
        : IQueryHandler<GetScoresByRoundAndLeagueIdQuery, Maybe<FrozenSet<ScoreItemDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetScoresByRoundAndLeagueIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<FrozenSet<ScoreItemDto>>> 
            Handle(GetScoresByRoundAndLeagueIdQuery request, CancellationToken cancellationToken)
        {
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            if (league == null)
            {
                return Maybe<FrozenSet<ScoreItemDto>>.None;
            }

            var results = await _unitOfWork.ResultRepository.SearchByLeague(league.Id, cancellationToken);
            var matches = await _unitOfWork.CalendarRepository.SearchByLeague(league.Id, cancellationToken);
            List<ScoreItemDto> list = new List<ScoreItemDto>();
            foreach (var currentMatch in matches.Where(x=>x.Round == request.Round))
            {
                var selectedResult = results.FirstOrDefault(x => x.RoundMatchId == currentMatch.Id);
                if (selectedResult == null || 
                    currentMatch.HomeTeam == null || 
                    currentMatch.AwayTeam == null)
                {
                    continue;
                }
                list.Add(new ScoreItemDto(currentMatch.Id,
                    currentMatch.MatchNo,
                    currentMatch.HomeTeamId ?? 0,
                    currentMatch.AwayTeamId ?? 0,
                    currentMatch.HomeTeam.Name,
                    currentMatch.AwayTeam.Name,
                    selectedResult.Attendency,
                    selectedResult.Venue,
                    selectedResult.HomeTeamPoints,
                    selectedResult.AwayTeamPoint,
                    currentMatch.Round,
                    selectedResult.Id,
                    currentMatch.DateTime));
            }

            return list.ToFrozenSet();
        }
    }
}
