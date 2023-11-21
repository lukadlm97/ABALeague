using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterHistoryByTeam
{
    public class GetRosterHistoryByTeamQueryHandler : IQueryHandler<GetRosterHistoryByTeamQuery, Maybe<RosterDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRosterHistoryByTeamQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<RosterDTO>> Handle(GetRosterHistoryByTeamQuery request, CancellationToken cancellationToken)
        {
            if (request.TeamId <= 0 || request.LeagueId <= 0)
            {
                return Maybe<RosterDTO>.None;
            }

            var seasonResources = await _unitOfWork.SeasonResourcesRepository.GetAll(cancellationToken);
            var players = await _unitOfWork.PlayerRepository.GetAll(cancellationToken);
            var leagues = await _unitOfWork.LeagueRepository.GetAll(cancellationToken);
            var fullRosterItems= await _unitOfWork.RosterRepository.GetAll(cancellationToken);
            var selectedRosterItems =
                fullRosterItems.Where(x => x.TeamId == request.TeamId && x.LeagueId ==  request.LeagueId);
            Dictionary<string, List<RosterItemDTO>> rosterDictionary = new Dictionary<string, List<RosterItemDTO>>();
            foreach (var item in selectedRosterItems)
            {
                var league = leagues.FirstOrDefault(x => x.Id == item.LeagueId);
                var leagueName = league.OfficalName + " - " + league.Season;
                var player = players.FirstOrDefault(x => x.Id == item.PlayerId);
                if (rosterDictionary.ContainsKey(leagueName))
                {
                    rosterDictionary[leagueName].Add(new RosterItemDTO(item.PlayerId, player.Name, player.Height, player.DateOfBirth));
                }
                else
                {
                    rosterDictionary[leagueName] = new List<RosterItemDTO>();
                }
            }

            RosterDTO roster = new RosterDTO(rosterDictionary.Select(x =>
                (x.Key, x.Value.Select(x => new RosterItemDTO(x.PlayerId, x.Name, x.Height, x.DateTime)))));

            return roster;
        }
    }
}
