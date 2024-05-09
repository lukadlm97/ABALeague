
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetExistingRostersByTeam
{
    public class GetExistingLeagueIdRostersByTeamQuery:IQuery<Maybe<LeaguesRoster>>
    {
        public GetExistingLeagueIdRostersByTeamQuery(int teamId)
        {
            TeamId = teamId;
        }

        public int TeamId { get;  }
    }
}
