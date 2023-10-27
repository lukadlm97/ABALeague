using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Enums;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetTeamsByLeagueId
{
    public class GetTeamsByLeagueIdQuery : IQuery<Maybe<IEnumerable<TeamDTO>>>
    {
        public int LeagueId { get; }
        public ProcessorType ProcessorType { get; }
        public GetTeamsByLeagueIdQuery(int leagueId, ProcessorType processorType = ProcessorType.Aba)
        {
            LeagueId = leagueId;
            ProcessorType = processorType;
        }
    }
}
