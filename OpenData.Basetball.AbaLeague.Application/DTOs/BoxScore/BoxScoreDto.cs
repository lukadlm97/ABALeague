using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore
{
    public record BoxScoreDto(IEnumerable<BoxScoreItemDto> DraftItems, 
                                IEnumerable<BoxScoreItemDto> ExistingItems,
                                IEnumerable<DraftRosterItemDto> DraftRosterItems,
                                IEnumerable<string> MissingPlayers);
}
