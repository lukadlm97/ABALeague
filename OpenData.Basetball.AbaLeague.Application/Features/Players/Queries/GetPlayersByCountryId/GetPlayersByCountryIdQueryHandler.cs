using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Game;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basketball.AbaLeague.Application.Utilities;
using Microsoft.Extensions.Options;
using OpenData.Basketball.AbaLeague.Application.Configurations.Players;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayersByCountryId
{
    public class GetPlayersByCountryIdQueryHandler
        : IQueryHandler<GetPlayersByCountryIdQuery, Maybe<PlayerItemsByCountryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PlayerSettings _playerSettings;

        public GetPlayersByCountryIdQueryHandler(IUnitOfWork unitOfWork,
            IOptions<PlayerSettings> options)
        {
            _unitOfWork = unitOfWork;
            _playerSettings = options.Value;
        }
        public async Task<Maybe<PlayerItemsByCountryDto>>
            Handle(GetPlayersByCountryIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var country = _unitOfWork.CountryRepository
                                            .Get()
                                            .FirstOrDefault(x => x.Id == request.CountryId);
                if (country == null)
                {
                    return Maybe<PlayerItemsByCountryDto>.None;
                }

                List<PlayerStatsItemByLeagueDto> list = new List<PlayerStatsItemByLeagueDto>();
                var players = _unitOfWork.PlayerRepository.Get()
                                            .Where(x => x.CountryId == request.CountryId)
                                            .ToList();
                var teams = await _unitOfWork.TeamRepository.GetAll(cancellationToken);
                var season = _unitOfWork.SeasonRepository.Get().FirstOrDefault(x => x.Name == "2023/24");
                if(season == null)
                {
                    return Maybe<PlayerItemsByCountryDto>.None;
                } 
                var leaguesBySeasonIds = _unitOfWork.LeagueRepository.Get()
                    .Where(x => x.SeasonId == season.Id)
                    .Select(x=>x.Id);
                var referentDate = DateTime.Now.Add(-_playerSettings.MaxTimeElapsedFromPerformance);
                foreach (var player in players)
                {
                    var latestRosterItem = _unitOfWork.RosterRepository.Get()
                        .Where(x => x.PlayerId == player.Id)
                        .OrderByDescending(x => x.DateOfInsertion)
                        .FirstOrDefault();

                    if (latestRosterItem == null || 
                        !leaguesBySeasonIds.Contains(latestRosterItem.LeagueId))
                    {
                        continue;
                    }
                    var teamRosterItem = teams.FirstOrDefault(x => x.Id == latestRosterItem.TeamId);
                    if (teamRosterItem == null)
                    {
                        continue;
                    }
                    var latestBoxscore = _unitOfWork.BoxScoreRepository
                        .GetWithRoundMatchIncluded()
                        .OrderBy(x => x.RoundMatchId)
                        .LastOrDefault(x => x.RosterItemId == latestRosterItem.Id);
                    if (latestBoxscore == null || 
                        latestBoxscore.RoundMatch.DateTime <= referentDate||
                        latestBoxscore.Minutes == default(TimeSpan))
                    {
                        continue;
                    }

                    list.Add(new PlayerStatsItemByLeagueDto(new PlayerItemDto(player.Id,
                                                                                player.Name, 
                                                                                player.PositionEnum,
                                                                                player.Height, 
                                                                                player.DateOfBirth,
                                                                                DistanceCalculator
                .CalculateAge(DateOnly.FromDateTime(player.DateOfBirth), DateOnly.FromDateTime(DateTime.UtcNow)), 
                                                                                player.CountryId,
                                                                                string.Empty),
                                                            teamRosterItem.Id,
                                                            teamRosterItem.Name,
                                                            latestBoxscore.Points,
                                                            latestBoxscore.ShotMade2Pt,
                                                            latestBoxscore.ShotAttempted2Pt,
                                                            latestBoxscore.ShotMade3Pt,
                                                            latestBoxscore.ShotAttempted3Pt,
                                                            latestBoxscore.ShotMade1Pt,
                                                            latestBoxscore.ShotAttempted1Pt,
                                                            latestBoxscore.DefensiveRebounds,
                                                            latestBoxscore.OffensiveRebounds,
                                                            latestBoxscore.TotalRebounds,
                                                            latestBoxscore.Assists,
                                                            latestBoxscore.Steals,
                                                            latestBoxscore.Turnover,
                                                            latestBoxscore.InFavoureOfBlock,
                                                            latestBoxscore.AgainstBlock,
                                                            latestBoxscore.CommittedFoul,
                                                            latestBoxscore.ReceivedFoul,
                                                            latestBoxscore.PointFromPain,
                                                            latestBoxscore.PointFrom2ndChance,
                                                            latestBoxscore.PointFromFastBreak,
                                                            latestBoxscore.PlusMinus,
                                                            latestBoxscore.RankValue,
                                                            latestBoxscore.Minutes,
                                                            latestBoxscore.RoundMatch.DateTime));
                }

                return new PlayerItemsByCountryDto(country.Id, country.Name, list.ToFrozenSet());
            }
            catch(Exception ex)

            {
                return Maybe<PlayerItemsByCountryDto>.None;
            }
           

        }
    }
}
