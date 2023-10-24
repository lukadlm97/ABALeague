﻿using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetTeamsByLeagueId
{
    public class GetTeamsByLeagueIdQuery : IQuery<Maybe<IEnumerable<TeamDTO>>>
    {
        public int LeagueId { get; }
        public GetTeamsByLeagueIdQuery(int leagueId) => LeagueId = leagueId;
    }
}
