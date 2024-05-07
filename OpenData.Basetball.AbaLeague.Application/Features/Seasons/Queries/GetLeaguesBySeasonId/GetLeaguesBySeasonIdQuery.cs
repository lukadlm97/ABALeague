using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Seasons.Queries.GetLeaguesBySeasonId
{
    public class GetLeaguesBySeasonIdQuery : IQuery<Maybe<LeaguesDto>>
    {
        public GetLeaguesBySeasonIdQuery(int seasonId)
        {
            SeasonId = seasonId;
        }
        public int SeasonId { get; init; }
    }
}
