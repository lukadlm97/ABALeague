using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Domain.Enums;
using System.Text.RegularExpressions;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetTeamsByLeagueId
{
    internal class GetTeamsByLeagueIdQueryHandler : IQueryHandler<GetTeamsByLeagueIdQuery, Maybe<IEnumerable<TeamDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentFetcher _documentFetcher;

        public GetTeamsByLeagueIdQueryHandler(IUnitOfWork unitOfWork, IDocumentFetcher documentFetcher)
        {
            _unitOfWork = unitOfWork;
            _documentFetcher = documentFetcher;
        }
        public async Task<Maybe<IEnumerable<TeamDTO>>> Handle(GetTeamsByLeagueIdQuery request, CancellationToken cancellationToken)
        {
            if (request.LeagueId <= 0)
            {
                return Maybe<IEnumerable<TeamDTO>>.None;
            }

            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId, cancellationToken);
            if (league == null)
            {
                return Maybe<IEnumerable<TeamDTO>>.None;
            }


            IWebPageProcessor webPageProcessor = null;
            if (request.ProcessorType == ProcessorType.Aba)
            {
                webPageProcessor = new WebPageProcessor(_documentFetcher);
            }
            else
            {
                webPageProcessor = new EuroPageProcessor(_documentFetcher);
            }

            var url = league.BaseUrl + league.StandingUrl;
            var teams = await webPageProcessor.GetTeams(url, cancellationToken);

            var existingSeasonResorces =
                await _unitOfWork.SeasonResourcesRepository.SearchByLeague(request.LeagueId, cancellationToken);
            var existingTeams = await _unitOfWork.TeamRepository.GetAll(cancellationToken);

            var list = new List<TeamDTO>();
            foreach (var (name,teamUrl) in teams)
            {
                if (!existingTeams.Any(x => name.ToLower().Contains(x.Name.ToLower())))
                {
                    list.Add(new TeamDTO(null, name, teamUrl, null, null, MaterializationStatus.TeamNoExist));
                    continue;
                }
                var existingTeam = existingTeams.First(x => name.ToLower().Contains(x.Name.ToLower()));
                if (existingSeasonResorces.Any(x => x.TeamName.ToLower().Contains(name.ToLower())))
                {
                    list.Add(new TeamDTO(existingTeam.Id, name, teamUrl.Trim(league.BaseUrl), null, null, MaterializationStatus.Exist));
                }
                else
                {
                    list.Add(new TeamDTO(existingTeam.Id, name, teamUrl.Trim(league.BaseUrl), null, null, MaterializationStatus.NotExist));
                }
            }
            return list;
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
