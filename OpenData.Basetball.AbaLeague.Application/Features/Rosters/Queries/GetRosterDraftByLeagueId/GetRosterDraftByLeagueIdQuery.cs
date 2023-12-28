using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterDraftByLeagueId
{
    public class GetRosterDraftByLeagueIdQuery : IQuery<Maybe<DraftRosterDto>>
    {
        public GetRosterDraftByLeagueIdQuery(int leagueId)
        {
            LeagueId = leagueId;
        }

        public int LeagueId { get; private set; }
    }
}
