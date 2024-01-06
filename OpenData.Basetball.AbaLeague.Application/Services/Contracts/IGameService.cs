using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Application.DTOs.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface IGameService
    {
         Task<(int? teamId,
                string? teamName,
                int? leagueId,
                string? leagueName,
                IEnumerable<GameStatsByTeamItemDto> games,
                AdvancedMatchCalcuationDto? advancedMatchCalcuation,
                AverageBoxscoreCalcuationDto? averageBoxscoreCalcuation)>
            GetPlayedByLeagueIdAndTeamId(int leagueId,
                                    int teamId,
                                    bool includeAdvancedCalculation = false,
                                    CancellationToken cancellationToken = default);
        Task<(int? teamId,
                string? teamName,
                int? leagueId,
                string? leagueName, 
            IEnumerable<MatchItemDto> games)> GetScheduledByLeagueIdAndTeamId(int leagueId,
                                                            int teamId,
                                                            bool includePlayed = false,
                                                            CancellationToken cancellationToken = default);
     }
}
