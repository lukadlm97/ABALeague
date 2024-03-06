using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Statistic;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Statistics.Queries.GetByPositionsPerLeague
{
    public class GetByPositionsPerLeagueQuery : IQuery<Maybe<CategoriesByPositionPerLeagueDto>>
    {
        public GetByPositionsPerLeagueQuery(int leagueId)
        {
            LeagueId = leagueId;
        }

        public int LeagueId { get; }
    }
}
