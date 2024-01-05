using Microsoft.Extensions.Logging;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Schedules.Queries.GetScheduleByLeagueId
{
    public class GetScheduleDraftByLeagueIdQueryHandler : IQueryHandler<GetScheduleDraftByLeagueIdQuery, Maybe<ScheduleDraftDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentFetcher _documentFetcher;
        private readonly ILoggerFactory _loggerFactory;

        public GetScheduleDraftByLeagueIdQueryHandler(IUnitOfWork unitOfWork, IDocumentFetcher documentFetcher, ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;
            _documentFetcher = documentFetcher;
            _loggerFactory = loggerFactory;
        }
        public async Task<Maybe<ScheduleDraftDto>>
            Handle(GetScheduleDraftByLeagueIdQuery request, CancellationToken cancellationToken)
        {
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            var teams = await _unitOfWork.TeamRepository.GetAll(cancellationToken);
            var seasonResources = await _unitOfWork.SeasonResourcesRepository.GetAll(cancellationToken);
            if (league is null || !teams.Any())
            {
                return Maybe<ScheduleDraftDto>.None;
            }

            IWebPageProcessor? processor = league.ProcessorTypeEnum switch
            {
                Domain.Enums.ProcessorType.Euro => new EuroPageProcessor(_documentFetcher, _loggerFactory),
                Domain.Enums.ProcessorType.Aba => new WebPageProcessor(_documentFetcher, _loggerFactory),
                Domain.Enums.ProcessorType.Unknow or null or _ => null
            };
            if (processor is null)
            {
                return Maybe<ScheduleDraftDto>.None;
            }
            IReadOnlyList<(string HomeTeamName, string AwayTeamName, int? HomeTeamPoints, int? AwayTeamPoints, DateTime? Date, int? MatchNo, int? Round)> scheduleItems = null;
            var subsetResources = seasonResources.Where(x => x.LeagueId == request.LeagueId);
            switch (league.ProcessorTypeEnum)
            {
                case Domain.Enums.ProcessorType.Euro:
                    var rawUrl = league.BaseUrl + league.CalendarUrl;
                    List<(string HomeTeamName, string AwayTeamName, int? HomeTeamPoints, int? AwayTeamPoints, DateTime? Date, int? MatchNo, int? Round)> list = new List<(string HomeTeamName, string AwayTeamName, int? HomeTeamPoints, int? AwayTeamPoints, DateTime? Date, int? MatchNo, int? Round)>();
                    for (int i=0;i< (subsetResources.Count()-1) * 2; i++)
                    {
                        var url = string.Format(rawUrl, i+1);
                        list.AddRange(await processor.GetRegularSeasonCalendar(i + 1, url, cancellationToken));
                    }
                    scheduleItems = list;
                    break;
                case Domain.Enums.ProcessorType.Aba:
                    scheduleItems = await processor.GetRegularSeasonCalendar(league.BaseUrl + league.CalendarUrl, cancellationToken);
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            var existingScheduleItems = await _unitOfWork.CalendarRepository.SearchByLeague(request.LeagueId, cancellationToken);
            if (scheduleItems is null)
            {
                return Maybe<ScheduleDraftDto>.None;
            }

            List<ScheduleItemDto> draftItems = new List<ScheduleItemDto>();
            List<ScheduleItemDto> existingItems = new List<ScheduleItemDto>();
            List<ScheduleItemDto> plannedItems = new List<ScheduleItemDto>();
            List<string> missingItems = new List<string>();
            foreach (var item in scheduleItems)
            {
                var teamSeasonResource = seasonResources.FirstOrDefault(x => x.TeamName.ToLower().Contains(item.HomeTeamName.ToLower()));
                if (teamSeasonResource is null)
                {
                    if (!missingItems.Any(x => x.ToLower() == item.HomeTeamName.ToLower()))
                    {
                        missingItems.Add(item.HomeTeamName);
                    }
                    continue;
                }
                var homeTeam = teams.FirstOrDefault(x => x.Id == teamSeasonResource.TeamId);

                teamSeasonResource = seasonResources.FirstOrDefault(x => x.TeamName.ToLower().Contains(item.AwayTeamName.ToLower()));
                if (teamSeasonResource is null)
                {
                    if (!missingItems.Any(x => x.ToLower() == item.AwayTeamName.ToLower()))
                    {
                        missingItems.Add(item.HomeTeamName);
                    }
                    continue;
                }
                var awayTeam = teams.FirstOrDefault(x => x.Id == teamSeasonResource.TeamId);

                if (homeTeam != null && awayTeam != null)
                {
                    if (existingScheduleItems.Any(x => x.HomeTeamId == homeTeam.Id && x.AwayTeamId == awayTeam.Id))
                    {
                        var existingScheduleItem = existingScheduleItems
                            .FirstOrDefault(x => x.HomeTeamId == homeTeam.Id && x.AwayTeamId == awayTeam.Id);
                        if (existingScheduleItem == null)
                        {
                            throw new NotImplementedException();
                        }

                        existingItems.Add
                            (
                                new ScheduleItemDto
                                (
                                    existingScheduleItem.Id,
                                    existingScheduleItem.HomeTeamId ?? 0,
                                    existingScheduleItem.AwayTeamId ?? 0,
                                    existingScheduleItem.HomeTeam.Name,
                                    existingScheduleItem.AwayTeam.Name,
                                    existingScheduleItem.Round,
                                    existingScheduleItem.MatchNo,
                                    existingScheduleItem.DateTime
                                )
                            );

                        continue;
                    }
                    if (item.Date == null || item.Date == default)
                    {
                        plannedItems.Add(new ScheduleItemDto(null, homeTeam.Id, awayTeam.Id, homeTeam.Name, awayTeam.Name, item.Round ?? 0, item.MatchNo ?? 0, item.Date ?? default));
                    }
                    else
                    {
                        draftItems.Add(new ScheduleItemDto(null, homeTeam.Id, awayTeam.Id, homeTeam.Name, awayTeam.Name, item.Round ?? 0, item.MatchNo ?? 0, item.Date ?? default));
                    }
                }
                else
                {
                    if (!missingItems.Any(x => x.ToLower() == item.HomeTeamName.ToLower()))
                    {
                        missingItems.Add(item.HomeTeamName);
                    }
                    if (!missingItems.Any(x => x.ToLower() == item.AwayTeamName.ToLower()))
                    {
                        missingItems.Add(item.AwayTeamName);
                    }
                }
            }


            return new ScheduleDraftDto(draftItems, plannedItems, existingItems, missingItems);
        }
    }
}
