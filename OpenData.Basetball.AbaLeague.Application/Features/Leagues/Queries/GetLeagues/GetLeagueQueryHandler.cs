using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.Contracts.Leagues;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues
{
    internal class GetLeagueQueryHandler : IQueryHandler<GetLeagueQuery, Maybe<LeaguesResponse>>
    {
        private readonly IGenericRepository<League> _leagueRepository;

        public GetLeagueQueryHandler(IGenericRepository<League> leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }
        public async Task<Maybe<LeaguesResponse>> Handle(GetLeagueQuery request, CancellationToken cancellationToken)
        {
            var leagues = await _leagueRepository.GetAll(cancellationToken);
            if (!leagues.Any())
            {
                return Maybe<LeaguesResponse>.None;
            }

            var response = leagues.Select(x => new LeagueResponse()
            {
                BaseUrl = x.BaseUrl,
                BoxScoreUrl = x.BoxScoreUrl,
                CalendarUrl = x.CalendarUrl,
                Id = x.Id,
                MatchUrl = x.MatchUrl,
                OfficalName = x.OfficalName,
                RosterUrl = x.RosterUrl,
                Season = x.Season,
                ShortName = x.ShortName,
                StandingUrl = x.StandingUrl,
            });

            return new LeaguesResponse(){Leagues = response};
        }
    }
}
