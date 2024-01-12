using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterHistoryByTeam
{
    public class GetRosterHistoryByTeamQuery:IQuery<Maybe<RosterOldDTO>>
    {
        public GetRosterHistoryByTeamQuery(int teamId, int leagueId)
        {
            TeamId = teamId;
            LeagueId = leagueId;
        }

        public int TeamId { get;  }
        public int LeagueId { get; set; }
    }
}
