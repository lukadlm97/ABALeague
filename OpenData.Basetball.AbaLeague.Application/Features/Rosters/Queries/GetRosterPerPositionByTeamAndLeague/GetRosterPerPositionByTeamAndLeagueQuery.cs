using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterPerPositionByTeamAndLeague
{
    public class GetRosterPerPositionByTeamAndLeagueQuery : IQuery<Maybe<RosterByPositionDto>>
    {
        public GetRosterPerPositionByTeamAndLeagueQuery(int teamId, int leagueId)
        {
            TeamId = teamId;
            LeagueId = leagueId;
        }

        public int TeamId { get; }
        public int LeagueId { get; }
    }
}
