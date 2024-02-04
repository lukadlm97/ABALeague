using AngleSharp.Dom;
using Microsoft.Extensions.Logging;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Implementation;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Application.DTOs.Score;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Score.Queries.GetScoreDraftByLeagueId
{
    public class GetScoreDraftByLeagueIdQueryHandler : IQueryHandler<GetScoreDraftByLeagueIdQuery, Maybe<ScoreDraftDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentFetcher _documentFetcher;
        private readonly ILoggerFactory _loggerFactory;

        public GetScoreDraftByLeagueIdQueryHandler(IUnitOfWork unitOfWork, IDocumentFetcher documentFetcher, ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;
            _documentFetcher = documentFetcher;
            _loggerFactory = loggerFactory;
        }
        public async Task<Maybe<ScoreDraftDto>> Handle(GetScoreDraftByLeagueIdQuery request, CancellationToken cancellationToken)
        {
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            if (league == null)
            {
                return Maybe<ScoreDraftDto>.None;
            }

            // determinate which processor is the most appropraite
            IWebPageProcessor? processor = league.ProcessorTypeEnum switch
            {
                Domain.Enums.ProcessorType.Euro => new EuroPageProcessor(_documentFetcher, _loggerFactory),
                Domain.Enums.ProcessorType.Aba => new AbaPageProcessor(_documentFetcher, _loggerFactory),
                Domain.Enums.ProcessorType.Kls => new KlsPageProcessor(_documentFetcher, _loggerFactory),
                Domain.Enums.ProcessorType.Unknow or null or _ => null
            };
            if (processor == null)
            {
                return Maybe<ScoreDraftDto>.None;
            }

            // found match round ids (schedule item) for which result exist
            var existingResults = await _unitOfWork.ResultRepository.SearchByLeague(request.LeagueId, cancellationToken);
            var roundIdsForExistingResult = existingResults.Select(x => x.RoundMatchId);
            
            // found match number for which you need result
            var matchesOnSchedule = await _unitOfWork.CalendarRepository.SearchByLeague(request.LeagueId, cancellationToken);
            matchesOnSchedule = matchesOnSchedule.OrderBy(x => x.MatchNo);
            var matchNumbers = new List<int>();
            foreach(var roundMatch in matchesOnSchedule.Where(x=>x.DateTime <= DateTime.UtcNow))
            {
                if (!roundIdsForExistingResult.Any(x => x == roundMatch.Id))
                {
                    matchNumbers.Add(roundMatch.MatchNo);
                }
            }    
               
            if(matchNumbers == null)
            {
                return Maybe<ScoreDraftDto>.None;
            }

            // build appropraite urls using match number
            IEnumerable<(int matchNo, string url)> matchNoUrlPair = matchNumbers.Select(x =>
            {
                string? url = league.ProcessorTypeEnum switch
                {
                    Domain.Enums.ProcessorType.Euro => league.BaseUrl +
                                                        string.Format(league.MatchUrl, x),
                    Domain.Enums.ProcessorType.Aba => league.BaseUrl +
                                                        string.Format(league.MatchUrl, x),
                    Domain.Enums.ProcessorType.Kls => string.Format(league.MatchUrl, x),
                    Domain.Enums.ProcessorType.Unknow or null or _ => null
                };
                if (string.IsNullOrEmpty(url))
                {
                    return (x, string.Empty);
                }
                return (x, url);
            });

            // fetch result from API
            var fetchedMatchScoreItems = await processor.GetMatchScores(matchNoUrlPair, cancellationToken);
            if (fetchedMatchScoreItems == null)
            {
                return Maybe<ScoreDraftDto>.None;
            }

            // classify result by next categories
            List<ScoreItemDto> draftItems = new List<ScoreItemDto>();
            List<ScoreItemDto> existingItems = new List<ScoreItemDto>();
            List<ScoreItemDto> plannedItems = new List<ScoreItemDto>();
            foreach (var item in fetchedMatchScoreItems)
            {
                var match = matchesOnSchedule.FirstOrDefault(x=> x.MatchNo == item.MatchNo);
                if (match == null)
                {
                    continue;
                }
                if(item.Attendency ==0 && item.HomeTeamPoint == 0 && item.AwayTeamPoint == 0)
                {
                    continue;
                }

                if (await _unitOfWork.ResultRepository.Exist(match.Id, cancellationToken))
                {
                    existingItems.Add(new ScoreItemDto(match.Id,
                                                        item.MatchNo??0,
                                                        match.HomeTeamId ?? 0,
                                                        match.AwayTeamId ?? 0,
                                                        match.HomeTeam.Name,
                                                        match.AwayTeam.Name,
                                                        item.Attendency ?? 0,
                                                        item.Venue,
                                                        item.HomeTeamPoint,
                                                        item.AwayTeamPoint,
                                                        match.Round,
                                                        null,
                                                        match.DateTime));
                    continue;
                }
                
                if(item.HomeTeamPoint == null || item.AwayTeamPoint == null)
                {
                    plannedItems.Add(new ScoreItemDto(match.Id,
                                                        item.MatchNo ?? 0,
                                                        match.HomeTeamId ?? 0,
                                                        match.AwayTeamId ?? 0,
                                                        match.HomeTeam.Name,
                                                        match.AwayTeam.Name,
                                                        item.Attendency ?? 0,
                                                        item.Venue,
                                                        item.HomeTeamPoint,
                                                        item.AwayTeamPoint,
                                                        match.Round,
                                                        null,
                                                        match.DateTime));
                    continue;
                }

                draftItems.Add(new ScoreItemDto(match.Id,
                                                    item.MatchNo ?? 0,
                                                    match.HomeTeamId ?? 0,
                                                    match.AwayTeamId ?? 0,
                                                    match.HomeTeam.Name,
                                                    match.AwayTeam.Name,
                                                    item.Attendency ?? 0,
                                                    item.Venue,
                                                    item.HomeTeamPoint,
                                                    item.AwayTeamPoint, 
                                                    match.Round,
                                                    null,
                                                    match.DateTime));
            }

            // populate existing result that we don't fetch from API
            foreach(var existingItem in existingResults)
            {

                if(!existingItems.Any(x=>x.MatchId == existingItem.Id))
                {
                    var scheduleItem = matchesOnSchedule.FirstOrDefault(x=>x.Id== existingItem.RoundMatchId);
                    if(scheduleItem != null)
                    {
                        existingItems.Add(new ScoreItemDto(existingItem.Id,
                                                      scheduleItem.MatchNo,
                                                      scheduleItem.HomeTeamId ?? 0,
                                                      scheduleItem.AwayTeamId ?? 0,
                                                      scheduleItem.HomeTeam.Name,
                                                      scheduleItem.AwayTeam.Name,
                                                      existingItem.Attendency,
                                                      existingItem.Venue,
                                                      existingItem.HomeTeamPoints,
                                                      existingItem.AwayTeamPoint,
                                                      scheduleItem.Round,
                                                      null,
                                                      scheduleItem.DateTime));
                        continue;
                    }
                    throw new NotImplementedException();
                }
            }

            return new ScoreDraftDto(draftItems, existingItems, plannedItems);
        }

     
    }
}
