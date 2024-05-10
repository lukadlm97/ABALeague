using OpenData.Basetball.AbaLeague.Domain.Enums;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Roster
{
    public record RosterByPositionDto(int TeamId, 
        string TeamName, 
        int LeagueId, 
        string LeagueName,
        FrozenDictionary<PositionEnum, FrozenSet<PlayerRosterItemDto>> RosterEntriesByPositions);
}
