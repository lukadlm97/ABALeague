using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.StatsProperties
{
    public record StatsPropertiesDto(IEnumerable<StatsPropertyItemDto> PropertyItems);
}
