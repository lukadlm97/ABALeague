using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Game;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Match.Queries.GetByMatchId
{
    public class GetByMatchIdQuery : IQuery<Maybe<ScheduleItemDto>>
    {
        public GetByMatchIdQuery(int matchId)
        {
            MatchId = matchId;
        }

        public int MatchId { get; }
    }
}
