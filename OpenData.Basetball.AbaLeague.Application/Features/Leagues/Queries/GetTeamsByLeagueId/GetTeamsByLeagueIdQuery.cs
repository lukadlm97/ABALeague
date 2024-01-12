using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Enums;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetTeamsByLeagueId
{
    public class GetTeamsByLeagueIdQuery : IQuery<Maybe<TeamSeasonResourceDto>>
    {
        public int LeagueId { get; }
        public GetTeamsByLeagueIdQuery(int leagueId)
        {
            LeagueId = leagueId;
        }
    }
}
