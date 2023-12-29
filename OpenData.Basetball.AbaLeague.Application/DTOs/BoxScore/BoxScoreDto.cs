using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Application.DTOs.Score;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore
{
    public record BoxScoreDto(ScoreItemDto MatchScore,
                                IEnumerable<BoxScoreItemDto> DraftItems, 
                                IEnumerable<BoxScoreItemDto> ExistingItems,
                                IEnumerable<DraftRosterItemDto> DraftRosterItems,
                                IEnumerable<string> MissingPlayers);
}
