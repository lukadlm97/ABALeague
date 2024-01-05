using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.League
{
    public record StandingsDto(int LeagueId,
                                string LeagueName,
                                int TotalRounds,
                                int PlayedRounds,
                                IEnumerable<StandingsItemDto> StandingItems);
}
