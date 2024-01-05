using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.SeasonResources.Queries.GetSeasonResourcesByTeam
{
    public class GetSeasonResourcesByTeamQueryHandler : IQueryHandler<GetSeasonResourcesByTeamQuery, Maybe<IEnumerable<SeasonResourceByTeamDto>>>
    {
        private IUnitOfWork _unitOfWork;

        public GetSeasonResourcesByTeamQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<IEnumerable<SeasonResourceByTeamDto>>> Handle(GetSeasonResourcesByTeamQuery request, CancellationToken cancellationToken)
        {
            var leagues = await _unitOfWork.LeagueRepository.GetAll(cancellationToken);
            var seasonResources = await _unitOfWork.SeasonResourcesRepository.GetAll(cancellationToken);

            var filteredSeasonResources = seasonResources.Where(x => x.TeamId == request.TeamId);

            List<SeasonResourceByTeamDto> availableResources = new List<SeasonResourceByTeamDto>();

            foreach(var resource in filteredSeasonResources)
            {
                var league = leagues.FirstOrDefault(x => x.Id == resource.LeagueId);
                if(league != null)
                {
                    availableResources.Add(new SeasonResourceByTeamDto(league.Id, league.OfficalName, league.Season,
                        league.ShortName, resource.TeamName, resource.TeamId, league.BaseUrl));
                }
            }

            return availableResources;
        }
    }
}
