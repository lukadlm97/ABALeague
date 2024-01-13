using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using static OpenData.Basketball.AbaLeague.Application.Validation.ValidationErrors;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueById
{
    public class GetLeagueByIdQueryHandler : IQueryHandler<GetLeagueByIdQuery, Maybe<LeagueItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLeagueByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<LeagueItemDto>> Handle(GetLeagueByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.LeagueId <= 0)
            {
                return Maybe<LeagueItemDto>.None;
            }

            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            if (league == null)
            {
                return Maybe<LeagueItemDto>.None;
            }
            var seasons = _unitOfWork.SeasonRepository.Get();
            var season = seasons.FirstOrDefault(y => y.Id == league.SeasonId);
            var response = new LeagueItemDto(league.Id,
                                                league.OfficalName, 
                                                league.ShortName,
                                                league.StandingUrl, 
                                                league.CalendarUrl, 
                                                league.MatchUrl,
                                                league.BoxScoreUrl,
                                                league.BaseUrl,
                                                league.RosterUrl,
                                                league.ProcessorTypeEnum ?? Domain.Enums.ProcessorType.Unknow,
                                                season.Id!,
                                                season.Name!,
                                                league.RoundsToPlay,
                                                league.CompetitionOrganizationEnum ?? Domain.Enums.CompetitionOrganizationEnum.League);

            return response;
        }
    }
}
