
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeams
{
    public class GetTeamsQuery : IQuery<Maybe<TeamDto>>
    {
        public GetTeamsQuery(string filter, int pageNumber, int pageSize)
        {
            Filter = filter;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public string Filter { get;  }
        public int PageNumber { get;  }
        public int PageSize { get;  }
    }
}
