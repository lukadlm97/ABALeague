using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Game;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Matches.Queries.GetMatchesByLeagueId
{
    internal class GetMatchesByLeagueIdQueryHandler
         : IQueryHandler<GetMatchesByLeagueIdQuery, Maybe<FrozenSet<ScheduleItemDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMatchesByLeagueIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async
            Task<Maybe<FrozenSet<ScheduleItemDto>>> 
            Handle(GetMatchesByLeagueIdQuery request, CancellationToken cancellationToken)
        {
            var selectedLeague = _unitOfWork.LeagueRepository.GetIncludedRoundMatches()
                                            .FirstOrDefault(x=>x.Id == request.LeagueId);
            if(selectedLeague == null || 
                selectedLeague.RoundMatches == null ||
                !selectedLeague.RoundMatches.Any())
            {
                return Maybe<FrozenSet<ScheduleItemDto>>.None;
            }

            var matches = new List<ScheduleItemDto>();
            foreach(var currentMatch in selectedLeague.RoundMatches)
            {
                var match = new ScheduleItemDto(currentMatch.Id, currentMatch.HomeTeamId, currentMatch.AwayTeamId, currentMatch.HomeTeam.Name, currentMatch.AwayTeam.Name, currentMatch.Round, currentMatch.MatchNo, currentMatch.DateTime);
                matches.Add(match);
            }

            return matches.ToFrozenSet();
        }
    }
}
