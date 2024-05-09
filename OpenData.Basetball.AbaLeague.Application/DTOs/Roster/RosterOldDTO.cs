using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Roster
{
    public record RosterOldDTO(IEnumerable<(string, IEnumerable<RosterItemOldDTO>)> Rosters);

    public record RosterItemOldDTO(int PlayerId, string Name, decimal Height, DateTime DateTime);
}
