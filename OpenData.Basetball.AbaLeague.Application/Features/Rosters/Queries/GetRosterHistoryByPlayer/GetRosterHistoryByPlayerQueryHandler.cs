using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterHistoryByPlayer
{
    internal class GetRosterHistoryByPlayerQueryHandler : IQueryHandler<GetRosterHistoryByPlayerQuery, Maybe<IEnumerable<SeasonResourceByTeamDto>>>
    {
        private IUnitOfWork _unitOfWork;

        public GetRosterHistoryByPlayerQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<IEnumerable<SeasonResourceByTeamDto>>> Handle(GetRosterHistoryByPlayerQuery request, CancellationToken cancellationToken)
        {
            var leagues = await _unitOfWork.LeagueRepository.GetAll(cancellationToken);
            var teams = await _unitOfWork.TeamRepository.GetAll(cancellationToken);
            var seasonResources = await _unitOfWork.SeasonResourcesRepository.GetAll(cancellationToken);
            var rosterItems = await _unitOfWork.RosterRepository.GetAll(cancellationToken);

            var filteredRosterItems = rosterItems.Where(x=>x.PlayerId == request.PlayerId);


            List<SeasonResourceByTeamDto> list = new List<SeasonResourceByTeamDto>();

            foreach (var item in filteredRosterItems)
            {
                var selectedLeague = leagues.FirstOrDefault(x=>x.Id == item.LeagueId);
                var  selectedTeam = teams.FirstOrDefault(x=>x.Id == item.TeamId);
                var selectedSeasonResources = seasonResources.FirstOrDefault(x=>x.LeagueId == item.LeagueId && x.TeamId == item.TeamId);

                list.Add(new SeasonResourceByTeamDto(selectedLeague.Id, 
                    selectedLeague.OfficalName,  selectedLeague.ShortName,
                    selectedTeam.Name, selectedTeam.Id, selectedSeasonResources.TeamUrl
                ));
            }
            
            return list;
        }
    }
}
