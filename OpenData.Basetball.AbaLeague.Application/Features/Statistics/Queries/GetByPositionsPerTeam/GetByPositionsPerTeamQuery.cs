using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Statistic;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Statistics.Queries.GetByPositionsPerTeam
{
    public class GetByPositionsPerTeamQuery : IQuery<Maybe<CategoriesByPositionPerTeamDto>>
    {
        public GetByPositionsPerTeamQuery(int teamId, int leagueId)
        {
            TeamId = teamId;
            LeagueId = leagueId;
        }
        public int TeamId { get; init; }
        public int LeagueId { get; init; }
    }
}
