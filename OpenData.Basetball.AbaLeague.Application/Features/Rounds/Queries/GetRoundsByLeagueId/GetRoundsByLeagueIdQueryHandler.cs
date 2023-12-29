using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Round;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rounds.Queries.GetRoundsByLeagueId
{
    public class GetRoundsByLeagueIdQueryHandler : IQueryHandler<GetRoundsByLeagueIdQuery, Maybe<AvailableRoundsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRoundsByLeagueIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<AvailableRoundsDto>> 
            Handle(GetRoundsByLeagueIdQuery request, CancellationToken cancellationToken)
        {
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            if(league == null)
            {
                return Maybe<AvailableRoundsDto>.None;
            }

            var roundMatches = await _unitOfWork.CalendarRepository.SearchByLeague(league.Id, cancellationToken);

            return new AvailableRoundsDto(roundMatches.Select(x => x.Round).Distinct());

        }
    }
}
