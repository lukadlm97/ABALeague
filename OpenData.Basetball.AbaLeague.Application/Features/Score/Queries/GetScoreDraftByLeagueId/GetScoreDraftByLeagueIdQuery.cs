using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Score;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Score.Queries.GetScoreDraftByLeagueId
{
    public class GetScoreDraftByLeagueIdQuery : IQuery<Maybe<ScoreDto>>
    {
        public  int LeagueId { get; init; }
        public GetScoreDraftByLeagueIdQuery(int leagueId)
        {
            LeagueId = leagueId;
        }
    }
}
