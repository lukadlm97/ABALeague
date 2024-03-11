using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueComparisonByLeagueIds
{
    public class GetLeagueComparisonByLeagueIdsQuery : IQuery<Maybe<LeagueCompareDto>>
    {
        public GetLeagueComparisonByLeagueIdsQuery(IReadOnlySet<int> leagueIds)
        {
            LeagueIds = leagueIds;
        }

        public IReadOnlySet<int> LeagueIds { get; }
    }
}
