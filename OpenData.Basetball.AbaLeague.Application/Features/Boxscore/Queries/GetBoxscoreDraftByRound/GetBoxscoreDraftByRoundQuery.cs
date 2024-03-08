using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreDraftByRound
{
    public class GetBoxscoreDraftByRoundQuery : IQuery<Maybe<BoxScoreByRoundDto>>,
        ICacheableMediatrQuery
    {
        public GetBoxscoreDraftByRoundQuery(int leagueId, int round)
        {
            LeagueId = leagueId;
            Round = round;
        }

        public int LeagueId { get; }
        public int Round { get; private set; }

        public bool BypassCache => false;

        public string CacheKey => $"BoxscoreByRound{Round}ByLeagueId{LeagueId}";

        public TimeSpan? SlidingExpiration => TimeSpan.FromSeconds(90);
    }
}
