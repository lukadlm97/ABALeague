using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterHistoryByTeam
{
    public class GetRosterHistoryByTeamQueryHandler : IQueryHandler<GetRosterHistoryByTeamQuery, Maybe<RosterOldDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRosterHistoryByTeamQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<RosterOldDTO>> Handle(GetRosterHistoryByTeamQuery request, CancellationToken cancellationToken)
        {
            if (request.TeamId <= 0 || request.LeagueId <= 0)
            {
                return Maybe<RosterOldDTO>.None;
            }

            var seasonResources = await _unitOfWork.SeasonResourcesRepository.GetAll(cancellationToken);
            var players = await _unitOfWork.PlayerRepository.GetAll(cancellationToken);
            var leagues = await _unitOfWork.LeagueRepository.GetAll(cancellationToken);
            var fullRosterItems= await _unitOfWork.RosterRepository.GetAll(cancellationToken);
            var selectedRosterItems =
                fullRosterItems.Where(x => x.TeamId == request.TeamId && x.LeagueId ==  request.LeagueId);
            Dictionary<string, List<RosterItemOldDTO>> rosterDictionary = new Dictionary<string, List<RosterItemOldDTO>>();
            foreach (var item in selectedRosterItems)
            {
                var league = leagues.FirstOrDefault(x => x.Id == item.LeagueId);
                var leagueName = league.OfficalName ;
                var player = players.FirstOrDefault(x => x.Id == item.PlayerId);
                if (rosterDictionary.ContainsKey(leagueName))
                {
                    rosterDictionary[leagueName].Add(new RosterItemOldDTO(item.PlayerId, player.Name, player.Height, player.DateOfBirth));
                }
                else
                {
                    rosterDictionary[leagueName] = new List<RosterItemOldDTO>();
                }
            }

            RosterOldDTO roster = new RosterOldDTO(rosterDictionary.Select(x =>
                (x.Key, x.Value.Select(x => new RosterItemOldDTO(x.PlayerId, x.Name, x.Height, x.DateTime)))));

            return roster;
        }
    }
}
