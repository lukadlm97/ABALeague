using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Game;
using OpenData.Basketball.AbaLeague.Application.Utilities;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayersStatByLeague
{
    public class GetPlayersStatByLeagueQueryHandler :
        IQueryHandler<GetPlayersStatByLeagueQuery, Maybe<PlayerStatsByLeagueDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPlayersStatByLeagueQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<PlayerStatsByLeagueDto>>
            Handle(GetPlayersStatByLeagueQuery request, CancellationToken cancellationToken)
        {
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken); 
            var players = await _unitOfWork.PlayerRepository.GetAll(cancellationToken);
            var teams = await _unitOfWork.TeamRepository.GetAll(cancellationToken);
            var rosterItems = await _unitOfWork.RosterRepository.GetAll(cancellationToken);
            var boxscores = await _unitOfWork.BoxScoreRepository.GetAll(cancellationToken);
            if (league == null)
            {
                return Maybe<PlayerStatsByLeagueDto>.None;
            }

            List<PlayerStatsItemByLeagueDto> list = new List<PlayerStatsItemByLeagueDto>();
            foreach(var (playerId, teamId) in rosterItems.Where(x => x.LeagueId == request.LeagueId).Select(x => (x.PlayerId,x.TeamId)))
            {
                var player = players.FirstOrDefault(x => x.Id == playerId);
                if (player == null)
                {
                    continue;
                }

                var team = teams.FirstOrDefault(x => x.Id == teamId);
                if (team == null)
                {
                    continue;
                }

                var rosterItem = rosterItems
                    .FirstOrDefault(x=>x.PlayerId == playerId && x.LeagueId == league.Id && x.TeamId == teamId);
                if(rosterItem == null)
                {
                    continue;
                }

                var selectedBoxscores = boxscores.Where(x => x.RosterItemId == rosterItem.Id);

                var gamesPlayed = selectedBoxscores.Select(x => x.RoundMatchId).Distinct().Count();
                if(gamesPlayed <= 0)
                {
                    continue;
                }
                list.Add(new PlayerStatsItemByLeagueDto(new DTOs.Player.PlayerItemDto(player.Id,
                    player.Name, 
                    player.PositionEnum,
                    player.Height, 
                    player.DateOfBirth,
                    DistanceCalculator
                .CalculateAge(DateOnly.FromDateTime(player.DateOfBirth), DateOnly.FromDateTime(DateTime.UtcNow)),
                    player.CountryId,
                    player.CountryId.ToString()
     ),
                    team.Id,
                    team.Name,
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.Points) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.ShotMade2Pt)/ gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.ShotAttempted2Pt) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.ShotMade3Pt) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.ShotAttempted3Pt) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.ShotMade1Pt) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.ShotAttempted1Pt) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.DefensiveRebounds) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.OffensiveRebounds) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.TotalRebounds) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.Assists) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.Steals) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.Turnover) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.InFavoureOfBlock) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.AgainstBlock) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.CommittedFoul) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.ReceivedFoul) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.PointFromPain) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.PointFrom2ndChance) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.PointFromFastBreak) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.PlusMinus) / gamesPlayed, 2),
                    Math.Round((decimal)selectedBoxscores.Sum(x => x.RankValue) / gamesPlayed, 2),
                    selectedBoxscores.Select(x => x.Minutes ?? TimeSpan.FromSeconds(0)).GetTimeSpanSum() / gamesPlayed
                    ));
            }

            return new PlayerStatsByLeagueDto(league.Id, league.OfficalName, list.OrderByDescending(x=>x.Points)
                                                                               .ThenBy(x=>x.ShotAttempted2Pt)
                                                                               .ThenBy(x=>x.ShotAttempted1Pt)
                                                                               .ThenBy(x=>x.ShotAttempted3Pt)
                                                                               );
        }
    }
}
