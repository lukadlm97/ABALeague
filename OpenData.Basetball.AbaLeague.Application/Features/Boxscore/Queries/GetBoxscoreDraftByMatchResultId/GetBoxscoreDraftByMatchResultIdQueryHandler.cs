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
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreDraftByMatchResultId
{
    public class GetBoxscoreDraftByMatchResultIdQueryHandler : IQueryHandler<GetBoxscoreDraftByMatchResultIdQuery, Maybe<BoxScoreDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentFetcher _documentFetcher;
        private readonly ILoggerFactory _loggerFactory;

        public GetBoxscoreDraftByMatchResultIdQueryHandler(IUnitOfWork unitOfWork, 
                                                            IDocumentFetcher documentFetcher, 
                                                            ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;
            _documentFetcher = documentFetcher;
            _loggerFactory = loggerFactory;
        }
        public async Task<Maybe<BoxScoreDto>> Handle(GetBoxscoreDraftByMatchResultIdQuery request, CancellationToken cancellationToken)
        {
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            var teams = await _unitOfWork.TeamRepository.GetAll(cancellationToken); 
            var players = await _unitOfWork.PlayerRepository.GetAll(cancellationToken); 
            var roundMatches = await _unitOfWork.CalendarRepository.SearchByLeague(request.LeagueId, cancellationToken);
            var rosterItems = await _unitOfWork.RosterRepository.GetAll(cancellationToken);
            if (league is null || !teams.Any() || roundMatches is null || !roundMatches.Any() || 
                players is null || !players.Any())
            {
                return Maybe<BoxScoreDto>.None;
            }

            var result = await _unitOfWork.ResultRepository.Get(request.MatchResultId, cancellationToken);
            if(result == null)
            {
                return Maybe<BoxScoreDto>.None;
            }

            var scheduleItem = roundMatches.FirstOrDefault(x=>x.Id == result.RoundMatchId);
            if (scheduleItem == null)
            {
                return Maybe<BoxScoreDto>.None;
            }

            var homeTeam  = await _unitOfWork.TeamRepository.Get(scheduleItem.HomeTeamId ?? 0, cancellationToken);
            var awayTeam  = await _unitOfWork.TeamRepository.Get(scheduleItem.AwayTeamId ?? 0, cancellationToken);
            if(homeTeam == null || awayTeam == null)
            {
                return Maybe<BoxScoreDto>.None;
            }

            IWebPageProcessor? processor = league.ProcessorTypeEnum switch
            {
                Domain.Enums.ProcessorType.Euro => new EuroPageProcessor(_documentFetcher),
                Domain.Enums.ProcessorType.Aba => new WebPageProcessor(_documentFetcher, _loggerFactory),
                Domain.Enums.ProcessorType.Unknow or null or _ => null
            };
            if (processor is null)
            {
                return Maybe<BoxScoreDto>.None;
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
                    throw new NotImplementedException();
                    rawUrl = league.BaseUrl + league.CalendarUrl;
                     url = string.Format(rawUrl, 1);
                    boxScoreItems = await processor.GetBoxScore(url, cancellationToken);
                    break;
                case Domain.Enums.ProcessorType.Aba:
                    rawUrl = league.BaseUrl + league.BoxScoreUrl;
                    url = string.Format(rawUrl, scheduleItem.MatchNo);
                    boxScoreItems = await processor.GetBoxScore(url, cancellationToken);
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            if(boxScoreItems.HomeTeam == null || boxScoreItems.AwayTeam == null) 
            {
                return Maybe<BoxScoreDto>.None;
            }

            List<BoxScoreItemDto> boxscoreDraftItems = new List<BoxScoreItemDto>();   
            List<BoxScoreItemDto> existingBoxScoreItems = new List<BoxScoreItemDto>();   
            List<DraftRosterItemDto> draftRosterItems = new List<DraftRosterItemDto>();   
            List<string> missingPlayers = new List<string>();   

            foreach(var item in boxScoreItems.HomeTeam)
            {
                var player = players.FirstOrDefault(x=> x.Name.ToLower() == item.Name.ToLower());  
                if(player == null)
                {
                    missingPlayers.Add(item.Name);
                    continue;
                }

                if(rosterItems.Any(x=>x.TeamId == scheduleItem.HomeTeamId && x.PlayerId == player.Id && x.LeagueId == league.Id))
                {
                    var selectedRosterItem = rosterItems.First(x => x.TeamId == scheduleItem.HomeTeamId && x.PlayerId == player.Id && x.LeagueId == league.Id);

                    if(await _unitOfWork.BoxScoreRepository.Exist(result.RoundMatchId, 
                                                                    selectedRosterItem.PlayerId, 
                                                                    cancellationToken))
                    {
                        boxscoreDraftItems.Add(new BoxScoreItemDto(selectedRosterItem.Id, scheduleItem.Id, player.Name, homeTeam.Name, scheduleItem.Round, scheduleItem.MatchNo,  item.Minutes, item.Points, item.ShotPrc, item.ShotMade2Pt, item.ShotAttempted2Pt, item.ShotPrc2Pt, item.ShotMade3Pt, item.ShotAttempted3Pt, item.shotPrc3Pt, item.ShotMade1Pt, item.ShotAttempted1Pt, item.ShotPrc1Pt, item.DefensiveRebounds, item.OffensiveRebounds, item.TotalRebounds, item.Assists, item.Steals, item.Turnover, item.InFavoureOfBlock, item.AgainstBlock, item.CommittedFoul, item.ReceivedFoul, item.PointFromPain, item.PointFrom2ndChance, item.PointFromFastBreak, item.PlusMinus, item.RankValue));
                        continue;
                    }
                    existingBoxScoreItems.Add(new BoxScoreItemDto(selectedRosterItem.Id, scheduleItem.Id, player.Name, homeTeam.Name, scheduleItem.Round, scheduleItem.MatchNo, item.Minutes, item.Points, item.ShotPrc, item.ShotMade2Pt, item.ShotAttempted2Pt, item.ShotPrc2Pt, item.ShotMade3Pt, item.ShotAttempted3Pt, item.shotPrc3Pt, item.ShotMade1Pt, item.ShotAttempted1Pt, item.ShotPrc1Pt, item.DefensiveRebounds, item.OffensiveRebounds, item.TotalRebounds, item.Assists, item.Steals, item.Turnover, item.InFavoureOfBlock, item.AgainstBlock, item.CommittedFoul, item.ReceivedFoul, item.PointFromPain, item.PointFrom2ndChance, item.PointFromFastBreak, item.PlusMinus, item.RankValue));
                }
                else
                {
                    draftRosterItems.Add(new DraftRosterItemDto(player.Id, player.Name, league.Id, league.OfficalName, scheduleItem.HomeTeamId??0, scheduleItem.HomeTeam.Name, DateTime.UtcNow, null));
                }
            }

            foreach (var item in boxScoreItems.AwayTeam)
            {
                var player = players.FirstOrDefault(x => x.Name.ToLower() == item.Name.ToLower());
                if (player == null)
                {
                    missingPlayers.Add(item.Name);
                    continue;
                }

                if (rosterItems.Any(x => x.TeamId == scheduleItem.AwayTeamId &&
                                            x.PlayerId == player.Id &&
                                            x.LeagueId == league.Id))
                {
                    var selectedRosterItem = rosterItems.First(x => x.TeamId == scheduleItem.AwayTeamId &&
                                                                        x.PlayerId == player.Id && 
                                                                        x.LeagueId == league.Id);

                    if (await _unitOfWork.BoxScoreRepository.Exist(result.RoundMatchId,
                                                                     selectedRosterItem.PlayerId,
                                                                     cancellationToken))
                    {
                        boxscoreDraftItems.Add(new BoxScoreItemDto(selectedRosterItem.Id, scheduleItem.Id, player.Name, awayTeam.Name, scheduleItem.Round, scheduleItem.MatchNo, item.Minutes, item.Points, item.ShotPrc, item.ShotMade2Pt, item.ShotAttempted2Pt, item.ShotPrc2Pt, item.ShotMade3Pt, item.ShotAttempted3Pt, item.shotPrc3Pt, item.ShotMade1Pt, item.ShotAttempted1Pt, item.ShotPrc1Pt, item.DefensiveRebounds, item.OffensiveRebounds, item.TotalRebounds, item.Assists, item.Steals, item.Turnover, item.InFavoureOfBlock, item.AgainstBlock, item.CommittedFoul, item.ReceivedFoul, item.PointFromPain, item.PointFrom2ndChance, item.PointFromFastBreak, item.PlusMinus, item.RankValue));
                        continue;
                    }
                    existingBoxScoreItems.Add(new BoxScoreItemDto(selectedRosterItem.Id, scheduleItem.Id, player.Name, awayTeam.Name, scheduleItem.Round, scheduleItem.MatchNo, item.Minutes, item.Points, item.ShotPrc, item.ShotMade2Pt, item.ShotAttempted2Pt, item.ShotPrc2Pt, item.ShotMade3Pt, item.ShotAttempted3Pt, item.shotPrc3Pt, item.ShotMade1Pt, item.ShotAttempted1Pt, item.ShotPrc1Pt, item.DefensiveRebounds, item.OffensiveRebounds, item.TotalRebounds, item.Assists, item.Steals, item.Turnover, item.InFavoureOfBlock, item.AgainstBlock, item.CommittedFoul, item.ReceivedFoul, item.PointFromPain, item.PointFrom2ndChance, item.PointFromFastBreak, item.PlusMinus, item.RankValue));
                }
                else
                {
                    draftRosterItems.Add(new DraftRosterItemDto(player.Id, player.Name, league.Id, league.OfficalName, scheduleItem.AwayTeamId ?? 0, scheduleItem.AwayTeam.Name, DateTime.UtcNow, null));
                }
            }

            return new BoxScoreDto(boxscoreDraftItems, existingBoxScoreItems, draftRosterItems, missingPlayers);
        }
    }
}
