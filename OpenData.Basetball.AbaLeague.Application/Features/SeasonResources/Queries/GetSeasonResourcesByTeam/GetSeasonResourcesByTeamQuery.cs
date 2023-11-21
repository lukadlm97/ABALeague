using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.SeasonResources.Queries.GetSeasonResourcesByTeam
{
    public class GetSeasonResourcesByTeamQuery : IQuery<Maybe<IEnumerable<SeasonResourceByTeamDto>>>
    {
        public GetSeasonResourcesByTeamQuery(int teamId)
        {
            TeamId = teamId;
        }
        public int TeamId { get; }
    }
}
