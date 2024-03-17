using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.Constants;
using OpenData.Basketball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.DTOs.Score;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Score.Queries.GetScoreDraftByLeagueId
{
    public class GetScoreDraftByLeagueIdQuery : IQuery<Maybe<ScoreDraftDto>>, ICacheableMediatrQuery
    {
        public  int LeagueId { get; init; }

        public bool BypassCache => false;

        public string CacheKey { get; init; }

        public TimeSpan? SlidingExpiration => TimeSpan.FromSeconds(90);

        public GetScoreDraftByLeagueIdQuery(int leagueId)
        {
            LeagueId = leagueId;
            CacheKey = string.Format(CacheKeyConstants.ScoresByLeagueId, leagueId);
        }
    }
}
