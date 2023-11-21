using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterHistoryByPlayer
{
    public class GetRosterHistoryByPlayerQuery : IQuery<Maybe<IEnumerable<SeasonResourceByTeamDto>>>
    {
        public GetRosterHistoryByPlayerQuery(int playerId)
        {
            PlayerId = playerId;
        }
        public int PlayerId { get; set; }
    }
}
