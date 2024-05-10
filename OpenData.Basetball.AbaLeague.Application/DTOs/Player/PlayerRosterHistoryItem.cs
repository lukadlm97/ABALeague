using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Player
{
    public record PlayerRosterHistoryItem(PlayerItemDto PlayerDetails, 
                                            FrozenSet<RosterItemDto> RosterItems);
}
