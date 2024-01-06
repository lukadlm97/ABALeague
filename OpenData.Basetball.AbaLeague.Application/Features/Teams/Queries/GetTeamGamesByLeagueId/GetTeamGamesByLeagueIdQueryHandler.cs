using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Application.DTOs.Game;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Application.Services.Implementation;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamGamesByLeagueId
{
    public class GetTeamGamesByLeagueIdQueryHandler : 
        IQueryHandler<GetTeamGamesByLeagueIdQuery, Maybe<MatchesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGameService _gameService;

        public GetTeamGamesByLeagueIdQueryHandler(IUnitOfWork unitOfWork, 
            IGameService gameService)
        {
            _unitOfWork = unitOfWork;
            _gameService = gameService;
        }
        public async Task<Maybe<MatchesDto>> 
            Handle(GetTeamGamesByLeagueIdQuery request, CancellationToken cancellationToken)
        {
            (int? teamId,
                 string? teamName,
                 int? leagueId,
                 string? leagueName,
                 IEnumerable<GameStatsByTeamItemDto> games,
                 AdvancedMatchCalcuationDto? advancedMatchCalcuation,
                 AverageBoxscoreCalcuationDto? averageBoxscoreCalcuation) playedGames =
                 await _gameService.GetPlayedByLeagueIdAndTeamId(request.LeagueId, request.TeamId, false, cancellationToken);

            (int? teamId,
             string? teamName,
             int? leagueId,
               string? leagueName,
                 IEnumerable<MatchItemDto> matches) gamesOnSchedule =
                 await _gameService.GetScheduledByLeagueIdAndTeamId(request.LeagueId, request.TeamId, false, cancellationToken);

            if(playedGames.teamId == null || gamesOnSchedule.teamId == null) 
            {
                return Maybe<MatchesDto>.None;
            }
            return new MatchesDto(playedGames.leagueId ?? -1,
                                    playedGames.teamId ?? -1, 
                                    playedGames.leagueName,
                                    playedGames.teamName, 
                                    playedGames.games, 
                                    gamesOnSchedule.matches);
        }
    }
}
