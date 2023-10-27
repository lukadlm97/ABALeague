
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetExistingRostersByTeam
{
    public class GetExistingRostersByTeamQuery:IQuery<Maybe<LeaguesRoster>>
    {
        public GetExistingRostersByTeamQuery(int teamId)
        {
            TeamId = teamId;
        }

        public int TeamId { get;  }
    }
}
