using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues
{
    internal class GetLeagueQueryHandler : IQueryHandler<GetLeagueQuery, Maybe<LeaguesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLeagueQueryHandler(IGenericRepository<League> leagueRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<LeaguesDto>> Handle(GetLeagueQuery request, CancellationToken cancellationToken)
        {
            var leagues = await _unitOfWork.LeagueRepository.GetAll(cancellationToken);
            var seasons = _unitOfWork.SeasonRepository.Get();
            if (!leagues.Any())
            {
                return Maybe<LeaguesDto>.None;
            }

            var response = leagues.Select(x => 
            {
                var season = seasons.FirstOrDefault(y => y.Id == x.SeasonId);

                return new LeagueItemDto(x.Id,
                                            x.OfficalName,
                                            x.ShortName,
                                            x.StandingUrl,
                                            x.CalendarUrl,
                                            x.MatchUrl,
                                            x.BoxScoreUrl,
                                            x.RosterUrl!,
                                            x.BaseUrl!,
                                            x.ProcessorTypeEnum ?? Domain.Enums.ProcessorType.Unknow,
                                            season.Id!,
                                            season.Name!,
                                            x.RoundsToPlay);
            });

            return new LeaguesDto(response);
        }
    }
}
