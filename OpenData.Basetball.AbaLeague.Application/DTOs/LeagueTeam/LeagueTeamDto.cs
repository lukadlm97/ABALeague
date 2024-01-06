using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.LeagueTeam
{
    public record LeagueTeamDto(int LeagueId, string LeagueName, int TeamId, string TeamName);
}
