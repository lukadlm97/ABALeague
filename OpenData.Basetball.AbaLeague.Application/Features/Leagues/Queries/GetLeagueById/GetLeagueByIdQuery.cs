using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.Contracts.Leagues;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueById
{
    public sealed class GetLeagueByIdQuery:IQuery<Maybe<LeagueResponse>>
    {
        public int LeagueId { get; }
        public GetLeagueByIdQuery(int leagueId) => LeagueId = leagueId;
    }
}
