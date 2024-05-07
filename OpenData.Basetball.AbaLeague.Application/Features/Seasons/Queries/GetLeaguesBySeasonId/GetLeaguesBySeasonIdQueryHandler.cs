using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Seasons.Queries.GetLeaguesBySeasonId
{
    public class GetLeaguesBySeasonIdQueryHandler
        : IQueryHandler<GetLeaguesBySeasonIdQuery, Maybe<LeaguesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLeaguesBySeasonIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<LeaguesDto>> 
            Handle(GetLeaguesBySeasonIdQuery request, CancellationToken cancellationToken)
        {
            var leagues = 
                _unitOfWork.LeagueRepository.Get()
                            .Where(x => x.SeasonId == request.SeasonId)
                            .ToList();

            return new LeaguesDto(leagues.Select(x => new LeagueItemDto(x.Id, x.OfficalName, x.ShortName, x.StandingUrl, x.CalendarUrl, x.MatchUrl, x.BoxScoreUrl, x.BaseUrl, x.RosterUrl, x.ProcessorTypeEnum, x.SeasonId, string.Empty, x.RoundsToPlay, x.CompetitionOrganizationEnum)));
        }
    }
}
