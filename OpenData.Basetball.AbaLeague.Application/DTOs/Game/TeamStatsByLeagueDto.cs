using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Game
{
    public record TeamStatsByLeagueDto(int LeagueId,
                                        string LeagueName, 
                                        IEnumerable<StatsItemByLeagueDto> StatItems);
}
