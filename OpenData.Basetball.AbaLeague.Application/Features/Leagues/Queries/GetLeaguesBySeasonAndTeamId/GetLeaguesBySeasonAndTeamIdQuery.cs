using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeaguesBySeasonAndTeamId
{
    public class GetLeaguesBySeasonAndTeamIdQuery : IQuery<Maybe<LeagueIdentifiers>>
    {
        public GetLeaguesBySeasonAndTeamIdQuery(int teamId, int seasonId)
        {
            TeamId = teamId;
            SeasonId = seasonId;
        }

        public int TeamId { get; }
        public int SeasonId { get; }
    }
}
