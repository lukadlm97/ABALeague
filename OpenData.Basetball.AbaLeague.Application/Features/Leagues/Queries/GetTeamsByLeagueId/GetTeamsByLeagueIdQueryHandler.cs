using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Domain.Enums;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Score;
using OpenData.Basketball.AbaLeague.Application.Utilities;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;
using System.Collections.Frozen;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetTeamsByLeagueId
{
    internal class GetTeamsByLeagueIdQueryHandler : 
        IQueryHandler<GetTeamsByLeagueIdQuery, Maybe<TeamSeasonResourceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentFetcher _documentFetcher;
        private readonly ILoggerFactory _loggerFactory;

        public GetTeamsByLeagueIdQueryHandler(IUnitOfWork unitOfWork, 
                                                IDocumentFetcher documentFetcher, 
                                                ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;
            _documentFetcher = documentFetcher;
            _loggerFactory = loggerFactory;
        }
        public async Task<Maybe<TeamSeasonResourceDto>>
            Handle(GetTeamsByLeagueIdQuery request, CancellationToken cancellationToken)
        {
            if (request.LeagueId <= 0)
            {
                return Maybe<TeamSeasonResourceDto>.None;
            }

            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            if (league == null)
            {
                return Maybe<TeamSeasonResourceDto>.None;
            }
            var seasonResources = await _unitOfWork.SeasonResourcesRepository.GetAll(cancellationToken);
            IWebPageProcessor? webPageProcessor = league.ProcessorTypeEnum switch
            {
                Domain.Enums.ProcessorType.Euro => new EuroPageProcessor(_documentFetcher, _loggerFactory),
                Domain.Enums.ProcessorType.Aba => new WebPageProcessor(_documentFetcher, _loggerFactory),
                Domain.Enums.ProcessorType.Unknow or null or _ => null
            };
            if (webPageProcessor == null)
            {
                return Maybe<TeamSeasonResourceDto>.None;
            }

            var url = league.BaseUrl + league.StandingUrl;

            IReadOnlyList<(string, string)> teams = league.ProcessorTypeEnum switch
            {
                Domain.Enums.ProcessorType.Euro => 
                await webPageProcessor.GetTeams(url, 
                                                await GetStandingSelector(request.LeagueId),
                                                await GetStandingRowNameSelector(request.LeagueId), 
                                                await GetStandingRowUrlSelector(request.LeagueId), cancellationToken),
                Domain.Enums.ProcessorType.Aba =>
                await webPageProcessor.GetTeams(url, null, null, null, cancellationToken),
                Domain.Enums.ProcessorType.Unknow or null or _ => null
            }; ;
            if (teams == null || !teams.Any())
            {
                return Maybe<TeamSeasonResourceDto>.None;
            }
            var existingSeasonResorces =
                await _unitOfWork.SeasonResourcesRepository.SearchByLeague(request.LeagueId, cancellationToken);
            var existingTeams = await _unitOfWork.TeamRepository.GetAll(cancellationToken);

            var existingTeamSeasonResources = new List<TeamItemDraftDto>();
            var draftTeamSeasonResources = new List<TeamItemDraftDto>();
            var missingTeamItems = new List<string>();
            foreach (var (name,teamUrl) in teams)
            {
                if (!existingTeams.Any(x => name.ToLower().Contains(x.Name.ToLower())))
                {
                    if(seasonResources.Where(x => x.LeagueId == request.LeagueId)
                        .Any(x=>x.TeamName.ToLower() == name.ToLower()))
                    {
                        var selectedSeasonResources= seasonResources
                            .FirstOrDefault(x=>x.LeagueId == request.LeagueId && 
                                                x.TeamName.ToLower() == name.ToLower());

                        existingTeamSeasonResources.Add(
                            new TeamItemDraftDto(selectedSeasonResources.TeamId,
                                                    name,
                                                    null,
                                                    teamUrl.Trim(league.BaseUrl),
                                                    teamUrl.Trim(league.BaseUrl)
                                                            .ExtractTeamCode()));
                        continue;
                    }
                    missingTeamItems.Add(name);
                    continue;
                }
                var existingTeam = existingTeams.First(x => name.ToLower().Contains(x.Name.ToLower()));
                if (existingSeasonResorces.Any(x => x.TeamName.ToLower().Contains(name.ToLower())))
                {
                    draftTeamSeasonResources.Add(
                        new TeamItemDraftDto(existingTeam.Id,
                                                name, 
                                                null, 
                                                teamUrl.Trim(league.BaseUrl), 
                                                teamUrl.Trim(league.BaseUrl)
                                                        .ExtractTeamCode()));
                }
                else
                {
                    missingTeamItems.Add(name);
                }
            }
            return new TeamSeasonResourceDto(existingTeamSeasonResources.ToFrozenSet(),
                                                draftTeamSeasonResources.ToFrozenSet(), 
                                                missingTeamItems.ToFrozenSet());
        }
        
        async Task<string> GetStandingSelector(int leagueId, CancellationToken cancellationToken = default)
        {
            var selector = await _unitOfWork.SelectorResourcesRepository
                .GetByLeagueIdAndSelectorType(leagueId, (short)Domain.Enums.HtmlQuerySelectorEnum.StandingsTable);
            if(selector == null)
            {
                return string.Empty;
            }
            return selector.Value;
        }
        async Task<string> GetStandingRowNameSelector(int leagueId, CancellationToken cancellationToken = default)
        {
            var selector = await _unitOfWork.SelectorResourcesRepository
                .GetByLeagueIdAndSelectorType(leagueId, (short)Domain.Enums.HtmlQuerySelectorEnum.StandingsRowName);
            if (selector == null)
            {
                return string.Empty;
            }
            return selector.Value;
        }


        async Task<string> GetStandingRowUrlSelector(int leagueId, CancellationToken cancellationToken = default)
        {
            var selector = await _unitOfWork.SelectorResourcesRepository
                .GetByLeagueIdAndSelectorType(leagueId, (short)Domain.Enums.HtmlQuerySelectorEnum.StandingsRowUrl);
            if (selector == null)
            {
                return string.Empty;
            }
            return selector.Value;
        }
        
    }
    public static class TrimStringExtensions
    {
        public static string Trim(this string s, string toBeTrimmed)
        {
            if (string.IsNullOrEmpty(toBeTrimmed)) return s;
            var literal = Regex.Escape(toBeTrimmed);
            return Regex.Replace(s, $"^{literal}|{literal}$", "");
        }
    }
}
