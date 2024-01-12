using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Roster
{
    public record DraftRosterDto(IEnumerable<DraftRosterItemDto> DraftRosterItems, 
                                    IEnumerable<DraftRosterItemDto> ExistingRosterItems,
                                    IEnumerable<PlayerItemDraftDto> MissingPlayers,
                                    IEnumerable<DraftRosterItemDto> DraftRosterItemsWithEndedContract);
}
