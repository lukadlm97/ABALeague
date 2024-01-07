using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Game
{
    public
        record PlayerStatsByLeagueDto(int LeagueId,
                                        string LeagueName,
                                        IEnumerable<PlayerStatsItemByLeagueDto> StatItems);
}
