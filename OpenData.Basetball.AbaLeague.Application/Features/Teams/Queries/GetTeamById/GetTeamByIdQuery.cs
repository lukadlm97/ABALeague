using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamById
{
    public class GetTeamByIdQuery : IQuery<Maybe<TeamDto>>
    {
        public GetTeamByIdQuery(int id)
        {
            TeamId = id;
        }

        public int TeamId { get;  }
    }
}
