using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Roster
{
    public record RemoveRosterItemDto(int PlayerId, int TeamId, int LeagueId);
}
