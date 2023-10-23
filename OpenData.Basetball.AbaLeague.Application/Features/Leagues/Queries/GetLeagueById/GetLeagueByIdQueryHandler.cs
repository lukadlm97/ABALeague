using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueById
{
    public class GetLeagueByIdQueryHandler : IQueryHandler<GetLeagueByIdQuery, Maybe<LeagueResponse>>
    {
        private readonly IGenericRepository<League> _leagueRepository;

        public GetLeagueByIdQueryHandler(IGenericRepository<League> leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }
        public async Task<Maybe<LeagueResponse>> Handle(GetLeagueByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.LeagueId <= 0)
            {
                return Maybe<LeagueResponse>.None;
            }

            var league = await _leagueRepository.Get(request.LeagueId, cancellationToken);
            if (league == null)
            {
                return Maybe<LeagueResponse>.None;
            }

            var response = new LeagueResponse(league.Id,
                league.OfficalName, 
                league.ShortName,
                league.Season,
                league.StandingUrl, 
                league.CalendarUrl, 
                league.MatchUrl,
                league.BoxScoreUrl,
                league.RosterUrl,
                league.BaseUrl);

            return response;
        }
    }
}
