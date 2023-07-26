using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Roster
{

    public record AddRosterItemDto(int PlayerId, int LeagueId,int TeamId);
}
