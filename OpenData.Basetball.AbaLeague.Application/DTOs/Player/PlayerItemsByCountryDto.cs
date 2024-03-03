using OpenData.Basketball.AbaLeague.Application.DTOs.Game;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Player
{
    public record PlayerItemsByCountryDto(int CountryId,
                                            string CountryName, 
                                            FrozenSet<PlayerStatsItemByLeagueDto> Players);
}
