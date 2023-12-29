using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Player
{
    public record AnotherNameDto(IEnumerable<AnotherNameItemDto> ExistingNames, string OriginalName);
}
