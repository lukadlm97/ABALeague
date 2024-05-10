using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Roster
{
    public record RosterItemDto(int PlayerId, string PlayerName, 
        int TeamId, string TeamName, 
        int LeagueId, string LeagueName);
}
