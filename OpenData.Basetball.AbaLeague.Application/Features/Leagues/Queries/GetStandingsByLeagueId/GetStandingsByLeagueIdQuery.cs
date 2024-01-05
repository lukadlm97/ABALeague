using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetStandingsByLeagueId
{
    public class GetStandingsByLeagueIdQuery : IQuery<Maybe<StandingsDto>>
    {
        public GetStandingsByLeagueIdQuery(int leagueId)
        {
            LeagueId = leagueId;
        }

        public int LeagueId { get; private set; }
    }
}
