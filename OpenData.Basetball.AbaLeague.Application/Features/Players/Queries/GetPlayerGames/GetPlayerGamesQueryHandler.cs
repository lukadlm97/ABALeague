using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Application.DTOs.Game;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamGamesByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayerGames
{
    public class GetPlayerGamesQueryHandler :
        IQueryHandler<GetPlayerGamesQuery, Maybe<MatchesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGameService _gameService;

        public GetPlayerGamesQueryHandler(IUnitOfWork unitOfWork,
            IGameService gameService)
        {
            _unitOfWork = unitOfWork;
            _gameService = gameService;
        }
        public async Task<Maybe<MatchesDto>>
            Handle(GetPlayerGamesQuery request, CancellationToken cancellationToken)
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
            var rosterItem = (await _unitOfWork.RosterRepository.GetTeamRosterByLeagueId(request.LeagueId, 
                request.TeamId,
                cancellationToken)).FirstOrDefault(x=>x.PlayerId == request.PlayerId);
            if (playedGames.teamId == null || gamesOnSchedule.teamId == null || rosterItem == null)
            {
                return Maybe<MatchesDto>.None;
            }
            List<GameStatsByTeamItemDto> gamesWherePlayerPlayed = new List<GameStatsByTeamItemDto>();

            foreach(var item in playedGames.games)
            {
                if(await _unitOfWork.BoxScoreRepository.Exist(item.MatchRoundId, rosterItem.Id, cancellationToken))
                {
                    gamesWherePlayerPlayed.Add(item);
                }
            }

            return new MatchesDto(playedGames.leagueId ?? -1,
                                    playedGames.teamId ?? -1,
                                    playedGames.leagueName,
                                    playedGames.teamName,
                                    gamesWherePlayerPlayed,
                                    gamesOnSchedule.matches);
        }
    }
}
