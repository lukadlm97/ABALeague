 
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetAvailableRosterHistoryByTeamId
{
    public class GetAvailableRosterHistoryByTeamIdQuery : IQuery<Maybe<AvailableRostersDto>>
    {
        public GetAvailableRosterHistoryByTeamIdQuery(int teamId)
        {
            TeamId = teamId;
        }

        public int TeamId { get; }
    }
}
