using Microsoft.Extensions.Logging;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Implementation;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.Constants;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterDraftByLeagueId
{
    public class GetRosterDraftByLeagueIdQueryHandler : IQueryHandler<GetRosterDraftByLeagueIdQuery, Maybe<DraftRosterDto>>
    {
        private readonly IDocumentFetcher _documentFetcher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerFactory _loggerFactory;

        public GetRosterDraftByLeagueIdQueryHandler(IUnitOfWork unitOfWork, IDocumentFetcher documentFetcher, ILoggerFactory loggerFactory)
        {
            _documentFetcher = documentFetcher;
            _unitOfWork = unitOfWork;
            _loggerFactory = loggerFactory;
        }
        public async Task<Maybe<DraftRosterDto>> Handle(GetRosterDraftByLeagueIdQuery request, 
                                                        CancellationToken cancellationToken)
        {
            var allTeamSeasonResources = await _unitOfWork.SeasonResourcesRepository.GetAll(cancellationToken);
            var teamSeasonResources = allTeamSeasonResources.Where(x=>x.LeagueId==request.LeagueId);
            var players = await _unitOfWork.PlayerRepository.GetAll(cancellationToken);
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            var teams = await _unitOfWork.TeamRepository.GetAll(cancellationToken);
            
            if(league == null || teamSeasonResources==null || players == null)
            {
                return Maybe<DraftRosterDto>.None;
            }
            var existingRosterItems = await _unitOfWork.RosterRepository.SearchByLeagueId(request.LeagueId, cancellationToken);
            IWebPageProcessor? processor = league.ProcessorTypeEnum switch
            {
                Domain.Enums.ProcessorType.Euro => new EuroPageProcessor(_documentFetcher, _loggerFactory),
                Domain.Enums.ProcessorType.Aba => new AbaPageProcessor(_documentFetcher, _loggerFactory),
                Domain.Enums.ProcessorType.Kls => new KlsPageProcessor(_documentFetcher, _loggerFactory),
                Domain.Enums.ProcessorType.Unknow or null or _ => null
            };
            if (processor is null)
            {
                return Maybe<DraftRosterDto>.None;
            }

            List<(int? No, string Name, string Position, decimal Height, DateTime DateOfBirth, string Nationality, DateTime Start, DateTime? End, int TeamId, int LeagueId)> rosterItems = new List<(int? No, string Name, string Position, decimal Height, DateTime DateOfBirth, string Nationality, DateTime Start, DateTime? End, int TeamId, int LeagueId)>();
            switch (league.ProcessorTypeEnum)
            {
                case Domain.Enums.ProcessorType.Euro:
                    foreach(var team in teamSeasonResources)
                    {
                        var url = string.Format(league.RosterUrl, team.IncrowdUrl);
                        var items = await processor.GetRoster(url, cancellationToken);
                        rosterItems.AddRange(items.Select(x=> (x.No,x.Name,x.Position,x.Height,x.DateOfBirth,x.Nationality,x.Start,x.End,team.TeamId, team.LeagueId)));
                    }
                    break;
                case Domain.Enums.ProcessorType.Aba:
                    foreach (var team in teamSeasonResources)
                    {
                        var url = league.BaseUrl + team.TeamSourceUrl;
                        var items = await processor.GetRoster(url, cancellationToken);
                        rosterItems.AddRange(items.Select(x => (x.No, x.Name, x.Position, x.Height, x.DateOfBirth, x.Nationality, x.Start, x.End, team.TeamId, team.LeagueId)));
                    }
                    break;
                case Domain.Enums.ProcessorType.Kls:
                    foreach (var team in teamSeasonResources)
                    {
                        var url = league.BaseUrl + string.Format(league.RosterUrl, team.TeamUrl);
                        var items = await processor.GetRoster(url, cancellationToken);
                        rosterItems.AddRange(items.Select(x => (x.No, x.Name, x.Position, x.Height, x.DateOfBirth, x.Nationality, x.Start, x.End, team.TeamId, team.LeagueId)));
                    }
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            List<DraftRosterItemDto> draftRosterItems = new List<DraftRosterItemDto>();
            List<DraftRosterItemDto> existingDraftRosterItems = new List<DraftRosterItemDto>();
            List<DraftRosterItemDto> draftRosterItemsWithEndedContract = new List<DraftRosterItemDto>();
            List<PlayerItemDraftDto> missingPlayers = new List<PlayerItemDraftDto>();

            foreach(var rosterItem in rosterItems)
            {
                if(rosterItem.Position.ToLower() == PlaceHolderContants.CoachPosition)
                {
                    continue;
                }
                var player= players.FirstOrDefault(x=>x.Name.ToLower() == rosterItem.Name.ToLower());
                var team= teams.FirstOrDefault(x=>x.Id == rosterItem.TeamId);
                if (team == null)
                {
                    return Maybe<DraftRosterDto>.None;
                }
                if(player == null)
                {
                    if (string.IsNullOrWhiteSpace(rosterItem.Name))
                    {
                        var logger =_loggerFactory.CreateLogger(Constants.PlaceHolderContants.CoachPosition);
                        logger.LogError("missing name:" + rosterItem.Name);
                        continue;
                    }
                    var natioanality = league.ProcessorTypeEnum switch
                    {
                        Domain.Enums.ProcessorType.Euro => await _unitOfWork.CountryRepository.GetByIso3(rosterItem.Nationality, cancellationToken),
                        Domain.Enums.ProcessorType.Aba => await _unitOfWork.CountryRepository
                        .GetByAbaCode(rosterItem.Nationality, cancellationToken),
                        Domain.Enums.ProcessorType.Kls => await _unitOfWork.CountryRepository
                        .GetByIso3(rosterItem.Nationality, cancellationToken),
                        Domain.Enums.ProcessorType.Unknow or null or _ => null
                    };

                    if(natioanality == null)
                    {
                        missingPlayers.Add(new PlayerItemDraftDto(rosterItem.Name, MapToEnum(rosterItem.Position), (int)rosterItem.Height, rosterItem.DateOfBirth, null));
                        continue;
                    }
                    player = await _unitOfWork.PlayerRepository.GetPlayerByAnotherName(rosterItem.Name, cancellationToken);
                    if (player == null)
                    {
                        missingPlayers.Add(new PlayerItemDraftDto(rosterItem.Name, MapToEnum(rosterItem.Position), (int)rosterItem.Height, rosterItem.DateOfBirth, natioanality.Id));
                        continue;
                    }
                    if(existingRosterItems.Any(x => x.TeamId == rosterItem.TeamId &&
                                                 x.PlayerId == player.Id &&
                                                 x.LeagueId == request.LeagueId))
                    {
                        var existingRosterItem = existingRosterItems.FirstOrDefault(x => x.TeamId == rosterItem.TeamId &&
                                                 x.PlayerId == player.Id && x.LeagueId == request.LeagueId);
                        existingDraftRosterItems.Add(new DraftRosterItemDto(existingRosterItem.PlayerId, existingRosterItem.Player.Name, existingRosterItem.LeagueId, existingRosterItem.League.OfficalName, team.Id, team.Name, existingRosterItem.DateOfInsertion, existingRosterItem.EndOfActivePeriod));
                        continue;
                    }
                    draftRosterItems.Add(
                        new DraftRosterItemDto(player.Id,
                                                player.Name,
                                                league.Id,
                                                league.OfficalName,
                                                team.Id,
                                                team.Name,
                                                DateTime.UtcNow,
                                                null));

                    continue;
                }

                if(existingRosterItems.Any(x=>x.TeamId == rosterItem.TeamId &&
                                                 x.PlayerId == player.Id && 
                                                 x.LeagueId == request.LeagueId))
                {
                    var existingRosterItem = existingRosterItems.FirstOrDefault(x=>x.TeamId == rosterItem.TeamId &&
                                                 x.PlayerId == player.Id && x.LeagueId == request.LeagueId);
                    existingDraftRosterItems.Add(new DraftRosterItemDto(existingRosterItem.PlayerId,existingRosterItem.Player.Name,existingRosterItem.LeagueId, existingRosterItem.League.OfficalName, team.Id, team.Name, existingRosterItem.DateOfInsertion, existingRosterItem.EndOfActivePeriod));
                    continue;
                }

                draftRosterItems.Add(
                    new DraftRosterItemDto(player.Id, 
                                            player.Name, 
                                            league.Id, 
                                            league.OfficalName, 
                                            team.Id,
                                            team.Name, 
                                            DateTime.UtcNow, 
                                            null));
            }
            var playesWithNotEndedContract = existingRosterItems.Where(x => x.EndOfActivePeriod == null);
            
            foreach(var item in playesWithNotEndedContract)
            {
                if (!rosterItems.Select(x => x.Name.ToLower()).Contains(item.Player.Name.ToLower()))
                {
                    draftRosterItemsWithEndedContract.Add(new DraftRosterItemDto(item.PlayerId, item.Player.Name, item.LeagueId, item.League.OfficalName, item.TeamId, item.Team.Name, item.DateOfInsertion, null));
                }
            }


            return new DraftRosterDto(draftRosterItems, existingDraftRosterItems, missingPlayers, draftRosterItemsWithEndedContract);
        }
        PositionEnum MapToEnum(string value)
        {
            switch (value.Trim().ToLower())
            {
                case "shooting guard":
                case "shootingguard":
                    return PositionEnum.ShootingGuard;
                case "center":
                    return PositionEnum.Center;
                case "power forward":
                case "powerforward":
                    return PositionEnum.PowerForward;
                case "guard":
                    return PositionEnum.Guard;
                case "forward":
                    return PositionEnum.Forward;


                default:
                    return PositionEnum.Coach;
            }
        }
    }
}
