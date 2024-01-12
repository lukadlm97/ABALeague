using Microsoft.Extensions.Logging;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Implementation;
using OpenData.Basetball.AbaLeague.Crawler.Models;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreDraftByRound
{
    public class GetBoxscoreDraftByRoundQueryHandler :
        IQueryHandler<GetBoxscoreDraftByRoundQuery, Maybe<BoxScoreByRoundDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentFetcher _documentFetcher;
        private readonly ILoggerFactory _loggerFactory;

        public GetBoxscoreDraftByRoundQueryHandler(IUnitOfWork unitOfWork, 
                                                    IDocumentFetcher documentFetcher, 
                                                    ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;
            _documentFetcher = documentFetcher;
            _loggerFactory = loggerFactory;
        }
        public async Task<Maybe<BoxScoreByRoundDto>> 
            Handle(GetBoxscoreDraftByRoundQuery request, CancellationToken cancellationToken)
        {
            var teams = await _unitOfWork.TeamRepository.GetAll(cancellationToken);
            var players = await _unitOfWork.PlayerRepository.GetAll(cancellationToken);
            var rosterItems = await _unitOfWork.RosterRepository.GetAll(cancellationToken);
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            var roundMatches = await _unitOfWork.CalendarRepository.SearchByLeague(request.LeagueId, cancellationToken);
            if (league == null || roundMatches == null || teams == null || players == null || rosterItems == null)
            {
                return Maybe<BoxScoreByRoundDto>.None;
            }

            roundMatches = roundMatches.Where(x=>x.Round == request.Round);
            if(!roundMatches.Any() || !teams.Any()|| !players.Any()  || !rosterItems.Any() )
            {
                return Maybe<BoxScoreByRoundDto>.None;
            }
            List<BoxScoreDto> boxscoresByRound = new List<BoxScoreDto>();
            foreach(var item in roundMatches)
            {
                if(item == null)
                {
                    continue;
                }
             
                var results = await _unitOfWork.ResultRepository.GetAll(cancellationToken);
                var result = results.FirstOrDefault(x => x.RoundMatchId == item.Id);
               
                if(result == null)
                {
                    continue;
                }

                var homeTeam = await _unitOfWork.TeamRepository.Get(item.HomeTeamId ?? 0, cancellationToken);
                var awayTeam = await _unitOfWork.TeamRepository.Get(item.AwayTeamId ?? 0, cancellationToken);
                if (homeTeam == null || awayTeam == null)
                {
                    continue;
                }

                IWebPageProcessor? processor = league.ProcessorTypeEnum switch
                {
                    Domain.Enums.ProcessorType.Euro => new EuroPageProcessor(_documentFetcher, _loggerFactory),
                    Domain.Enums.ProcessorType.Aba => new WebPageProcessor(_documentFetcher, _loggerFactory),
                    Domain.Enums.ProcessorType.Unknow or null or _ => null
                };
                if (processor is null)
                {
                    return Maybe<BoxScoreByRoundDto>.None;
                }

                (IReadOnlyList<PlayerScore>
                HomeTeam,
                IReadOnlyList<PlayerScore>
                AwayTeam) boxScoreItems = default;
                var rawUrl = string.Empty;
                var url = string.Empty;
                switch (league.ProcessorTypeEnum)
                {
                    case Domain.Enums.ProcessorType.Euro:
                        rawUrl = league.BaseUrl + league.BoxScoreUrl;
                        url = string.Format(rawUrl, item.MatchNo);
                        boxScoreItems = await processor.GetBoxScore(url, cancellationToken);
                        break;
                    case Domain.Enums.ProcessorType.Aba:
                        rawUrl = league.BaseUrl + league.BoxScoreUrl;
                        url = string.Format(rawUrl, item.MatchNo);
                        boxScoreItems = await processor.GetBoxScore(url, cancellationToken);
                        break;
                    default:
                        throw new NotImplementedException();
                        break;
                }

                if (boxScoreItems.HomeTeam == null || boxScoreItems.AwayTeam == null)
                {
                    continue;
                }

                List<BoxScoreItemDto> boxscoreDraftItems = new List<BoxScoreItemDto>();
                List<BoxScoreItemDto> existingBoxScoreItems = new List<BoxScoreItemDto>();
                List<DraftRosterItemDto> draftRosterItems = new List<DraftRosterItemDto>();
                List<string> missingPlayers = new List<string>();

                foreach (var homeTeamItem in boxScoreItems.HomeTeam)
                {
                    var player = players.FirstOrDefault(x => x.Name.ToLower() == homeTeamItem.Name.ToLower());
                    if (player == null)
                    {
                        var existingPlayerWitAnotherName = await _unitOfWork.PlayerRepository
                            .GetAnotherNamePlayerByAnotherName(homeTeamItem.Name, cancellationToken);
                        if (existingPlayerWitAnotherName != null)
                        {
                            player = players.FirstOrDefault(x => x.Id == existingPlayerWitAnotherName.PlayerId);
                        }
                        else
                        {
                            missingPlayers.Add(homeTeamItem.Name);
                            continue;
                        }
                    }

                    if (rosterItems.Any(x => x.TeamId == item.HomeTeamId && 
                                                x.PlayerId == player.Id && 
                                                x.LeagueId == league.Id))
                    {
                        var selectedRosterItem = rosterItems.First(x => x.TeamId == item.HomeTeamId && 
                                                                    x.PlayerId == player.Id && 
                                                                    x.LeagueId == league.Id);

                        if (!await _unitOfWork.BoxScoreRepository.Exist(item.Id,
                                                                        selectedRosterItem.Id,
                                                                        cancellationToken))
                        {
                            boxscoreDraftItems.Add(new BoxScoreItemDto(selectedRosterItem.Id,
                                                                        item.Id, 
                                                                        player.Name,
                                                                        homeTeam.Name, 
                                                                        item.Round,
                                                                        item.MatchNo, 
                                                                        homeTeamItem.Minutes,
                                                                        homeTeamItem.Points,
                                                                        homeTeamItem.ShotPrc,
                                                                        homeTeamItem.ShotMade2Pt,
                                                                        homeTeamItem.ShotAttempted2Pt,
                                                                        homeTeamItem.ShotPrc2Pt,
                                                                        homeTeamItem.ShotMade3Pt,
                                                                        homeTeamItem.ShotAttempted3Pt,
                                                                        homeTeamItem.shotPrc3Pt,
                                                                        homeTeamItem.ShotMade1Pt,
                                                                        homeTeamItem.ShotAttempted1Pt, 
                                                                        homeTeamItem.ShotPrc1Pt, 
                                                                        homeTeamItem.DefensiveRebounds, 
                                                                        homeTeamItem.OffensiveRebounds,
                                                                        homeTeamItem.TotalRebounds, 
                                                                        homeTeamItem.Assists, 
                                                                        homeTeamItem.Steals,
                                                                        homeTeamItem.Turnover,
                                                                        homeTeamItem.InFavoureOfBlock,
                                                                        homeTeamItem.AgainstBlock, 
                                                                        homeTeamItem.CommittedFoul,
                                                                        homeTeamItem.ReceivedFoul, 
                                                                        homeTeamItem.PointFromPain, 
                                                                        homeTeamItem.PointFrom2ndChance, 
                                                                        homeTeamItem.PointFromFastBreak, 
                                                                        homeTeamItem.PlusMinus, 
                                                                        homeTeamItem.RankValue));
                            continue;
                        }
                        existingBoxScoreItems.Add(new BoxScoreItemDto(selectedRosterItem.Id,
                                                                        item.Id, 
                                                                        player.Name,
                                                                        homeTeam.Name, 
                                                                        item.Round, 
                                                                        item.MatchNo,
                                                                        homeTeamItem.Minutes,
                                                                        homeTeamItem.Points, 
                                                                        homeTeamItem.ShotPrc,
                                                                        homeTeamItem.ShotMade2Pt, 
                                                                        homeTeamItem.ShotAttempted2Pt, 
                                                                        homeTeamItem.ShotPrc2Pt,
                                                                        homeTeamItem.ShotMade3Pt, 
                                                                        homeTeamItem.ShotAttempted3Pt,
                                                                        homeTeamItem.shotPrc3Pt, 
                                                                        homeTeamItem.ShotMade1Pt, 
                                                                        homeTeamItem.ShotAttempted1Pt,
                                                                        homeTeamItem.ShotPrc1Pt, 
                                                                        homeTeamItem.DefensiveRebounds,
                                                                        homeTeamItem.OffensiveRebounds,
                                                                        homeTeamItem.TotalRebounds, 
                                                                        homeTeamItem.Assists, 
                                                                        homeTeamItem.Steals, 
                                                                        homeTeamItem.Turnover, 
                                                                        homeTeamItem.InFavoureOfBlock,
                                                                        homeTeamItem.AgainstBlock,
                                                                        homeTeamItem.CommittedFoul,
                                                                        homeTeamItem.ReceivedFoul, 
                                                                        homeTeamItem.PointFromPain, 
                                                                        homeTeamItem.PointFrom2ndChance,
                                                                        homeTeamItem.PointFromFastBreak, 
                                                                        homeTeamItem.PlusMinus, 
                                                                        homeTeamItem.RankValue));
                    }
                    else
                    {
                        draftRosterItems.Add(new DraftRosterItemDto(player.Id, 
                                                                        player.Name, 
                                                                        league.Id,
                                                                        league.OfficalName, 
                                                                        item.HomeTeamId ?? 0, 
                                                                        item.HomeTeam.Name, 
                                                                        DateTime.UtcNow, 
                                                                        null));
                    }
                }

                foreach (var awayTeamItem in boxScoreItems.AwayTeam)
                {
                    var player = players.FirstOrDefault(x => x.Name.ToLower() == awayTeamItem.Name.ToLower());
                    if (player == null)
                    {
                        var existingPlayerWitAnotherName = await _unitOfWork.PlayerRepository
                            .GetAnotherNamePlayerByAnotherName(awayTeamItem.Name, cancellationToken);
                        if (existingPlayerWitAnotherName != null)
                        {
                            player = players.FirstOrDefault(x => x.Id == existingPlayerWitAnotherName.PlayerId);
                        }
                        else
                        {
                            missingPlayers.Add(awayTeamItem.Name);
                            continue;
                        }
                    }

                    if (rosterItems.Any(x => x.TeamId == item.AwayTeamId &&
                                                x.PlayerId == player.Id &&
                                                x.LeagueId == league.Id))
                    {
                        var selectedRosterItem = rosterItems.First(x => x.TeamId == item.AwayTeamId &&
                                                                            x.PlayerId == player.Id &&
                                                                            x.LeagueId == league.Id);

                        if (!await _unitOfWork.BoxScoreRepository.Exist(item.Id,
                                                                         selectedRosterItem.Id,
                                                                         cancellationToken))
                        {
                            boxscoreDraftItems.Add(new BoxScoreItemDto(selectedRosterItem.Id,
                                item.Id, 
                                player.Name, 
                                awayTeam.Name,
                                item.Round, 
                                item.MatchNo, 
                                awayTeamItem.Minutes,
                                awayTeamItem.Points,
                                awayTeamItem.ShotPrc,
                                awayTeamItem.ShotMade2Pt, 
                                awayTeamItem.ShotAttempted2Pt,
                                awayTeamItem.ShotPrc2Pt,
                                awayTeamItem.ShotMade3Pt, 
                                awayTeamItem.ShotAttempted3Pt,
                                awayTeamItem.shotPrc3Pt, 
                                awayTeamItem.ShotMade1Pt,
                                awayTeamItem.ShotAttempted1Pt, 
                                awayTeamItem.ShotPrc1Pt, 
                                awayTeamItem.DefensiveRebounds,
                                awayTeamItem.OffensiveRebounds,
                                awayTeamItem.TotalRebounds, 
                                awayTeamItem.Assists, 
                                awayTeamItem.Steals, 
                                awayTeamItem.Turnover, 
                                awayTeamItem.InFavoureOfBlock, 
                                awayTeamItem.AgainstBlock, 
                                awayTeamItem.CommittedFoul, 
                                awayTeamItem.ReceivedFoul,
                                awayTeamItem.PointFromPain, 
                                awayTeamItem.PointFrom2ndChance,
                                awayTeamItem.PointFromFastBreak, 
                                awayTeamItem.PlusMinus, 
                                awayTeamItem.RankValue));
                            continue;
                        }
                        existingBoxScoreItems.Add(new BoxScoreItemDto(selectedRosterItem.Id,
                                                                        item.Id, 
                                                                        player.Name,
                                                                        awayTeam.Name, 
                                                                        item.Round, 
                                                                        item.MatchNo,
                                                                        awayTeamItem.Minutes,
                                                                        awayTeamItem.Points, 
                                                                        awayTeamItem.ShotPrc, 
                                                                        awayTeamItem.ShotMade2Pt,
                                                                        awayTeamItem.ShotAttempted2Pt,
                                                                        awayTeamItem.ShotPrc2Pt,
                                                                        awayTeamItem.ShotMade3Pt,
                                                                        awayTeamItem.ShotAttempted3Pt, 
                                                                        awayTeamItem.shotPrc3Pt, 
                                                                        awayTeamItem.ShotMade1Pt, 
                                                                        awayTeamItem.ShotAttempted1Pt,
                                                                        awayTeamItem.ShotPrc1Pt, 
                                                                        awayTeamItem.DefensiveRebounds,
                                                                        awayTeamItem.OffensiveRebounds, 
                                                                        awayTeamItem.TotalRebounds, 
                                                                        awayTeamItem.Assists, 
                                                                        awayTeamItem.Steals, 
                                                                        awayTeamItem.Turnover, 
                                                                        awayTeamItem.InFavoureOfBlock,
                                                                        awayTeamItem.AgainstBlock, 
                                                                        awayTeamItem.CommittedFoul, 
                                                                        awayTeamItem.ReceivedFoul, 
                                                                        awayTeamItem.PointFromPain, 
                                                                        awayTeamItem.PointFrom2ndChance, 
                                                                        awayTeamItem.PointFromFastBreak,
                                                                        awayTeamItem.PlusMinus, 
                                                                        awayTeamItem.RankValue));
                    }
                    else
                    {
                        draftRosterItems.Add(new DraftRosterItemDto(player.Id,
                                                                    player.Name,
                                                                    league.Id,
                                                                    league.OfficalName,
                                                                    item.AwayTeamId ?? 0,
                                                                    item.AwayTeam.Name,
                                                                    DateTime.UtcNow, 
                                                                    null));
                    }
                }

                boxscoresByRound.Add(new BoxScoreDto(
                    new DTOs.Score.ScoreItemDto(item.Id,
                                                item.MatchNo,
                                                homeTeam.Id,
                                                awayTeam.Id,
                                                homeTeam.Name,
                                                awayTeam.Name,
                                                result?.Attendency,
                                                result?.Venue,
                                                result?.HomeTeamPoints,
                                                result?.AwayTeamPoint,
                                                item.Round,
                                                result?.Id),
                    boxscoreDraftItems,
                    existingBoxScoreItems,
                    draftRosterItems,
                    missingPlayers));
            }

            return new BoxScoreByRoundDto(league.OfficalName, boxscoresByRound);
        }
    }
}
