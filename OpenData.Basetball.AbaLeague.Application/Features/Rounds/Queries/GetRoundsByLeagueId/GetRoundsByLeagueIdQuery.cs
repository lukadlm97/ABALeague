using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Round;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rounds.Queries.GetRoundsByLeagueId
{
    public class GetRoundsByLeagueIdQuery : IQuery<Maybe<AvailableRoundsDto>>
    {
        public GetRoundsByLeagueIdQuery(int leagueId)
        {
            LeagueId = leagueId;
        }

        public int LeagueId { get; private set; }
    }
}
