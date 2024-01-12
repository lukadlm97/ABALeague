using System;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues
{
    public sealed class GetLeagueQuery : IQuery<Maybe<LeaguesDto>>
    {

    }
}
