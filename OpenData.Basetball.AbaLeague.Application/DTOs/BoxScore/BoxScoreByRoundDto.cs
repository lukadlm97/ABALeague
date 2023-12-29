using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore
{
    public record BoxScoreByRoundDto(string LeagueName, IEnumerable<BoxScoreDto> BoxScoreItems);
}
