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

namespace OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreByTeamIdAndLeagueId
{
    public class GetBoxscoreByTeamIdAndLeagueIdQueryHandler :
        IQueryHandler<GetBoxscoreByTeamIdAndLeagueIdQuery, Maybe<BoxscoreByTeamAndLeagueDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGameService _gameService;

        public GetBoxscoreByTeamIdAndLeagueIdQueryHandler(IUnitOfWork unitOfWork, IGameService gameService)
        {
            _unitOfWork = unitOfWork;
            _gameService = gameService;
        }
        public async Task<Maybe<BoxscoreByTeamAndLeagueDto>> 
            Handle(GetBoxscoreByTeamIdAndLeagueIdQuery request, CancellationToken cancellationToken)
        {
            (int? teamId,
                 string? teamName,
                 int? leagueId,
                 string? leagueName,
                 IEnumerable<GameStatsByTeamItemDto> games,
                 AdvancedMatchCalcuationDto? advancedMatchCalcuation,
                 AverageBoxscoreCalcuationDto? averageBoxscoreCalcuation) result = 
                 await _gameService.GetPlayedByLeagueIdAndTeamId(request.LeagueId, request.TeamId, true, cancellationToken);


            if(result.teamId == null)
            {
                return Maybe<BoxscoreByTeamAndLeagueDto>.None;
            }

            return new BoxscoreByTeamAndLeagueDto(result.teamId ?? 0, 
                                                    result.teamName,
                                                    result.leagueId ?? 0, 
                                                    result.leagueName, 
                                                    result.games, 
                                                    result.averageBoxscoreCalcuation, 
                                                    result.advancedMatchCalcuation);
        }
    }
}
