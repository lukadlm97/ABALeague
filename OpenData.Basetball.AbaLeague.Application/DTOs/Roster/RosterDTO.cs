using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Roster
{
    public record RosterDTO(IEnumerable<(string, IEnumerable<RosterItemDTO>)> Rosters);

    public record RosterItemDTO(int PlayerId, string Name, decimal Height, DateTime DateTime);
}
