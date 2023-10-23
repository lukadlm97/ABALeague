using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Contracts.Leagues;

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

            var response = new LeagueResponse()
            {
                BaseUrl = league.BaseUrl,
                BoxScoreUrl = league.BoxScoreUrl,
                CalendarUrl = league.CalendarUrl,
                Id = league.Id,
                ShortName = league.ShortName,
                MatchUrl = league.MatchUrl,
                OfficalName = league.OfficalName,
                RosterUrl = league.RosterUrl,
                Season = league.Season,
                StandingUrl = league.StandingUrl,
            };

            return response;
        }
    }
}
