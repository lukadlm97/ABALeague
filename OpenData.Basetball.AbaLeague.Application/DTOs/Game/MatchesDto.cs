using OpenData.Basketball.AbaLeague.Application.DTOs.LeagueTeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Game
{
    public record MatchesDto(int LeagueId, 
                                int TeamId,
                                string LeagueName,
                                string TeamName, 
                                IEnumerable<GameStatsByTeamItemDto> GameStatsItems,
                                IEnumerable<MatchItemDto> MatchItems) 
                                : LeagueTeamDto(LeagueId, 
                                                LeagueName, 
                                                TeamId, 
                                                TeamName);
}
