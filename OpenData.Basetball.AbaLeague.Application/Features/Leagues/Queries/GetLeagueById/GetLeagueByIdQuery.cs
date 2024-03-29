﻿using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueById
{
    public sealed class GetLeagueByIdQuery : IQuery<Maybe<LeagueItemDto>>
    {
        public int LeagueId { get; }
        public GetLeagueByIdQuery(int leagueId) => LeagueId = leagueId;
    }
}
