using OpenData.Basketball.AbaLeague.Application.DTOs.Round;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore
{
    public record BoxscoreByMatchResultDto(FinishedRoundMatchDto RoundMatchDetails,
                                            FrozenSet<BoxScoreItemDto> HomeTeamItems,
                                            FrozenSet<BoxScoreItemDto> AwayTeamItems);
}
