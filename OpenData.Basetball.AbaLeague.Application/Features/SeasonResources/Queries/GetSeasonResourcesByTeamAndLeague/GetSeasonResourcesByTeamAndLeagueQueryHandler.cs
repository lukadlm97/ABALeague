using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.SeasonResources.Queries.GetSeasonResourcesByTeamAndLeague
{
    internal class GetSeasonResourcesByTeamAndLeagueQueryHandler : IQueryHandler<GetSeasonResourcesByTeamAndLeagueQuery, Maybe<SeasonResourceByTeamDto>>
    {
        private IUnitOfWork _unitOfWork;

        public GetSeasonResourcesByTeamAndLeagueQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Maybe<SeasonResourceByTeamDto>> Handle(GetSeasonResourcesByTeamAndLeagueQuery request, CancellationToken cancellationToken)
        {
            var leagues = await _unitOfWork.LeagueRepository.GetAll(cancellationToken);
            var seasonResources = await _unitOfWork.SeasonResourcesRepository.GetAll(cancellationToken);

            var filteredSeasonResource = seasonResources.FirstOrDefault(x => x.TeamId == request.TeamId && x.LeagueId == request.LeagueId);


            if (filteredSeasonResource != null)
            {
                var league = leagues.FirstOrDefault(x => x.Id == filteredSeasonResource.LeagueId);
                if (league != null)
                {
                    return new SeasonResourceByTeamDto(league.Id, league.OfficalName, league.Season,
                        league.ShortName, filteredSeasonResource.TeamName, league.BaseUrl);
                }
            }
            return Maybe<SeasonResourceByTeamDto>.None;
        }
    }
}
