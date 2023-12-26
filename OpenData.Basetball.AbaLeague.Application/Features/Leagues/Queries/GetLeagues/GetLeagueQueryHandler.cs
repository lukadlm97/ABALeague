using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
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

            var response = leagues.Select(x => new LeagueResponse(x.Id, 
                x.OfficalName,
                x.ShortName,
                x.Season,
                x.StandingUrl,
                x.CalendarUrl,
                x.MatchUrl, 
                x.BoxScoreUrl, 
                x.RosterUrl, 
                x.BaseUrl,
                x.ProcessorTypeId));

            return new LeaguesResponse(response);
        }
    }
}
