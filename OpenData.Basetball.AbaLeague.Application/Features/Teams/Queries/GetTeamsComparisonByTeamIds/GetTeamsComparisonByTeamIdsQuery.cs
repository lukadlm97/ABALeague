using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamsComparisonByTeamIds
{
    public class GetTeamsComparisonByTeamIdsQuery : IQuery<Maybe<TeamCompareDto>>
    {
        public GetTeamsComparisonByTeamIdsQuery(IReadOnlySet<(int teamId, int leagueId)> teamAndLeagueIds)
        {
            TeamAndLeagueIds = teamAndLeagueIds;
        }

        public IReadOnlySet<(int teamId, int leagueId)> TeamAndLeagueIds { get; }
    }
}
