using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Implementation;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations
{
    public class WebPageProcessor:IWebPageProcessor
    {
        private readonly ILeagueFetcher _leagueFetcher;

        public WebPageProcessor(ILeagueFetcher leagueFetcher)
        {
            _leagueFetcher = leagueFetcher;
        }
        public async Task<IReadOnlyList<Team>> GetTeams(string leagueUrl, CancellationToken cancellationToken = default)
        {
            var webDocument = await _leagueFetcher
                .FetchTeams(leagueUrl, cancellationToken);

            var teams = new List<Team>();
            var teamElements = webDocument.QuerySelectorAll("table > tbody > tr");

            foreach (var teamElement in teamElements)
            {
                var team= new Team()
                {
                    Name = teamElement
                        .QuerySelectorAll("td")[1]
                        .QuerySelectorAll("a")[0]
                        .InnerHtml
                        .Trim(),
                };
                teams.Add(team);
            }

            return teams;

        }
    }
}
