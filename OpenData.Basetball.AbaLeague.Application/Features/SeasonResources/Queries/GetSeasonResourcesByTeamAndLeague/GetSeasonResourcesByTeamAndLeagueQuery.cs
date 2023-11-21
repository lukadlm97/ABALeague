using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.SeasonResources.Queries.GetSeasonResourcesByTeamAndLeague
{
    public class GetSeasonResourcesByTeamAndLeagueQuery : IQuery<Maybe<SeasonResourceByTeamDto>>
    {
        public GetSeasonResourcesByTeamAndLeagueQuery(int teamId, int leagueId)
        {
            TeamId = teamId;
            LeagueId = leagueId;
        }
        public int TeamId { get; }
        public int LeagueId { get; }
    }
}
